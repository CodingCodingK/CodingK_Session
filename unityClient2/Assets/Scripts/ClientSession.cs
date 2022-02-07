using System;
using CodingK_Session;
using test.Protocol;
using UnityEngine;

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
        if (msg.cmd == CMD.RspLogin)
        {
            var datas = msg.rspLogin.info[0];
            Debug.Log(string.Format("From Server:Sid:{0}, Datas:{1} {2} {3}", m_sessionId, datas.lv, datas.exp, datas.money));
        }
        else
        {
            Debug.Log(string.Format("From Server:Sid:{0}, Msg:{1}", m_sessionId, msg.info));
        }
    }
}