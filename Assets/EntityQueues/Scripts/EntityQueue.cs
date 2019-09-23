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

        private void Start()
        {
            // spot 0 is first
            for (int i = 0; i < spots.Count; i++)
            {
                spots[i].qid = i;
                if (i == 0) continue;
                spots[i].next = spots[i - 1];
            }
        }


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
            foreach (var spot in spots)
            {
                //Debug.Log("Process spot " + spot.qid);
                spot.Process();
            }
        }

        internal EntityQueueSpot FindEmptySpot()
        {
            foreach(var spot in spots)
            {
                if (!spot.IsOccupied() && spot.incomingEntities.Count == 0)
                {
                    return spot;
                }
            }
            return spots[spots.Count -1];
        }
    }
}
