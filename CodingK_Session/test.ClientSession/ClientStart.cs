using System;
using System.Threading.Tasks;
using CodingK_Session;
using proto.test;

namespace test.ClientSession
{
    /// <summary>
    /// 控制台模拟客户端
    /// </summary>
    class ClientStart
    {
        public const string ip = "127.0.0.1";
        public const int port = 17666;

        static CodingK_Net<ClientSession, NetMsg> client;
        static Task<bool> checkTask;

        static void Main(string[] args)
        {
            client = new CodingK_Net<ClientSession, NetMsg>();
            client.StartAsClient(ip, port, CodingK_ProtocolMode.Proto);
            checkTask = client.ConnectServer(200, 5000);

            Task.Run(ConnectCheck);

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "quit")
                {
                    client.CloseClient();
                    break;
                }
                else if (input == "login")
                {
                    client.clientSession.SendMsg(new NetMsg
                    {
                        Cmd = CMD.ReqLogin,
                        ReqLogin = new ReqLogin
                        {
                            Acct = "test1",
                            Psd = "test2",
                        }
                    });
                }
                else
                {
                    client.clientSession.SendMsg(new NetMsg
                    {
                        Info = input,
                    });
                }
            }

            Console.ReadKey();
        }

        private static int linkCounter;
        static async Task ConnectCheck()
        {
            while (true)
            {
                await Task.Delay(3000);
                if (checkTask != null && checkTask.IsCompleted)
                {
                    if (checkTask.Result)
                    {
                        CodingK_SessionTool.ColorLog(CodingK_LogColor.Green, "ConnectServer Success.");
                        checkTask = null;
                        await Task.Run(SendPingMsg);
                    }
                    else
                    {
                        if (++linkCounter > 4)
                        {
                            CodingK_SessionTool.Error("Connect failed too many times, Check your Network.");
                            checkTask = null;
                            break;
                        }
                        else
                        {
                            CodingK_SessionTool.Error($"Connect failed {linkCounter} Times, Reconnecting...");
                            checkTask = client.ConnectServer(200, 500);
                        }
                    }
                }

            }
        }

        static async Task SendPingMsg()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (client?.clientSession != null)
                {
                    client.clientSession.SendMsg(new NetMsg
                    {
                        Cmd = CMD.Ping,
                        Ping = new proto.test.Ping
                        {
                            IsOver = false,
                        }
                    });

                    CodingK_SessionTool.ColorLog(CodingK_LogColor.Green, "Client Send Ping Msg.");
                }
                else
                {
                    CodingK_SessionTool.ColorLog(CodingK_LogColor.Green, "Ping Task Cancel.");
                    break;
                }
            }
        }
    }
}
