//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Bright.Serialization;

namespace proto.test
{

    public  sealed class ReqLogin :  Bright.Serialization.BeanBase 
    {
        public ReqLogin()
        {
        }

        public ReqLogin(Bright.Common.NotNullInitialization _) 
        {
            Acct = "";
            Psd = "";
        }

        public static void SerializeReqLogin(ByteBuf _buf, ReqLogin x)
        {
            x.Serialize(_buf);
        }

        public static ReqLogin DeserializeReqLogin(ByteBuf _buf)
        {
            var x = new test.ReqLogin();
            x.Deserialize(_buf);
            return x;
        }

         public string Acct;

         public string Psd;


        public const int __ID__ = 0;
        public override int GetTypeId() => __ID__;

        public override void Serialize(ByteBuf _buf)
        {
            _buf.WriteString(Acct);
            _buf.WriteString(Psd);
        }

        public override void Deserialize(ByteBuf _buf)
        {
            Acct = _buf.ReadString();
            Psd = _buf.ReadString();
        }

        public override string ToString()
        {
            return "test.ReqLogin{ "
            + "Acct:" + Acct + ","
            + "Psd:" + Psd + ","
            + "}";
        }
    }

}