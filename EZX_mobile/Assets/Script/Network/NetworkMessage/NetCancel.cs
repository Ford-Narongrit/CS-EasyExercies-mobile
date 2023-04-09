using Unity.Networking.Transport;
using UnityEngine;

public class NetCancel : NetworkMessage
{
    public NetCancel()
    {
        Code = OpCode.CANCEL;
    }
    public NetCancel(DataStreamReader reader)
    {
        Code = OpCode.CANCEL;
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
        NetUtility.C_CANCEL?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_CANCEL?.Invoke(this, cnn);
    }
}
