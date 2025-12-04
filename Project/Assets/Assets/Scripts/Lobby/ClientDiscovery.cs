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
        try
        {
            udp = new UdpClient(listenPort);
            udp.BeginReceive(OnReceive, null);
            Debug.Log("[ClientDiscovery] Começando a ouvir broadcasts na LAN");
        }
        catch (Exception e)
        {
            Debug.LogError($"[ClientDiscovery] Erro ao iniciar UDP: {e.Message}");
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, listenPort);
        byte[] data;

        try
        {
            data = udp.EndReceive(ar, ref ep);
        }
        catch (Exception e)
        {
            Debug.LogError($"[ClientDiscovery] Erro ao receber UDP: {e.Message}");
            return;
        }

        string msg = Encoding.UTF8.GetString(data);
        Debug.Log($"[ClientDiscovery] Broadcast recebido de {ep.Address}: {msg}");

        string[] parts = msg.Split('|');
        if (parts.Length != 3)
        {
            Debug.LogWarning("[ClientDiscovery] Mensagem UDP inválida");
            udp.BeginReceive(OnReceive, null);
            return;
        }

        string code = parts[0];
        string ip = parts[1];
        int port = int.Parse(parts[2]);

        rooms[code] = (ip, port);

        Debug.Log($"[ClientDiscovery] Sala registrada: {code} => {ip}:{port}");

        udp.BeginReceive(OnReceive, null);
    }

    public bool TryGetRoom(string code, out string ip, out int port)
    {
        if (rooms.ContainsKey(code))
        {
            ip = rooms[code].ip;
            port = rooms[code].port;
            Debug.Log($"[ClientDiscovery] Sala encontrada: {code} => {ip}:{port}");
            return true;
        }

        ip = null;
        port = 0;
        Debug.Log($"[ClientDiscovery] Sala NÃO encontrada: {code}");
        return false;
    }
}