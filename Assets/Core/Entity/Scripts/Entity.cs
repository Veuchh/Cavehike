using CaveHike.Data;
using UnityEngine;

namespace CaveHike.Entity
{
    public class Entity : MonoBehaviour
    {
        public EntityData EntityData => _entityData;

        protected EntityData _entityData;

        protected virtual void Awake()
        {
            _entityData = new EntityData();
        }
    }
}