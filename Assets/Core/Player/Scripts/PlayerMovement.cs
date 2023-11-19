using CaveHike.Player;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _movementSpeed = 10;
    [SerializeField] float _gravity = -9;
    [SerializeField] float _minMomentumSpeed = 10;
    [SerializeField] float _slowDownMomentumValue = .35f;
    [Header("Jump")]
    [SerializeField] float _jumpStrength;
    [SerializeField] float _releaseJumpButtonFallSpeed = 10;
    [SerializeField] int _maxJumpAmount = 2;
    [SerializeField] float _fastFallStartupDelay = .1f;

    int _remainingJumps;
    bool _isJumpButtonPressed = false;
    Coroutine _startJumpDownRoutine;
    PlayerEntity _playerEntity;
    Vector3 _velocity;

    private void Awake()
    {
        _playerEntity = GetComponent<PlayerEntity>();
    }

    private void Update()
    {
        if (_playerEntity.PlayerData.IsGrappling)
            return;
        
        if (_playerEntity.PlayerData.Controller.isGrounded && _velocity.y < 0)
        {
            _remainingJumps = _maxJumpAmount;
            _velocity.y = _releaseJumpButtonFallSpeed;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }

        //if
        //(big velocity and
        //velocity and input both are towards the same direction)
        //or if no inputs, we simply slightly decrease velocity
        if (!_playerEntity.PlayerData.Controller.isGrounded && 
            Mathf.Abs(_velocity.x) > _minMomentumSpeed &&
        (_velocity.x * _playerEntity.PlayerData.CurrentInput.x > 0 || _playerEntity.PlayerData.CurrentInput.x == 0))
        {
            if (_velocity.x > 0)
                _velocity.x -= _slowDownMomentumValue * Time.deltaTime;
            else
                _velocity.x += _slowDownMomentumValue * Time.deltaTime;
        }

        else if (!_playerEntity.PlayerData.Controller.isGrounded && Mathf.Abs(_playerEntity.PlayerData.CurrentInput.x) < .1f)
        {
            if (_velocity.x > 0)
                _velocity.x -= _slowDownMomentumValue * Time.deltaTime;
            else
                _velocity.x += _slowDownMomentumValue * Time.deltaTime;

            if (_velocity.x < _slowDownMomentumValue)
            {
                _velocity.x = 0;
            }
        }

        //else, we do normal movement
        else
        {
            _velocity.x = _playerEntity.PlayerData.CurrentInput.x * _movementSpeed;
        }

        _playerEntity.PlayerData.Controller.Move(_velocity * Time.deltaTime);
    }

    public void OnMoveInput(Vector2 newValue)
    {
        _playerEntity.PlayerData.CurrentInput = newValue;
    }

    public void OnJumpInput(bool isJumpPressed)
    {
        _isJumpButtonPressed = isJumpPressed;

        if (isJumpPressed && _remainingJumps > 0)
        {
            if (_startJumpDownRoutine != null)
            {
                StopCoroutine(_startJumpDownRoutine);
                _startJumpDownRoutine = null;
            }
            _velocity.y = _jumpStrength;
            _remainingJumps--;
        }
        else if (!isJumpPressed && _velocity.y > _releaseJumpButtonFallSpeed)
        {
            _startJumpDownRoutine = StartCoroutine(StartFastFall());
        }
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        _velocity = newVelocity;
    }

    IEnumerator StartFastFall()
    {
        yield return new WaitForSeconds(_fastFallStartupDelay);

        _velocity.y = _releaseJumpButtonFallSpeed;
    }
}
