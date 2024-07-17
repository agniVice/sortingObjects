using UnityEngine;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        public static Level Instance { get; private set; }

        [SerializeField] private int _levelTime;
        [SerializeField] private int _rewardCount;
        [SerializeField] private int _rewardBoost;
        [SerializeField] private int _boxCount;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        private void Start()
        {
            Initialize();
        }
        private void Initialize()
        {
            BoxManager.Instance.Initialize(_boxCount);

            StartLevel();
        }
        public void StartLevel()
        {
            GameState.Instance.ChangeState(GameState.State.InGame);
            Timer.Instance.StartTimer(_levelTime);
            if (PlayerPrefs.GetInt("TutorialComplete", 0) == 0)
                Tutorial.Instance.CompleteElements();
        }
        public void OnLevelSuccess()
        {
            PlayerPrefs.SetInt("CurrentLevel", LevelManager.Instance.LevelId + 1);
            if (LevelManager.Instance.LevelId < 8)
                UserBalance.Instance.ChangeMoney(_rewardCount);
            else
                UserBalance.Instance.ChangeMoney(_rewardCount + _rewardBoost * (LevelManager.Instance.LevelId-7));
            SortingUI.Instance.UpdateMoney();
        }
        public void OnLevelFailed()
        { 
            
        }
        public int GetReward()
        {
            if (LevelManager.Instance.LevelId < 8)
                return _rewardCount;
            else
                return _rewardCount + _rewardBoost * (LevelManager.Instance.LevelId - 7);
        }
    }
}
