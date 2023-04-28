using UnityEngine;
using Thuleanx.AI.FSM;

namespace Thuleanx.TArt {
    public partial class Player {
        public enum State {
            PlayerControlled,
            CutsceneControlled
        }

        public class PlayerControlledState : State<Player> {
        }
    }
}
