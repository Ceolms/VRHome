using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHouse(GameObject blueprint)
    {
        Debug.Log(blueprint);
        GameObject old = GameObject.FindGameObjectWithTag("House");
        GameObject.DestroyImmediate(old);
        GameObject newHouse = Instantiate(blueprint) as GameObject;
        newHouse.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        newHouse.transform.SetParent(GameObject.Find("Home").transform);
        GameObject player = GameObject.Find("Player");
        player.transform.SetPositionAndRotation(newHouse.transform.Find("position").transform.position, Quaternion.identity);
    }
}
