using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{
    private MenuScript scriptMenu;
    public GameObject CubePointHand;
    public GameObject CubePointFinger;
    public GameObject pointedObject;
    public GameObject grabbedObject;
    public string hand = "left";
    public Material matCircle;
    private string rotationType = "rotationX";

    LineRenderer line;
    public bool isRaycasting;
    public bool isGrabbing;
    private bool canTurn;

    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        scriptMenu = GameObject.Find("Interface").GetComponent<MenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = false;
        isRaycasting = false;

        GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.localPosition = new Vector3(0, 0, 0);
        if (!scriptMenu.menuVisible)
        {
            if (hand == "left") UpdateLeft();
            else UpdateRight();

            if (pointedObject) ShowBounds();
        }
        else
        {
            line.enabled = true;
            isRaycasting = true;
            CastRayUI();
        }
        
    }
    //update de la mian gauche 
    public void UpdateLeft()
    {
        if (SteamVR_Actions._default.GrabGripLeft.GetStateDown(SteamVR_Input_Sources.Any) && pointedObject != null) // on attrape un objet
        {
            //Debug.Log("grabbing down");
            grabbedObject = pointedObject;
            isGrabbing = true;
            //pointedObject = null;
            grabbedObject.transform.parent = this.transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (SteamVR_Actions._default.GrabGripLeft.GetStateUp(SteamVR_Input_Sources.Any) && grabbedObject != null) // on lache un objet
        {
            //Debug.Log("grabbing up");
            HideCircle(grabbedObject);
            grabbedObject.transform.parent = GameObject.Find("Home").transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject = null;
            isGrabbing = false;
        }

        if (!isGrabbing)
        {
            if (!SteamVR_Actions._default.PointingIndexLeft.GetState(SteamVR_Input_Sources.Any)) // on pointe l´index
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
            float x = 0;
            if (SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x < -0.2) // rotation perso
            {
                x = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x;
                if (canTurn)
                {
                     if(x > 0.2) GameObject.Find("Player").transform.Rotate(0, 20, 0);
                     else GameObject.Find("Player").transform.Rotate(0, -20, 0);
                    canTurn = false;
                }
            }
            x = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x;
            if ((x < 0.2 && x > -0.2) && !canTurn) canTurn = true;
        }
        else
        { // rotation de l´object tenu
            float x = 0;
            float y = 0;

            if(SteamVR_Actions._default.TriggerIndexLeft.GetStateDown(SteamVR_Input_Sources.Any)) // on change l'axe de rotation
            {
                if (rotationType == "rotationY") rotationType = "rotationX";
                else if (rotationType == "rotationX") rotationType = "rotationZ";
                else rotationType = "rotationY";
            }

            DrawCircle(grabbedObject, 1, .02f, rotationType);

            if (SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x < -0.2)
                x = SteamVR_Actions._default.JoystickLeft.GetAxis(SteamVR_Input_Sources.Any).x;

            if(rotationType == "rotationX") grabbedObject.transform.Rotate(x, 0, 0);
            else if (rotationType == "rotationY") grabbedObject.transform.Rotate(0, x, 0);
            else grabbedObject.transform.Rotate(0, 0, x);

            // on approche ou éloigne l'objet
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
            if (SteamVR_Actions._default.ClickY.GetStateDown(SteamVR_Input_Sources.Any)) // Suppression Objet
            {
                Destroy(grabbedObject);
                grabbedObject = null;
                pointedObject = null;
                isGrabbing = false;
            }
        }
    }

    //update de la main droite 
    public void UpdateRight()
    {
        if (SteamVR_Actions._default.GrabGripRight.GetStateDown(SteamVR_Input_Sources.Any) && pointedObject != null) // on attrape un objet
        {
            grabbedObject = pointedObject;
            isGrabbing = true;
            //pointedObject = null;
            grabbedObject.transform.parent = this.transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (SteamVR_Actions._default.GrabGripRight.GetStateUp(SteamVR_Input_Sources.Any) && grabbedObject != null) // on lache un objet
        {
            HideCircle(grabbedObject);
            grabbedObject.transform.parent = GameObject.Find("Home").transform;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject = null;
            isGrabbing = false;
        }

        if (!isGrabbing)
        {
            if (!SteamVR_Actions._default.PointingIndexRight.GetState(SteamVR_Input_Sources.Any)) // on pointe l´index
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
            float x = 0;
            if (SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x < -0.2) // rotation perso
            {
                x = SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x;
                if (canTurn)
                {
                    if (x > 0.2) GameObject.Find("Player").transform.Rotate(0, 20, 0);
                    else GameObject.Find("Player").transform.Rotate(0, -20, 0);
                    canTurn = false;
                }
            }
            x = SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x;
            if ((x < 0.2 && x > -0.2) && !canTurn) canTurn = true;
        }
        else
        { // rotation de l´object tenu
            float x = 0;
            float y = 0;
            
            if(SteamVR_Actions._default.TriggerIndexRight.GetStateDown(SteamVR_Input_Sources.Any)) // on change l'axe de rotation
            {
                if (rotationType == "rotationY") rotationType = "rotationX";
                else if(rotationType == "rotationX") rotationType = "rotationZ";
                else rotationType = "rotationY";
            }

            DrawCircle(grabbedObject, 1, .02f, rotationType);

            if (SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x > 0.2 || SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x < -0.2)
                x = SteamVR_Actions._default.JoystickRight.GetAxis(SteamVR_Input_Sources.Any).x;
            if (rotationType == "rotationX") grabbedObject.transform.Rotate(x, 0, 0);
            else if (rotationType == "rotationY") grabbedObject.transform.Rotate(0, x, 0);
            else grabbedObject.transform.Rotate(0, 0, x);

            // on approche ou éloigne l'objet
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
            if (SteamVR_Actions._default.ClickB.GetStateDown(SteamVR_Input_Sources.Any))
            {
                Destroy(grabbedObject);
                grabbedObject = null;
                pointedObject = null;
                isGrabbing = false;
            }
        }
    }

    // raycast visible qui part de l´index
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

    // Raycast sur l´UI
    public void CastRayUI()
    {
        Ray ray = new Ray(CubePointHand.transform.position, CubePointFinger.transform.position - CubePointHand.transform.position);
        Vector3 end = (CubePointHand.transform.position) + (1000f * (CubePointFinger.transform.position - CubePointHand.transform.position));

        RaycastHit rh;
        HideBounds();
        pointedObject = null;

        if (Physics.Raycast(ray, out rh, 1000f))
        {
            end = rh.point;

            if (rh.collider.gameObject.tag == "ButtonMenu")
            {

                if (hand == "left" && SteamVR_Actions._default.TriggerIndexLeft.GetStateDown(SteamVR_Input_Sources.Any))
                {
                  //  Debug.Log(rh.collider.gameObject.name);
                    scriptMenu.ClickBtn(rh.collider.gameObject.name);
                }
                else if (hand == "right" && SteamVR_Actions._default.TriggerIndexRight.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    scriptMenu.ClickBtn(rh.collider.gameObject.name);
                }
            }

            if (rh.collider.gameObject.tag == "ButtonMeuble")
            {
                GameObject prefabMeuble;
                GameObject meuble = null;

                if ((hand == "left" && SteamVR_Actions._default.GrabGripLeft.GetStateDown(SteamVR_Input_Sources.Any)))
                {
                    string nomMeuble = rh.collider.gameObject.GetComponent<MeublePreview>().nomMeuble;
                    scriptMenu.menuVisible = false;
                    scriptMenu.canvas.SetActive(false);
                    if (scriptMenu.menuActuel == "menuLivingRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/LivingRoom/" + nomMeuble);
                        //meuble = (GameObject)Instantiate(prefabMeuble, rh.collider.gameObject.transform.position, Quaternion.identity);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble,v, Quaternion.identity);
                        
                    }
                    else if (scriptMenu.menuActuel == "menuBedRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/BedRoom/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }
                    else if (scriptMenu.menuActuel == "menuKitchen")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/Kitchen/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }
                    else if (scriptMenu.menuActuel == "menuBathRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/BathRoom/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }

                }
                else if (hand == "right" && SteamVR_Actions._default.GrabGripRight.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    string nomMeuble = rh.collider.gameObject.GetComponent<MeublePreview>().nomMeuble;
                    scriptMenu.menuVisible = false;
                    scriptMenu.canvas.SetActive(false);
                    if (scriptMenu.menuActuel == "menuLivingRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/LivingRoom/" + nomMeuble);
                        //meuble = (GameObject)Instantiate(prefabMeuble, rh.collider.gameObject.transform.position, Quaternion.identity);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);

                    }
                    else if (scriptMenu.menuActuel == "menuBedRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/BedRoom/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }
                    else if (scriptMenu.menuActuel == "menuKitchen")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/Kitchen/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }
                    else if (scriptMenu.menuActuel == "menuBathRoom")
                    {
                        prefabMeuble = Resources.Load<GameObject>("Prefabs/Furnitures/BathRoom/" + nomMeuble);
                        Vector3 v = (CubePointHand.transform.position) + (15f * (CubePointFinger.transform.position - CubePointHand.transform.position));
                        meuble = (GameObject)Instantiate(prefabMeuble, v, Quaternion.identity);
                    }
                }
                if (meuble != null)
                {
                    isGrabbing = true;
                    grabbedObject = meuble;
                    grabbedObject.transform.parent = this.transform;
                }
            }
            else if (rh.collider.gameObject.tag == "ButtonBlueprint")
            {
                if ((hand == "left" && SteamVR_Actions._default.GrabGripLeft.GetStateDown(SteamVR_Input_Sources.Any)))
                {
                    scriptMenu.menuVisible = false;
                    scriptMenu.canvas.SetActive(false);
                    GameManager.Instance.LoadHouse(rh.collider.gameObject.GetComponent<BlueprintHolder>().blueprint);
                }
                else if (hand == "right" && SteamVR_Actions._default.GrabGripRight.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    scriptMenu.menuVisible = false;
                    scriptMenu.canvas.SetActive(false);
                    GameManager.Instance.LoadHouse(rh.collider.gameObject.GetComponent<BlueprintHolder>().blueprint);
                }    
            }
        }

        line.SetPosition(0, CubePointHand.transform.position);
        line.SetPosition(1, end);
    }

    public void DrawCircle(GameObject container, float radius, float lineWidth,string rotation)
    {
        GameObject overlay = container;
            //container.transform.Find("Overlay").gameObject;


        if (overlay.transform.Find("CircleObjet") == null)
        {
            GameObject circleObjet = new GameObject("CircleObjet");
            circleObjet.transform.position = container.transform.position;
            circleObjet.transform.SetParent(overlay.transform);
            circleObjet.transform.localPosition = new Vector3(0, 0.1f, 0);
            circleObjet.AddComponent<LineRenderer>();
        }
        LineRenderer line = overlay.transform.Find("CircleObjet").GetComponent<LineRenderer>();
        line.enabled = true;
        var segments = 360;
        
        line.enabled = true;
        line.material = matCircle;
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
        if (rotation == "rotationX")
        {
           Debug.Log(rotation);
           overlay.transform.Find("CircleObjet").localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (rotation == "rotationY")
        {
            Debug.Log(rotation);
            overlay.transform.Find("CircleObjet").localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (rotation == "rotationZ")
        {
            Debug.Log(rotation);
            overlay.transform.Find("CircleObjet").localRotation = Quaternion.Euler(90, 0, 0);
        }
    }

    public static void HideCircle(GameObject container)
    {

        foreach (Transform t in container.transform)
        {
            if (t.name == "CircleObjet") t.GetComponent<LineRenderer>().enabled = false;
        }
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
