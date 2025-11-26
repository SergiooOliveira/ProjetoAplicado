using FishNet.Broadcast;
using System.Collections.Generic;

public struct CreateRoomRequest : IBroadcast { }

public struct CreateRoomResponse : IBroadcast { public string code; }

public struct JoinRoomRequest : IBroadcast { public string code; }

public struct JoinRoomResponse : IBroadcast { public bool success; }

public struct PlayerListUpdate : IBroadcast { public List<string> playerIds; }