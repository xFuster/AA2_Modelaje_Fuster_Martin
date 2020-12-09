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
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;
        float animTime = 0;
        bool isPlaying = false;
        public Vector3 start = new Vector3(-4.19f, -2.052f, -2.37f);
        public Vector3 end = new Vector3(-4.19f, -2.052f, 47.0f);

        float distanceBetweenFutureBases = 1.0f;
        //LEGS
        Transform[] legTargets = new Transform[6];
        Transform[] legFutureBases = new Transform[6];
        MyTentacleController[] _legs = new MyTentacleController[6];

        private Vector3[] copy;
       

        private float[] distances;


        #region public
        public void InitLegs(Transform[] LegRoots, Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for (int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                legFutureBases[i] = LegFutureBases[i];
                legTargets[i] = LegTargets[i];
                //TODO: initialize anything needed for the FABRIK implementation
            }
            distances = new float[_legs[0].Bones.Length - 1];
            copy = new Vector3[_legs[0].Bones.Length];
        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //TODO: Initialize anything needed for the Gradient Descent implementation
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {
            isPlaying = true;
            animTime = 0;
            animationRange = 5;
           
            //float animTime = 0;
            //animationRange = 5;

            //Debug.Log(animationRange);

            //while (animTime < animationRange)
            //{
            //    Debug.Log("ALTOKE");

            //    updateLegs();
            //    animTime += 0.005f;
            //}

        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            if (isPlaying == true)
            {
                //copy = copy.Reverse()
                animTime += Time.deltaTime;
                // Debug.Log(animTime);
                if (animTime < animationRange)
                {
                    updateLegPos();
                }
                else
                {
                    isPlaying = false;
                }

            }


            //updateLegs();
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            for (int j = 0; j < 6; j++)
            {
                if (Vector3.Distance(_legs[j].Bones[0].position, legTargets[j].position) > distanceBetweenFutureBases)
                    updateLegs(j);
            }

        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {

        }
        //TODO: implement fabrik method to move legs 
        private void updateLegs(int idPata)
        {
            
                for (int i = 0; i <= _legs[0].Bones.Length - 1; i++)
                {

                    copy[i] = _legs[idPata].Bones[i].position;
                }
                for (int i = 0; i <= _legs[idPata].Bones.Length - 2; i++)
                {

                    distances[i] = Vector3.Distance(_legs[idPata].Bones[i].position, _legs[idPata].Bones[i + 1].position);
                }


                float targetRootDist = Vector3.Distance(copy[0], legTargets[idPata].position);

                while (Vector3.Distance(copy[copy.Length - 1], legTargets[idPata].position) != 0 || Vector3.Distance(copy[0], _legs[idPata].Bones[0].position) != 0)
                {

                    copy[copy.Length - 1] = legTargets[idPata].position;

                    for (int i = _legs[idPata].Bones.Length - 2; i >= 0; i--)
                    {
                        Vector3 vectorDirector = (copy[i + 1] - copy[i]).normalized;
                        Vector3 movementVector = vectorDirector * distances[i];
                        copy[i] = copy[i + 1] - movementVector;
                    }

                    copy[0] = _legs[idPata].Bones[0].position;

                    for (int i = 1; i < _legs[idPata].Bones.Length - 1; i++)
                    {
                        Vector3 vectorDirector = (copy[i - 1] - copy[i]).normalized;
                        Vector3 movementVector = vectorDirector * distances[i - 1];
                        copy[i] = copy[i - 1] - movementVector;

                    }
                }
                for (int i = 0; i <= _legs[idPata].Bones.Length - 2; i++)
                {
                    Vector3 direction = (copy[i + 1] - copy[i]).normalized;
                    Vector3 antDir = (_legs[idPata].Bones[i + 1].position - _legs[idPata].Bones[i].position).normalized;
                    Quaternion rot = Quaternion.FromToRotation(antDir, direction);
                    _legs[idPata].Bones[i].rotation = rot * _legs[idPata].Bones[i].rotation;
                    Debug.DrawLine(_legs[idPata].Bones[i].position, _legs[idPata].Bones[i + 1].position, Color.yellow);
                    Debug.DrawLine(copy[i], copy[i + 1], Color.red);

                }


            #endregion
        }
    }
}


