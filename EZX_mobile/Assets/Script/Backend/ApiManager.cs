using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiManager : MonoBehaviour
{
    #region Singleton implementation
    public static ApiManager Instance { set; get; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    public User user { get; set; }
    public ActivitiesList activitiesList { get; set; }
}
