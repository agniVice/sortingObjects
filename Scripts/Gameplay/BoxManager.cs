using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class BoxManager : MonoBehaviour
    {
        public static BoxManager Instance { get; private set; }

        [SerializeField] private List<Box> _boxes = new List<Box>();

        [SerializeField] private List<ElementTypes> _allTypes;

        [SerializeField] private List<Sprite> _elementSprites;

        private List<Box> _avaiableBoxes = new List<Box>();

        public Element TargetElement;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void Initialize(int boxCount)
        {
            _avaiableBoxes.Clear();
            for (int i = 0; i < boxCount; i++)
                _avaiableBoxes.Add(_boxes[i]);

            InitializeBoxes();
        }
        public void InitializeBoxes()
        {
            foreach (Box box in _boxes)
            {
                if (_avaiableBoxes.Contains(box))
                    box.Initialize(false);
                else
                    box.Initialize(true);
            }
            List<ElementTypes> availableTypes = _allTypes.ToList();

            List<ElementTypes> allAvaiable = new List<ElementTypes>();
            for (int i = 0; i < _avaiableBoxes.Count; i++)
            {
                if (i < availableTypes.Count)
                    allAvaiable.Add(availableTypes[i]);
                else
                    allAvaiable.Add(availableTypes[i % availableTypes.Count]);
            }
            availableTypes.Clear();
            availableTypes = allAvaiable;
           
            List<ElementTypes> elementTypes = new List<ElementTypes>();
            for (int i = 0; i < availableTypes.Count; i++)
            {
                for (int b = 0; b < 3; b++)
                    elementTypes.Add(availableTypes[i]);
            }

            List<ElementTypes> boxElements = new List<ElementTypes>();
            for (int i = 0; i < _avaiableBoxes.Count; i++)
            {
                for(int b = 0; b < 3; b++)
                {
                    ElementTypes type = elementTypes[UnityEngine.Random.Range(0, elementTypes.Count)];
                    boxElements.Add(type);
                    elementTypes.Remove(type);
                }
                _avaiableBoxes[i].InitializeELements(boxElements);
                boxElements.Clear();
            }
            CheckBoxes();
        }
        public Sprite GetElementSprite(int id) => _elementSprites[id];
        public void GetAllDown()
        {
            foreach (var item in _avaiableBoxes)
                item.GetAllDown();
        }
        public void CheckBoxes()
        {
            bool completed = true;
            foreach (var item in _boxes)
            { 
                if(!item.CheckBox() && !item.IsLocked)
                    completed = false;
            }
            if (completed)
                GameState.Instance.OnGameSuccess();
        }
        public void UseHint()
        {
            Box box = _avaiableBoxes[UnityEngine.Random.Range(0, _avaiableBoxes.Count)];
            
            while (box.CheckBox())
                box = _avaiableBoxes[UnityEngine.Random.Range(0, _avaiableBoxes.Count)];

            box.SortCorrect();
            UserBalance.Instance.ChangeHints(-1);
        }
        public void OnHummerClicked()
        {
            foreach (var box in _avaiableBoxes)
            {
                if (!box.IsCorrect())
                    box.ShowHammer();
            }
        }
        public void OnCloseHammerClicked()
        {
            SortingUI.Instance.HideHammerSelection();
            foreach (var box in _avaiableBoxes)
                box.HideHammer();
        }
        public void UseHammer(Box box)
        {
            OnCloseHammerClicked();
            box.SortCorrect();
            UserBalance.Instance.ChangeHammers(-1);
            SortingUI.Instance.UpdateBoosters();
        }
        public Box GetBoxWith(Box currentBox, ElementTypes type)
        {
            foreach (var box in _avaiableBoxes)
            {
                if (box != currentBox)
                {
                    for (int i = 0; i < box.Elements.Count; i++)
                    {
                        if (box.Elements[i].Type == type)
                            return box;
                    }
                }
            }
            return null;
        }
        public void RemoveFromAvaiable(Box box) => _avaiableBoxes.Remove(box);
    }
}
