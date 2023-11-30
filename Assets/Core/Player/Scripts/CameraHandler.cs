using CaveHike.Data;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaveHike.Player
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] CinemachineBrain _cinemachineBrain;
        [SerializeField] CinemachineVirtualCamera _vcam;
        [SerializeField] float _horizontalCamCatchupSpeed = 10f;
        [SerializeField] float _camSeeFartherMultiplier = 1f;
        PlayerEntity _playerEntity;

        private void Awake()
        {
            _playerEntity = GetComponent<PlayerEntity>();
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

            GrappleHandler.OnGrapple += OnGrapple;
        }

        private void OnDestroy()
        {
            GrappleHandler.OnGrapple -= OnGrapple;
        }

        void OnGrapple()
        {
            _cinemachineBrain.m_UpdateMethod = _playerEntity.PlayerData.GrapplingState != GrappleState.None ? CinemachineBrain.UpdateMethod.FixedUpdate : CinemachineBrain.UpdateMethod.LateUpdate;
        }

        private void Update()
        {
            if (Mathf.Abs(_playerEntity.PlayerData.CurrentInput.x) > .1f)
                _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX =
                    Mathf.Lerp(_vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX,
                    .5f - _playerEntity.PlayerData.CurrentInput.x * _camSeeFartherMultiplier,
                    _horizontalCamCatchupSpeed);
        }
    }
}