using CaveHike.Player;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _movementSpeed = 30;
    [SerializeField] float _gravity = -9;
    [SerializeField] float _letItRip = 35;
    [SerializeField] float _slowDownValue = .35f;
    [Header("Jump")]
    [SerializeField] float _jumpStrength;
    [SerializeField] float releaseJumpButtonFallSpeed = 10;

    PlayerEntity _playerEntity;
    Vector3 velocity;

    private void Awake()
    {
        _playerEntity = GetComponent<PlayerEntity>();
    }

    private void Update()
    {
        velocity.y += _gravity * Time.deltaTime;

        //if
        //(big velocity and
        //velocity and input both are towards the same direction)
        //or if no inputs, we simply slightly decrease velocity
        if (Mathf.Abs(velocity.x) > _letItRip &&
            (velocity.x * _playerEntity.PlayerData.CurrentInput.x > 0 || _playerEntity.PlayerData.CurrentInput.x == 0))
        {
            if (velocity.x > 0)
                velocity.x -= _slowDownValue * Time.deltaTime;
            else
                velocity.x += _slowDownValue * Time.deltaTime;
        }

        else if (Mathf.Abs(_playerEntity.PlayerData.CurrentInput.x) < .1f)
        {
            if (velocity.x > 0)
                velocity.x -= _slowDownValue * Time.deltaTime;
            else
                velocity.x += _slowDownValue * Time.deltaTime;

            if (velocity.x < _slowDownValue)
            {
                velocity.x = 0;
            }
        }
        

        //else, we do normal movement
        else
        {
            velocity.x = _playerEntity.PlayerData.CurrentInput.x * _movementSpeed;
        }

        _playerEntity.PlayerData.Controller.Move(velocity * Time.deltaTime);
    }

    public void OnMoveInput(Vector2 newValue)
    {
        _playerEntity.PlayerData.CurrentInput = newValue;
    }

    public void OnJumpInput(bool isJumpPressed)
    {
        if (isJumpPressed)
            velocity.y = _jumpStrength;
        else if (velocity.y > releaseJumpButtonFallSpeed)
            velocity.y = releaseJumpButtonFallSpeed;
    }

    [Button]
    public void TargetFramerate10()
    {
        Application.targetFrameRate = 10;
    }

    [Button]
    public void TargetFramerate60()
    {
        Application.targetFrameRate = 60;
    }

    [Button]
    public void TargetFramerate120()
    {
        Application.targetFrameRate = 120;
    }
}
