using Assets.Scripts.Services;
using Assets.Scripts.Services.PersistentProgress;
using System;

namespace Assets.Scripts.Core.Money.Services
{
    public class MoneyManager : IMoneyManager
    {
        public event Action<string> OnMoneyChanged;
        BigNumber _money;
        readonly IPersistentProgressService _persistentProgressService;

        public string Money {get => _money.ToString(); set => _money = new BigNumber(value); }

        public MoneyManager(string money, IPersistentProgressService persistentProgressService) { 
            _money = new BigNumber(money);
            _persistentProgressService = persistentProgressService;
        }

        public void AddMoney(string money)
        {
            _money = _money + new BigNumber(money);
            SaveMoney();
            OnMoneyChanged?.Invoke(Money);
        }

        private void SaveMoney()
        {
            _persistentProgressService.Progress.money = Money;
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
