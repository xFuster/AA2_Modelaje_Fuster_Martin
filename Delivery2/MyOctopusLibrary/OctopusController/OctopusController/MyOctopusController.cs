using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace OctopusController
{
    public enum TentacleMode { LEG, TAIL, TENTACLE };

    public class MyOctopusController
    {

        MyTentacleController[] _tentacles = new MyTentacleController[4];

        Transform _currentRegion;
        Transform _target;

        Transform[] _randomTargets;// = new Transform[4];


        float _twistMin, _twistMax;
        float _swingMin, _swingMax;

        float start;
        float end;
        bool isShooting;
        #region public methods
        //DO NOT CHANGE THE PUBLIC METHODS!!

        Vector3 r2;
        public float TwistMin { set => _twistMin = value; }
        public float TwistMax { set => _twistMax = value; }
        public float SwingMin { set => _swingMin = value; }
        public float SwingMax { set => _swingMax = value; }

        float[] _theta, _sin, _cos;

        public void TestLogging(string objectName)
        {


            Debug.Log("hello, I am initializing my Octopus Controller in object " + objectName);


        }

        public void Init(Transform[] tentacleRoots, Transform[] randomTargets)
        {
            _tentacles = new MyTentacleController[tentacleRoots.Length];

            // foreach (Transform t in tentacleRoots)
            for (int i = 0; i < tentacleRoots.Length; i++)
            {

                _tentacles[i] = new MyTentacleController();
                _tentacles[i].LoadTentacleJoints(tentacleRoots[i], TentacleMode.TENTACLE);
                //TODO: initialize any variables needed in ccd
            }
            _randomTargets = randomTargets;
            //TODO: use the regions however you need to make sure each tentacle stays in its region

        }


        public void NotifyTarget(Transform target, Transform region)
        {
            _currentRegion = region;
            _target = target;
        }

        public void NotifyShoot() {
            //TODO. what happens here?

            Debug.Log("Shoot");
            start = 0;
            end = 3;
            isShooting = true;

        }


        public void UpdateTentacles()
        {
            //TODO: implement logic for the correct tentacle arm to stop the ball and implement CCD method
            update_ccd();
            if (isShooting == true)
            {
                start+=Time.deltaTime;
                if (start > end)
                {
                    start = 0;
                    isShooting = false;
                }
            }
        }




        #endregion


        #region private and internal methods
        //todo: add here anything that you need

        void update_ccd()
        {
            for (int i = 0; i < _tentacles.Length; i++)
            {
                _theta = new float[_tentacles[i].Bones.Length];
                _sin = new float[_tentacles[i].Bones.Length];
                _cos = new float[_tentacles[i].Bones.Length];
                {
                    // starting from the second last joint (the last being the end effector)
                    // going back up to the root
                    for (int j = _tentacles[i].Bones.Length - 2; j >= 0; j--)
                    {
                        // The vector from the ith joint to the end effector
                        Vector3 r1 = _tentacles[i].Bones[_tentacles[i].Bones.Length - 1].transform.position - _tentacles[i].Bones[j].transform.position;

                        // The vector from the ith joint to the target
                        if (isShooting == true)
                        {
                             r2 = _target.transform.position - _tentacles[i].Bones[j].transform.position;

                        }
                        else
                        {
                             r2 = _randomTargets[i].transform.position - _tentacles[i].Bones[j].transform.position;
                        }
                        // to avoid dividing by tiny numbers
                        if (r1.magnitude * r2.magnitude <= 0.001f)
                        {

                            // cos ? sin? 
                            _cos[j] = 1;
                            _sin[j] = 0;

                        }
                        else
                        {
                            // find the components using dot and cross product
                            _cos[j] = Vector3.Dot(r1, r2) / (r1.magnitude * r2.magnitude);
                            _sin[j] = Vector3.Cross(r1, r2).magnitude / (r1.magnitude * r2.magnitude);

                        }

                        Vector3 axis = Vector3.Cross(r1, r2).normalized;

                        // find the angle between r1 and r2 (and clamp values if needed avoid errors)
                        _theta[j] = Mathf.Acos(Mathf.Clamp(_cos[j], -1, 1));


                        //Optional. correct angles if needed, depending on angles invert angle if sin component is negative
                        if (_sin[j] < 0.0f)
                            _theta[j] *= -1.0f;

                        // obtain an angle value between -pi and pi, and then convert to degrees
                        if (_theta[j] > Mathf.PI)
                        {
                            _theta[j] -= Mathf.PI * 2;
                        }
                        if (_theta[j] < -Mathf.PI)
                        {
                            _theta[j] += Mathf.PI * 2;
                        }

                        _theta[j] *= Mathf.Rad2Deg;
                        if(_theta[j] > 15.0f)
                        {
                            _theta[j] = 15;
                        }
                        else if (_theta[j] < -15)
                        {
                            _theta[j] = -15;
                        }

                        // rotate the ith joint along the axis by theta degrees in the world space.
                        _tentacles[i].Bones[j].transform.Rotate(axis, _theta[j], Space.World);


                        Quaternion twist = new Quaternion(0, _tentacles[i].Bones[j].transform.localRotation.y, 0, _tentacles[i].Bones[j].transform.localRotation.w);

                        twist = twist.normalized;

                        Quaternion swing = _tentacles[i].Bones[j].transform.localRotation * Quaternion.Inverse(twist);



                        _tentacles[i].Bones[j].transform.localRotation = swing.normalized;

                    }


                }


            }
        }
    }
}


        

        #endregion







