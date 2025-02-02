using Assets.Scripts.Core.Booster;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "BoosterData", menuName = "Static Data/Booster")]
    public class BoosterStaticData : ScriptableObject
    {
        public Booster booster;
        public Sprite icon;
        public string description;
    }
}
