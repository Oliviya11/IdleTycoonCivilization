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
        SourcesCollection sourcesCollection;
        ProgressBar progressBar;
        IGameStateMachine gameStateMachine;
        int maxValue;

        public SourcesLevelProgress(ProgressBar progressBar, SourcesCollection sourcesCollection, IGameStateMachine gameStateMachine)
        {
            this.progressBar = progressBar;
            this.sourcesCollection = sourcesCollection;
            this.gameStateMachine = gameStateMachine;
            CountMaxValue();
            Update();
        }

        void CountMaxValue()
        {
            foreach (var source in sourcesCollection.sources)
            {
                maxValue += source.upgrade.MaxLevels();
            }
        }

        int CountCurrentValue()
        {
            int currentValue = 0;
            foreach (var source in sourcesCollection.sources)
            {
                currentValue += source.upgrade.CurrentLevel;
            }

            return currentValue;
        }

        public void Update()
        {
            int currentValue = CountCurrentValue();
            progressBar.SetValue(currentValue, maxValue);

            if (currentValue == maxValue)
            {
                gameStateMachine.Enter<LoadNextLevelState>();
            }
        }
    }
}
