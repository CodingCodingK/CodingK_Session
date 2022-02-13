using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodingK_Session;
using proto.test;
using UnityEngine;

public class Test : MonoBehaviour
{
    public const string ip = "127.0.0.1";
    public const int port = 17666;

    static CodingK_Net<ClientSession, NetMsg> client;
    static Task<bool> checkTask;
    
    void Start()
    {
        client = new CodingK_Net<ClientSession, NetMsg>();
        client.StartAsClient(ip, port, CodingK_ProtocolMode.Proto);
        checkTask = client.ConnectServer(200, 5000);

        Task.Run(ConnectCheck);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("VAR");
            client.CloseClient();
        }
        else if(Input.GetKeyDown(KeyCode.L))
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
                        Debug.Log( "ConnectServer Success.");
                        checkTask = null;
                        await Task.Run(SendPingMsg);
                    }
                    else
                    {
                        if (++linkCounter > 4)
                        {
                            Debug.Log("Connect failed too many times, Check your Network.");
                            checkTask = null;
                            break;
                        }
                        else
                        {
                            Debug.Log($"Connect failed {linkCounter} Times, Reconnecting...");
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

                    Debug.Log("Client Send Ping Msg.");
                }
                else
                {
                    Debug.Log("Ping Task Cancel.");
                    break;
                }
            }
        }
}
