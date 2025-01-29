using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Inputs;
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
        int _lastClickedSourceId;
        UnlockPopup _lastPopup;
        AllServices _services;

        void Start()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick += OpenUnlockPopup;
            }

            _services.Single<IInputService>().OnClick += OnMapClick;
        }

        void OnDestroy()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick -= OpenUnlockPopup;
            }

            _services.Single<IInputService>().OnClick -= OnMapClick;
        }

        public void Construct(AllServices services)
        {
            _services = services;
        }

        void OpenUnlockPopup(Source source)
        {
            HidePopup();

            if (source.gameObject.GetInstanceID() == _lastClickedSourceId)
            {
                _lastClickedSourceId = 0;
                return;
            }

            _lastClickedSourceId = source.gameObject.GetInstanceID();
            Vector3 position = source.transform.position;
            position.y += 2;
            UnlockPopup.Params @params = new UnlockPopup.Params(delegate ()
            {
                OpenSource(source);
            });
            UnlockPopup.OpenLevelPopUp(@params, _services.Single<IGameFactory>(), position,
            delegate (UnlockPopup p)
            {
                _lastPopup = p;
            });
        }

        void OpenSource(Source source)
        {
            SetProductState(source);
            _lastPopup = null;
            _services.Single<ISourcesManager>().OpenSource(source.state.Product, source.transform.position);
        }

        void HidePopup()
        {
            if (_lastPopup != null)
            {
                _lastPopup.Hide();
                _lastPopup = null;
            }
        }

        void OnMapClick(Vector2 v)
        {
            _lastClickedSourceId = 0;
            HidePopup();
        }

        void SetProductState(Source source)
        {
            source.state.EnableAccordingToState(SourceState.State.Product1);
        }
    }
}
