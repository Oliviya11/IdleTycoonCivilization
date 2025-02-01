using UnityEngine;

namespace Assets.Scripts.Particles
{
   public class ParticleController : MonoBehaviour
    {
        ParticleSystem particleSystem;
        bool isPlaying;

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
            if (isPlaying) return;

            isPlaying = true;
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        public void Hide()
        {
            if (!isPlaying) return;

            isPlaying = false;
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }

}
