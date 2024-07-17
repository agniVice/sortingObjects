using DG.Tweening;
using Global;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class SortingUI : MonoBehaviour
    {
        public static SortingUI Instance { get; private set; }

        [Header("General")]
        [SerializeField] private CanvasGroup _sortingGroup;
        [SerializeField] private List<Transform> _sortingTransforms;

        [Space]
        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _hintsCount;
        [SerializeField] private TextMeshProUGUI _hammersCount;
        [SerializeField] private TextMeshProUGUI _levelNumber;

        [SerializeField] private GameObject _hammerButton;
        [SerializeField] private GameObject _closeHammerButton;

        [Header("SuccessWindow")]
        [SerializeField] private CanvasGroup _successGroup;
        [SerializeField] private List<Transform> _successTransforms;
        [SerializeField] private TextMeshProUGUI _rewardText;

        [Header("FailWindow")]
        [SerializeField] private CanvasGroup _failGroup;
        [SerializeField] private List<Transform> _failTransforms;

        private int _startMoney;
        private int _currentMoney;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        private void FixedUpdate()
        {
            if (Timer.Instance.IsTimerWork)
                _timer.text = Mathf.Round(Timer.Instance.CurrentTime()).ToString();
        }
        public void OpenGame()
        {
            LevelManager.Instance.SpawnLevel();
            _sortingGroup.alpha = 0f;
            _sortingGroup.blocksRaycasts = true;
            _sortingGroup.DOFade(1, 0.3f).SetLink(_sortingGroup.gameObject);

            float delay = 0.2f;
            foreach (var item in _sortingTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }

            _timer.text = Timer.Instance.CurrentTime().ToString();
            UpdateMoney();
            UpdateHammers();
            UpdateHints();
        }
        public void UpdateLevelNumber(int number) => _levelNumber.text = "LEVEL " + (number+1);
        public void UpdateMoney()
        {
            _currentMoney = _startMoney;
            DOTween.To(() => _currentMoney, x => _currentMoney = x, UserBalance.Instance.Money, 1f).SetEase(Ease.Linear).OnUpdate(UpdateMoneyText);
        }
        private void UpdateMoneyText()
        {
            _money.text = _currentMoney.ToString();
            _startMoney = UserBalance.Instance.Money;
        }
        public void OnShopClicked() => ShopUI.Instance.OpenShop();
        public void OnHummerClicker()
        {
            if (UserBalance.Instance.Hammers > 0)
            {
                _hammerButton.gameObject.SetActive(false);
                _closeHammerButton.SetActive(true);
                BoxManager.Instance.OnHummerClicked();
            }
            else
                BoostersUI.Instance.OpenBoosters();
            UpdateHammers();
        }
        public void OnCloseHammerSelectionClicked() => BoxManager.Instance.OnCloseHammerClicked();
        public void HideHammerSelection()
        { 
            _hammerButton.gameObject.SetActive(true);
            _closeHammerButton.SetActive(false);
        }
        public void OnHintClicked()
        {
            if (UserBalance.Instance.Hints > 0)
                BoxManager.Instance.UseHint();
            else
                BoostersUI.Instance.OpenBoosters();
            UpdateHints();
        }
        public void OnAddBoostClicked()
        {
            BoostersUI.Instance.OpenBoosters();
        }
        public void UpdateBoosters()
        {
            UpdateHints();
            UpdateHammers();
        }
        public void OnSuccess()
        {
            _rewardText.text = Level.Instance.GetReward().ToString();
            _successGroup.blocksRaycasts = true;
            _successGroup.alpha = 0f;
            _successGroup.DOFade(1, 0.3f).SetLink(_successGroup.gameObject);

            float delay = 0.1f;
            foreach (var item in _successTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
        }
        public void OnFail()
        {
            _failGroup.blocksRaycasts = true;
            _failGroup.alpha = 0f;
            _failGroup.DOFade(1, 0.3f).SetLink(_failGroup.gameObject);

            float delay = 0.1f;
            foreach (var item in _failTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
        }
        public void CloseMenus()
        {
            _failGroup.blocksRaycasts = false;
            _successGroup.blocksRaycasts = false;
            _failGroup.DOFade(0, 0.2f).SetLink(_failGroup.gameObject);
            _successGroup.DOFade(0, 0.2f).SetLink(_successGroup.gameObject);
        }
        public void OnBonusClicked()
        {
            CloseMenus();
            GlobalInterface.Instance.OpenSlot();
        }
        public void OnRetryClicked()
        {
            CloseMenus();
            LevelManager.Instance.SpawnLevel();
        }
        public void CloseGame()
        {
            _sortingGroup.blocksRaycasts = false;
            _sortingGroup.DOFade(0, 0.3f).SetLink(_sortingGroup.gameObject);
        }
        private void UpdateTimer() => _timer.text = Timer.Instance.CurrentTime().ToString();
        private void UpdateHints() => _hintsCount.text = UserBalance.Instance.Hints.ToString();
        private void UpdateHammers() => _hammersCount.text = UserBalance.Instance.Hammers.ToString();
    }
}
