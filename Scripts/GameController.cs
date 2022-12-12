using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject friendlyObj;
    public GameObject enemyObj;

    //change mats to detect closest enemy
    public Material enemyMat;
    public Material enemyCloseMat;


    //cleaner workspace = parent enemies to parent object 
    public Transform enemyParent;
    public Transform friendlyParent;

    //Store all soldiers in these lists
    List<Soldier> enemySoldiers = new List<Soldier>();
    List<Soldier> friendlySoldiers = new List<Soldier>();

    //Save the closest enemies for easier change back to its mat
    List<Soldier> closeEnemies = new List<Soldier>();

    //Grid data
    float mapWidth = 50f;
    int cellSize = 10;

    //Number of soldiers on each team
    public int numOfSoldiers = 100;

    //Spatial partition grid
    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid((int)mapWidth, cellSize);
        
        //Add Random Enemies and friendlies and store em in a list
        for (int i = 0; i < numOfSoldiers; i++)
        {
            //Give enemy a random position
            Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

            //create new enemy
            GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;

            //Add enemy to a list
            enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

            //Parent it
            newEnemy.transform.parent = enemyParent;


            //Give the friendly a randomPos
            randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

            //Create new friendly
            GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;

            //Add friendly to list
            friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

            //Parent
            newFriendly.transform.parent = friendlyParent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move enemies
        for (int i = 0; i < enemySoldiers.Count; i++)
        {
            enemySoldiers[i].Move();
        }

        //Reset material of closest enemies
        for (int i = 0; i < closeEnemies.Count; i++)
        {
            closeEnemies[i].soldierMeshRenderer.material = enemyMat;
        }

        //Reset list with closest enemies
        closeEnemies.Clear();

        //For each friendly, find closest enemy, change its color, and chase it
        for (int i = 0; i < friendlySoldiers.Count; i++)
        {
            //Soldier closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);

            //the fast version
            Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);

            //if we find an enemy
            if(closestEnemy != null)
            {
                //Change Mat
                closestEnemy.soldierMeshRenderer.material = enemyCloseMat;

                closeEnemies.Add(closestEnemy);

                //Move friendly in dir of enemy
                friendlySoldiers[i].Move(closestEnemy);
            }
        }
    }

    //Find closest enemy - slow
    Soldier FindClosestEnemySlow(Soldier soldier)
    {
        Soldier closestEnemy = null;

        float bestDistSqr = Mathf.Infinity;

        //loop through all enemies
        for (int i = 0; i < enemySoldiers.Count; i++)
        {
            //the dist sqr between the soldier and this enemy
            float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

            //if dist is beter than previous best dist, then we've found a closer enemy
            if (distSqr < bestDistSqr)
            {
                bestDistSqr = distSqr;

                closestEnemy = enemySoldiers[i];
            }
        }

        return closestEnemy;
    }
}
