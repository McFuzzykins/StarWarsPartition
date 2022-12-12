using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //need this to convert from world coords pos to cell pos
    int cellSize;

    //actual grid
    Soldier[,] cells;

    //init the grid
    public Grid(int mapWidth, int cellSize)
    {
        this.cellSize = cellSize;

        int numOfCells = mapWidth / cellSize;

        cells = new Soldier[numOfCells, numOfCells];
    }

    public void Add(Soldier soldier)
    {
        //determine which grid cell soldier is in
        int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
        int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

        //add soldier to front of list
        soldier.prevSoldier = null;
        soldier.nextSoldier = cells[cellX, cellZ];

        //Associate this cell with this soldier
        cells[cellX, cellZ] = soldier;

        if(soldier.nextSoldier != null)
        {
            //set this soldier to be prev soldierof next soldier of this soldier
            soldier.nextSoldier.prevSoldier = soldier;
        }
    }

    //Get the closest enemy from the grid
    public Soldier FindClosestEnemy(Soldier friendlySoldier)
    {
        //Determine which grid cell the friendly soldier is in
        int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
        int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

        //Get the first enemy in grid
        Soldier enemy = cells[cellX, cellZ];

        //Find the closest soldier of all in the linked list
        Soldier closestSoldier = null;

        float bestDistSqr = Mathf.Infinity;

        //Loop through the linked list
        while (enemy != null)
        {
            //The distance sqr between the soldier and this enemy
            float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;

            //If this distance is better than the previous best distance, then we have found an enemy that's closer
            if (distSqr < bestDistSqr)
            {
                bestDistSqr = distSqr;

                closestSoldier = enemy;
            }

            //Get the next enemy in the list
            enemy = enemy.nextSoldier;
        }

        return closestSoldier;
    }


    //A soldier in the grid has moved, so see if we need to update in which grid the soldier is
    public void Move(Soldier soldier, Vector3 oldPos)
    {
        //See which cell it was in 
        int oldCellX = (int)(oldPos.x / cellSize);
        int oldCellZ = (int)(oldPos.z / cellSize);

        //See which cell it is in now
        int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
        int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

        //If it didn't change cell, we are done
        if (oldCellX == cellX && oldCellZ == cellZ)
        {
            return;
        }

        //Unlink it from the list of its old cell
        if (soldier.prevSoldier != null)
        {
            soldier.prevSoldier.nextSoldier = soldier.nextSoldier;
        }

        if (soldier.nextSoldier != null)
        {
            soldier.nextSoldier.prevSoldier = soldier.prevSoldier;
        }

        //If it's the head of a list, remove it
        if (cells[oldCellX, oldCellZ] == soldier)
        {
            cells[oldCellX, oldCellZ] = soldier.nextSoldier;
        }

        //Add it bacl to the grid at its new cell
        Add(soldier);
    }
}
