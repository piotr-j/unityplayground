using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityQueues
{
    public class RandomMaterialColor : MonoBehaviour
    {
        [SerializeField]
        public Material material;

        [SerializeField]
        public MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer.material = new Material(material)
            {
                color = Random.ColorHSV()
            };
        }
    }
}
