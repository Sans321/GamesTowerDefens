using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum projectType
{
    rock, arrow, fireball
}

public class Project : MonoBehaviour
{
    [SerializeField]
    int attackDam;

    [SerializeField]
    projectType pType;

    public int AttackDamage
    {
        get
        {
            return attackDam;
        }


    }
    public projectType Ptype
    {
        get
        {
            return pType;
        }
    }



}
