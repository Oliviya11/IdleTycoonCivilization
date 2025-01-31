using Assets.Scripts.Services;
using System;

namespace Assets.Scripts.Core.Money.Services
{
    public interface IMoneyManager : IService
    {
        public string Money { get; }

        public event Action<string> OnMoneyChanged;

        public void AddMoney(string money);

        public void SubtractMoney(string money);

        bool IsEnoughMoney(string money);
    }
}
