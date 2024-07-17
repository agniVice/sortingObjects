using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class BoostersUI : MonoBehaviour
    {
        public static BoostersUI Instance { get; private set; }

        [SerializeField] private CanvasGroup _boostersGroup;
        [SerializeField] private List<Transform> _boostersTransforms;

        [SerializeField] private List<TextMeshProUGUI> _pricesTexts;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void OpenBoosters()
        {
            Timer.Instance.PauseTimer();
            _boostersGroup.blocksRaycasts = true;
            _boostersGroup.alpha = 0f;
            _boostersGroup.DOFade(1, 0.2f).SetLink(_boostersGroup.gameObject);

            float delay = 0.2f;
            foreach (Transform item in _boostersTransforms)
            {
                Vector3 startScale = item.localScale;
                item.localScale = Vector3.zero;
                item.DOScale(startScale, 0.15f).SetLink(item.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
            for (int i = 0; i < Boosters.Instance.GetPriceCount(); i++)
                _pricesTexts[i].text = Boosters.Instance.GetPrice(i).ToString();
        }
        public void OnCloseClicked()
        {
            Timer.Instance.ContinueTimer();
            _boostersGroup.blocksRaycasts = false;
            _boostersGroup.DOFade(0, 0.2f).SetLink(_boostersGroup.gameObject);
        }
        public void OnShopClikced()
        {
            OnCloseClicked();
            ShopUI.Instance.OpenShop();
        }
        public void OnBuyBooster(int id)
        {
            Boosters.Instance.BuyBooster(id);
        }
    }
}
