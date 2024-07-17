using UnityEngine;
using Menu;
using Gameplay;
using Slot;

namespace Global
{
    public class GlobalInterface : MonoBehaviour
    {
        public static GlobalInterface Instance { get; private set; }

        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _game;

        private void Awake()
        {
            if(Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        private void Start()
        {
            OpenMenu();
        }
        public void OpenMenu()
        {
            _menu.SetActive(true);
            _game.SetActive(false);
            SortingUI.Instance.CloseGame();
            MenuUI.Instance.SetMenu(0);
        }
        public void OpenGame()
        {
            _menu.SetActive(false);
            _game.SetActive(true);
            SortingUI.Instance.OpenGame();
        }
        public void OpenSlot()
        {
            _menu.SetActive(false);
            _game.SetActive(true);
            SortingUI.Instance.CloseGame();
            SlotUI.Instance.OpenSlot();
        }
    }
}
