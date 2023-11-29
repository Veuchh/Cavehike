using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Lightsystem
{
    public class LightInteractingMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        public enum LightInteractingMeshType
        {
            LightBlocker,
            VisibleInTheDark,
        }
        public LightInteractingMeshType Type;
        [Range(0f, 1f)]
        public float GlowAmount;

        public MeshFilter MeshFilter { get => _meshFilter; private set => _meshFilter = value; }
    }
}
