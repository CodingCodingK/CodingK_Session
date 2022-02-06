using System;
using System.Collections.Generic;
using CodingK_Session;
using ProtoBuf;

namespace test.Protocol
{
    /// <summary>
    /// 网络通信数据协议
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class NetMsg : CodingK_Msg
    {
        [ProtoMember(1)]
        public string info;
        [ProtoMember(2)]
        public CMD cmd;
        [ProtoMember(3)]
        public Ping ping;
        [ProtoMember(4)]
        public ReqLogin reqLogin;
        [ProtoMember(5)]
        public RspLogin rspLogin;
    }

    /// <summary>
    /// 心跳机制
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class Ping
    {
        [ProtoMember(1)]
        public bool isOver;
    }

    #region 业务数据

    [Serializable]
    [ProtoContract]
    public class ReqLogin
    {
        [ProtoMember(1)]
        public string acct;
        [ProtoMember(2)]
        public string psd;
    }

    [Serializable]
    [ProtoContract]
    public class RspLogin
    {
        [ProtoMember(1)]
        public List<LoginInfo> info;
    }

    [ProtoContract]
    [Serializable]
    public class LoginInfo
    {
        [ProtoMember(1)]
        public string lv;
        [ProtoMember(2)]
        public string exp;
        [ProtoMember(3)]
        public string money;
    }
    #endregion

    /// <summary>
    /// 本数据包传递的 业务类型
    /// </summary>
    [ProtoContract]
    public enum CMD
    {
        None,
        Ping,
        ReqLogin,
        RspLogin,
    }
}
