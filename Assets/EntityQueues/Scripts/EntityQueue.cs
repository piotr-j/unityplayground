using System;
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
    public class EntityQueue : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        public int entityLimit;

        [Header("Stuff")]
        [SerializeField]
        public List<EntityQueueSpot> spots;


        /// <summary>
        /// Let entities pass the queue
        /// </summary>
        internal void Process(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                ProcessSingle();
            }
        }

        private void ProcessSingle ()
        {

        }

        internal EntityQueueSpot FindEmptySpot()
        {
            foreach(var spot in spots)
            {
                if (!spot.IsOccupied())
                {
                    return spot;
                }
            }
            return spots[spots.Count -1];
        }
    }
}
