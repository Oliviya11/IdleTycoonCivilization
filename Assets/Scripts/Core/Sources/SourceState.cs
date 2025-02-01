using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sources
{
    public class SourceState : MonoBehaviour
    {
        public Product Product { get; set; }

        [SerializeField] Source source;
        [SerializeField] Image productIcon;
        [SerializeField] Transform blank;
        [SerializeField] Transform arrow;
        [SerializeField] Transform icon;
        [SerializeField] Transform upgradeIcon;

        [SerializeField] List<Transform> productPlaces;
        [SerializeField] List<Transform> poducerPlaces;
        [SerializeField] List<GameObject> elements;

        State currentState;
        public int MaxPlacesCount => _maxPlacesCount;
        public State CurrentState => currentState;

        AllServices _services;
        int _maxPlacesCount;

        public enum State
        {
            None = 0,
            Blank = 1,
            BlankWithArrow = 2,
            ProductPlace1 = 3,
            ProductPlace2 = 4,
            ProductPlace3 = 5,
        }

        private void Awake()
        {

            foreach (var element in elements)
            {
                element.SetActive(false);
            }
        }

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public void EnableArrow(bool enabled)
        {
            arrow.gameObject.SetActive(enabled);
        }

        public void EnableIcon(bool enabled)
        {
            icon.gameObject.SetActive(enabled);
        }

        public void EnableBlank(bool enabled)
        {
            blank.gameObject.SetActive(enabled);
        }

        public void ShowUpgrade()
        {
            upgradeIcon.gameObject.SetActive(true);
        }

        public void HideUpgrade()
        {
            upgradeIcon.gameObject.SetActive(false);
        }

        public void EnableAccordingToState(State state)
        {
            currentState = state;

            if (currentState == State.Blank)
            {
                EnableBlank(true);
                EnableArrow(false);
                EnableIcon(false);
                HideUpgrade();
            }
            else if (currentState == State.BlankWithArrow)
            {
                EnableBlank(true);
                EnableArrow(true);
                EnableIcon(false);
                HideUpgrade();
            }
            else
            {
                _services.Single<ISourcesManager>().OpenSource(Product, source);

                SetProduct1State();

                if (currentState == State.ProductPlace2)
                {
                    SetProduct2State();
                }
                else if (currentState == State.ProductPlace3)
                {
                    SetProduct2State();
                    SetProduct3State();
                }
            }
        }

        public void SetProduct1State()
        {
            EnableBlank(false);
            EnableArrow(false);
            EnableIcon(true);
            ProduceProductPlace(0);
            SetIcon();
            HideUpgrade();
            ++_maxPlacesCount;
            currentState = State.ProductPlace1;
        }

        public void SetProduct2State()
        {
            ProduceProductPlace(1);
            ++_maxPlacesCount;
            currentState = State.ProductPlace2;
        }

        public void SetProduct3State()
        {
            ProduceProductPlace(2);
            ++_maxPlacesCount;
            currentState = State.ProductPlace3;
        }

        public void SetProductPlace(int upgrade)
        {
            if (upgrade >= productPlaces.Count) return;

            if (upgrade == 1)
            {
                SetProduct2State();
            }
            else if (upgrade == 2)
            {
                SetProduct3State();
            }
        }

        void ProduceProductPlace(int index)
        {
            elements[index].gameObject.SetActive(true);
            ProduceProductPlace(productPlaces[index].position);
        }

        public GameObject ProduceProduct(Vector3 position, Vector3 scale)
        {
            GameObject go = null;
            if (Product == Product.Pumpkin)
            {
                go = _services.Single<IGameFactory>().CreatePumpkin(position);
            }
            else if (Product == Product.Egg)
            {
                go = _services.Single<IGameFactory>().CreateEgg(position);
            }
            else if (Product == Product.Tomato)
            {
                go = _services.Single<IGameFactory>().CreateTomato2(position);
            }

            go.transform.localScale = scale;

            return go;
        }

        public void ProduceProductPlace(Vector3 position)
        {
            if (Product == Product.Pumpkin)
            {
                position.y += 0.3f;
                _services.Single<IGameFactory>().CreatePumpkin(position);
            }
            else if (Product == Product.Egg)
            {
                _services.Single<IGameFactory>().CreateChicken(position, Quaternion.Euler(new Vector3(0, 160, 0)));
            }
            else if (Product == Product.Tomato)
            {
                _services.Single<IGameFactory>().CreateTomato1(position);
            }
        }

        void SetIcon()
        {
            productIcon.sprite = GetIcon();
        }

        public Sprite GetIcon()
        {
            return AllServices.Container.Single<IAssetProvider>().LoadProductIcon(Product.ToString());
        }

        public bool ProduceProduct() => currentState == State.ProductPlace1 || currentState == State.ProductPlace2;

        public void SetProgress(SourceData data)
        {
            EnableAccordingToState(data.state);
        }

        public  SourceState.State LoadProgress()
        {
            return currentState;
        }
    }
}
