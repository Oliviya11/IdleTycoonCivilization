﻿using Assets.Scripts.Core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sources
{
    public class Source : MonoBehaviour
    {
        public SourceState state;
        public SourceClick click;
        public SourcePlaces places;
        public SourceUpgrade upgrade;
        public List<SourceElementClick> sourceElementClicks;
    }
}
