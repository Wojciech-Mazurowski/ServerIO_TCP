using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibServerTCP;
using ServerComunication;

namespace serveratcp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerCommunication server = new ServerCommunication(IPAddress.Parse("127.0.0.1"), 6000);
            server.Start();

        }
    }
}
