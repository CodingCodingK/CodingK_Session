using System;
using System.Collections.Generic;
using System.Text;
using CodingK_Session;
using proto.test;

namespace test.ClientSession
{
    /// <summary>
    /// 客户端Session连接 : KCPSession<数据协议>
    /// </summary>
    public class ClientSession : CodingK_Session<NetMsg>
    {
        protected override void OnDisConnected()
        {

        }

        protected override void OnConnected()
        {

        }

        protected override void OnUpDate(DateTime now)
        {

        }

        protected override void OnReceiveMsg(NetMsg msg)
        {
            if (msg.Cmd == CMD.RspLogin)
            {
                var datas = msg.RspLogin.Info[0];
                CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "From Server:Sid:{0}, Datas:{1} {2} {3}", m_sessionId, datas.Lv, datas.Exp, datas.Money);
            }
            else
            {
                CodingK_SessionTool.ColorLog(CodingK_LogColor.Magenta, "From Server:Sid:{0}, Msg:{1}", m_sessionId, msg.Info);
            }
        }
    }
}
