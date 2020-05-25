using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;

public class ClientSystem : MonoBehaviour
{
    static ClientSystem _instance;
    public static ClientSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<ClientSystem>();
            }
            return _instance;
        }
    }
    public Socket client;
    public bool blackAready = false;
    public bool whiteAready = false;
    private void Start()
    {
        whiteAready = true;
        SendMessage("0000001");
        ReceiveMessage();
    }
    public new async void SendMessage(string message)
    {
        try
        {
            byte[] data = Encoding.Default.GetBytes(message);
            int a = await client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
            if (a == 0)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                client.Dispose();
            }
            Array.Clear(data, 0, data.Length);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client.Dispose();
        }

    }
    public async void ReceiveMessage()
    {
        try
        {
            byte[] data = new byte[1024];
            int a = await client.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
            if (a == 0)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                client.Dispose();
            }
            DoDispose(data);
            Array.Clear(data, 0, data.Length);
            ReceiveMessage();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client.Dispose();
        }
    }
    public void DoDispose(byte[] data)
    {
        string message = Encoding.Default.GetString(data);
        //表示服务器已经准备好了
        if (message.Contains("0000000"))
        {
            ServerSystem.Instance.blackAready = true;

            return;
        }

        string[] array = message.Split(',');
        ChestBoard.Instacne.PlayChest(new int[2] { (int)(int.Parse(array[0]) + 7.5f), (int)(int.Parse(array[1]) + 7.45f) });
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

