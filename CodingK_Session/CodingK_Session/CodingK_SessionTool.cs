using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace CodingK_Session
{
    public static class CodingK_SessionTool
    {
        #region For Unity
        public static Action<string> LogFunc;
        public static Action<string> WarnFunc;
        public static Action<string> ErrorFunc;
        public static Action<CodingK_LogColor, string> ColorLogFunc;
        #endregion

        public static void SetSessionMode()
        {

        }

        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (LogFunc != null)
            {
                LogFunc.Invoke(msg);
            }
            else
            {
                ConsoleLog(msg, CodingK_LogColor.None);
            }
        }

        public static void Warn(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (WarnFunc != null)
            {
                WarnFunc.Invoke(msg);
            }
            else
            {
                ConsoleLog(msg, CodingK_LogColor.Yellow);
            }
        }
        public static void Error(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ErrorFunc != null)
            {
                ErrorFunc.Invoke(msg);
            }
            else
            {
                ConsoleLog(msg, CodingK_LogColor.Red);
            }
        }

        public static void ColorLog(CodingK_LogColor color, string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ColorLogFunc != null)
            {
                ColorLogFunc.Invoke(color, msg);
            }
            else
            {
                ConsoleLog(msg, color);
            }
        }

        private static void ConsoleLog(string msg, CodingK_LogColor color)
        {
            int tid = Thread.CurrentThread.ManagedThreadId;
            msg = string.Format("Thread:{0} {1}", tid, msg);

            switch (color)
            {
                case CodingK_LogColor.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CodingK_LogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CodingK_LogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case CodingK_LogColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case CodingK_LogColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case CodingK_LogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case CodingK_LogColor.None:
                default:
                    break;

            }

            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static byte[] ProtoSerialize<T>(T msg) where T : CodingK_Msg
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    ProtoBuf.Serializer.Serialize(ms, msg);
                    bytes = new byte[ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, bytes, 0, (int)ms.Length);
                }
                catch (SerializationException se)
                {
                    Error("Failed to protoSerialized: {0}", se.Message);
                    throw se;
                }
                
            }

            return bytes;
        }

        public static T ProtoDeSerialize<T>(byte[] bytes) where T : CodingK_Msg
        {
            T msg;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    msg = ProtoBuf.Serializer.Deserialize<T>(ms);
                }
                catch (Exception e)
                {
                    Error("Failed to protoDeserialized: {0}. bytesLength: {1}", e.Message, bytes.Length);
                    throw e;
                }
            }

            return msg;
        }

        public static byte[] Serialize<T>(T msg) where T : CodingK_Msg
        {
            
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, msg);
                    ms.Seek(0, SeekOrigin.Begin);
                    return Compress(ms.ToArray());
                }
                catch (SerializationException se)
                {
                    Error("Failed to serialized: {0}", se.Message);
                    throw se;
                }
            }
        }

        public static T DeSerialize<T>(byte[] bytes) where T : CodingK_Msg
        {
            bytes = DeCompress(bytes);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    T msg = (T)bf.Deserialize(ms);
                    return msg;
                }
                catch (Exception e)
                {
                    Error("Failed to Deserialized: {0}. bytesLength: {1}", e.Message, bytes.Length);
                    throw e;
                }
            }
        }

        private static byte[] Compress(byte[] input)
        {
            using (MemoryStream outMS = new MemoryStream())
            {
                using (GZipStream gzs = new GZipStream(outMS, CompressionMode.Compress, true))
                {
                    gzs.Write(input, 0, input.Length);
                    gzs.Close();
                    return outMS.ToArray();
                }
            }
        }

        private static byte[] DeCompress(byte[] input)
        {
            using (MemoryStream inputMS = new MemoryStream(input))
            {
                using (MemoryStream outputMS = new MemoryStream())
                {
                    using (GZipStream gzs = new GZipStream(inputMS, CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[1024];
                        int len = 0;
                        while ((len = gzs.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            outputMS.Write(bytes, 0, len);
                        }

                        gzs.Close();
                        return outputMS.ToArray();
                    }
                }
            }
        }

        private static readonly DateTime utcStart = new DateTime(1970, 1, 1);
        public static ulong GetUTCStartMilliseconds()
        {
            return (ulong)(DateTime.UtcNow - utcStart).TotalMilliseconds;
        }
    }

    public enum CodingK_LogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public enum CodingK_ProtocolMode
    {
        Normal,
        Proto,
    }
}
