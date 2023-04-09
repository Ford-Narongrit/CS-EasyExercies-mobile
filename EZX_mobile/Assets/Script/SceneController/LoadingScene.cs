using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text alertText;

    private bool _isReadytoGo = false;

    void Update()
    {
        if (_isReadytoGo)
        {
            SceneManager.LoadScene("StartMenuScene");
        }
    }

    public void login()
    {
        StartCoroutine(ApiCall.login(username.text, password.text,
        () =>
        {
            Debug.Log("on success");
            _isReadytoGo = true;
        },
        () =>
        {
            Debug.Log("on fail");
            alertText.text = "fail to login username or password is incorrect.";
        }));
    }

    public void register()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}
