using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityQueues
{
    /// <summary>
    /// Single line entity queue
    /// 
    /// Entities are let through one by one
    /// </summary>
    public class EntityQueueController : MonoBehaviour
    {
        [SerializeField]
        public EntityQueue queue;
        void Start()
        {
            InvokeRepeating("ProcessQueue", 0, 1f);
        } 

        void ProcessQueue ()
        {
            queue.Process(1);
        }
    }
}
