using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityQueues
{
    public class EntityQueueSpot : MonoBehaviour
    {
        internal GameObject occupier;
        private List<IncomingEntity> incomingEntities = new List<IncomingEntity>();

        internal bool Enter(GameObject gameObject)
        {
            if (occupier != null && occupier != gameObject) return false;
            occupier = gameObject;
            foreach(var ie in incomingEntities)
            {
                if (ie.gameObject == gameObject) continue;
                ie.onOccupied.Invoke();
            }
            incomingEntities.Clear();
            return true;
        }

        internal bool IsOccupied()
        {
            return occupier != null;
        }

        internal void Incoming(GameObject gameObject, Action onOccupied)
        {
            incomingEntities.Add(new IncomingEntity()
            {
                gameObject = gameObject,
                onOccupied = onOccupied
            });
        }

        private struct IncomingEntity
        {
            public GameObject gameObject;
            public Action onOccupied;
        }
    }
}