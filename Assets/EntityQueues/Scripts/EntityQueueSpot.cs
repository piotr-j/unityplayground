using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityQueues
{
    public class EntityQueueSpot : MonoBehaviour
    {
        // if null, this is the first spot
        internal EntityQueueSpot next;

        internal GameObject occupier;
        private Action<EntityQueueSpot> onProcess;

        internal List<IncomingEntity> incomingEntities = new List<IncomingEntity>();
        internal int qid;

        internal bool Enter(GameObject gameObject, Action<EntityQueueSpot> onProcess)
        {
            if (occupier != null && occupier != gameObject) return false;
            occupier = gameObject;
            this.onProcess = onProcess;
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

        internal void Process()
        {
            if (onProcess == null) return;
            occupier = null;
            Action<EntityQueueSpot> action = onProcess;
            onProcess = null;
            // allow to skip spot if it becomes empty in mean time
            EntityQueueSpot target = next;
            while (target != null && target.next != null && !target.next.IsOccupied())
            {
                target = target.next;
            }
            // reset stuff before invoking
            action.Invoke(target);
        }

        internal struct IncomingEntity
        {
            public GameObject gameObject;
            public Action onOccupied;
        }
    }
}