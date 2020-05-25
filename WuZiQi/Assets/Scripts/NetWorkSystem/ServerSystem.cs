using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;

public class ServerSystem : MonoBehaviour
{

    private static ServerSystem _instance;
    public static ServerSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<ServerSystem>();
            }
            return _instance;
        }
    }
    private ServerSystem()
    {

    }
    //设置开启服务器
    string address = "192.168.0.1";
    int port = 8899;
    //这是一个服务券监听用的
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public List<Socket> lSockets = new List<Socket>();
    public bool blackAready = false;
    public bool whiteAready = false;
    public void Start()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(10);
        WaitConnect();

    }
    /// <summary>
    /// 等到客户端的连接,创建一个用于通讯的装用服务器
    /// </summary>
    public async void WaitConnect()
    {
        try
        {
            Debug.Log("等待连接中...");
            ListenerSocket listenerSocket = new ListenerSocket();
            listenerSocket.listener = await serverSocket.AcceptAsync();
            Debug.Log("连接到了");
            blackAready = true;
            listenerSocket.SendMessage("0000000");
            //将已经连接到客户端的监听放到同一个list集合中去进行管理
            lSockets.Add(listenerSocket.listener);
            listenerSocket.ReceiveMessage();
            //等待下一个客户端连接
            WaitConnect();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
            serverSocket.Dispose();
        }
    }
    private void Update()
    {
        if (blackAready && whiteAready)
        {
            Debug.Log("可以开始下棋了");
            blackAready = false;
            whiteAready = false;
        }
    }
}
