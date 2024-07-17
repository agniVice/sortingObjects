using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance { get; private set; }

        [SerializeField] private CanvasGroup _shopGroup;
        [SerializeField] private List<Transform> _shopTransforms;

        private void Awake()
        {
            if(Instance != this && Instance != null)
                Destroy(gameObject); 
            else
                Instance = this;
        }
        public void OpenShop()
        {
            Timer.Instance.PauseTimer();
            _shopGroup.blocksRaycasts = true;
            _shopGroup.alpha = 0f;
            _shopGroup.DOFade(1, 0.2f).SetLink(_shopGroup.gameObject);

            float delay = 0.1f;
            foreach(Transform t in _shopTransforms) 
            {
                Vector3 startScale = t.localScale;
                t.localScale = Vector3.zero;
                t.DOScale(startScale, 0.15f).SetLink(t.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
                delay += 0.05f;
            }
        }
        public void OnCloseClicked()
        {
            _shopGroup.blocksRaycasts = false;
            _shopGroup.DOFade(0, 0.15f).SetLink(_shopGroup.gameObject);
            if(GameState.Instance.CurrentState != GameState.State.Finished)
                Timer.Instance.ContinueTimer();
        }
        public void OnBuyButtonClicked(int id)
        { 
            
        }
    }
}
