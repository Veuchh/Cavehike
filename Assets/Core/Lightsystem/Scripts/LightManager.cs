using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Lightsystem
{
    /// <summary>
    /// Light manager is a singleton that will manage all the lights in the scene,
    /// it will often refer to "Light world" which is the area filmed by the CamToRenderTexture Camera that simulates circular lights
    /// As  opposed to the "Real world" where the actual  games happen
    /// </summary>
    public class LightManager : MonoBehaviour
    {
        //Singleton pattern with awake
        #region Singleton pattern
        public static LightManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one LightsRenderer in scene!");
                return;
            }
            Instance = this;
        }
        #endregion

        [Tooltip("The game is in 2.5D, what is our Z \"Reference\" ? The one where the light is computed, the one we take as reference for the range of the light, the one where the player moves ?")]
        public float ZReferenceLayer = 0;

        [Tooltip("The camera that films the light world")]
        [SerializeField] private Camera _lightWorldCamera;

        [Tooltip("Prefab used to make the lights in light world")]
        [SerializeField] private GameObject _lightPrefab;

        [Tooltip("Prefab used to make the light blockers in light world")]
        [SerializeField] private GameObject _lightBlockerPrefab;

        //Each light source in the level is associated with a transform in light world
        private Dictionary<Lightsource, Light> _lightsourcesAndEquivalents=new Dictionary<Lightsource, Light>();
        //Each light blocker in the level is associated with a transform in light world
        private Dictionary<Transform, Transform> _lightBlockers=new Dictionary<Transform, Transform>();

        private void Start()
        {

        }

        private void LateUpdate()
        {
            //The camera the films the light world is maintained at the right distance
            _lightWorldCamera.transform.localPosition = new Vector3(0, 0, -Mathf.Abs(ZReferenceLayer - Camera.main.transform.position.z));

            foreach (Lightsource original in _lightsourcesAndEquivalents.Keys)
            {
                SynchronizeEquivalent(original.transform, _lightsourcesAndEquivalents[original].transform);
            }
            foreach (Transform blocker in _lightBlockers.Keys)
            {
                SynchronizeEquivalent(blocker, _lightBlockers[blocker]);
            }
        }


        //Move the equivalent in light world of an object to the right position
        private void SynchronizeEquivalent(Transform original, Transform equivalent)
        {
            equivalent.position = _lightWorldCamera.transform.position + (original.position - Camera.main.transform.position);
        }

        /// <summary>
        /// Update the range of the light in light world
        /// </summary>
        /// <param name="originalLightsource">The light in the real world</param>
        public void UpdateLightRange(Lightsource originalLightsource)
        {
            _lightsourcesAndEquivalents[originalLightsource].range = originalLightsource.Range;
        }

        //Call it when a new lightsource arrive on screen
        public void RegisterLightsource(Lightsource lightsource)
        {
            var go = Instantiate(_lightPrefab);
            Light light = go.GetComponent<Light>();  
            go.transform.parent = this.transform;
            _lightsourcesAndEquivalents.Add(lightsource, light);

            SynchronizeEquivalent(lightsource.transform, go.transform);
            UpdateLightRange(lightsource);
            lightsource.OnRangeUpdate += UpdateLightRange;
        }

        public void UnregisterLightsource(Lightsource lightsource)
        {
            Destroy(_lightsourcesAndEquivalents[lightsource].gameObject);
            _lightsourcesAndEquivalents.Remove(lightsource);
        }

        //Same for light blockers
        public void RegisterLightblocker(Transform blocker)
        {
            var meshFilter = blocker.GetComponentInChildren<MeshFilter>();
            if (meshFilter == null) return;
            if (meshFilter.mesh == null) return;

            var go = Instantiate(_lightBlockerPrefab);
            go.transform.localScale = blocker.lossyScale;
            go.transform.rotation = blocker.rotation;
            go.GetComponent<MeshFilter>().mesh = meshFilter.mesh;
            go.transform.parent = this.transform;
            _lightBlockers.Add(blocker, go.transform);

            SynchronizeEquivalent(blocker.transform, go.transform);
        }

        public void UnregisterLightblocker(Transform blocker)
        {
            Destroy(_lightBlockers[blocker].gameObject);
            _lightBlockers.Remove(blocker);
        }
    }

}
