using System;
using UnityEngine;

namespace CaveHike.Data
{
    [Serializable]
    public class PlayerData : EntityData
    {
        public CharacterController Controller { get => _controller; set => _controller = value; }
        public Vector2 CurrentInput { get => _currentInput; set => _currentInput = value; }
        public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
        public Collider Collider { get => _collider; set => _collider = value; }
        public Vector2 CurrentGrappleAimInput { get => _currentRightStickInput; set => _currentRightStickInput = value; }
        public GrappleState GrapplingState { get => _grapplingState; set => _grapplingState = value; }

        private CharacterController _controller;
        private Vector2 _currentInput;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Vector2 _currentRightStickInput;
        private GrappleState _grapplingState;
    }
}
