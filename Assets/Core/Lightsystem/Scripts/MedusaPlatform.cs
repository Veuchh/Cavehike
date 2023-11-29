using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Lightsystem
{
    public class MedusaPlatform : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _intangibleAlpha=0.5f;
        [Range(0f, 1f)] 
        [SerializeField] private float _tangibleAlpha=1;

        [SerializeField] private float _becomeTangibleSpeed = 0.2f;
        [SerializeField] private AnimationCurve _becomeTangibleCurve;
        [SerializeField] private float _becomeIntangibleSpeed = 0.2f;
        [SerializeField] private AnimationCurve _becomeIntangibleCurve;

        public void Awake()
        {
            BecomeIntangible();
        }

        public void BecomeTangible()
        {
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer renderer in meshRenderers)
            {
                var materials= renderer.materials;
                foreach(Material material in materials)
                {
                    var finalColor = new Color(material.color.r,material.color.g,material.color.b,_tangibleAlpha);
                    material.DOColor(finalColor, _becomeIntangibleSpeed).SetEase(_becomeTangibleCurve);
                }
            }
        }

        public void BecomeIntangible()
        {
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                var materials = renderer.materials;
                foreach (Material material in materials)
                {
                    var finalColor = new Color(material.color.r, material.color.g, material.color.b, _intangibleAlpha);
                    material.DOColor(finalColor, _becomeIntangibleSpeed).SetEase(_becomeIntangibleCurve);
                }
            }
        }
    }
}
