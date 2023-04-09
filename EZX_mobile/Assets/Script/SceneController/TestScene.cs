using System;
using UnityEngine;
using TMPro;

public class TestScene : MonoBehaviour
{
    [SerializeField] private Animator startSceneAnimator;

    [SerializeField] TMP_Text gyroX;
    [SerializeField] TMP_Text gyroY;
    [SerializeField] TMP_Text gyroZ;

    [SerializeField] TMP_Text accelX;
    [SerializeField] TMP_Text accelY;
    [SerializeField] TMP_Text accelZ;
    public void OnclickTest()
    {
        startSceneAnimator.SetTrigger("test");
    }

    public void ping()
    {
        Debug.Log("log click ping.");
        try
        {
            Client.Instance.SendToServer(new NetPing());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void Start()
    {
        GyroManager.Instance.EnableGyro();
        AccelerationManager.Instance.EnableAccel();
    }

    private void Update()
    {
        float gyro_x = GyroManager.Instance.GetGyroRotation().x;
        float gyro_y = GyroManager.Instance.GetGyroRotation().y;
        float gyro_z = GyroManager.Instance.GetGyroRotation().z;
        gyroX.text = "Gyro X: " + gyro_x;
        gyroY.text = "Gyro Y: " + gyro_y;
        gyroZ.text = "Gyro Z: " + gyro_z;


        float accel_x = AccelerationManager.Instance.GetAcceleration().x;
        float accel_y = AccelerationManager.Instance.GetAcceleration().y;
        float accel_z = AccelerationManager.Instance.GetAcceleration().z;
        accelX.text = "Accel X: " + accel_x;
        accelY.text = "Accel Y: " + accel_y;
        accelZ.text = "Accel Z: " + accel_z;

        try
        {
            Client.Instance.SendToServer(new NetGyro(gyro_x, gyro_y, gyro_z));
            Client.Instance.SendToServer(new NetAcceler(accel_x, accel_y, accel_z));
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Client not found, No connection.");
        }
    }
}
