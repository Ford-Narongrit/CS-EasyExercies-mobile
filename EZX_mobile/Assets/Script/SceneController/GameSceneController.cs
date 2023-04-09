using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameSceneController : MonoBehaviour
{
    public void OnClickHome()
    {
        Client.Instance.SendToServer(new NetCancel());
        SceneManager.LoadScene("StartMenuScene");
    }
}
