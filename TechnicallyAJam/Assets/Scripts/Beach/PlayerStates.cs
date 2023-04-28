using UnityEngine;
using Thuleanx.AI.FSM;
using System.Collections;

namespace Thuleanx.TArt {

public partial class Player {
    public enum State {
        Grounded,
        Pickup
    }

    public class PlayerGrounded: State<Player> {
        public override int Update(Player agent) {
            agent.velocity = agent.Input.WorldSpace_Movement * agent.movementSpeed;
            agent.Move(agent.velocity * Time.deltaTime);

            if (agent.velocity.sqrMagnitude > 0)
                agent.transform.rotation = Quaternion.LookRotation(agent.velocity, Vector3.up);

            if (agent.Input.Interact)
                return (int) State.Pickup;

            return -1;
        }
    }

    public class PlayerPickup : State<Player> {
        public override void Begin(Player agent) {
            agent.StartAnimationWait();
            agent.velocity = Vector2.zero; // ideally we want to slow player down to a stop
            agent.Anim.SetTrigger("Pickup");
        }

        public override IEnumerator Coroutine(Player agent) {
            yield return agent._waitForTrigger();
            stateMachine.SetState((int) Player.State.Grounded);
        }
    }
}

}
