using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Animator startSceneAnimator;

    [SerializeField] private TMP_Text username_text;
    [SerializeField] private TMP_Text info_user;

    private void Awake()
    {
        username_text.text = ApiManager.Instance.user.username;
        info_user.text = ApiManager.Instance.user.weight + " Kg/ " + ApiManager.Instance.user.height + " cm/ age" + ApiManager.Instance.user.age;
    }
    public void OnClickPlay()
    {
        startSceneAnimator.SetTrigger("play");
    }

    public void OnClickSetting()
    {
        startSceneAnimator.SetTrigger("setting");
    }

    public void OnClickHealth()
    {
        startSceneAnimator.SetTrigger("health");
    }

    public void OnClickQRcode()
    {
        SceneManager.LoadScene("QRcodeConnectScene");
    }

    public void OnClickRun()
    {
        Client.Instance.SendToServer(new NetRun());
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickSquat()
    {
        Client.Instance.SendToServer(new NetSquat());
        SceneManager.LoadScene("GameScene");
    }

    public void OnDisconnected()
    {
        ApiManager.Instance.user = null;
        SceneManager.LoadScene("LoadingScene");
    }
}
