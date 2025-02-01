using UnityEngine;

namespace Assets.Scripts.Particles
{
   public class ParticleController : MonoBehaviour
    {
        ParticleSystem effect;
        bool isPlaying;

        void Awake()
        {
            isPlaying = true;

            effect = GetComponent<ParticleSystem>();

            if (effect == null)
            {
                Debug.LogError("No ParticleSystem found on " + gameObject.name);
            }
        }

        public void Show()
        {
            if (isPlaying) return;

            isPlaying = true;
            if (effect != null)
            {
                effect.Play();
            }
        }

        public void Hide()
        {
            if (!isPlaying) return;

            isPlaying = false;
            if (effect != null)
            {
                effect.Stop();
            }
        }
    }

}
