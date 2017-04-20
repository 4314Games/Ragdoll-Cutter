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
        private bool hitMoveable;   //If hit is a moveable/body part
        private bool rotating;  //if hit is rotating button
        Camera mainCam;
        GameObject ragDoll;
        bool setRoat;   //So i can set previous rotation to new vec3 instead of using old one.
        public Gameplay gamePlayScript;
        float buttonHeldTime;   //How long button is held down, used for score calculation
        GameObject bodyPartRotating; //For dancing score some body parts cant use hit so I set the part here. 
        bool sameBodyPart;  //If the body part selected is the same selected again, then no score will be added
        public int scoreTimerMax;   //Stop scoring after this amount of time

        void Start()
        {
            mainCam = FindCamera(); //Sets cam from component
            ragDoll = GameObject.Find("Mannequin_5_3_Optimized (1)");   //finds the ragdoll gameobject
        }


        private void Update()
        {
            if (Input.GetMouseButtonUp(0))  //If mouse up.....
            {
                if (hitMoveable)
                {
                    StartCoroutine("FreezeAllMovement");   //Freeze ragdoll movement
                    setRoat = false;
                }
                if (rotating)
                {
                    CancelInvoke("RotateRagdollAntiClockwise");         //Stop characters rotation
                    CancelInvoke("RotateRagdollClockwise");
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
            if (hit.collider.transform.parent.GetComponent<Rigidbody>()) hit.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            if (hit.collider.transform.parent.GetComponent<Rigidbody>()) hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (hit.collider.transform.parent.parent.GetComponent<Rigidbody>()) hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            print("Movement Froezen");
        }

        public bool Hitter()
        {
            
            hit = new RaycastHit();
            // We need to actually hit an object
            if (!Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition).origin,
                mainCam.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                Physics.DefaultRaycastLayers))
            {
                return false;
            }
            
            if (hit.collider.gameObject.layer != 8) return false;

            print("HitterStart");

            if (hit.collider.tag == "Arm" || hit.collider.tag == "Leg" || hit.collider.tag == "Head") //If its a part of the ragdoll prepare to move
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
                    BodyPartChecker(hit.collider.gameObject);
                    bodyPartRotating = hit.collider.gameObject;
                    hitMoveable = true;
                }
                else if (hit.collider.name == "ForeArm_L" || hit.collider.name == "ForeArm_R")
                {
                    hit.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    BodyPartChecker(hit.transform.parent.gameObject);
                    bodyPartRotating = hit.transform.parent.gameObject;
                    hitMoveable = true;

                }
                //Feet
                else if (hit.collider.name == "Foot_L" || hit.collider.name == "Foot_R")
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    BodyPartChecker(hit.collider.gameObject);
                    bodyPartRotating = hit.collider.gameObject;
                    hitMoveable = true;
                }
                else if (hit.collider.name == "Shin_L" || hit.collider.name == "Shin_R")
                {
                    hit.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    BodyPartChecker(hit.collider.gameObject);
                    bodyPartRotating = hit.collider.gameObject;
                    hitMoveable = true;
                }
                //Head
                else if (hit.collider.name == "Head")
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    BodyPartChecker(hit.collider.gameObject);
                    bodyPartRotating = hit.collider.gameObject;
                    hitMoveable = true;
                }
                StartCoroutine("DragObject", hit.distance);
            }
            //Rotate Buttons
            if (hit.collider.name == "TurnAnticlockwise")   //If rotate button hit rotate
            {
                InvokeRepeating("RotateRagdollAntiClockwise", 0.02f, 0.02f);
                rotating = true;
                BodyPartChecker(ragDoll);
                bodyPartRotating = ragDoll;
                print("Rotating");
            }
            else if (hit.collider.name == "TurnClockwise")  //If rotate button hit rotate
            {
                InvokeRepeating("RotateRagdollClockwise", 0.02f, 0.02f);
                rotating = true;
                BodyPartChecker(ragDoll);
                bodyPartRotating = ragDoll;
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
                if (hit.collider.tag == "Arm" || hit.collider.tag == "Head") StartCoroutine(RotationDifference(bodyPartRotating.transform.rotation.x));
                else if (hit.collider.tag == "Leg") StartCoroutine(RotationDifference(bodyPartRotating.transform.rotation.z));
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
            StartCoroutine(RotationDifference(bodyPartRotating.transform.rotation.z));
        }
        void RotateRagdollClockwise()       //Rotate Clockwise
        {
            ragDoll.transform.RotateAround(ragDoll.GetComponent<Collider>().bounds.center, new Vector3(0, 0, 0.5f), -1.0f);
            StartCoroutine(RotationDifference(bodyPartRotating.transform.rotation.z));
        }

        IEnumerator RotationDifference(float rotFloat)  //Calculates Score by getting diffence between rotation of ragdoll part
        {
            yield return new WaitForSeconds(0.1f);  //Gives time for there to be a difference in rotation poisiton
            if (setRoat == false)    //resets time on mouse up 
            {
                buttonHeldTime = 0;
                setRoat = true;
            }
            //Time used for score control
            if (buttonHeldTime >= scoreTimerMax) buttonHeldTime = scoreTimerMax;    //No need to go higher then 3, score stops adding after 3 seconds
            else buttonHeldTime += Time.deltaTime;
            if (!sameBodyPart)  //if bodypart  is the same as just held so score added
            {
                //Add score based on rotation pos compared to old pos
                if (hit.collider.tag == "Arm" || hit.collider.tag == "Head")   //Arm/Head rotations use x axis
                {
                    gamePlayScript.score += (Mathf.Abs(bodyPartRotating.transform.rotation.z - rotFloat) * (gamePlayScript.multiplier + 1) - (Mathf.Abs(bodyPartRotating.transform.rotation.z - rotFloat) * (buttonHeldTime / scoreTimerMax)) * (gamePlayScript.multiplier + 1));
                }
                else if (hit.collider.tag == "Leg" || rotating)  //Leg rotation uses z axis
                {
                    gamePlayScript.score += (Mathf.Abs(bodyPartRotating.transform.rotation.x - rotFloat) * (gamePlayScript.multiplier + 1) - (Mathf.Abs(bodyPartRotating.transform.rotation.x - rotFloat) * (buttonHeldTime / scoreTimerMax) * (gamePlayScript.multiplier + 1)));
                }
            }
        }

        void BodyPartChecker(GameObject p_gameObject)
        {
            if (bodyPartRotating == p_gameObject) sameBodyPart = true;
            else sameBodyPart = false;
        }
    }
}
