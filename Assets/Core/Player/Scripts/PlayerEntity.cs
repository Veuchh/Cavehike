using CaveHike.Data;
using System;
using UnityEngine;

namespace CaveHike.Player
{
    public class PlayerEntity : Entity.Entity
    {
        public PlayerData PlayerData => (PlayerData)_entityData;

        protected override void Awake()
        {
            _entityData = new PlayerData();

            PlayerData.Controller = GetComponent<CharacterController>();
        }
    }
}