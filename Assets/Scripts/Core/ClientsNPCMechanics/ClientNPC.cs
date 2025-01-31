using Assets.Scripts.GUI;
using Assets.Scripts.Sources;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ClientNPC : NPC
    {
        public Product Product { get; set; }
        public ProductVisualizer productVisualizer;

        public enum State
        {
            None,
            Come,
            ReadyToOrder,
            WaitingForProducer,
            ProcessOrder,
            WaitingForOrder,
            Leave
        }

        public State CurrentState { get; set; }

        protected override void Awake()
        {
            base.Awake();
            productVisualizer.Hide();
        }
    }
}
