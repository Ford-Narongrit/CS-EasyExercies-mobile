using Unity.Networking.Transport;
using UnityEngine;

public class NetConfirm : NetworkMessage
{
    public NetConfirm()
    {
        Code = OpCode.CONFIRM;
    }
    public NetConfirm(DataStreamReader reader)
    {
        Code = OpCode.CONFIRM;
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
        NetUtility.C_CONFIRM?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_CONFIRM?.Invoke(this, cnn);
    }
}
