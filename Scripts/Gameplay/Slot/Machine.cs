using Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slot
{
    public class Machine : MonoBehaviour
    {
        public static Machine Instance { get; private set; }

        [SerializeField] private List<MainDrum> _drums;
        [SerializeField] private MultiplierDrum _multiplierDrum;

        public int FinalBet { get; private set; }

        [SerializeField] private List<int> _bets;
        [SerializeField] private float _winChance;
        [SerializeField] private int _multiplierWin;

        private bool _isSpinning;
        private int _currentBetId;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            FinalBet = _bets[_currentBetId];
        }
        public void Spin()
        {
            if (_isSpinning)
                return;
            if (UserBalance.Instance.Money < FinalBet)
                return;

            Audio.Instance.PlaySound(Audio.Instance.Spin, 0.8f);
            SlotUI.Instance.OnStartSpin();
            UserBalance.Instance.ChangeMoney(-FinalBet);
            _isSpinning = true;
            float delay = 0f;
            MainType randomType = (MainType)Random.Range(0, 19);
            
            bool winGame = IsWinGame();

            _multiplierDrum.Spin();
            foreach (var item in _drums)
            {
                if (winGame)
                    item.SpinWithDelay(delay, true, randomType);
                else
                    item.SpinWithDelay(delay);
                delay += 0.05f;
            }
        }
        public bool IsWinGame()
        {
            int random = Random.Range(0, 100);
            return random <= _winChance;
        }
        public void OnMultiplierDrumEndSpin(MultiplierType type)
        {
            switch(type) 
            {
                case MultiplierType.One:
                    _multiplierWin = 1;
                    break;
                case MultiplierType.Two:
                    _multiplierWin = 2;
                    break;
                case MultiplierType.Three:
                    _multiplierWin = 3;
                    break;
                case MultiplierType.Five:
                    _multiplierWin = 5;
                    break;
            }
        }
        public void OnDrumEndSpin(MainDrum drum)
        {
            bool endSpinning = true;
            foreach (var item in _drums)
            {
                if (item.IsSpinning)
                    endSpinning = false;
            }
            if (endSpinning)
            {
                if (CheckWin())
                { 
                    StartCoroutine(EndSpinWithDelay(0.5f));
                    UserBalance.Instance.ChangeMoney(FinalBet * _multiplierWin);
                    SlotUI.Instance.OnWin(FinalBet * _multiplierWin);
                    Audio.Instance.PlaySound(Audio.Instance.SlotWin, 0.8f);
                }
                else
                    StartCoroutine(EndSpinWithDelay(0f));

                SlotUI.Instance.CheckSpinButton();
            }
        }
        private IEnumerator EndSpinWithDelay(float delay = 0f)
        { 
            yield return new WaitForSeconds(delay);
            _isSpinning = false;
        }
        public void IncreaseBet()
        {
            if (_currentBetId < _bets.Count - 1)
                _currentBetId++;
            FinalBet = _bets[_currentBetId];
        }
        public void DecreaseBet()
        {
            if (_currentBetId > 0)
                _currentBetId--;
            FinalBet = _bets[_currentBetId];
        }
        public void AllIn()
        {
            FinalBet = UserBalance.Instance.Money;
        }
        public List<int> GetBets() => _bets;
        public int GetBetId() => _currentBetId;
        private bool CheckWin() => _drums.All(drum => drum.GetMainType() == _drums[0].GetMainType());
    }
}
