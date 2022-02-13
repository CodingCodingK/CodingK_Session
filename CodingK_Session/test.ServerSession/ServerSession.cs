using System;
using System.Collections.Generic;
using System.Text;
using CodingK_Session;
using proto.test;

namespace test.ServerSession
{
    /// <summary>
    /// 服务端Session连接 : KCPSession<数据协议>
    /// </summary>
    public class ServerSession : CodingK_Session<NetMsg>
    {
        protected override void OnDisConnected()
        {
            CodingK_SessionTool.Warn("Client Offline {0}", m_sessionId);
        }

        protected override void OnConnected()
        {
            CodingK_SessionTool.ColorLog(CodingK_LogColor.Green, "Client Online {0}", m_sessionId);
        }

        private int checkCounter;
        private DateTime checkTime = DateTime.UtcNow.AddSeconds(5);
        protected override void OnUpDate(DateTime now)
        {
            if (now > checkTime)
            {
                checkTime = now.AddSeconds(5);
                checkCounter++;
                if (checkCounter > 3)
                {
                    NetMsg pingMsg = new NetMsg
                    {
                        Cmd = CMD.Ping,
                        Ping = new proto.test.Ping { IsOver = true },
                    };
                    // 3次心跳检测超时，本地模拟关闭消息
                    CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "PING IS OVER BEFORE");
                    OnReceiveMsg(pingMsg);
                }
            }
        }

        protected override void OnReceiveMsg(NetMsg msg)
        {
            CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "Get Msg from client. Sid:{0}, CMD:{1}, Msg:{2}", m_sessionId, msg.Cmd, msg.Info);

            if (msg.Cmd == CMD.Ping)
            {
                if (msg.Ping.IsOver)
                {
                    CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "PING IS OVER");
                    // 在本地执行清楚Session的操作
                    CloseSession();
                }
                else
                {
                    CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "PingCounter = 0;");
                    // 收到ping请求，重置检查计数
                    checkCounter = 0;
                    var pingMsg = new NetMsg
                    {
                        Cmd = CMD.Ping,
                        Ping = new proto.test.Ping { IsOver = false },
                        Info = "ping test",
                    };
                    // 对对应的Client Session发送msg
                    SendMsg(pingMsg);
                }
            }
            else if (msg.Cmd == CMD.ReqLogin)
            {
                var datas = msg.ReqLogin;
                CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "Client Login:" + datas.Acct + " " + datas.Psd);

                var infoMsg = new NetMsg
                {
                    Cmd = CMD.RspLogin,
                    RspLogin = new RspLogin
                    {
                        Info = new List<LoginInfo>(),
                    }
                };
                infoMsg.RspLogin.Info.Add(new LoginInfo
                {
                    Exp = "10",
                    Lv = "1",
                    Money = "110",
                });
                // 对对应的Client Session发送msg
                SendMsg(infoMsg);
            }
        }
    }
}
