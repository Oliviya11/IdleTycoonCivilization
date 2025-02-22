﻿using Assets.Scripts.GUI;
using Assets.Scripts.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ProducerNPC : NPC
    {
        public Timer timer;
        public ParticleController sleepingParticles;
        public Transform productPlace;
        public ProfitVisualizer profitVisualizer;

        public enum State {
            None,
            Sleep,
            MoveToClientForOrder,
            ProcessOrder,
            MoveToSource,
            ProduceProduct,
            MoveToClientWithOrder,
            SleepWithOrder,
            GiveOrder,
        }

        public State CurrentState {  get; set; }

        protected override void Awake()
        {
            base.Awake();
            CurrentState = State.Sleep;
            timer.Hide();
            profitVisualizer.Hide();
        }
    }
}
