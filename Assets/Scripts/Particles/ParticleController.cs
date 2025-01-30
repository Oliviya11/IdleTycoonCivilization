using UnityEngine;

namespace Assets.Scripts.Particles
{
   public class ParticleController : MonoBehaviour
    {
        ParticleSystem particleSystem;

        void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();

            if (particleSystem == null)
            {
                Debug.LogError("No ParticleSystem found on " + gameObject.name);
            }
        }

        public void Show()
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        public void Hide()
        {
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }

}
