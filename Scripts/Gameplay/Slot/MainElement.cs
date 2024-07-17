using UnityEngine;
using UnityEngine.UI;
namespace Slot
{
    public class MainElement : MonoBehaviour
    {
        private MainDrum _drum;
        private Image _elementImage;
        private MainType _type;

        private void Awake()
        {
            _elementImage = GetComponent<Image>();
        }
        public void Initialize(MainDrum drum)
        { 
            _drum = drum;
        }
        public void SetType(MainType type, Sprite sprite)
        {
            _type = type;
            _elementImage.sprite = sprite;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bottom"))
            {
                _drum.OnElementSpin(this);
            }
        }
        public MainType GetMainType() => _type;
    }
}
