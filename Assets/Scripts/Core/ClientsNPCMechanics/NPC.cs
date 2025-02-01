using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public abstract class NPC : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;

        protected virtual void Awake()
        {
            agent.updateRotation = true;
        }

        public void Move(Vector3 destination)
        {
            agent.isStopped = false;
            agent.destination = destination;
        }

        public void Stop()
        {
            agent.isStopped = true;
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

        public void Rotate(float targetAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        }
    }
}
