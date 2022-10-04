using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.StateMachine.States
{
    public abstract class BaseState
    {
        protected Transform GlobalTargetTransform;


        public BaseState(Transform globalTargetTransform)
        {
            GlobalTargetTransform = globalTargetTransform;
        }


        public abstract void Start();
        public abstract void Update();
        public abstract void Stop();
    } 
}
