using Gameplay;
using Menu;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
    [SerializeField] private int _dailyBonus;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private Image _locker;

    private const string LastLoginKey = "LastLoginDate";
    private const string StreakKey = "LoginStreak";

    private bool _isLocked;
    private int _streak;

    private DateTime _lastLoginDate;
    private DateTime _currentDate;

    private void Start()
    {
        _streak = PlayerPrefs.GetInt(StreakKey, 1);
        _count.text = (_dailyBonus * _streak).ToString();
        CheckDailyBonus();
    }

    private void CheckDailyBonus()
    {
        DateTime lastLoginDate = GetLastLoginDate();
        DateTime currentDate = DateTime.Now.Date;

        _lastLoginDate = lastLoginDate;
        _currentDate = currentDate;

        if (_lastLoginDate < _currentDate)
            UnLock();
        else
            Lock();
    }

    private DateTime GetLastLoginDate()
    {
        string lastLoginDateString = PlayerPrefs.GetString(LastLoginKey, string.Empty);

        if (DateTime.TryParse(lastLoginDateString, out DateTime lastLoginDate))
            return lastLoginDate;

        return DateTime.MinValue;
    }

    private void UnLock()
    {
        _isLocked = false;
        _locker.gameObject.SetActive(false);
    }

    private void Lock()
    {
        _isLocked = true;
        _locker.gameObject.SetActive(true);
    }

    public void OnGetClicked()
    {
        if (!_isLocked)
        {
            UserBalance.Instance.ChangeMoney(_dailyBonus * _streak);
            MenuUI.Instance.UpdateMoney();

            if (_lastLoginDate == _currentDate.AddDays(-1))
                _streak++;
            else
                _streak = 1;

            PlayerPrefs.SetInt(StreakKey, _streak);

            _currentDate = DateTime.Now.Date;
            PlayerPrefs.SetString(LastLoginKey, _currentDate.ToString());

            _count.text = (_dailyBonus * _streak).ToString();

            Lock();
        }
    }
}
