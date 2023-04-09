using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine;

public class NetUser : NetworkMessage
{
    public FixedString4096Bytes access_token;
    public NetUser(FixedString4096Bytes access_token)
    {
        Code = OpCode.USER;
        this.access_token = access_token;
    }
    public NetUser(DataStreamReader reader)
    {
        Code = OpCode.USER;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteFixedString4096(access_token);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        access_token = reader.ReadFixedString4096();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_USER?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_USER?.Invoke(this, cnn);
    }
}
