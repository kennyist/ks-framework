using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Minimap : MonoBehaviour {

    public Texture2DArray mapBackground;

    public Transform player;

    public bool orientToPlayer = true;

    private KS_MapData mapData;



    public void Setup()
    {
        mapData = KS_MapData.Instance;

    }



}


public class KS_MapData
{
    private static KS_MapData instance;
    public static KS_MapData Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                return new KS_MapData();
            }
        }
    }

    public KS_MapData()
    {
        instance = this;
    }


}
