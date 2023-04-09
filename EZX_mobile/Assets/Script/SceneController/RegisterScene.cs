using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RegisterScene : MonoBehaviour
{

    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField confirm_password;
    [SerializeField] private TMP_InputField weight;
    [SerializeField] private TMP_InputField height;
    [SerializeField] private TMP_InputField year;
    [SerializeField] private TMP_InputField month;
    [SerializeField] private TMP_InputField day;
    [SerializeField] private TMP_Text alertText;

    public void register()
    {
        StartCoroutine(ApiCall.register(
            username.text, password.text, confirm_password.text, int.Parse(weight.text), int.Parse(height.text),
            int.Parse(year.text), int.Parse(month.text), int.Parse(day.text),
        () =>
        {
            Debug.Log("on success");
            SceneManager.LoadScene("LoadingScene");
        },
        () =>
        {
            Debug.Log("on fail");
            alertText.text = "Please enter all field, or enter correct data.";
        }));
    }

    public void cancel()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
