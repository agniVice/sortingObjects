using UnityEngine;

namespace Gameplay
{
    public class UserBalance : MonoBehaviour
    {
        public static UserBalance Instance { get; private set; }

        public int Money { get; private set; }
        public int Hints { get; private set; }
        public int Hammers { get; private set; }

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            Initialize();
        }
        private void Initialize()
        {
            Money = PlayerPrefs.GetInt("Money", 100);
            Hints = PlayerPrefs.GetInt("Hints", 2);
            Hammers = PlayerPrefs.GetInt("Hammers", 2);
        }
        public void ChangeMoney(int count)
        {
            Money += count;
            Save();
        }
        public void ChangeHints(int count)
        {
            Hints += count;
            Save();
        }
        public void ChangeHammers(int count)
        {
            Hammers += count;
            Save();
        }
        private void Save()
        {
            PlayerPrefs.SetInt("Money", Money);
            PlayerPrefs.SetInt("Hints", Hints);
            PlayerPrefs.SetInt("Hammers", Hammers);
        }
    }

}
