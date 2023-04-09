using Unity.Networking.Transport;

public class NetAcceler : NetworkMessage
{
    public float accel_x;
    public float accel_y;
    public float accel_z;
    public NetAcceler(float accel_x, float accel_y, float accel_z) // <-- Making the box
    {
        Code = OpCode.ACCEL;
        this.accel_x = accel_x;
        this.accel_y = accel_y;
        this.accel_z = accel_z;
    }

    public NetAcceler(DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.ACCEL;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteFloat(accel_x);
        writer.WriteFloat(accel_y);
        writer.WriteFloat(accel_z);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        accel_x = reader.ReadFloat();
        accel_y = reader.ReadFloat();
        accel_z = reader.ReadFloat();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_ACCEL?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_ACCEL?.Invoke(this, cnn);
    }
}
