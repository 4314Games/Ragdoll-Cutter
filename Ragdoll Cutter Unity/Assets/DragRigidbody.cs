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


        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.name != "") StartCoroutine("giveMeABreak");
            }

            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            if (Hitter() == false) return;

            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;

            m_SpringJoint.spring = k_Spring;
            m_SpringJoint.damper = k_Damper;
            m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.connectedBody = hit.rigidbody;

            StartCoroutine("DragObject", hit.distance);


        }

        IEnumerator giveMeABreak()
        {
            yield return new WaitForSeconds(0.08f);
            hit.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        bool Hitter()
        {
            var mainCamera = FindCamera();
            hit = new RaycastHit();
            // We need to actually hit an object
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))
            {
                return false;
            }
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                return false;
            }

            if (!m_SpringJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                Rigidbody body = go.AddComponent<Rigidbody>();
                m_SpringJoint = go.AddComponent<SpringJoint>();
                body.isKinematic = true;
            }

            //Arms
            if (hit.collider.name == "Hand_L")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                // hit.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            }
            else if (hit.collider.name == "Hand_R")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                // hit.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            //Legs
            else if (hit.collider.name == "Foot_L" || hit.collider.name == "Foot_R")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
                hit.collider.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationY ;
                hit.collider.transform.parent.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            //Head
            else if(hit.collider.name == "Head")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
            else if(hit.collider.name == "Chest")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            }
            else if (hit.collider.name == "Hips")
            {
                hit.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
               // if (hit.transform.position.y < 1) hit.transform.position = new Vector3(hit.transform.position.x, 1, hit.transform.position.z);
            }
            else return false;

            print("Hitting");
            return true;

        }


        private IEnumerator DragObject(float distance)
        {
            var oldDrag = m_SpringJoint.connectedBody.drag;
            var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = k_Drag;
            m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
            var mainCamera = FindCamera();
            while (Input.GetMouseButton(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                m_SpringJoint.transform.position = ray.GetPoint(distance);
                yield return null;
            }
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = oldDrag;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody = null;
            }
            
        }


        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }

       
    }
}
