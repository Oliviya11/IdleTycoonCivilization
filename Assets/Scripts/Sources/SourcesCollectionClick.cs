using Assets.Scripts.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sources
{
    public class SourcesCollectionClick : MonoBehaviour
    {
        [SerializeField] SourcesCollection sourcesCollection;

        void Start()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick += OpenUnlockPopup;
            }
        }

        void OnDestroy()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick -= OpenUnlockPopup;
            }
        }

        void OpenUnlockPopup(Source source)
        {
            Vector3 position = source.transform.position;
            position.y += 2;
            UnlockPopup.OpenLevelPopUp(new UnlockPopup.Params(delegate()
            {
                SetProductState(source);
            }),
            position);
        }

        void SetProductState(Source source)
        {
            source.state.EnableAccordingToState(SourceState.State.Product1);
        }
    }
}
