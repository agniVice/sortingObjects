using UnityEngine;

namespace Gameplay
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }
        public enum State
        {
            InGame,
            OutGame,
            Finished
        }
        public State CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void ChangeState(State state) => CurrentState = state;
        public void OnGameSuccess()
        {
            Audio.Instance.PlaySound(Audio.Instance.Win);
            Timer.Instance.StopTimer();
            SortingUI.Instance.OnSuccess();
            Level.Instance.OnLevelSuccess();
            CurrentState = State.Finished;
        }
        public void OnGameFail() 
        {
            Audio.Instance.PlaySound(Audio.Instance.Lose);
            Timer.Instance.StopTimer();
            SortingUI.Instance.OnFail();
            CurrentState = State.Finished;
        }
    }
}
