using Assets.Scripts.Core.Sources;
using Assets.Scripts.Data;
using Assets.Scripts.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Sources
{
    public class SourcesCollection : MonoBehaviour, ISavedProgress
    {
        public List<Source> sources;
        public SourcesCollectionClick click;
        public SourcesLevelProgress levelProgress;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.sources == null)
            {
                foreach (var source in sources)
                {
                    source.upgrade.UpgradeTill(0, 0);
                }
            }
            else
            {
                for (int i = 0; i < progress.sources.Count; i++)
                {
                    sources[i].upgrade.SetProgress(progress.sources[i]);
                    sources[i].state.SetProgress(progress.sources[i]);
                }
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.sources = new List<SourceData>();
            foreach (Source source in sources)
            {
                SourceData sourceData = source.upgrade.LoadProgress();
                sourceData.state = source.state.LoadProgress();
                progress.sources.Add(sourceData);
            }
        }
    }
}
