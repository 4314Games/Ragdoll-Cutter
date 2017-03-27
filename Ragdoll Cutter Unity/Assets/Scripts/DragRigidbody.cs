using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        const float k_Spring = 5000.0f;
        const float k_Damper = 0f;
        public const float k_Drag = 10000.0f;
        public const float k_AngularDrag = 5.0f;
        const float k_Distance = 0.2f;
        const bool k_AttachToCenterOfMass = false;
        RaycastHit hit = new RaycastHit();
        private SpringJoint m_SpringJoint;
        private bool hitMoveable;
        private bool rotating;
        Camera mainCam;
        GameObject ragDoll;
        bool setRoat;   //So i can set previous rotation to new vec3 instead of using old one.
        float score;
        float time;

        void Start()
        {
            mainCam = FindCamera(); //Sets cam from component
            ragDoll = GameObject.Find("Mannequin_5_3_Optimized (1)");   //finds the ragdoll gameobject
        }


        private void Update()
        {
            if (Input.GetMouseButtonUp(0))  //If mouse up.....
            {
                if (hitMoveable) StartCoroutine("FreezeAllMovement");   //Freeze ragdoll movement
                if (rotating)
                {
                    CancelInvoke("RotateRagdollAntiClockwise");         //Stop characters rotation
                    CancelInvoke("RotateRagdollClockwise");
                    setRoat = false;
                }
            }
            if (Input.GetMouseButtonDown(0))            //Finds if an object is hit on mouse click
            {
                Hitter();
            }
        }

        IEnumerator FreezeAllMovement()     //Freezes all movements after player finished moving ragdoll
        {
            yield return new WaitForSeconds(0.08f);
            hit.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            if (hit.collider.transform.parent.GetComponent<Rigidbody>()) hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (hit.collider.transform.parent.parent.GetComponent<Rigidbody>()) hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            print("Movement Froezen");
        }

        bool Hitter()
        {
            print("HitterStart");
            hit = new RaycastHit();
            // We need to actually hit an object
            if (!Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition).origin,
                mainCam.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                Physics.DefaultRaycastLayers))
            {
                return false;
            }

            if (hit.collider.tag == "Arm" || hit.collider.tag == "Leg") //If its a part of the ragdoll prepare to move
            {
                if (!m_SpringJoint)
                {
                    var go = new GameObject("Rigidbody dragger");
                    Rigidbody body = go.AddComponent<Rigidbody>();
                    m_SpringJoint = go.AddComponent<SpringJoint>();
                    body.isKinematic = true;
                }

                //Arms
                if (hit.collider.name == "Hand_L" || hit.collider.name == "Hand_R")
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hitMoveable = true;
                }
                else if (hit.collider.name == "ForeArm_L" || hit.collider.name == "ForeArm_R")
                {
                    hit.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    hitMoveable = true;

                }
                //Feet
                else if (hit.collider.name == "Foot_L" || hit.collider.name == "Foot_R")
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hitMoveable = true;
                }
                else if (hit.collider.name == "Shin_L" || hit.collider.name == "Shin_R")
                {
                    hit.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    hitMoveable = true;
                }
                //Head
                else if (hit.collider.name == "Head")
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hitMoveable = true;
                }                
                StartCoroutine("DragObject", hit.distance);
            }
            if (hit.collider.name == "TurnAnticlockwise")   //If rotate button hit rotate
            {
                InvokeRepeating("RotateRagdollAntiClockwise", 0.02f, 0.02f);
                rotating = true;
                print("Rotating");
            }
            else if (hit.collider.name == "TurnClockwise")  //If rotate button hit rotate
            {
                InvokeRepeating("RotateRagdollClockwise", 0.02f, 0.02f);
                rotating = true;
                print("Rotating");                
            }
            return true;    //Was using this method as a bool. Not anymore so just return. 
        } 

        private IEnumerator DragObject(float distance)  //Drags the selected body part of the ragdoll
        {
            print("DragSarting");
            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;

            m_SpringJoint.spring = k_Spring;
            m_SpringJoint.damper = k_Damper;
            m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.connectedBody = hit.rigidbody;

            var oldDrag = m_SpringJoint.connectedBody.drag;
            var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = k_Drag;
            m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
            var mainCamera = FindCamera();           
            while (Input.GetMouseButton(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                m_SpringJoint.transform.position = ray.GetPoint(distance);
                StartCoroutine(RotationDifference(hit.transform.rotation.x));
               // print(score);
                yield return null;
            }
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = oldDrag;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody = null;
            }
            print("DragObjectFinisihed");          
        }

        private Camera FindCamera() //Returns camera component
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }
            return Camera.main;
        }
        
        void RotateRagdollAntiClockwise()   //Rotate Anit Clockwise
        {
            ragDoll.transform.RotateAround(ragDoll.GetComponent<Collider>().bounds.center, new Vector3(0, 0, 0.5f), 1.0f);
        }
        void RotateRagdollClockwise()       //Rotate Clockwise
        {
            ragDoll.transform.RotateAround(ragDoll.GetComponent<Collider>().bounds.center, new Vector3(0, 0, 0.5f), -1.0f);
        }

        IEnumerator RotationDifference(float rotFloat)
        {
            yield return new WaitForSeconds(0.1f);
            if(setRoat == false)
            {
                time = 1;
                setRoat = true;                   
            }
            time += Time.deltaTime * 100;
            //if (time >= 3) time = 3;
            if(hit.collider.tag == "Arm")
            {
                print(Mathf.Abs((hit.transform.rotation.x - rotFloat) / time * 100));
                score += Mathf.Abs((hit.transform.rotation.x - rotFloat)  / time * 100);
            }
        }
    }
}
