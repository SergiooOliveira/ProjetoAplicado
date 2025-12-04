using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class HostDiscovery : MonoBehaviour
{
    private UdpClient udp;
    private int port = 8888;
    private float broadcastInterval = 1f;
    private string roomCode;

    public void StartBroadcasting(string code)
    {
        roomCode = code;
        udp = new UdpClient();
        udp.EnableBroadcast = true;

        InvokeRepeating(nameof(Broadcast), 0f, broadcastInterval);
    }

    private void Broadcast()
    {
        var msg = $"{roomCode}|{GetLocalIPAddress()}|{7777}";
        var data = Encoding.UTF8.GetBytes(msg);

        udp.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));
    }

    private void OnDestroy()
    {
        if (udp != null) udp.Close();
    }

    private string GetLocalIPAddress()
    {
        foreach (var ni in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ni.AddressFamily == AddressFamily.InterNetwork)
                return ni.ToString();
        }
        return "127.0.0.1";
    }
}