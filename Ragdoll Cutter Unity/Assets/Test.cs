using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    bool mouse;
    private Camera FindCamera()
    {
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
    void Update()
    {

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var mainCamera = FindCamera();

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (
            !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                             mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                             Physics.DefaultRaycastLayers))
        {
            return;
        }
        //    if (Input.GetButtonDown("Fire1"))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray)) { }

        //    }

        //    if (mouse) GetComponent<Rigidbody>().MovePosition(Input.mousePosition);
        //    mouse = false;
        //}
        //void OnMouseDrag()
        //{
        //    mouse = true;
        //}
        //void FixedUpdate()
        //{

        //}
    }
}