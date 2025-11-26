using FishNet.Broadcast;

public struct CreateRoomRequest : IBroadcast
{
}

public struct CreateRoomResponse : IBroadcast
{
    public string code;
}

public struct JoinRoomRequest : IBroadcast
{
    public string code;
}

public struct JoinRoomResponse : IBroadcast
{
    public bool success;
}