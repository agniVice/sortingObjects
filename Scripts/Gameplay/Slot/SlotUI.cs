using DG.Tweening;
using Gameplay;
using Global;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class SlotUI : MonoBehaviour
    {
        public static SlotUI Instance { get; private set; }

        [SerializeField] private CanvasGroup _slotGroup;
        [SerializeField] private List<Transform> _slotTransforms;

        [SerializeField] private CanvasGroup _winGroup;

        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private TextMeshProUGUI _mainMoney;
        [SerializeField] private TextMeshProUGUI _bet;
        [SerializeField] private TextMeshProUGUI _win;

        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;

        [SerializeField] private Image _spinImage;
        [SerializeField] private Button _spinButton;

        private int _startMoney;
        private int _currentMoney;

        private int _currentWin;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void OpenSlot()
        {
            UpdateMoney();
            UpdateBet();

            _slotGroup.blocksRaycasts = true;
            _slotGroup.alpha = 0f;
            _slotGroup.DOFade(1, 0.3f).SetLink(_slotGroup.gameObject);

            float delay = 0.2f;
            foreach (var item in _slotTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
        }
        public void OnMenuClicked()
        {
            CloseSlot();
            GlobalInterface.Instance.OpenMenu();
        }
        public void OnPlayClikced()
        {
            CloseSlot();
            GlobalInterface.Instance.OpenGame();
        }
        private void CloseSlot()
        {
            _slotGroup.blocksRaycasts = false;
            _slotGroup.DOFade(0, 0.3f).SetLink(_slotGroup.gameObject);
        }
        public void OnSpinClicked()
        {
            Machine.Instance.Spin();
            UpdateMoney();
        }
        public void OnDecreaseClicked()
        {
            Machine.Instance.DecreaseBet();
            UpdateBet();
        }
        public void OnIncreaseClicked()
        {
            Machine.Instance.IncreaseBet();
            UpdateBet();
        }
        public void OnAllInClicked()
        {
            Machine.Instance.AllIn();
            UpdateBet();
        }
        public void UpdateBet()
        {
            if (Machine.Instance.GetBetId() == 0)
                _decreaseButton.interactable = false;
            else
                _decreaseButton.interactable = true;
            if (Machine.Instance.GetBetId() == Machine.Instance.GetBets().Count - 1)
                _increaseButton.interactable = false;
            else
                _increaseButton.interactable = true;

            _bet.text = Machine.Instance.FinalBet.ToString();
            CheckSpinButton();
        }
        public void UpdateMoney()
        {
            _currentMoney = _startMoney;
            DOTween.To(() => _currentMoney, x => _currentMoney = x, UserBalance.Instance.Money, 1f).SetEase(Ease.Linear).OnUpdate(UpdateMoneyText);
        }
        private void UpdateMoneyText()
        {
            _money.text = _currentMoney.ToString();
            _mainMoney.text = _currentMoney.ToString();
            _startMoney = UserBalance.Instance.Money;
        }
        public void OnStartSpin()
        {
            _spinButton.interactable = false;
            _spinImage.transform.DORotate(new Vector3(0, 0, 540f), 2f, RotateMode.FastBeyond360).SetEase(Ease.InOutBack);
        }
        public void CheckSpinButton() => _spinButton.interactable = UserBalance.Instance.Money >= Machine.Instance.FinalBet;
        public void OnWin(int count)
        {
            UpdateMoney();
            _winGroup.DOFade(1, 0.2f).SetLink(_winGroup.gameObject);
            _winGroup.DOFade(0, 0.2f).SetLink(_winGroup.gameObject).SetDelay(1.2f);
            _currentWin = 0;
            DOTween.To(() => _currentWin, x => _currentWin = x, count, 1f).SetEase(Ease.Linear).OnUpdate(UpdateWinText);
        }
        public void UpdateWinText()
        {
            _win.text = "+" + _currentWin.ToString();
        }
    }
}
