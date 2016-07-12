from threading import Thread
import socket
import SocketServer
import sys
import serial
import time
#import recognition
from math import atan2, degrees
import curses
import numpy as np
from video import create_capture
from common import clock, draw_str
import cv2
from picamera.array import PiRGBArray
from picamera import PiCamera


#Recognition part

xSource = 0
ySource = 0
hSource = 0
count = 0
authenticated = True

def detect(img, cascade):
    rects = cascade.detectMultiScale(img, scaleFactor=1.3, minNeighbors=4, minSize=(20, 20),
                                     flags=cv2.CASCADE_SCALE_IMAGE)
    if len(rects) == 0:
        return []
    rects[:,2:] += rects[:,:2]
    return rects

def draw_rects(img, rects, color):

    global count
    global xSource
    global ySource
    global hSource

    for x1, y1, x2, y2 in rects:

        if count == 0:
            xSource = x1
            ySource = y1
            hSource = x2-x1
        if count == 40:
            # print ("xs - x1")
            # print (xSource - x1)
            if((xSource - x1) < 25) & ((xSource - x1) > -25):
                count=35
                # Nothing
            else:
                # print("hsource - x2-x1")
                # print (hSource - (x2 - x1))
                if (hSource - (x2-x1)) > 60:
                    print ("Backward")
                elif (hSource - (x2-x1)) < -60:
                    print ("Forward")
                else:
                    getDirection(xSource,ySource,x1,y1)
                count = -1

        count+=1
        cv2.rectangle(img, (x1, y1), (x2, y2), color, 1)

def sum (x,y):
    print (x+y)


def getDirection(xSource,ySource,xDestination,yDestination):
    angle = degrees(atan2(yDestination-ySource,xDestination-xSource))
    if angle < 0:
        angle += 360
    if (angle > 337.5) | (angle < 22.5):
        print ("Right")
    elif (angle > 22.5) & (angle < 67.5):
        print ("Down Right")
    elif (angle > 67.5) & (angle < 112.5):
        print ("Down")
    elif (angle > 112.5) & (angle < 157.5):
        print ("Down Left")
    elif (angle > 157.5)  & (angle < 202.5):
        print ("Left")
    elif (angle > 202.5) & (angle < 247.5):
        print ("Up Left")
    elif (angle > 247.5) & (angle < 292.5):
        print ("Up")
    elif (angle > 292.5) & (angle < 337.5):
        print ("Up Right")

def RecognitionThread():
    # initialize the camera and grab a reference to the raw camera capture
    camera = PiCamera()
    camera.resolution = (320,208)
    camera.framerate = 16
    rawCapture = PiRGBArray(camera, size=(320,208))
    cascade = cv2.CascadeClassifier("haarcascade_hand.xml")
    # allow the camera to warmup
    time.sleep(0.1)
 
    # capture frames from the camera
    for frame in camera.capture_continuous(rawCapture, format="bgr", use_video_port=True):
        # grab the raw NumPy array representing the image, then initialize the timestamp
       	# and occupied/unoccupied text
	    image = frame.array
    	gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
	    gray = cv2.equalizeHist(gray)
	    rects = detect(gray, cascade)
	    vis = image.copy()
        draw_rects(vis, rects, (0, 255, 0))
	    cv2.imshow("Frame", vis)
	    key = cv2.waitKey(1) & 0xFF
 
	    # clear the stream in preparation for the next frame
	    rawCapture.truncate(0)
 
	    # if the `q` key was pressed, break from the loop
	    if key == ord("q"):
		    break

#End Recognition





#Socket Server
class MyTCPHandler(SocketServer.BaseRequestHandler):
       def handle(self):
        # self.request is the TCP socket connected to the client
        self.data = self.request.recv(1024).strip()
        print "{} wrote:".format(self.client_address[0])
        print self.data
        parseMsg(self.data)
        # just send back the same data, but upper-cased
        #self.request.sendall(self.data.upper())

#Serial PORT
port = serial.Serial("/dev/ttyAMA0", baudrate=115200, timeout=0)
msg = ""
TCP_IP = '192.168.4.1'
TCP_PORT = 23
TCP_BUFFER_SIZE = 1024
def serialAvailable():
    global msg
    if port.isOpen()== False:
        port.open()
    else: pass
    rcv = port.readline()
    msg += rcv
    if len(msg)>0:
        print ("available data: " + msg)
        if ";" in msg:
            msgs = msg.split(';')
            print ("messages are: " + str(msgs))
            msg = msgs[-1]
            for m in msgs:
                parseMsg(m)

def parseMsg(m):
    if "$CFG$" in m:
        print "Configuration msg: " + m
        file = open('config.cfg', 'w+')
        file.write(m[5:])
        file.close()
    elif "$CTRL$" in m:
        print "Control msg: " + m
        cfg = open('config.cfg', 'r+').readline().split('#')#read configs
        cmd_key = m.split('$')[2]#get the key from message m
        cmd_val = cfg[indexContainingSubstring(cfg,cmd_key)].split('=')[1]#get the saved value in the config
        print "cmd_key: " + cmd_key
        print "cmd_val: " + cmd_val
        socketSendCmd(cmd_val)
    else:
        print "Other msg: " + m
def indexContainingSubstring(list, substring):
    for i, s in enumerate(list):
        if substring in s:
              return i
    return -1
    
def socketSendCmd(cmd):
    print "Connecting to socket IP:"+str(TCP_IP)+" PORT:" +str(TCP_PORT)
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    print "sending socket: "+ "$"+cmd+"#"
    s.send("$"+cmd+"#")
    s.close()

def SerialThread():
    while True:
        serialAvailable()
        time.sleep(0.1)
def SocketServerThread():
    HOST, PORT = "", 8787
    # Create the server, binding to localhost on port 9999
    server = SocketServer.TCPServer((HOST, PORT), MyTCPHandler)

    # Activate the server; this will keep running until you
    # interrupt the program with Ctrl-C
    server.serve_forever()
    

try:
    SerialThreadObject = Thread(target=SerialThread, args=())
    SerialThreadObject.start()
    SerialThreadObject = Thread(target=SocketServerThread, args=())
    SerialThreadObject.start()
    SerialThreadObject = Thread(target=RecognitionThread, args=())
    SerialThreadObject.start()
    while True:
        print "I'm Alive..."
        time.sleep(30)
except:
    print "Error: an error has occured"




  
