using System;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class ListenerSocket
{
    public Socket listener;
    public async void SendMessage(string message)
    {
        try
        {
            byte[] data = Encoding.Default.GetBytes(message);
            int a = await listener.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
            if (a == 0)
            {
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
            Array.Clear(data, 0, data.Length);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }


    }
    public async void ReceiveMessage()
    {
        try
        {
            byte[] data = new byte[1024];
            int a = await listener.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
            if (a == 0)
            {
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
            DoDispose(data);
            Array.Clear(data, 0, data.Length);
            ReceiveMessage();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }
    }

    //解析消息
    public void DoDispose(byte[] data)
    {
        string message = Encoding.Default.GetString(data);
        //表示客户端已经连接上了
        if(message.Contains("0000001"))
        {
            ClientSystem.Instance.whiteAready = true;
            return;
        }
        string[] array = message.Split(',');
        ChestBoard.Instacne.PlayChest(new int[2] { (int)(int.Parse(array[0]) + 7.5f), (int)(int.Parse(array[1]) + 7.45f) });
    }
}
