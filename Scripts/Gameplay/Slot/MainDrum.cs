using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public class MainDrum : MonoBehaviour
    {
        public bool IsSpinning;

        [SerializeField] private float _spinSpeed;
        [SerializeField] private List<MainElement> _elements;

        [SerializeField] private List<Vector3> _positions;

        [SerializeField] private List<Sprite> _elementSprites;

        private float _spinDelay;
        private int _countOfSpins;

        private bool _isWinGame;
        private MainType _winType;

        private void Start()
        {
            foreach (var element in _elements)
            {
                int randomType = Random.Range(0, 19);
                element.Initialize(this);
                element.SetType((MainType)randomType, _elementSprites[randomType]);
            }
        }
        private void FixedUpdate()
        {
            if (!IsSpinning)
                return;

            foreach (var element in _elements)
                element.transform.position += Vector3.right * Time.fixedDeltaTime * _spinSpeed;
        }
        public void SpinWithDelay(float delay = 0, bool isWinGame = false, MainType type = MainType.Seven)
        {
            _isWinGame = isWinGame;
            if (_isWinGame)
                _winType = type;

            _spinDelay = delay;
            StartCoroutine(Spin());
        }
        private IEnumerator Spin()
        { 
            yield return new WaitForSeconds(_spinDelay);
            _countOfSpins = 0;
            IsSpinning = true;
        }
        public void OnElementSpin(MainElement element)
        {
            if(_countOfSpins % 9 == 0)
                Audio.Instance.PlaySound(Audio.Instance.DrumSpin);
            _countOfSpins++;
            _elements.Remove(element);
            element.transform.localPosition = _elements[0].transform.localPosition + new Vector3(-114, 0);
            _elements.Insert(0, element);
            int randomType = Random.Range(0, 19);
            _elements[0].SetType((MainType)randomType, _elementSprites[randomType]);
            if (_countOfSpins == 18 && _isWinGame)
            {
                _elements[0].SetType(_winType, _elementSprites[(int)_winType]);
                _isWinGame = false;
            }
            if (_countOfSpins == 21)
            {
                IsSpinning = false;
                Machine.Instance.OnDrumEndSpin(this);
                for (int i = 0; i < _elements.Count; i++)
                    _elements[i].transform.DOLocalMove(_positions[i], 0.15f).SetLink(_elements[i].gameObject);
            }
        }
        public MainType GetMainType() => _elements[3].GetMainType();
    }
}
