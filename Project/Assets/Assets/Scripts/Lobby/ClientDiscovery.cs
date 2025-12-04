using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientDiscovery : MonoBehaviour
{
    private UdpClient udp;
    private int listenPort = 8888;

    public Dictionary<string, (string ip, int port)> rooms =
        new Dictionary<string, (string, int)>();

    private void Start()
    {
        udp = new UdpClient(listenPort);
        udp.BeginReceive(OnReceive, null);
    }

    private void OnReceive(IAsyncResult ar)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, listenPort);
        byte[] data = udp.EndReceive(ar, ref ep);

        string msg = Encoding.UTF8.GetString(data);
        string[] parts = msg.Split('|');

        string code = parts[0];
        string ip = parts[1];
        int port = int.Parse(parts[2]);

        rooms[code] = (ip, port);

        udp.BeginReceive(OnReceive, null);
    }

    public bool TryGetRoom(string code, out string ip, out int port)
    {
        if (rooms.ContainsKey(code))
        {
            ip = rooms[code].ip;
            port = rooms[code].port;
            return true;
        }

        ip = null;
        port = 0;
        return false;
    }
}