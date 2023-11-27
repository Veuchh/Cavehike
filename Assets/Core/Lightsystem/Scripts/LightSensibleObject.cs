using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Core.Lightsystem
{
    public class LightSensibleObject : MonoBehaviour
    {
        public UnityEvent OnEnterLight;
        public UnityEvent OnExitLight;
        public UnityEvent OnStayLight;
        [SerializeField] private bool _activateOnStayLight;
        [SerializeField] private bool _activateOnExitLight;

        [Tooltip("Obstacles layer mask")]
        [SerializeField] private LayerMask _layerMask;

        private List<Lightsource> _lightSources=new List<Lightsource>();
        private float _colliderRadius;

        private bool _isOn = false;
        private bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                if (_isOn == value) return; 
                _isOn = value;
                
                if(_isOn)
                {
                    OnEnterLight?.Invoke();
                    if (_activateOnExitLight || _activateOnExitLight) return;
                    this.enabled = false; //We don't have any event to invoke on stay  or exit, we are useless  now that we have been lighted, we should stop updating
                    return;
                }

                if(_activateOnExitLight)
                {
                    OnExitLight?.Invoke();
                }
            }
        }

        private void Start()
        {
            _colliderRadius = GetComponent<SphereCollider>().radius;
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
            if (IsOn && !_activateOnExitLight && !_activateOnStayLight) return;

            bool lighted = false;
            foreach (Lightsource lightSource in _lightSources)
            {
                if(!LightIsInSight(lightSource)) continue;
                if (IsOn && _activateOnStayLight) OnStayLight?.Invoke();
                lighted = true;
                break;
            }

            IsOn = lighted; 
        }

        private bool LightIsInSight(Lightsource lightSource)
        {
            RaycastHit raycastHit;
            Physics.Raycast(transform.position, (lightSource.transform.position - transform.position),out raycastHit, Mathf.Infinity,_layerMask);

            if(raycastHit.collider == null) return true;
            if (raycastHit.distance >= Vector3.Distance(transform.position,lightSource.transform.position)-_colliderRadius) return true;

            return false;
        }
    }
}