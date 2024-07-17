using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Box : MonoBehaviour
    {
        public List<Element> Elements = new List<Element>();
        
        [Header("Visual")]
        [SerializeField] private Image _boxImage;
        [SerializeField] private Image _hammerImage;
        [SerializeField] private Sprite _boxUnlocked;
        [SerializeField] private Sprite _boxLocked;

        private bool _isCorrect;
        public bool IsLocked;

        public void Initialize(bool isLocked)
        { 
            IsLocked = isLocked;

            if (IsLocked)
            {
                _boxImage.sprite = _boxLocked;
                HideElements();
            }
            else
                _boxImage.sprite = _boxUnlocked;
        }
        public void InitializeELements(List<ElementTypes> types)
        {
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].Initialize(types[i], this);
        }
        public void HideElements()
        {
            foreach (var item in Elements)
                item.Hide();
        }
        public void GetAllDown()
        {
            foreach (var item in Elements)
                item.GetDown();
        }
        public void OnCorrect()
        {
            float delay = 0f;
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].AnimScaling(delay);
                delay += 0.1f;
            }
        }
        public void SetElement(Element element) => Elements.Add(element);
        public void RemoveElement(Element element) => Elements.Remove(element);
        public bool CheckBox()
        {
            if (IsLocked)
                return false;
            if (Elements.All(e => e.Type == Elements[0].Type))
            {
                if (!_isCorrect)
                {
                    Audio.Instance.PlaySound(Audio.Instance.Success);
                    OnCorrect();
                    _isCorrect = true;
                    BoxManager.Instance.RemoveFromAvaiable(this);
                }
                return true;
            }
            else
                return false;
        }
        public void Broke()
        { 
            Elements.Clear();
        }
        public void SortCorrect()
        {
            for (int i = 1; i < Elements.Count; i++)
            {
                if (!CheckBox())
                {
                    Box box = BoxManager.Instance.GetBoxWith(this, Elements[0].Type);
                    for (int b = 0; b < box.Elements.Count; b++)
                    {
                        if (box.Elements[b].Type == Elements[0].Type)
                        {
                            ElementTypes tempType = Elements[i].Type;
                            Elements[i].Initialize(Elements[0].Type, this);
                            box.Elements[b].Initialize(tempType, box);
                        }
                    }
                }
            }
            BoxManager.Instance.CheckBoxes();
        }
        public void ShowHammer()
        {
            _hammerImage.gameObject.SetActive(true);
        }
        public void HideHammer()
        {
            _hammerImage.gameObject.SetActive(false);
        }
        public void OnHammerClicked()
        {
            BoxManager.Instance.UseHammer(this);
        }
        public bool IsCorrect() => _isCorrect;
    }
}
