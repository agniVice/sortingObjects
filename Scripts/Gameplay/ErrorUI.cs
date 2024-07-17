using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class ErrorUI : MonoBehaviour
    {
        public static ErrorUI Instance { get; private set; }

        [SerializeField] private CanvasGroup _errorGroup;
        [SerializeField] private List<Transform> _errorTransforms;

        [SerializeField] private TextMeshProUGUI _moneyText;

        private void Awake()
        {
            if(Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void OpenError(int count)
        {
            Audio.Instance.PlaySound(Audio.Instance.Error, 0.8f);
            Timer.Instance.PauseTimer();
            _errorGroup.blocksRaycasts = true;
            _errorGroup.alpha = 0f;
            _errorGroup.DOFade(1, 0.2f).SetLink(_errorGroup.gameObject);
            _moneyText.text = count.ToString();

            float delay = 0.2f;
            foreach (var item in _errorTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
        }
        public void OnCloseClicked()
        {
            _errorGroup.blocksRaycasts = false;
            _errorGroup.DOFade(0, 0.2f).SetLink(_errorGroup.gameObject);
            Timer.Instance.ContinueTimer();
        }
        public void OnShopClicked()
        {
            OnCloseClicked();
            ShopUI.Instance.OpenShop();
        }
    }
}
