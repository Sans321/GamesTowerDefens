using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    TowerControler towerObject;
    [SerializeField]
    Sprite dragSprite;
    [SerializeField]
    int towerPrice;


    public TowerControler TowerObject
    {
        get
        {



            return towerObject;
        }
    }
    public Sprite DragSprite
    {
        get
        {



            return dragSprite;
        }
    }


    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }

}
