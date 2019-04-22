
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObject : SaveableObject
{

    [SerializeField]
    [HideInInspector]
    private string specificName;

    public override void Place(string[] values)
    {
        specificName = saveMore;
        base.Place(values);
    }
    public override void Save(int id)
    {
        saveMore = this.name;
        saveMore = saveMore.Replace("(Clone)", "");
        base.Save(id);
    }

    void Update()
    {
        //GameObject gm=null;
        //gm.GetComponent<SaveGameManager>().Save();
    }

}


