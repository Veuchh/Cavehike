using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Core.Lightsystem
{
    //Make a custom editor script for the class "LightSource"
    [CustomEditor(typeof(Lightsource))]
    public class LightsourceEditor : Editor
    {
        //Add a button that makes something
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update light"))
            {
                target.GetComponent<Lightsource>().ComputeOuterToInnerSpot();
                target.GetComponent<Lightsource>().SetDistance();
                target.GetComponent<Lightsource>().UpdateLight(true);
                target.GetComponent<Lightsource>().UpdateCollider();
            }
        }

        public LightManager FindManager()
        {
            return FindObjectOfType<LightManager>();
        }
    }
}