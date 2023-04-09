using Unity.Networking.Transport;
using UnityEngine;

public class NetRun : NetworkMessage
{
    public NetRun()
    {
        Code = OpCode.RUN;
    }
    public NetRun(DataStreamReader reader)
    {
        Code = OpCode.RUN;
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
        NetUtility.C_RUN?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_RUN?.Invoke(this, cnn);
    }
}
