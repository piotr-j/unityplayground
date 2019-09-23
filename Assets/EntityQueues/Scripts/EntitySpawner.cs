using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityQueues
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField]
        public GameObject entityPrefab;

        [SerializeField]
        public Transform spawnPoint;

        [SerializeField]
        public Transform destination;

        [SerializeField]
        public Transform overflow;

        [SerializeField]
        public EntityQueue queue;

        public void Spawn ()
        {
            Vector3 pos = new Vector3
            {
                x = spawnPoint.position.x + Random.Range(-3f, 3f),
                y = spawnPoint.position.y + Random.Range(-1.5f, 1.5f)
            };
            GameObject go = Instantiate(entityPrefab, pos, Quaternion.identity, transform);
            Entity entity = go.GetComponent<Entity>();
            entity.Init(destination, overflow, queue);
        }
    }
}
