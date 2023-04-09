using Unity.Networking.Transport;

public class NetGyro : NetworkMessage
{
    public float gyro_x;
    public float gyro_y;
    public float gyro_z;
    public NetGyro(float gyro_x, float gyro_y, float gyro_z) // <-- Making the box
    {
        Code = OpCode.GYRO;
        this.gyro_x = gyro_x;
        this.gyro_y = gyro_y;
        this.gyro_z = gyro_z;
    }

    public NetGyro(DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.GYRO;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteFloat(gyro_x);
        writer.WriteFloat(gyro_y);
        writer.WriteFloat(gyro_z);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        gyro_x = reader.ReadFloat();
        gyro_y = reader.ReadFloat();
        gyro_z = reader.ReadFloat();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_GYRO?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_GYRO?.Invoke(this, cnn);
    }
}
