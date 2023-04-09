using Unity.Networking.Transport;
using UnityEngine;

public class NetSquat : NetworkMessage
{
    public NetSquat()
    {
        Code = OpCode.SQUAT;
    }
    public NetSquat(DataStreamReader reader)
    {
        Code = OpCode.SQUAT;
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
        NetUtility.C_SQUAT?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_SQUAT?.Invoke(this, cnn);
    }
}
