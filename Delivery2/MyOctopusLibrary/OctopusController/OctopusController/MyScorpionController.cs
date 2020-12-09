using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace OctopusController
{
  
    public class MyScorpionController
    {
        //TAIL
        Transform tailTarget;
        Vector3 auxito;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6];

        float threeshold = 0.05f;

        float rate = 120.0f;

        #region public
        public void InitLegs(Transform[] LegRoots,Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for(int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                //TODO: initialize anything needed for the FABRIK implementation
            }

        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //TODO: Initialize anything needed for the Gradient Descent implementation
            tailEndEffector = _tail.Bones[_tail.Bones.Length - 1];
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
            tailTarget = target;
        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {

        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            if (Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position)>threeshold)
            {
                float deltaZ = 0.01f;

                float distanceBetweenEndEffectorAndTarget = Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position);

                _tail.Bones[0].transform.Rotate(Vector3.up * deltaZ);

                float distanceBetweenEndEffectorAndTarget2 = Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position);

                _tail.Bones[0].transform.Rotate(Vector3.up * -deltaZ);

                float slope = (distanceBetweenEndEffectorAndTarget2 - distanceBetweenEndEffectorAndTarget) / deltaZ;

                _tail.Bones[0].transform.Rotate((Vector3.up * -slope)*rate);
            }

          
        }

        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            //
        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {

        }
     

        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {

        }
        #endregion
    }
}
