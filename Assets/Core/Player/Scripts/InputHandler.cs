using UnityEngine;
using UnityEngine.InputSystem;

namespace CaveHike.Player
{
    public class InputHandler : MonoBehaviour
    {
        PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        public void OnMove(InputValue value)
        {
            _playerMovement.OnMoveInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            _playerMovement.OnJumpInput(value.Get<float>() > .5f);
        }
    }
}