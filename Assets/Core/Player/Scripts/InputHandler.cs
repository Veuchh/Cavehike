using UnityEngine;
using UnityEngine.InputSystem;

namespace CaveHike.Player
{
    public class InputHandler : MonoBehaviour
    {
        PlayerMovement _playerMovement;
        GrappleHandler _grapplerHandler;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _grapplerHandler = GetComponent<GrappleHandler>();
        }
        public void OnMove(InputValue value)
        {
            _playerMovement.OnMoveInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            _playerMovement.OnJumpInput(value.Get<float>() > .5f);
        }

        public void OnAimGrapple(InputValue value)
        {
            _grapplerHandler.OnAimInput(value.Get<Vector2>());
        }

        public void OnGrapple(InputValue value)
        {
            _grapplerHandler.OnGrappleInput(value.Get<float>() > .5f);
        }
    }
}