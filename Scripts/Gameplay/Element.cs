using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay
{
    public class Element : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public ElementTypes Type;

        public Box ElementBox;
        public bool Active;

        private Image _elementImage;
        private Transform _parent;
        private RectTransform _rectTransform;
        private Camera _mainCamera;

        private Vector3 _startPosition;
        private Vector3 _offset;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _elementImage = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _parent = transform.parent;
            _startPosition = transform.position;
        }
        public void SetOtherParent(Transform p)
        {
            _parent = p;
            transform.SetParent(p);
        }
        public void AnimScaling(float delay)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Vector3 startScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(startScale, 0.3f).SetLink(gameObject).SetEase(Ease.OutBack).SetDelay(delay);
            _elementImage.DOFade(0.6f, 1f).SetLink(gameObject).SetDelay(delay);
        }
        public void Initialize(ElementTypes type, Box box)
        {
            ElementBox = box;
            Type = type;
            _elementImage.sprite = BoxManager.Instance.GetElementSprite((int)Type);
            _elementImage.SetNativeSize();

            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

            boxCollider2D.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height) * 0.8f;
        }
        public void Hide()
        {
            _elementImage.enabled = false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (GameState.Instance.CurrentState != GameState.State.InGame)
                return;
            if (ElementBox.IsCorrect())
                return;
            Audio.Instance.PlaySound(Audio.Instance.PickUp);
            _offset = transform.position - GetMouseWorldPos();
            transform.SetParent(BoxManager.Instance.transform);
            Active = true;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (GameState.Instance.CurrentState != GameState.State.InGame)
                return;
            if (ElementBox.IsCorrect())
                return;
            transform.position = GetMouseWorldPos() + _offset;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (GameState.Instance.CurrentState != GameState.State.InGame)
                return;
            if (ElementBox.IsCorrect())
                return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);
            foreach (RaycastResult result in raycastResults)
            {
                Element target = result.gameObject.GetComponent<Element>();
                if (target != null && target != this && !target.ElementBox.IsCorrect())
                {
                    Transform tempParent = _parent;
                    SetOtherParent(target.GetParent());
                    target.SetOtherParent(tempParent);

                    Vector3 targetPosition = target._startPosition;
                    target.transform.DOLocalMove(_startPosition, 0.1f).SetLink(gameObject);
                    _startPosition = targetPosition;
                    transform.DOLocalMove(_startPosition, 0.1f).SetLink(gameObject);
                    BoxManager.Instance.GetAllDown();
                    BoxManager.Instance.TargetElement = null;
                    Active = false;

                    target.ElementBox.RemoveElement(target);
                    target.ElementBox.SetElement(this);

                    ElementBox.RemoveElement(this);
                    ElementBox.SetElement(target);

                    Box tempBox = ElementBox;
                    ElementBox = target.ElementBox;
                    target.ElementBox = tempBox;
                    Audio.Instance.PlaySound(Audio.Instance.Change);

                    BoxManager.Instance.CheckBoxes();
                    return;
                }
            }

            Active = false;
            transform.SetParent(_parent);

            BoxManager.Instance.TargetElement = null;
            BoxManager.Instance.GetAllDown();
        }
        public Transform GetParent() => _parent;
        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = _mainCamera.WorldToScreenPoint(transform.position).z;
            return _mainCamera.ScreenToWorldPoint(mousePoint);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Element"))
            {
                if (collision.GetComponent<Element>().Active)
                {
                    if (!Active)
                    {
                        if(BoxManager.Instance.TargetElement != null)
                            BoxManager.Instance.TargetElement.GetDownFast();
                        BoxManager.Instance.TargetElement = this;
                    }
                    Audio.Instance.PlaySound(Audio.Instance.ElementSelected, 1, Random.Range(0.95f, 1.05f));
                    transform.DOLocalMove(_startPosition + new Vector3(0,30), 0.1f).SetLink(gameObject).SetEase(Ease.OutBack);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Element"))
            {
                if(collision.GetComponent<Element>().ElementBox == null)
                    return;
                if (collision.GetComponent<Element>().ElementBox.IsCorrect())
                    return;
                if (BoxManager.Instance.TargetElement == this)
                        BoxManager.Instance.TargetElement.GetDownFast();
                    BoxManager.Instance.TargetElement = null;
            }
        }
        public void GetDown()
        {
            if (!Active && this != BoxManager.Instance.TargetElement)
                GetDownFast();
        }
        public void GetDownFast() => transform.DOLocalMove(_startPosition, 0.1f).SetLink(gameObject).SetEase(Ease.OutBack);
    }
}