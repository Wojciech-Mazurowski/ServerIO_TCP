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

        /// <summary>
        /// Konstruktor inicjalizujacy server
        /// </summary>
        /// <param name="ip">server ip</param>
        /// <param name="port">server port</param>
        public ServerTCP(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
            LoginServiceS.CheckFile();
        }

        /// <summary>
        /// Funckja pozwalajaca łączyć się klientom 
        /// </summary>
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
        /// <summary>
        /// Funkcja zamykajaca polączenie z użytkownikiem
        /// </summary>
        /// <param name="ar"></param>
        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.Close();
        }
        /// <summary>
        /// Funkcja odpowiadajaca za pierwszą wymiane danych z użytkownikiem, pytająca czy użytkownik chce sie zalogowac czy zarejestrowac
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        private void BeginDataTransmission(NetworkStream stream)
        {
            buffer = new byte[_data_length];
          
            string anwser;
            string login;
            string pass;
            string message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
            message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            while (true)
            {
                //do
                //{
                //    ReceivedDataLength = stream.Read(buffer, 0, _data_length);
                //} while ((anwser = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n" || (anwser = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r");

                while (stream.DataAvailable)
                {
                    ReceivedDataLength = stream.Read(buffer, 0, _data_length);
                    anwser = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength);

                    if (anwser[0] == '1')
                    {

                        LoginServiceS.LoginHandle(stream);
                    }
                    else if (anwser[0] == '2')
                    {
                        LoginServiceS.RegisterHandle(stream);
                    }
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
