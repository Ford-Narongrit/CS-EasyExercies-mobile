using Unity.Networking.Transport;
using UnityEngine;

public class NetPing : NetworkMessage
{
    public NetPing()
    {
        Code = OpCode.PING;
    }
    public NetPing(DataStreamReader reader)
    {
        Code = OpCode.PING;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        // We already read the byte in the NetUtility::ondata
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_PING?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_PING?.Invoke(this, cnn);
    }
}
