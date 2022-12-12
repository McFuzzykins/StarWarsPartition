using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Soldier
{
    //position soldier is heading for when moving
    Vector3 curTarget;

    //the position the soldier had before it moved, so we can change cells if need be
    Vector3 oldPos;

    //width of map to generate random coords
    float mapWidth;

    //grid
    Grid grid;

    //Init enemy
    public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
    {
        //save what we need
        this.soldierTrans = soldierObj.transform;

        this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();

        this.mapWidth = mapWidth;

        this.grid = grid;

        //add this unit to grid
        grid.Add(this);

        //Init old pos
        oldPos = soldierTrans.position;

        this.walkSpeed = 5f;

        //give random coord
        GetNewTarget();
    }

    public override void Move()
    {
        //move towards target
        soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

        //See if the cube has moved to another cell
        grid.Move(this, oldPos);

        //Save oldPos
        oldPos = soldierTrans.position;

        //if soldier has reached target, find new
        if ((soldierTrans.position - curTarget).magnitude < 1f)
        {
            GetNewTarget();
        }
    }

    //give enemy new target 
    void GetNewTarget()
    {
        curTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

        //get rotated, idiot
        soldierTrans.rotation = Quaternion.LookRotation(curTarget - soldierTrans.position);
    }
}
