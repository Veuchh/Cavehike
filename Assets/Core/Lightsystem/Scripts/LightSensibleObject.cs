using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Serialization;
using NaughtyAttributes;
using Core.SensibleObjects;

namespace Core.Lightsystem
{
    public class LightSensibleObject : SensibleObject
    {
        [Tooltip("Obstacles layer mask")]
        [FormerlySerializedAs("_layerMask")][SerializeField] private LayerMask _obstaclesLayerMask;

        private List<Lightsource> _lightSources = new List<Lightsource>();
        private float _colliderRadius;

        private void Start()
        {
            _colliderRadius = Mathf.Max(GetComponent<Collider>().bounds.extents.x, GetComponent<Collider>().bounds.extents.y, GetComponent<Collider>().bounds.extents.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<Lightsource>()) return;
            Lightsource lightSource=other.GetComponent<Lightsource>();
            if (_lightSources.Count(x => x == lightSource) > 0) return;

            _lightSources.Add(lightSource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<Lightsource>()) return;
            Lightsource lightSource = other.GetComponent<Lightsource>();
      
            _lightSources.Remove(lightSource);
     
            if (_lightSources.Count > 0) return;

            //If we don't have any lightsource left and we're still marked as on,  we must mark ourselves as off
            if(IsOn)
            {
                IsOn = false;
            }
        }

        private void Update()
        {
            if (_lightSources.Count == 0) return;
            if (IsOn && !_activateOnTurnOff && !_activateOnStayOn) return;

            bool lighted = false;
            foreach (Lightsource lightSource in _lightSources)
            {
                if(!LightIsInSight(lightSource)) continue;
                if (IsOn && _activateOnStayOn) OnStayOn?.Invoke();
                lighted = true;
                break;
            }

            IsOn = lighted; 
        }

        private bool LightIsInSight(Lightsource lightSource)
        {
            RaycastHit raycastHit;
            Physics.Raycast(transform.position, (lightSource.transform.position - transform.position),out raycastHit, Mathf.Infinity,_obstaclesLayerMask);

            if(raycastHit.collider == null) return true;
            if (raycastHit.distance >= Vector3.Distance(transform.position,lightSource.transform.position)-_colliderRadius) return true;

            return false;
        }
    }
}