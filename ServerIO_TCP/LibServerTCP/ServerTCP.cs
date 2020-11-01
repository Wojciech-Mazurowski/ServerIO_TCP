using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LoginService;


namespace LibServerTCP
{
    public class ServerTCP
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);

        TcpListener _tcpListener;
        NetworkStream _stream;
        IPAddress _ip;
        int _port;
        int _data_length = 1024;
        byte[] buffer;
        int ReceivedDataLength;

        public ServerTCP(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
            LoginServiceS.CheckFile();
        }


        public void Start()
        {
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                _stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                transmissionDelegate.BeginInvoke(_stream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.Close();
        }

        private void BeginDataTransmission(NetworkStream stream)
        {
            buffer = new byte[_data_length];
            string message = "Wpisz 1 aby sie zalogowac \r\n2 zeby sie zarejestrowac \r\n";
            string anwser;
            string login;
            string pass;
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            while (true)
            {
                ReceivedDataLength = stream.Read(buffer, 0, _data_length);
                anwser = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength);
                if (anwser == "1")
                {

                    LoginServiceS.LoginHandle(stream);
                }
                else if (anwser == "2")
                {
                    LoginServiceS.RegisterHandle(stream);
                }
                else
                {
                    message = "Wpisz 1 aby sie zalogowac \r\n2 zeby sie zarejestrowac \r\n ";
                    stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                }



            }

        }

        public void Run()
        {
            _tcpListener = new TcpListener(_ip, _port);
            _tcpListener.Start();

            Start();
        }

    }
}
