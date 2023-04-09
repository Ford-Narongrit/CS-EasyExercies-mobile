using System;
using Unity.Collections;
using UnityEngine;
using Unity.Networking.Transport;

public class Client : MonoBehaviour
{
    #region Singleton implementation
    public static Client Instance { set; get; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;

    public Action connectionDropped;

    // Methods
    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endPoint = NetworkEndPoint.Parse(ip, port);

        connection = driver.Connect(endPoint);

        Debug.Log("Attemping to connec to Server on " + endPoint.Address);

        isActive = true;

        RegisterToEvent();
    }
    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();
            connection = default(NetworkConnection);
            isActive = false;
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }

    public void Update()
    {
        if (!isActive)
            return;

        driver.ScheduleUpdate().Complete();
        CheckAlive();

        UpdateMessagePump();
    }
    private void CheckAlive() //check is server alive;
    {
        if (!connection.IsCreated && isActive)
        {
            Debug.Log("Something went wrong, lost connection to server.");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;

        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                SendToServer(new NetWelcome());
                Debug.Log("We're connected!!");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                // receive data
                NetUtility.OnData(stream, default(NetworkConnection));
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                Shutdown();
            }
        }

    }

    // Client specific
    public void SendToServer(NetworkMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

    // Event parsing
    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }
    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }
    private void OnKeepAlive(NetworkMessage nm)
    {
        // Send it back, to keep both side alive
        SendToServer(nm);
    }
}
