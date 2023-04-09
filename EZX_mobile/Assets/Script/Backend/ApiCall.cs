using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCall : MonoBehaviour
{
    #region Singleton implementation
    public static ApiCall Instance { set; get; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion
    // private static string URL = "http://cnc.cs.sci.ku.ac.th:9900/api";
    private static string URL = "http://localhost/api";

    internal static IEnumerator login(string username, string password, Action successAction, Action errorAction)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("username", username);
        formData.AddField("password", password);

        using (UnityWebRequest webRequset = UnityWebRequest.Post(URL + "/auth/login", formData))
        {
            yield return webRequset.SendWebRequest();

            switch (webRequset.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequset.error);
                    errorAction();
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error: " + webRequset.error);
                    errorAction();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequset.downloadHandler.text);

                    // set local user
                    User user = JsonUtility.FromJson<User>(webRequset.downloadHandler.text);
                    ApiManager.Instance.user = user;

                    successAction();
                    break;
            }
        }
    }
    internal static IEnumerator me(string access_token, Action successAction, Action errorAction)
    {
        WWWForm formData = new WWWForm();

        UnityWebRequest webRequset = UnityWebRequest.Post(URL + "/auth/me", formData);
        webRequset.SetRequestHeader("Accept", "application/json");
        webRequset.SetRequestHeader("Authorization", "Bearer " + access_token);
        yield return webRequset.SendWebRequest();

        switch (webRequset.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(webRequset.downloadHandler.text);

                // set local user
                User user = JsonUtility.FromJson<User>(webRequset.downloadHandler.text);
                ApiManager.Instance.user = user;

                successAction();
                break;
        }
    }

    internal static IEnumerator register(
        string username, string password, string password_confirmation, int weight, int height,
        int year, int month, int day,
        Action successAction, Action errorAction)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("username", username);
        formData.AddField("password", password);
        formData.AddField("password_confirmation", password_confirmation);
        formData.AddField("weight", weight);
        formData.AddField("height", height);
        formData.AddField("year", year);
        formData.AddField("month", month);
        formData.AddField("day", day);

        using (UnityWebRequest webRequset = UnityWebRequest.Post(URL + "/auth/register", formData))
        {
            yield return webRequset.SendWebRequest();

            switch (webRequset.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequset.error);
                    errorAction();
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error: " + webRequset.error);
                    errorAction();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequset.downloadHandler.text);
                    successAction();
                    break;
            }
        }
    }

    internal static IEnumerator storeRecord(int score, int burned_cal, int time, string name, string access_token,
    Action successAction, Action errorAction)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("name", name);
        formData.AddField("score", score);
        formData.AddField("burned_cal", burned_cal);
        formData.AddField("time", time);

        UnityWebRequest webRequset = UnityWebRequest.Post(URL + "/activity", formData);
        webRequset.SetRequestHeader("Accept", "application/json");
        webRequset.SetRequestHeader("Authorization", "Bearer " + access_token);

        yield return webRequset.SendWebRequest();

        switch (webRequset.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(webRequset.downloadHandler.text);
                successAction();
                break;
        }

    }



    [Serializable]
    public class ActivityResponse
    {
        public string date;
        public int burned_cal;
        public int time;
    }

    [Serializable]
    public class ActivitiesListResponse
    {
        public int score;
        public int time;
        public int burned_cal;
        public ActivityResponse[] activity;
    }
    internal static IEnumerator getRecord(string access_token, Action successAction, Action errorAction)
    {
        UnityWebRequest webRequset = UnityWebRequest.Get(URL + "/activity");
        webRequset.SetRequestHeader("Accept", "application/json");
        webRequset.SetRequestHeader("Authorization", "Bearer " + access_token);

        yield return webRequset.SendWebRequest();

        switch (webRequset.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Error: " + webRequset.error);
                errorAction();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(webRequset.downloadHandler.text);

                // set local user
                ActivitiesListResponse activitiesList = JsonUtility.FromJson<ActivitiesListResponse>(webRequset.downloadHandler.text);

                ApiManager.Instance.activitiesList = new ActivitiesList();
                ApiManager.Instance.activitiesList.burned_cal = activitiesList.burned_cal;
                ApiManager.Instance.activitiesList.score = activitiesList.score;
                ApiManager.Instance.activitiesList.time = activitiesList.time;
                foreach (ActivityResponse activity in activitiesList.activity)
                {
                    ApiManager.Instance.activitiesList.activity.Add(new Activity(activity.date, activity.burned_cal, activity.time));
                }

                successAction();
                break;
        }
    }

}
