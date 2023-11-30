using Core.SensibleObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public class PlayerSensible : SensibleObject
    {
        private int _numberOfPlayer;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() == null) return;
            _numberOfPlayer++;
            IsOn = (_numberOfPlayer > 0);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() == null) return;
            _numberOfPlayer--;
            if (_numberOfPlayer < 0) _numberOfPlayer = 0;
            IsOn = (_numberOfPlayer > 0);
        }
    }
}

