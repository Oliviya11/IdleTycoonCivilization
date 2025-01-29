using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ClientNPC : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] float targetAngle = 180;
        enum State
        {
            None,
            Come,
            Rotate,
            Process,
            Leave
        }

        State currentState;
        State previousState;
        bool isStateProcessing;
        Vector3 destination;

        public Vector3 Destination { get { return destination; } set { destination = value; } }
        public bool IsStateProcessing { get { return isStateProcessing; } set { isStateProcessing = value; } }

        void Awake()
        {
            currentState = State.Come;
        }

        public void Move(Vector3 destination)
        {
            agent.destination = destination;
            agent.updateRotation = true;
        }

        public bool IsMoving()
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Agent has reached the destination
                    return false;
                }
            }

            return true;
        }

        public void Rotate()
        {
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
            currentState = State.Rotate;
        }
    }
}
