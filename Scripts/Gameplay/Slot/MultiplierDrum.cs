using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public class MultiplierDrum : MonoBehaviour
    {
        public static MultiplierDrum Instance { get; private set; }
        
        public bool IsSpinning;

        [SerializeField] private List<MultiplierElement> _elements;
        [SerializeField] private List<Sprite> _elementSprites;
        [SerializeField] private List<Vector3> _positions;

        [SerializeField] private float _spinSpeed;

        private int _countOfSpins;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            Instance = this;
        }
        private void Start()
        {
            foreach (var element in _elements)
            {
                int randomType = Random.Range(0, 4);
                element.Initialize(this);
                element.SetType((MultiplierType)randomType, _elementSprites[randomType]);
            }
        }
        private void FixedUpdate()
        {
            if (!IsSpinning)
                return;

            foreach (var element in _elements)
                element.transform.position += Vector3.right * Time.fixedDeltaTime * _spinSpeed;
        }
        public void Spin()
        {
            _countOfSpins = 0;
            IsSpinning = true;
        }
        public void OnElementSpin(MultiplierElement element)
        {
            _countOfSpins++;
            if (_countOfSpins % 5 == 0)
                Audio.Instance.PlaySound(Audio.Instance.DrumSpin);
            _elements.Remove(element);
            element.transform.localPosition = _elements[0].transform.localPosition + new Vector3(-114, 0);
            _elements.Insert(0, element);
            int randomType = Random.Range(0, 4);
            _elements[0].SetType((MultiplierType)randomType, _elementSprites[randomType]);
            if (_countOfSpins == 21)
            {
                IsSpinning = false;
                Machine.Instance.OnMultiplierDrumEndSpin(_elements[3].GetMainType());
                for (int i = 0; i < _elements.Count; i++)
                    _elements[i].transform.DOLocalMove(_positions[i], 0.5f).SetLink(_elements[i].gameObject).SetEase(Ease.OutBack);
            }
        }
    }
}
