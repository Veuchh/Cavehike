using UnityEngine;

namespace CaveHike.Data
{
    public class PlayerData : EntityData
    {
        public CharacterController Controller { get => _controller; set => _controller = value; }
        public Vector2 CurrentInput { get => _currentInput; set => _currentInput = value; }

        private CharacterController _controller;
        private Vector2 _currentInput;
    }
}
