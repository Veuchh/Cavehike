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
            LightManager.Instance.RegisterLightblocker(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Lightsource>() != null)
            {
                LightManager.Instance.UnregisterLightsource(other.GetComponent<Lightsource>());
                return;
            }
            LightManager.Instance.UnregisterLightblocker(other.transform);
        }
    }
}

