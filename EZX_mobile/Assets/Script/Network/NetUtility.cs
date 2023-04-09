using System;
using Unity.Networking.Transport;
using UnityEngine;

public enum OpCode // make this enum in other file.
{
    KEEP_ALIVE = 1,
    WELCOME = 2,
    PING = 3,
    GYRO = 4,
    ACCEL = 5,
    CONFIRM = 6,
    CANCEL = 7,
    RUN = 8,
    SQUAT = 9,
    USER = 10,
}

public static class NetUtility
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetworkMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.KEEP_ALIVE: msg = new NetKeepAlive(stream); break;
            case OpCode.WELCOME: msg = new NetWelcome(stream); break;
            case OpCode.PING: msg = new NetPing(stream); break;
            case OpCode.GYRO: msg = new NetGyro(stream); break;
            case OpCode.ACCEL: msg = new NetAcceler(stream); break;
            case OpCode.CONFIRM: msg = new NetConfirm(stream); break;
            case OpCode.CANCEL: msg = new NetCancel(stream); break;
            case OpCode.RUN: msg = new NetRun(stream); break;
            case OpCode.SQUAT: msg = new NetSquat(stream); break;
            case OpCode.USER: msg = new NetUser(stream); break;

            default:
                Debug.LogError("Message received had no OpCode");
                break;
        }

        if (server != null)
        {
            msg.ReceivedOnServer(cnn);
        }
        else
        {
            msg.ReceivedOnClient();
        }
    }

    // Network message (action)
    public static Action<NetworkMessage> C_KEEP_ALIVE;
    public static Action<NetworkMessage> C_WELCOME;
    public static Action<NetworkMessage> C_PING;
    public static Action<NetworkMessage> C_GYRO;
    public static Action<NetworkMessage> C_ACCEL;
    public static Action<NetworkMessage> C_CONFIRM;
    public static Action<NetworkMessage> C_CANCEL;
    public static Action<NetworkMessage> C_RUN;
    public static Action<NetworkMessage> C_SQUAT;
    public static Action<NetworkMessage> C_USER;
    public static Action<NetworkMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetworkMessage, NetworkConnection> S_WELCOME;
    public static Action<NetworkMessage, NetworkConnection> S_PING;
    public static Action<NetworkMessage, NetworkConnection> S_GYRO;
    public static Action<NetworkMessage, NetworkConnection> S_ACCEL;
    public static Action<NetworkMessage, NetworkConnection> S_CONFIRM;
    public static Action<NetworkMessage, NetworkConnection> S_CANCEL;
    public static Action<NetworkMessage, NetworkConnection> S_RUN;
    public static Action<NetworkMessage, NetworkConnection> S_SQUAT;
    public static Action<NetworkMessage, NetworkConnection> S_USER;
}
