using FishNet.Connection;
using System.Collections.Generic;

public class LobbyRoom
{
    public string Code;
    public NetworkConnection host;
    public List<NetworkConnection> players;
}