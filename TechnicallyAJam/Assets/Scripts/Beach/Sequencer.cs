using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using NaughtyAttributes;
using Cinemachine;

namespace Thuleanx.TArt {

public class Sequencer : MonoBehaviour {
    [BoxGroup("General")]
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;

    [BoxGroup("Starting Sequence")]
    [SerializeField] Transform location_start;
    [BoxGroup("Starting Sequence")]
    [SerializeField] CinemachineVirtualCamera cinematicCamera_start;

    [Button]
    public void SeekOrigin() {
        StartCoroutine(_seekOrigin());
    }

    public IEnumerator _seekOrigin() {
        Player player = FindObjectOfType<Player>();
        PlayerInputSystem playerInputSystem = player.GetComponent<PlayerInputSystem>();
        PlayerInput input = player.Input;


        playerInputSystem.overriden = true;

        // set destination to a specified location
        player.NavMeshAgent.SetDestination(location_start.position);

        Func<bool> hasReachDestination = () => {
            NavMeshAgent agent = player.NavMeshAgent;
            return !agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        };

        while (!hasReachDestination()) {
            input.SetMovement_WorldSpace(player.NavMeshAgent.desiredVelocity);
            yield return null;
        }

        /* CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>(); */
        /* cinematicCamera_start.Priority = 100; */

        /* while (CinemachineCore.Instance.IsLive(playerFollowCamera)) */
        /*     yield return null; */
        /* Debug.Log("Good"); */
        /* yield return new WaitForSeconds(4); */

        /* cinematicCamera_start.Priority = 0; */

        playerInputSystem.overriden = false;
    }
}

}
