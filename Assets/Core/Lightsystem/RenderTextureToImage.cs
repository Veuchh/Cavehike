using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Render camera to sprite and ensure that the sprite is always the right size
/// </summary>
public class RenderCameraToSprite : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _image;

    private RenderTexture _renderTexture;
    private Sprite _sprite;
    private int _previousWidth=0;
    private int _previousHeight=0;
    private Rect _textureRect;
    Texture2D _texture;

    void Start()
    {
        Resize();
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture.active = _renderTexture;
        _texture.ReadPixels(_textureRect, 0, 0);
        _texture.Apply();

        _sprite = Sprite.Create(_texture, _textureRect, Vector2.zero);

        _image.sprite = _sprite;

        if (_previousWidth == Screen.width) return;
        if (_previousHeight == Screen.height) return;

        Resize();
    }

    void Resize()
    {
        if(_camera == null) return;

        _previousWidth = Screen.width;
        _previousHeight = Screen.height;

        _renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        _textureRect = new Rect(0,0,_previousWidth, _previousHeight);
        _texture = new Texture2D(_previousWidth, _previousHeight, TextureFormat.RGBA32, false);

        _camera.targetTexture = _renderTexture;
    }
}
