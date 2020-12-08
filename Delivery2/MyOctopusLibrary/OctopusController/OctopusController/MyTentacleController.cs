﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




namespace OctopusController
{

    
    internal class MyTentacleController

    //MAINTAIN THIS CLASS AS INTERNAL
    {

        TentacleMode tentacleMode;
        Transform[] _bones;
        Transform[] _endEffectorSphere;

        public Transform[] Bones { get => _bones; }

        //Exercise 1.
        public Transform[] LoadTentacleJoints(Transform root, TentacleMode mode)
        {
            //TODO: add here whatever is needed to find the bones forming the tentacle for all modes
            //you may want to use a list, and then convert it to an array and save it into _bones
            List<Transform> joints = new List<Transform>();
            tentacleMode = mode;
            Transform auxiliar = root;
            switch (tentacleMode){
                case TentacleMode.LEG:
                    //TODO: in _endEffectorsphere you keep a reference to the base of the leg
                    auxiliar = auxiliar.GetChild(0);
                    while (auxiliar.name != "Joint2")
                    {
                        auxiliar = auxiliar.GetChild(1);
                        joints.Add(auxiliar);
                    }
                    //_endEffectorSphere[0] = auxiliar.GetChild(1);
                break;
                case TentacleMode.TAIL:
                    //TODO: in _endEffectorsphere you keep a reference to the red sphere 
                    joints.Add(auxiliar);

                    while (auxiliar.name != "joint_4")
                    {
                        auxiliar = auxiliar.GetChild(1);
                        joints.Add(auxiliar);
                    }
                    //_endEffectorSphere[0] = auxiliar.GetChild(1);

                    break;
                case TentacleMode.TENTACLE:
                    //TODO: in _endEffectorphere you  keep a reference to the sphere with a collider attached to the endEffector
                    auxiliar = auxiliar.GetChild(0);
                    auxiliar = auxiliar.GetChild(0);
              
                    while (auxiliar.name != "Bone.001_end")
                    {
                        auxiliar = auxiliar.GetChild(0);
                      joints.Add(auxiliar);
                    }
                    //_endEffectorSphere[0] = auxiliar.GetChild(0);

                    break;
            }
            _bones = joints.ToArray();
          
            
            return Bones;
        }
    }
}
