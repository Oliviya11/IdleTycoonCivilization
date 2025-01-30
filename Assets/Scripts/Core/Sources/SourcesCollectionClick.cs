using Assets.Scripts.Core.Sources;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Inputs;
using UnityEngine;

namespace Assets.Scripts.Sources
{
    public class SourcesCollectionClick : MonoBehaviour
    {
        [SerializeField] SourcesCollection sourcesCollection;
        int _lastClickedSourceId;
        Popup _lastPopup;
        AllServices _services;
        ClickId _clickId;

        enum ClickId
        {
            None,
            Unlock,
            Upgrade,
        }

        void Start()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick += OpenUnlockPopup;
                source.click.OnSourceIconClick += OpenSourceUpgradePopUp;
                foreach (SourceElementClick sourceElementClick in source.sourceElementClicks)
                {
                    sourceElementClick.OnSourceElementClick += OpenSourceUpgradePopUp;
                }
            }

            _services.Single<IInputService>().OnClick += OnMapClick;
        }

        void OnDestroy()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick -= OpenUnlockPopup;
                source.click.OnSourceIconClick -= OpenSourceUpgradePopUp;
                foreach (SourceElementClick sourceElementClick in source.sourceElementClicks)
                {
                    sourceElementClick.OnSourceElementClick -= OpenSourceUpgradePopUp;
                }
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

            if (source.gameObject.GetInstanceID() == _lastClickedSourceId && _clickId == ClickId.Unlock)
            {
                _lastClickedSourceId = 0;
                return;
            }

            _clickId = ClickId.Unlock;
            _lastClickedSourceId = source.gameObject.GetInstanceID();
            Vector3 position = source.transform.position;
            position.y += 2;
            UnlockPopup.Params @params = new UnlockPopup.Params(delegate ()
            {
                OpenSource(source);
            }, "5");
            UnlockPopup.OpenLevelPopUp(@params, _services.Single<IGameFactory>(), position,
            delegate (UnlockPopup p)
            {
                _lastPopup = p;
            });
        }

        void OpenSourceUpgradePopUp(Source source)
        {
            HidePopup();

            if (source.gameObject.GetInstanceID() == _lastClickedSourceId && _clickId == ClickId.Upgrade)
            {
                _lastClickedSourceId = 0;
                return;
            }

            _clickId = ClickId.Upgrade;

            _lastClickedSourceId = source.gameObject.GetInstanceID();
            Vector3 position = source.transform.position;
            position.y += 2;

            UpgradeSourcePopup.Params @params = new UpgradeSourcePopup.Params(delegate ()
            {

            }, 1, 10, "7", 0.5f, 2, 0, true);

            UpgradeSourcePopup.OpenLevelPopUp(@params, _services.Single<IGameFactory>(), position,
            delegate (UpgradeSourcePopup p)
            {
                _lastPopup = p;
            });
        }

        void OpenSource(Source source)
        {
            SetProductState(source);
            _lastPopup = null;
            _services.Single<ISourcesManager>().OpenSource(source.state.Product, source);
        }

        void HidePopup()
        {
            if (_lastPopup != null)
            {
                _lastPopup.Hide();
                _lastPopup = null;
            }
        }

        void OnMapClick(Vector2 v, string name)
        {
            _lastClickedSourceId = 0;
            HidePopup();
        }

        void SetProductState(Source source)
        {
            source.state.EnableAccordingToState(SourceState.State.ProductPlace1);
        }
    }
}
