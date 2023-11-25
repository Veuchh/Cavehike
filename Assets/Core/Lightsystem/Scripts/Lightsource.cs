using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Lightsystem
{
    public class Lightsource : MonoBehaviour
    {
        [Tooltip("The radius of the circle of light, will be graphically enforced by changing the outer spot angle of the spot light. Anything light sensible object in this range will also be triggered")]
        [SerializeField] private float _range;
        [SerializeField] private Light _spotLight;
        [SerializeField] private SphereCollider _collider;

        private float _distance; //Distance between the lightsource and ZReferenceLayer (basically the z of the 2d gameplay)
        private float _innerToOuterSpot; //The value that must be added to find the inner sot from the outerspot, in degrees

        public float Range
        {
            get => _range; set
            {
                _range = value;
                UpdateLight();
                UpdateCollider();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (_spotLight.type != LightType.Spot)
                Debug.LogError("Lightsource must be a spot light");

            ComputeOuterToInnerSpot();
            _distance = GetDistance();

            UpdateLight();
            UpdateCollider();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noCrunch">If true the outer angle will be capped between the difference between inner and outer angle and max spot angle</param>
        public void UpdateLight(bool noCrunch = false)
        {
            float outerAngle = Mathf.Rad2Deg * GetAngleInRadians();

            if (noCrunch)
            {
                //179.9 is approximately the max value of the light spot's outer angle
                outerAngle = Mathf.Clamp(outerAngle, _innerToOuterSpot, 179.9f);
            }

            float innerAngle = outerAngle - _innerToOuterSpot;
            _spotLight.spotAngle = outerAngle;
            _spotLight.innerSpotAngle = innerAngle;
        }

        public void UpdateCollider()
        {
            _collider.radius = Range;
        }

        public float GetDistance()
        {
            float distance = Mathf.Abs(transform.position.z - _spotLight.transform.position.z);
            return distance;
        }


        float GetAngleInRadians()
        {
            //https://docs.unity3d.com/Manual/Lighting.html, I multiply the arctan by two because the arctan gets me the angle between the central "rod" and one edge
            return 2 * Mathf.Atan(Range / _distance);
        }

        public void ComputeOuterToInnerSpot()
        {
            _innerToOuterSpot = _spotLight.spotAngle - _spotLight.innerSpotAngle;
        }

#if UNITY_EDITOR
        public void SetDistance()
        {
            _distance = GetDistance();
        }
#endif
    }
}