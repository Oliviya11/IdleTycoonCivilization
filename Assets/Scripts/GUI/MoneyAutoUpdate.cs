using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Services;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    internal class MoneyAutoUpdate : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        void Awake()
        {
            SetMoney(AllServices.Container.Single<IMoneyManager>().Money);
            AllServices.Container.Single<IMoneyManager>().OnMoneyChanged += SetMoney;
        }

        void OnDestroy()
        {
            AllServices.Container.Single<IMoneyManager>().OnMoneyChanged -= SetMoney;
        }

        void SetMoney(string money)
        {
            text.text = money;
        }
    }
}
