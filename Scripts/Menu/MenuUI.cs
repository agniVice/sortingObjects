using DG.Tweening;
using Gameplay;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuUI : MonoBehaviour
    {
        public static MenuUI Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private Image[] _navButtons;
        [SerializeField] private Sprite[] _navSprites;
        [SerializeField] private CanvasGroup[] _groups;

        [SerializeField] private Image _musicImage;
        [SerializeField] private Image _soundImage;

        [SerializeField] private Sprite _musicEnabled;
        [SerializeField] private Sprite _musicDisabled;

        [SerializeField] private Sprite _soundEnabled;
        [SerializeField] private Sprite _soundDisabled;

        private int _currentMenu;
        private int _startMoney;
        private int _currentMoney;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        private void Start()
        {
            UpdateMoney();
        }
        public void UpdateNav()
        {
            for (int i = 0; i < _navButtons.Length; i++)
            {
                if (i == _currentMenu)
                    _navButtons[i].sprite = _navSprites[i + 3];
                else
                    _navButtons[i].sprite = _navSprites[i];
            }
        }
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
        public void UpdateSettings()
        {
            if (Audio.Instance.IsMusicEnabled)
                _musicImage.sprite = _musicEnabled;
            else
                _musicImage.sprite = _musicDisabled;

            if (Audio.Instance.IsSoundEnabled)
                _soundImage.sprite = _soundEnabled;
            else
                _soundImage.sprite = _soundDisabled;
        }
        public void SetMenu(int id)
        {
            _currentMenu = id;
            UpdateNav();
            UpdateMenus();
        }
        public void UpdateMenus()
        {
            for (int i = 0; i < _groups.Length; i++)
            {
                if (i == _currentMenu)
                {
                    _groups[i].blocksRaycasts = true;
                    _groups[i].alpha = 0f;
                    _groups[i].DOFade(1, 0.3f).SetLink(_groups[i].gameObject);
                }
                else
                {
                    _groups[i].blocksRaycasts = false;
                    _groups[i].alpha = 0f;
                }
            }
        }
        public void OnPlayClicked() => GlobalInterface.Instance.OpenGame();
        public void OnSoundClicked()
        {
            Audio.Instance.OnSoundToggle();
            UpdateSettings();
        }
        public void OnMusicClicked()
        {
            Audio.Instance.OnMusicToggle();
            UpdateSettings();
        }
        public void OnExitClicked() => Application.Quit();
    }
}
