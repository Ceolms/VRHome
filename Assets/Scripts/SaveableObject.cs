
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum ObjectType { specific, wall, prop }

public abstract class SaveableObject : MonoBehaviour
{
    protected string saveMore;

    [SerializeField]
    private ObjectType ObjectType;

    // Start is called before the first frame update
    private void Start()
    {
        SaveGameManager.Instance.SaveableObjects.Add(this);
    }
    public virtual void Save(int id)
    {
        PlayerPrefs.SetString(id.ToString(), ObjectType +"+"+ transform.position.ToString() + "+" +transform.localScale + "+" + transform.localRotation+"+"+ saveMore);
    }
 
    public virtual void Place(string[] values)
    {
        Debug.Log("placing" + values[4]);
       transform.position = SaveGameManager.Instance.StringToVector(values[1]);
        //transform.localScale = SaveGameManager.Instance.StringToVector(values[2]);
       transform.rotation = SaveGameManager.Instance.StringToQuaternion(values[3]);
    }
    public void DestroySaveable()
    {

    }
    
}
