using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Sockets.Kcp;
using System.Text;

namespace CodingK_Session
{
    /// <summary>
    /// KCP数据处理器
    /// </summary>
    public class KCPHandle : IKcpCallback
    {
        public Action<Memory<byte>> Out;
        public Action<byte[]> Recv;

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            using (buffer)
            {
                Out(buffer.Memory.Slice(0, avalidLength));
            }
        }

        public void Receive(byte[] buffer)
        {
            Recv?.Invoke(buffer);
        }
    }
}
