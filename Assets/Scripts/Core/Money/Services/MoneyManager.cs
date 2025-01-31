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
            OnMoneyChanged?.Invoke(_money.ToString());
        }

        public void SubtractMoney(string money)
        {
            _money = _money - new BigNumber(money);
            OnMoneyChanged?.Invoke(_money.ToString());
        }

        public bool IsEnoughMoney(string money)
        {
            BigNumber number = new BigNumber(Money);
            BigNumber currentNumber = new BigNumber(money);
            return number >= currentNumber;
        }
    }
}
