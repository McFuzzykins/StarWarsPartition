using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier
{
    //To change mat
    public MeshRenderer soldierMeshRenderer;

    //move soldier
    public Transform soldierTrans;

    //Speed of soldier
    protected float walkSpeed;

    //linked list stuff
    public Soldier prevSoldier;
    public Soldier nextSoldier;

    //enemy doesn't need any outside info
    public virtual void Move()
    {

    }

    //friendly has to move which soldier is closest
    public virtual void Move(Soldier soldier)
    {

    }
}
