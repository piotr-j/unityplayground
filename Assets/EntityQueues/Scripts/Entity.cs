using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EntityQueues
{
    public class Entity : MonoBehaviour
    {

        [SerializeField]
        public float speed = 2f;

        private Transform destination;

        private Transform overflow;
        
        private EntityQueue queue;

        private State state = State.INVALID;

        private int enterQueueCount = 0;


        public void Init (Transform destination, Transform overflow, EntityQueue queue)
        {
            this.destination = destination;
            this.overflow = overflow;
            this.queue = queue;
            SetState(State.QUEUE);
        }

        void Update()
        {
            if (state == State.INVALID) return;
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.WALKING:

                    break;
            }
        }

        private IEnumerator SetStateWithDelay (State state, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetState(state);
           
        }

        private void SetState(State state, float delay = 0)
        {
            if (delay > 0)
            {
                StartCoroutine(SetStateWithDelay(state, delay));
                return;
            }   
            if (this.state == state) return;
            // exit
            switch (this.state)
            {
                case State.IDLE:
                    break;
                case State.WALKING:
                    break;
            }
            this.state = state;
            // enter
            switch (this.state)
            {
                case State.IDLE:
                    WalkTo(overflow.position, () => {
                        Debug.Log("Destination reached");
                    });
                    break;
                case State.WALKING:

                    break;
                case State.OVERFLOW:
                    WalkTo(overflow.position, () => {
                        Debug.Log("Oerflow reached " + name);
                        Destroy(gameObject);
                    });
                    break;
                case State.DESTINATION:
                    WalkTo(destination.position, () => {
                        Debug.Log("Destination reached " + name);
                        Destroy(gameObject);
                    });
                    break;
                case State.QUEUE:
                    EntityQueueSpot spot = queue.FindEmptySpot();
                    // we want to know if this spot is taken before we get to it
                    enterQueueCount++;
                    WalkTo(spot.transform.position, () => {
                        Debug.Log("Destination reached " + name);
                        if (Enter(spot))
                        {
                            // we are in spot, waiting to move to next spot
                            SetState(State.IN_QUEUE);
                        }
                        else
                        {
                            TryQueue();
                        }
                    });
                    spot.Incoming(gameObject, () =>
                    {
                        TryQueue();
                    });
                    break;
            }
        }

        private void TryQueue()
        {
            if (enterQueueCount > 3)
            {
                // we tried few times, screw that!
                SetState(State.OVERFLOW);
            }
            else
            {
                // try again, delay?
                SetState(State.QUEUE);
            }
        }

        private bool Enter(EntityQueueSpot spot)
        {
            return spot.Enter(gameObject, (next) =>
            {
                if (next == null)
                {
                    Debug.Log("Exiting queue");
                    SetState(State.DESTINATION);
                }
                else
                {
                    Debug.Log("Enter next spot");
                    Enter(next);
                    WalkTo(next.transform.position);
                }
            });
        }

        Tween walkTween;
        private void WalkTo(Vector3 target, Action done = null)
        {
            SetState(State.WALKING);
            if (walkTween != null) walkTween.Kill();

            float duration = (transform.position - target).magnitude/speed;

            walkTween = transform
                .DOMove(target, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => {
                    done?.Invoke();
                    walkTween = null;
                });
            
        }

        enum State
        {
            INVALID, IDLE, WALKING, OVERFLOW, QUEUE, IN_QUEUE, DESTINATION
        }
    }
}