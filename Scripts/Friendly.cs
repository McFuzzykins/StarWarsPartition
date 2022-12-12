using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : Soldier
{
    //init friendly 
    public Friendly(GameObject soldierObj, float mapWidth)
    {
        this.soldierTrans = soldierObj.transform;

        this.walkSpeed = 2f;
    }

    public override void Move(Soldier closestEnemy)
    {
        //Rotated again, punk
        soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);

        //Move towards closest enemy
        soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
    }
}
