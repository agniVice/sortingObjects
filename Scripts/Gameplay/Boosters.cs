using UnityEngine;

namespace Gameplay
{
    public class Boosters : MonoBehaviour
    {
        public static Boosters Instance { get; private set; }

        [SerializeField] private int[] _prices;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void BuyBooster(int id)
        {
            if (UserBalance.Instance.Money >= _prices[id])
            {
                Audio.Instance.PlaySound(Audio.Instance.Buy);
                UserBalance.Instance.ChangeMoney(-_prices[id]);
                SortingUI.Instance.UpdateMoney();
                if (id == 0)
                    UserBalance.Instance.ChangeHints(1);
                if (id == 1)
                    UserBalance.Instance.ChangeHammers(1);
            }
            else
            {
                BoostersUI.Instance.OnCloseClicked();
                ErrorUI.Instance.OpenError(_prices[id] - UserBalance.Instance.Money);
            }
            SortingUI.Instance.UpdateBoosters();
        }
        public int GetPrice(int id) => _prices[id];
        public int GetPriceCount() => _prices.Length;
    }
}
