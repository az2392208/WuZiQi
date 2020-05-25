using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    //判断是服务器还是客户端
    public void Start()
    {
        string address = "192.168.0.1";
        int port = 8899;
        //这是一个服务券监听用的
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
        try
        {
            clientSocket.ConnectAsync(iPEndPoint);
            Debug.Log("连接上了服务器");
            //作为客户端运行
            ClientSystem client = gameObject.AddComponent<ClientSystem>();
            client.client = clientSocket;
        }
        catch (System.Exception e)
        {
            //作为服务器运行
            Debug.Log(e.Message);
            ServerSystem server = gameObject.AddComponent<ServerSystem>();
        }
    }
}
