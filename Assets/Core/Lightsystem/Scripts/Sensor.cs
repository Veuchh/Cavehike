using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Lightsystem
{
    public class Sensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Lightsource>() != null)
            {
                LightManager.Instance.RegisterLightsource(other.GetComponent<Lightsource>());
                return;
            }
            if(other.GetComponent<LightInteractingMesh>() != null)
            {
                LightManager.Instance.RegisterLightInteractingMesh(other.GetComponent<LightInteractingMesh>());
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Lightsource>() != null)
            {
                LightManager.Instance.UnregisterLightsource(other.GetComponent<Lightsource>());
                return;
            }
            
            if (other.GetComponent<LightInteractingMesh>() != null)
            {
                LightManager.Instance.UnregisterLightblocker(other.transform);
            }
        }
    }
}

