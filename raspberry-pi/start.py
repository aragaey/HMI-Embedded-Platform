from threading import Thread
import socket
import sys
import serial
import time

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

try:
    SerialThreadObject = Thread(target=SerialThread, args=())
    SerialThreadObject.start()
    while True:
        print "I'm Alive..."
        time.sleep(3)
except:
    print "Error: an error has occured"




  
