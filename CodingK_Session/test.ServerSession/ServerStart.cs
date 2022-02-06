using System;
using CodingK_Session;
using test.Protocol;

namespace test.ServerSession
{
    /// <summary>
    /// 控制台模拟服务端
    /// </summary>
    class ServerStart
    {
        public const string ip = "127.0.0.1";
        public const int port = 17666;

        static CodingK_Net<ServerSession, NetMsg> server;

        static void Main(string[] args)
        {
            server = new CodingK_Net<ServerSession, NetMsg>();
            server.StartAsServer(ip, port, CodingK_ProtocolMode.Proto);

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "quit")
                {
                    server.CloseServer();
                    break;
                }
                else
                {
                    server.BroadCastMsg(new NetMsg { info = input });
                }
            }

            Console.ReadKey();
        }
    }
}
