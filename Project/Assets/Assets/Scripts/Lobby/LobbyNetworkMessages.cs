using FishNet.Transporting;
using FishNet.Managing;

public struct CreateRoomRequest { }
public struct CreateRoomResponse { public string code; }

public struct JoinRoomRequest { public string code; }
public struct JoinRoomResponse { public bool success; }