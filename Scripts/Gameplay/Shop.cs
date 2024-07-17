using UnityEngine;

namespace Gameplay
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance {  get; private set; }

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
    }
}
