using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_EP
{
    public class Serial
    {
        SerialPort MySerialPort;
        bool is_connected = false;
        string portName; int baudRate; Parity parity; int dataBits; StopBits stopBits;
        public Serial(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.portName = portName; this.baudRate = baudRate; this.parity = parity; this.dataBits = dataBits; this.stopBits = stopBits;
            SerialConnect();
            //MySerialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            //MySerialPort.Open();
            //MySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //is_connected = true;
        }
        public void SerialConnect()
        {
            MySerialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            MySerialPort.Open();
            MySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            is_connected = true;
            Console.WriteLine("Connected to serial");
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //parse data
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if(indata.Length > 0)
            Console.WriteLine("data: " + indata);
        }

        public void SerialDisconnect()
        {
            if (MySerialPort.IsOpen)
                MySerialPort.Close();
            is_connected = false;
        }
        public void SerialSend(String s)
        {
         if (!SerialIsConnected())
                SerialConnect();
            MySerialPort.Write(s);
        }
        public bool SerialIsConnected()
        {
            return MySerialPort.IsOpen;
        }
    }
}
