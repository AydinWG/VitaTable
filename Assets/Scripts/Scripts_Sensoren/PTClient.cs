using PTClient;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PTClient
{
    public static class Global
    {
        public static Mutex mutexCIN = new Mutex();
        public static Mutex mutexCOUT = new Mutex();
        public static string globalMessageIN = "";
        public static string globalMessageOUT = "Hallo vom *** CLIENT ***";
    }
    class PTClient
    {
        static void Main(string[] args)
        {
            try
            {
                //TCP
                //System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
                //byte[] inStream = new byte[1024];
                //StringBuilder myCompleteMessage = new StringBuilder();
                //int numberOfBytesRead = 0;

                //Console.WriteLine("Client Started");
                //clientSocket.Connect("127.0.0.1", 8181);
                //Console.WriteLine(" >> " + "Connect to 127.0.0.1:8181");
                //NetworkStream serverStream = clientSocket.GetStream();

                //UDP
                Thread udpCThread = new Thread(udpThread);
                udpCThread.Priority = ThreadPriority.Highest;
                udpCThread.Start();


                //while (true)
                //{
                //    Global.mutexCOUT.WaitOne();
                //    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Global.globalMessageOUT);
                //    serverStream.Write(outStream, 0, outStream.Length);
                //    Global.mutexCOUT.ReleaseMutex();
                //    serverStream.Flush();

                //    if (serverStream.CanRead)
                //    {

                //        do
                //        {
                //            numberOfBytesRead = serverStream.Read(outStream, 0, outStream.Length);
                //            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(outStream, 0, numberOfBytesRead));
                //        }
                //        while (serverStream.DataAvailable);


                //        Global.mutexCIN.WaitOne();
                //        Global.globalMessageIN = myCompleteMessage.ToString();
                //        Global.mutexCIN.ReleaseMutex();

                //        Console.WriteLine(" >> " + "From Client :  " + Global.globalMessageIN);
                //        myCompleteMessage.Clear();
                //    }
                //}

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private static void udpThread()
        {

            //UDP
            int listenPort = 11000;
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    //TODO in Spiele einbinden
                    byte[] bytes = listener.Receive(ref groupEP);

                    //Console.WriteLine($" Received broadcast from {groupEP} :");
                    Console.WriteLine($"Empfangen: {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }

            finally
            {
                listener.Close();
            }

        }
    }
}
