using Assets.Scripts.Core.ClientsNPCMechanics;
using Assets.Scripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public SourcesCollection sourcesPrefab;
        public List<SourceStaticData> sourcesData;
        public Vector3 producerPosition;
        public float producerRotationAngle;
    }
}
