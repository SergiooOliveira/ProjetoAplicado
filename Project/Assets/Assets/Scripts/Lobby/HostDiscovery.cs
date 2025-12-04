using System.Net;
using System.Net.NetworkInformation;
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
        foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
        {
            var ipProps = ni.GetIPProperties();
            foreach (var ip in ipProps.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(ip.Address) &&
                    ip.Address.ToString().StartsWith("192.168.1")) // garante IP da LAN
                {
                    return ip.Address.ToString();
                }
            }
        }
        return "127.0.0.1";
    }
}