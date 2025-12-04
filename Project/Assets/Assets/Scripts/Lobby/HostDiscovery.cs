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

        Debug.Log($"[HostDiscovery] Começando a broadcast da sala {roomCode}");

        InvokeRepeating(nameof(Broadcast), 0f, broadcastInterval);
    }

    private void Broadcast()
    {
        var msg = $"{roomCode}|{GetLocalIPAddress()}|{7777}";
        var data = Encoding.UTF8.GetBytes(msg);

        udp.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));

        Debug.Log($"[HostDiscovery] Broadcast enviado: {msg}");
    }

    private void OnDestroy()
    {
        if (udp != null) udp.Close();
    }

    private string GetLocalIPAddress()
    {
        foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
        {
            // Ignora interfaces inativas ou loopback
            if (ni.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up ||
                ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
                continue;

            var ipProps = ni.GetIPProperties();
            foreach (var ip in ipProps.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    // Apenas IPs privados da LAN (192.168.x.x ou 10.x.x.x ou 172.16-31.x.x)
                    byte[] bytes = ip.Address.GetAddressBytes();
                    if ((bytes[0] == 192 && bytes[1] == 168) ||
                        (bytes[0] == 10) ||
                        (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31))
                    {
                        Debug.Log($"[HostDiscovery] IP local correto da LAN detectado: {ip.Address}");
                        return ip.Address.ToString();
                    }
                }
            }
        }
        return "127.0.0.1"; // fallback
    }
}