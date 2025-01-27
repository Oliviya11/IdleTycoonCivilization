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
        [SerializeField] State initialState;
        [SerializeField] Product product;
        [SerializeField] Transform blank;
        [SerializeField] Transform arrow;
        [SerializeField] Transform icon;
        [SerializeField] Transform upgradeIcon;

        [SerializeField] List<Transform> productPlaces;
        [SerializeField] List<Transform> poducerPlaces;

        State currentState;

        public State InitialState => initialState;

        public enum State
        {
            Blank = 0,
            BlankWithArrow = 1,
            Product1 = 2,
            Product2 = 3,
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
                EnableBlank(false);
                EnableArrow(false);
                EnableIcon(true);
                ProduceProduct(0);
                if (currentState == State.Product2)
                {
                    ProduceProduct(1);
                }
                HideUpgrade();
            }
        }

        public void SetProduct1State()
        {
            EnableBlank(false);
            EnableArrow(false);
            EnableIcon(true);
            ProduceProduct(0);
            HideUpgrade();
        }

        public void SetProduct2State()
        {
            ProduceProduct(1);
        }

        void ProduceProduct(int index)
        {
            if (product == Product.Pumpkin)
            {
                Vector3 position = productPlaces[index].position;
                position.y += 0.3f;
                AllServices.Container.Single<IGameFactory>().CreatePumpkin(position);
            }
        }
    }
}
