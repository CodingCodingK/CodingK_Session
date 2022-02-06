# CodingK_Session

UDP/TCP C-S Session powered by proto, KCP, PELog on .net standard 2.0.3, Kcp 2.3.0, protobuf-net 3.9.62 .

基于KCPNet进行进一步优化+适应游戏开发环境的 **服务端 - 客户端 解决方案**。

1. 通过KCP算法实现可靠UDP通信。

2. 可选择使用Protobuf来实现底层序列化反序列化。

项目中含有 .net core服务端 和 .net framework客户端 的测试demo。

本人博客(CodingCodingK.top) 中有proto笔记、UDP笔记，以及github有自制proto协议代码批量生成工具（wpf）。

# 使用

## 1.定义协议

需要继承自`CodingK_Msg`。

## 2.分别定义 Client 和 Server 的Session

需要继承自`CodingK_Session< 1中定义的协议 >`。

## 3.分别在服务端和客户端启用

持有 `CodingK_Net < 2中的Session, 1中的协议 >`。再调用 CodingK_Net 中的API：

`public void StartAsClient(string ip, int port, CodingK_ProtocolMode protocolMode)`

`public void StartAsServer(string ip, int port, CodingK_ProtocolMode protocolMode)`

在启用时可以选择使用普通C#原生（压缩反压缩优化），或者Proto协议。**推荐Proto协议，实测效果显著，即使上了压缩优化，.Net Serialize下最好情况仍然比Proto字节数消耗高十倍以上。**

```csharp
// 服务端
private CodingK_Net<ServerSession, NetMsg> server;
...
server = new CodingK_Net<ServerSession, NetMsg>();
server.StartAsServer(ip, port, CodingK_ProtocolMode.Proto); // 使用Proto协议
...

// 客户端
private CodingK_Net<ClientSession, NetMsg> client;
... 
client = new CodingK_Net<ClientSession, NetMsg>();
client.StartAsClient(ip, port, CodingK_ProtocolMode.Proto); // 使用Proto协议
checkTask = client.ConnectServer(200, 5000); // 连接服务器，返回 Task<Bool> 来确认是否连接成功
...
```

