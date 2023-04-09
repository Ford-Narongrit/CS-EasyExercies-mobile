using UnityEngine;

public class GyroManager : MonoBehaviour
{
    #region Singleton implementation
    public static GyroManager Instance { set; get; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [Header("Logic")]
    private Gyroscope gyroscope;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro()
    {
        // Already activated
        if (gyroActive)
            return;

        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
            gyroActive = gyroscope.enabled;
        }
        else
        {
            Debug.LogError("Gyro is not supported on this device.");
        }
    }

    private void Update()
    {
        if (gyroActive)
        {
            rotation = gyroscope.attitude;
            Debug.Log("gyroscope rotation is : " + rotation);
        }
    }

    public Quaternion GetGyroRotation()
    {
        return rotation;
    }
}
