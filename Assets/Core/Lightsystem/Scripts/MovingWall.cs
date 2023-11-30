using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Vector3 _deltaPosition;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private bool _isLooping = false;

    private Vector3 _startPosition;
    private Rigidbody _rigidbody;

    private  TweenerCore<Vector3,Vector3,VectorOptions> _tweening;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetOn()
    {
        if(_tweening != null)
        {
            _tweening.Kill();
        }
        _tweening=_rigidbody.transform.DOMove(transform.position+_deltaPosition,_duration).SetEase(_animationCurve);

        if (_isLooping)
        {
            _tweening.SetLoops(-1,LoopType.Yoyo);
        }
    }

    public void SetOff()
    {
        if (_tweening != null)
        {
            _tweening.Kill();
        }
        _tweening = _rigidbody.transform.DOMove(_startPosition, _duration).SetEase(_animationCurve);

    }
}
