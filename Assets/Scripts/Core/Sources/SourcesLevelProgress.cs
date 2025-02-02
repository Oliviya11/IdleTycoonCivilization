using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Sources;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Core.Sources
{
    public class SourcesLevelProgress
    {
        readonly SourcesCollection _sourcesCollection;
        readonly ProgressBar _progressBar;
        readonly IGameStateMachine _gameStateMachine;
        int _maxValue;

        public SourcesLevelProgress(ProgressBar progressBar, SourcesCollection sourcesCollection, IGameStateMachine gameStateMachine)
        {
            _progressBar = progressBar;
            _sourcesCollection = sourcesCollection;
            _gameStateMachine = gameStateMachine;
            CountMaxValue();
            Update();
        }

        void CountMaxValue()
        {
            foreach (var source in _sourcesCollection.sources)
            {
                _maxValue += source.upgrade.MaxLevels();
            }
        }

        int CountCurrentValue()
        {
            int currentValue = 0;
            foreach (var source in _sourcesCollection.sources)
            {
                currentValue += source.upgrade.CurrentLevel;
            }

            return currentValue;
        }

        public void Update()
        {
            int currentValue = CountCurrentValue();
            _progressBar.SetValue(currentValue, _maxValue);

            if (currentValue == _maxValue)
            {
                _gameStateMachine.Enter<LoadNextLevelState>();
            }
        }
    }
}
