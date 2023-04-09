using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ConnectScene : MonoBehaviour
{
    [SerializeField] private QRScanner qRScanner;
    [SerializeField] private Button connectBtn;
    private ushort port = 8007;
    private bool isConnected = false;

    private void Update()
    {
        if (qRScanner.result != null)
        {
            connectBtn.gameObject.SetActive(true);
        }
        else
        {
            connectBtn.gameObject.SetActive(false);
        }
    }

    public void OnConnectToServer()
    {
        isConnected = true;
        Client.Instance.Init(qRScanner.result, port);

        RegisterEvents();
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene("StartMenuScene");
    }

    #region 
    private void RegisterEvents()
    {
        NetUtility.C_WELCOME += OnWelcomeClient;
    }
    private void UnRegisterEvents()
    {
        NetUtility.C_WELCOME -= OnWelcomeClient;
        isConnected = false;
    }
    private void OnWelcomeClient(NetworkMessage msg)
    {
        // Receive the connection message
        NetWelcome netWelcome = msg as NetWelcome;
        Debug.Log("Welcome");
        Client.Instance.SendToServer(new NetUser(ApiManager.Instance.user.access_token));
        SceneManager.LoadScene("StartMenuScene");
    }
    #endregion
}
