using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class MultiplierElement : MonoBehaviour
    {
        private MultiplierDrum _drum;
        private Image _elementImage;
        private MultiplierType _type;

        private void Awake()
        {
            _elementImage = GetComponent<Image>();
        }
        public void Initialize(MultiplierDrum drum)
        {
            _drum = drum;
        }
        public void SetType(MultiplierType type, Sprite sprite)
        {
            _type = type;
            _elementImage.sprite = sprite;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Right"))
            {
                _drum.OnElementSpin(this);
            }
        }
        public MultiplierType GetMainType() => _type;
    }
}
