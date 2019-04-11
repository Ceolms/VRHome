using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeublePreview : MonoBehaviour
{
    public Texture2D previewImg;
    public string nomMeuble;

    // Start is called before the first frame update
    void Start()
    {
        nomMeuble = this.name;
        //previewImg = Resources.Load<Sprite>("Sprites/Furnitures/img_" + nomMeuble) ;
        previewImg = UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/Furnitures/BedRoom/" + nomMeuble));
        if(previewImg == null) previewImg = UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/Furnitures/LivingRoom/" + nomMeuble));
        if (previewImg == null) previewImg = UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/Furnitures/Kitchen/" + nomMeuble));
        if (previewImg == null) previewImg = UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/Furnitures/BathRoom/" + nomMeuble));
       // Debug.Log(previewImg);
        this.GetComponent<RawImage>().texture = previewImg;
        this.GetComponentInChildren<Text>().text = nomMeuble;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

