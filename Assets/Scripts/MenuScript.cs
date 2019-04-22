using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class MenuScript : MonoBehaviour
{
    public List<GameObject> meublesBedroom;
    public List<GameObject> meublesBathroom;
    public List<GameObject> meublesKitchen;
    public List<GameObject> meublesLivingroom;
    public List<GameObject> blueprints;

    [HideInInspector]
    public bool menuVisible = false;
    [HideInInspector]
    public string menuActuel;
    public GameObject canvas;
    public GameObject scrollViewPanel;
    public GameObject itemsPanel;
    public GameObject buttonPrefab;

    public GameObject btnLivingRoom;
    public GameObject btnBedRoom;
    public GameObject btnKitchen;
    public GameObject btnBathRoom;
    public GameObject btnBlueprints;

    // Start is called before the first frame update
    void Start()
    {
        meublesBedroom = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Furnitures/BedRoom"));
        meublesBathroom = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Furnitures/BathRoom"));
        meublesKitchen = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Furnitures/Kitchen"));
        meublesLivingroom = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Furnitures/LivingRoom"));
        blueprints = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Houses/HousesButtons"));

        scrollViewPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.ClickX.GetStateDown(SteamVR_Input_Sources.Any) || SteamVR_Actions._default.ClickA.GetStateDown(SteamVR_Input_Sources.Any))
        {
            menuVisible = !menuVisible;
            if (menuVisible)
            {
                canvas.SetActive(true);
            }
            else
            {
                canvas.SetActive(false);
                menuActuel = "";
            }
        }
    }

    public void ClickBtn(string btnClicked)
    {
        if (btnClicked == "btnLivingRoom")
        {
            CreerBoutons("LivingRoom");
            menuActuel = "menuLivingRoom";
        }
        else if (btnClicked == "btnBedRoom")
        {
            CreerBoutons("BedRoom");
            menuActuel = "menuBedRoom";
        }
        else if (btnClicked == "btnKitchen")
        {
            CreerBoutons("Kitchen");
            menuActuel = "menuKitchen";
        }
        else if (btnClicked == "btnBathRoom")
        {
            CreerBoutons("BathRoom");
            menuActuel = "menuBathRoom";
        }
        else if (btnClicked == "btnBlueprints")
        {
            CreerBoutons("Blueprints");
            menuActuel = "menuBlueprints";
        }
        else if (btnClicked == "btnLoad")
        {
            GameManager.Instance.GetComponent<SaveGameManager>().Load();
        }
        else if (btnClicked == "btnSave")
        {
            GameManager.Instance.GetComponent<SaveGameManager>().Save();
        }
    }


    public void CreerBoutons(string menu)
    {
        Debug.Log(scrollViewPanel);
        scrollViewPanel.SetActive(true);
        
        foreach(Transform child in itemsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (menu == "BedRoom")
        {
            foreach (GameObject m in meublesBedroom)
            {
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.SetParent(itemsPanel.transform, false);
                newButton.GetComponentInChildren<Text>().text = "BedRoom";
                newButton.name = m.name;
               // newButton.GetComponent
            }
        }
        else if (menu == "Kitchen")
        {
            foreach (GameObject m in meublesKitchen)
            {
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.SetParent(itemsPanel.transform, false);
                newButton.GetComponentInChildren<Text>().text = "Kitchen";
                newButton.name = m.name;
            }
        }
        else if (menu == "LivingRoom")
        {
            foreach (GameObject m in meublesLivingroom)
            {
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.SetParent(itemsPanel.transform, false);
                newButton.GetComponentInChildren<Text>().text = "LivingRoom";
                newButton.name = m.name;
            }
        }
        else if(menu == "BathRoom") //BathRoom
        {
            foreach (GameObject m in meublesBathroom)
            {
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.SetParent(itemsPanel.transform, false);
                newButton.GetComponentInChildren<Text>().text = "BathRoom";
                newButton.name = m.name;
            }
        }
        else if (menu == "Blueprints") //BathRoom
        {
            foreach (GameObject b in blueprints)
            {
                GameObject newButton = Instantiate(b) as GameObject;
                newButton.transform.SetParent(itemsPanel.transform, false);
                newButton.name = newButton.GetComponent<BlueprintHolder>().nom;
            }
        }
    }
}
