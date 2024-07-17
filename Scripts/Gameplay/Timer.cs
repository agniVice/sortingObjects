using UnityEngine;

namespace Gameplay
{
    public class Timer : MonoBehaviour
    {
        public static Timer Instance { get; private set; }
        public bool IsTimerWork { get; private set; }

        private float _startTime;
        private float _currentTime;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        private void FixedUpdate()
        {
            if (!IsTimerWork)
                return;

            if (GameState.Instance.CurrentState == GameState.State.Finished)
                return;
            if (_currentTime > 0)
                _currentTime -= Time.fixedDeltaTime;
            else
                GameState.Instance.OnGameFail();
        }
        public void StartTimer(float time)
        {
            GameState.Instance.ChangeState(GameState.State.InGame);
            _startTime = time;
            _currentTime = _startTime;
            IsTimerWork = true;
        }
        public void StopTimer()
        {
            GameState.Instance.ChangeState(GameState.State.OutGame);
            _currentTime = 0;
            IsTimerWork = false;
        }
        public void ContinueTimer()
        {
            if(GameState.Instance.CurrentState == GameState.State.InGame)
                IsTimerWork = true;
        }
        public void PauseTimer()
        {
            if(GameState.Instance.CurrentState != GameState.State.Finished)
                GameState.Instance.ChangeState(GameState.State.InGame);
            IsTimerWork = false;
        }
        public float CurrentTime() => _currentTime;
    }
}