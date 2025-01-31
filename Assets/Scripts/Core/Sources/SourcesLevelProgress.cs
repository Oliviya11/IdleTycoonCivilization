using Assets.Scripts.GUI;
using Assets.Scripts.Sources;
using System.Runtime.CompilerServices;

namespace Assets.Scripts.Core.Sources
{
    public class SourcesLevelProgress
    {
        SourcesCollection sourcesCollection;
        ProgressBar progressBar;
        int maxValue;

        public SourcesLevelProgress(ProgressBar progressBar, SourcesCollection sourcesCollection)
        {
            this.progressBar = progressBar;
            this.sourcesCollection = sourcesCollection;
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
            progressBar.SetValue(CountCurrentValue(), maxValue);
        }
    }
}
