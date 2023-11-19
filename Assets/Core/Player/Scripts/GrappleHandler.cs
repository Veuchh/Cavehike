using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaveHike.Player
{
    public class GrappleHandler : MonoBehaviour
    {
        [SerializeField] Rigidbody _targetRigidbody;
        [SerializeField] LineRenderer _lr;
        [SerializeField] LayerMask targetableGrapple;
        [SerializeField] float grappleMaxDistance;

        PlayerEntity _player;
        ConfigurableJoint _joint;

        private void Awake()
        {
            _player = GetComponent<PlayerEntity>();
            _player.PlayerData.Rigidbody = GetComponent<Rigidbody>();
            _player.PlayerData.Collider = GetComponent<CapsuleCollider>();
            _joint = _targetRigidbody.GetComponent<ConfigurableJoint>();
            _targetRigidbody.transform.parent = null;
            _lr.transform.parent = null;
            _lr.transform.position = Vector3.zero;

            EnableBehaviour(false);
        }

        void Update()
        {
            if (_player.PlayerData.IsGrappling)
            {
                _lr.SetPosition(0, transform.position);
                _lr.SetPosition(1, _targetRigidbody.transform.position);
            }

            else if (_player.PlayerData.CurrentRightStickInput != Vector2.zero)
            {
                Ray ray = new Ray(transform.position, _player.PlayerData.CurrentRightStickInput);

                if (Physics.Raycast(ray, out RaycastHit hit, grappleMaxDistance, targetableGrapple))
                {
                    _targetRigidbody.transform.position = hit.point;
                }
            }
        }

        public void OnAimInput(Vector2 aimDirection)
        {
            _player.PlayerData.CurrentRightStickInput = aimDirection;
        }

        public void OnGrappleInput(bool isGrapplePressed)
        {
            if (isGrapplePressed)
            {
                Debug.LogWarning("TODO : Behaviour when no right stick input");
                Vector3 rayDirection = (_player.PlayerData.CurrentRightStickInput == Vector2.zero) ? Vector3.right : _player.PlayerData.CurrentRightStickInput;

                Ray ray = new Ray(transform.position, rayDirection);

                if (Physics.Raycast(ray, out RaycastHit hit, grappleMaxDistance, targetableGrapple))
                {
                    _targetRigidbody.transform.position = hit.point;
                    _player.PlayerData.IsGrappling = true;
                    EnableBehaviour(true);
                }
                else
                {
                    _player.PlayerData.IsGrappling = false;
                }
            }
            else
            {
                if (_player.PlayerData.IsGrappling)
                {
                    EnableBehaviour(false);
                }
                _player.PlayerData.IsGrappling = false;
            }
        }

        private void EnableBehaviour(bool enabled)
        {
            _player.PlayerData.Collider.enabled = enabled;
            _player.PlayerData.Controller.enabled = !enabled;
            _player.PlayerData.Rigidbody.useGravity = enabled;
            _player.PlayerData.Rigidbody.isKinematic = !enabled;

            _joint.connectedBody =
                enabled ?
                    _player.PlayerData.Rigidbody :
                    null;

            if (enabled)
            {
                SoftJointLimit softJointLimit = new SoftJointLimit();
                softJointLimit.limit = Vector3.Distance(transform.position, _joint.transform.position);

                _joint.linearLimit = softJointLimit;

                _player.PlayerData.Rigidbody.velocity = _player.PlayerData.Controller.velocity;
            }
        }
    }
}