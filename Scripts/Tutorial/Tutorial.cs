using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Instance { get; private set; }

        [SerializeField] private CanvasGroup _tutorialGroup;
        [SerializeField] private Image _correctImage;
        [SerializeField] private CanvasGroup _closeGroup;

        [SerializeField] private List<Transform> _elements;

        [SerializeField] private List<Vector3> _endPositions;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void CompleteElements()
        {
            Timer.Instance.PauseTimer();
            _correctImage.fillAmount = 0f;
            _tutorialGroup.blocksRaycasts = true;
            _tutorialGroup.DOFade(1, 0.1f).SetLink(_tutorialGroup.gameObject);

            for (int i = 0; i < _elements.Count; i++)
                _elements[i].DOLocalMove(_endPositions[i], 0.3f).SetLink(_elements[i].gameObject).SetDelay(1f);
            _correctImage.DOFillAmount(1, 0.3f).SetLink(_correctImage.gameObject).SetDelay(1.3f);
            _closeGroup.DOFade(1, 0.3f).SetLink(_closeGroup.gameObject).SetDelay(1.6f).OnKill(() => {
                _closeGroup.GetComponent<Button>().interactable = true;
            });
        }
        public void CloseTutorial()
        {
            _tutorialGroup.blocksRaycasts = false;
            _tutorialGroup.DOFade(0, 0.3f).SetLink(_tutorialGroup.gameObject);
            PlayerPrefs.SetInt("TutorialComplete", 1);
            Timer.Instance.ContinueTimer();
        }
    }
}
