﻿using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sources
{
    public class SourceState : MonoBehaviour
    {
        public Product Product { get; set; }

        [SerializeField] Image productIcon;
        [SerializeField] Transform blank;
        [SerializeField] Transform arrow;
        [SerializeField] Transform icon;
        [SerializeField] Transform upgradeIcon;

        [SerializeField] List<Transform> productPlaces;
        [SerializeField] List<Transform> poducerPlaces;

        State currentState;
        public int MaxPlacesCount => _maxPlacesCount;

        AllServices _services;
        int _maxPlacesCount;

        public enum State
        {
            Blank = 0,
            BlankWithArrow = 1,
            ProductPlace1 = 2,
            ProductPlace2 = 3,
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
                SetProduct1State();

                if (currentState == State.ProductPlace2)
                {
                    SetProduct2State();
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
        }

        public void SetProduct2State()
        {
            ProduceProductPlace(1);
            ++_maxPlacesCount;
        }

        void ProduceProductPlace(int index)
        {
            ProduceProductPlace(productPlaces[index].position);
        }

        public GameObject ProduceProduct(Vector3 position)
        {
            GameObject go = null;
            if (Product == Product.Pumpkin)
            {
                position.y += 0.3f;
                go = _services.Single<IGameFactory>().CreatePumpkin(position);
            }

            return go;
        }

        public void ProduceProductPlace(Vector3 position)
        {
            if (Product == Product.Pumpkin)
            {
                position.y += 0.3f;
                _services.Single<IGameFactory>().CreatePumpkin(position);
            }
        }

        void SetIcon()
        {
            productIcon.sprite = AllServices.Container.Single<IAssetProvider>().LoadProductIcon(Product.ToString());
        }
    }
}
