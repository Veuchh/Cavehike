using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.SensibleObjects
{
    public class SensibleObject : MonoBehaviour
    {
        [FormerlySerializedAs("OnEnterLight")] public UnityEvent OnTurnOn;
        [FormerlySerializedAs("OnExitLight")] public UnityEvent OnTurnOff;
        [FormerlySerializedAs("OnStayLight")] public UnityEvent OnStayOn;

        [SerializeField] protected bool _activateOnStayOn;
        [SerializeField] protected bool _activateOnTurnOff;
        [Tooltip("Delay  the activation of the OnTurnOff event")]
        [SerializeField] protected float _onTurnOffDelay = 0f;
        //Set true when the light stop shining on the object, and false when the objects  fully turns off.  Avoid being turned on before we should
        private bool _onTurnOffBeingResolved = false;

        [ShowNonSerializedField]
        protected bool _isOn = false;
        protected bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                if (_isOn == value) return;
                _isOn = value;

                if (_isOn && _onTurnOffBeingResolved)
                {
                    _onTurnOffBeingResolved = false;
                    StopCoroutine(_delayedExitLightCoroutine);
                    return;
                }

                if (_isOn)
                {
                    OnTurnOn?.Invoke();
                    if (_activateOnStayOn || _activateOnTurnOff) return;
                    this.enabled = false; //We don't have any event to invoke on stay  or exit, we are useless  now that we have been lighted, we should stop updating
                    return;
                }

                if (_activateOnTurnOff)
                {
                    OnTurnOffFunction();
                }
            }
        }

        private Coroutine _delayedExitLightCoroutine;

        private void OnTurnOffFunction()
        {
            if (_delayedExitLightCoroutine != null) StopCoroutine(_delayedExitLightCoroutine);
            _onTurnOffBeingResolved = true;
            _delayedExitLightCoroutine = StartCoroutine(DelayedTurnOffCoroutine());
        }

        private IEnumerator DelayedTurnOffCoroutine()
        {
            yield return new WaitForSeconds(_onTurnOffDelay);
            OnTurnOff?.Invoke();
            _onTurnOffBeingResolved = false;
        }
    }
}