using System.Globalization;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager instance;

    public List<SaveableObject> SaveableObjects { get; private set; }

    public static SaveGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SaveGameManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        SaveableObjects = new List<SaveableObject>();
    }

    public void Save()
    {
        Debug.Log("Save");
        PlayerPrefs.SetInt("ObjectCount", SaveableObjects.Count);
        for (int i = 0; i < SaveableObjects.Count; i++)
        {
            SaveableObjects[i].Save(i);
        }
    }

    public void Load()
    {
        Debug.Log("Loading item");
        foreach (SaveableObject obt in SaveableObjects)
        {
            if(obt!= null)
            {
                Destroy(obt.gameObject);
            }
        }
        SaveableObjects.Clear();
        int ObjectCount = PlayerPrefs.GetInt("ObjectCount");
        for (int i = 0; i < ObjectCount; i++)
        {
            string[] value = PlayerPrefs.GetString(i.ToString()).Split('+');
            GameObject tmp = null;

            switch (value[0])
            {

                case "specific":
                    //tmp = Instantiate(Resources.Load(value[4]) as GameObject);

                    string[] pathAsset = AssetDatabase.FindAssets("t:prefab "+value[4],new[] {"Assets/Resources"});

                    /*
                    for (int j = 0; j < pathAsset.Length; j++)
                    {
                        String s = AssetDatabase.GUIDToAssetPath(pathAsset[j]);
                        AssetDatabase.

                        if(s == value[4])
                    } */


                    string path = AssetDatabase.GUIDToAssetPath(pathAsset[0]);
                    path = path.Replace("Assets/Resources/", "");
                    path = path.Replace(".prefab", "");
                    Debug.Log("path: " + path);
                    tmp = Instantiate(Resources.Load(path) as GameObject);
                    if (tmp.tag == "House") tmp.transform.SetParent(GameObject.Find("Home").transform);
                   // else tmp.transform.SetParent(GameObject.FindGameObjectWithTag("House").transform);
                    Debug.Log("obj:2" + tmp);
                        break;

                default:
                    tmp = Instantiate(Resources.Load("wall") as GameObject);
                    break;
            }


            if (tmp != null)
            {
                tmp.GetComponent<SaveableObject>().Place(value);
            }

            GameObject player = GameObject.Find("Player");
            player.transform.SetPositionAndRotation(GameObject.FindGameObjectWithTag("House").transform.Find("position").transform.position, Quaternion.identity);
        }
    }

    public Vector3 StringToVector(string value)
    {
        /*Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ",";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;*/
        value = value.Trim(new char[] { '(', ')' });// UN-parse vector coordinates
        value = value.Replace(" ", ""); //sanitize value
        string[] pos = value.Split(','); //coordonnees
        float q1 = Mathf.Abs(float.Parse(pos[0])) + Mathf.Abs(float.Parse(pos[1])) / 10;
        if (pos[0].Contains("-"))
            q1 = -q1;
        float q2 = Mathf.Abs(float.Parse(pos[2])) + Mathf.Abs(float.Parse(pos[3])) / 10;
        if (pos[2].Contains("-"))
            q2 = -q2;
        float q3 = Mathf.Abs(float.Parse(pos[4])) + Mathf.Abs(float.Parse(pos[5])) / 10;//fait pour éviter les problemes de parametres windows
        if (pos[4].Contains("-"))
            q3 = -q3;
        return new Vector3(q1,q2,q3);
    }

    public Quaternion StringToQuaternion(string value)
    {
        //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        value = value.Trim(new char[] { '(', ')' });// UN-parse vector coordinates
        value = value.Replace(" ", ""); //sanitize value
        string[] pos = value.Split(','); //coordonnees
        float q1 = Mathf.Abs(float.Parse(pos[0])) + Mathf.Abs(float.Parse(pos[1])) / 10;
        if (pos[0].Contains("-"))
            q1 = -q1;
        float q2 = Mathf.Abs(float.Parse(pos[2])) + Mathf.Abs(float.Parse(pos[3])) / 10;
        if (pos[2].Contains("-"))
            q2 = -q2;
        float q3 = Mathf.Abs(float.Parse(pos[4])) + Mathf.Abs(float.Parse(pos[5])) / 10;
        if (pos[4].Contains("-"))
            q3 = -q3;
        float q4 = Mathf.Abs(float.Parse(pos[6])) + Mathf.Abs(float.Parse(pos[7])) / 10; //fait pour éviter les problemes de parametres windows
        if (pos[6].Contains("-"))
            q4 = -q4;
        return new Quaternion(q1, q2 , q3, q4); 
    }

}
