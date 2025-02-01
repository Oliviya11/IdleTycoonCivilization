using Assets.Scripts.Services;
using System;

namespace Assets.Scripts.Core.Money.Services
{
    public class MoneyManager : IMoneyManager
    {
        public event Action<string> OnMoneyChanged;
        BigNumber _money;
        public string Money => _money.ToString();

        public MoneyManager(string money) { 
            _money = new BigNumber(money);
        }

        public void AddMoney(string money)
        {
            _money = _money + new BigNumber(money);
            OnMoneyChanged?.Invoke(Money);
        }

        public void SubtractMoney(string money)
        {
            _money = _money - new BigNumber(money);
            OnMoneyChanged?.Invoke(Money);
        }

        public bool IsEnoughMoney(string money)
        {
            BigNumber currentNumber = new BigNumber(money);
            return _money >= currentNumber;
        }
    }
}
