using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfflineEarningsManager : MonoBehaviour
{
    [SerializeField] private float earningsPerSecond = 2f;
    [SerializeField] private GameObject earningsPanel;
    [SerializeField] private TextMeshProUGUI earningsTMP;

    public float TotalSecond { get; set; }

    private float _earnings;
    private readonly string OFFLINE_TIME_KEY = "MyGame_Earnings";

    void Start()
    {
        CalculateTimeOffline();
        CalculateEarnings();
        ShowEarnings();
    }

    public void CollectEarnings()
    {
        GoldManager.Instance.AddGold(_earnings);
        TotalSecond = 0;
        earningsPanel.SetActive(false);
        PlayerPrefs.DeleteKey(OFFLINE_TIME_KEY);
    }

    private void CalculateEarnings()
    {
        if (TotalSecond > 0)
        {
            _earnings = TotalSecond * earningsPerSecond;
        }
    }

    private void ShowEarnings()
    {
        if (TotalSecond > 0)
        {
            earningsPanel.SetActive(true);
            earningsTMP.text = _earnings.ToCurrency();
        }
    }

    private void CalculateTimeOffline()
    {
        string storedTime = PlayerPrefs.GetString(OFFLINE_TIME_KEY, string.Empty);
        if (!string.IsNullOrEmpty(storedTime))
        {
            DateTime elapsedTime = DateTime.FromBinary(Convert.ToInt64(storedTime));
            TimeSpan currentTime = DateTime.Now.Subtract(elapsedTime);
            TotalSecond = Mathf.FloorToInt((float)currentTime.TotalSeconds);
            Debug.Log("TotalSecond:" + TotalSecond);
        }
    }

    private void SaveLastPlayTime()
    {
        PlayerPrefs.SetString(OFFLINE_TIME_KEY, DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    void OnDisable()
    {
        SaveLastPlayTime();
    }
}
