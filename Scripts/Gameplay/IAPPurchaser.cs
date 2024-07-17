using Menu;
using UnityEngine;

namespace Gameplay
{
    public class IAPPurchaser : MonoBehaviour
    {
        public static IAPPurchaser Instance { get; private set; }

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(Instance);
            else
                Instance = this;
        }
        public void On1000MoneyBuySucess()
        {
            UserBalance.Instance.ChangeMoney(1000);
            MenuUI.Instance.UpdateMoney();
            SortingUI.Instance.UpdateMoney();
        }
        public void On5000MoneyBuySucess()
        {
            UserBalance.Instance.ChangeMoney(5000);
            MenuUI.Instance.UpdateMoney();
            SortingUI.Instance.UpdateMoney();
        }
        public void On25HintsBuySucess()
        {
            UserBalance.Instance.ChangeHints(25);
            SortingUI.Instance.UpdateBoosters();
        }
        public void On25HammersBuySucess()
        {
            UserBalance.Instance.ChangeHammers(25);
            SortingUI.Instance.UpdateBoosters();
        }
    }
}
