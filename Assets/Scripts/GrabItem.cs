using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GrabItem : MonoBehaviour
{
    public GameObject CubePointHand;
    public GameObject CubePointFinger;
    public GameObject pointedObject;
    public GameObject grabbedObject;
    public string hand = "left";

    LineRenderer line;
    public bool isRaycasting;
    public bool isGrabbing;

    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = false;
        isRaycasting = false;
        

        if (hand == "left") UpdateLeft();
        else UpdateRight();

        if (pointedObject) ShowBounds();
    }

    public void UpdateLeft()
    {
        if (SteamVR_Actions._default.GrabGripLeft.GetStateDown(SteamVR_Input_Sources.Any) && pointedObject != null)
        {
            Debug.Log("grabbing down");
            grabbedObject = pointedObject;
            isGrabbing = true;
            //pointedObject = null;
            grabbedObject.transform.parent = this.transform;
        }
        if (SteamVR_Actions._default.GrabGripLeft.GetStateUp(SteamVR_Input_Sources.Any) && grabbedObject != null)
        {
            Debug.Log("grabbing up");
            grabbedObject.transform.parent = GameObject.Find("Home").transform;
            grabbedObject = null;
            isGrabbing = false;
        }

        if (!isGrabbing)
        {
            if (!SteamVR_Actions._default.PointingIndexLeft.GetState(SteamVR_Input_Sources.Any))
            {
                isRaycasting = true;
                line.enabled = true;
                CastRay();

            }
            else pointedObject = null;
            float x = 0;
            if (SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x < -0.2)
                x = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x;
            GameObject.Find("Player").transform.Rotate(0, x, 0);
        }
        else
        {
            float x = 0;
            float y = 0;


            if (SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x < -0.2)
                x = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x;
            grabbedObject.transform.Rotate(0, x, 0);

            if (SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).y > 0.2 || SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).y < -0.2)
                y = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).y;

            if (y < 0)
            {
                float speed = -y * Time.deltaTime * 5;
                grabbedObject.transform.position = Vector3.MoveTowards(grabbedObject.transform.position, this.transform.position, speed);
            }
            else if (y >0)
            {
                float speed = y * Time.deltaTime * 5;
                Vector3 end = (CubePointHand.transform.position) + (1000f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                grabbedObject.transform.position = Vector3.MoveTowards(grabbedObject.transform.position, end, speed);
            }
        }
    }

    public void UpdateRight()
    {
        if (SteamVR_Actions._default.GrabGripRight.GetStateDown(SteamVR_Input_Sources.Any) && pointedObject != null)
        {
            Debug.Log("grabbing down");
            grabbedObject = pointedObject;
            isGrabbing = true;
            //pointedObject = null;
            grabbedObject.transform.parent = this.transform;
        }
        if (SteamVR_Actions._default.GrabGripRight.GetStateUp(SteamVR_Input_Sources.Any) && grabbedObject != null)
        {
            Debug.Log("grabbing up");
            grabbedObject.transform.parent = GameObject.Find("Home").transform;
            grabbedObject = null;
            isGrabbing = false;
        }

        if (!isGrabbing)
        {
            if (!SteamVR_Actions._default.PointingIndexRight.GetState(SteamVR_Input_Sources.Any))
            {
                isRaycasting = true;
                line.enabled = true;
                CastRay();
            }
            else if (pointedObject)
            {
                HideBounds();
                pointedObject = null;
            } 
        }
        else
        {
            float x = 0;
            float y = 0;

            if (SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x < -0.2)
                x = SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x;
            grabbedObject.transform.Rotate(0, x, 0);

            if (SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).y > 0.2 || SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).y < -0.2)
                y = SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).y;

            if (y < 0)
            {
                float speed = -y * Time.deltaTime * 5;
                grabbedObject.transform.position = Vector3.MoveTowards(grabbedObject.transform.position, this.transform.position, speed);
            }
            else if (y > 0)
            {
                float speed = y * Time.deltaTime * 5;
                Vector3 end = (CubePointHand.transform.position) + (1000f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                grabbedObject.transform.position = Vector3.MoveTowards(grabbedObject.transform.position, end, speed);
            }
        }
    }

    public void CastRay()
    {
        Ray ray = new Ray(CubePointHand.transform.position, CubePointFinger.transform.position - CubePointHand.transform.position);
        Vector3 end = (CubePointHand.transform.position) + (1000f * (CubePointFinger.transform.position - CubePointHand.transform.position));

        RaycastHit rh;
        HideBounds();
        pointedObject = null;

        if (Physics.Raycast(ray, out rh, 1000f))
        {
            end = rh.point;
            
            if (rh.collider.gameObject.tag == "Movable")
            {
                //Debug.Log(rh.collider.gameObject);
                this.pointedObject = rh.collider.gameObject;
            }
        }

        line.SetPosition(0, CubePointHand.transform.position);
        line.SetPosition(1, end);
    }

    public void ShowBounds()
    {
        if(pointedObject)
             pointedObject.transform.Find("Overlay").GetComponent<MeshRenderer>().enabled = true;
    }

    public void HideBounds()
    {
        if (pointedObject)
            pointedObject.transform.Find("Overlay").GetComponent<MeshRenderer>().enabled = false;
    }

}
