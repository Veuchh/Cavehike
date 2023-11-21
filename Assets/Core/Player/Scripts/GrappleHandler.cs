using Cinemachine;
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
        [SerializeField] LayerMask _targetableGrapple;
        [SerializeField] float _grappleMaxDistance;
        [SerializeField] float _sphereCastRadius = 1f;
        [SerializeField] float _swingStrength = 1f;

        public static event Action OnGrapple;

        PlayerEntity _player;
        PlayerMovement _playerMovement;
        ConfigurableJoint _joint;

        private void Start()
        {
            _player = GetComponent<PlayerEntity>();
            _playerMovement = GetComponent<PlayerMovement>();
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
            Debug.DrawRay(transform.position, _player.PlayerData.CurrentGrappleAimInput * _grappleMaxDistance, Color.red);
         
            if (_player.PlayerData.IsGrappling)
            {
                _lr.SetPosition(0, transform.position);
                _lr.SetPosition(1, _targetRigidbody.transform.position);
            }

            else if (_player.PlayerData.CurrentGrappleAimInput != Vector2.zero)
            {
                _targetRigidbody.gameObject.SetActive(false); 
                
                RaycastHit hit = GetAimPosition();

                if (hit.collider != null)
                {
                    _targetRigidbody.transform.position = hit.point;
                    _targetRigidbody.gameObject.SetActive(true);
                }
            }

            else
            {
                _targetRigidbody.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (_player.PlayerData.IsGrappling)
            {
                _player.PlayerData.Rigidbody.AddForce(Vector3.right * _player.PlayerData.CurrentInput.x * _swingStrength, ForceMode.VelocityChange);
            }
        }

        public void OnAimInput(Vector2 aimDirection)
        {
            _player.PlayerData.CurrentGrappleAimInput = aimDirection;
        }

        public void OnGrappleInput(bool isGrapplePressed)
        {
            if (isGrapplePressed)
            {
                RaycastHit hit = GetAimPosition();

                if (hit.collider != null)
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

            OnGrapple?.Invoke();
        }

        private void EnableBehaviour(bool enabled)
        {
            if (!enabled)
            {
                _playerMovement.SetVelocity(_player.PlayerData.Rigidbody.velocity);
            }


            _player.PlayerData.Collider.enabled = enabled;
            _player.PlayerData.Controller.enabled = !enabled;
            _player.PlayerData.Rigidbody.useGravity = enabled;
            _player.PlayerData.Rigidbody.isKinematic = !enabled;
            _lr.enabled = enabled;

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

        RaycastHit GetAimPosition()
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(transform.position, _player.PlayerData.CurrentGrappleAimInput);
            Physics.Raycast(ray, out hit, _grappleMaxDistance, _targetableGrapple);

            if (hit.collider != null)
            {
                return hit;

            }

            Physics.SphereCast(ray, _sphereCastRadius, out hit, _grappleMaxDistance, _targetableGrapple);

            return hit;
        }
    }
}