using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Core.Lightsystem
{
    /// <summary>
    /// Render camera to sprite and ensure that the sprite is always the right size
    /// </summary>
    public class RenderCameraToPlane : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private MeshRenderer _plane;
        private RenderTexture _renderTexture;

        private int _screenWidth = 0;
        private int _screenHeight = 0;

        void Start()
        {
            Resize();
        }

        // Update is called once per frame
        void Update()
        {
            if (_screenWidth == Screen.width && _screenHeight == Screen.height) return;

            Resize();
        }

        //Exectued each time the screen is resized
        void Resize()
        {
            if (_camera == null) return;

            //Log the width and height, avoid resizing each frame as it is a costly operation
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;

            //We must recreate the render texture to ensure it's the size of the display as a render texture cannot be resized (the option techincall exists in the code but it will throw errors)
            _renderTexture = new RenderTexture(_screenWidth, _screenHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            _camera.targetTexture = _renderTexture;
            _plane.material.mainTexture = _renderTexture;

            //We then rescale the plane so it's always at the same aspect ration
            float planeHeight = _plane.transform.localScale.y;
            //Width and thickness are negative because the plane is facing the camera 
            float planeWidth = -planeHeight * ((float)_screenWidth / (float)_screenHeight);

            _plane.transform.localScale = new Vector3(planeWidth, planeHeight, -1);
        }
    }
}