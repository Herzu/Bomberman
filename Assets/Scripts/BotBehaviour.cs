using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotBehaviour : Character
{
    public GameObject bombPrefab;
    int cooldown = 0;

    private List<Vector3> availablePositions = new List<Vector3>();
    private string[,] elementMap;
    public NavMeshAgent agent;
    public GameObject player;
    int playerX, playerY;
    enum BotState {moving, escaping, planting, stationary};
    BotState state = BotState.stationary;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        GameObject go = GameObject.Find("GameController");
        GameController gc = go.GetComponent<GameController>();
        elementMap = new string[gc.mapXSize+1, gc.mapYSize+1];
        //agent.SetDestination(new Vector3(3, 0, 3));
        //Debug.Log(gc.mapXSize);
        //Debug.Log(gc.mapYSize);
        tableSetup();
        }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkBomb();
        checkImmunity();
        if (cooldown != 0)
            cooldown--;
        for (int i = 0; i < elementMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j < elementMap.GetUpperBound(1); j++)
            {
                elementMap[i, j] = "a";

            }
        }
        tableSetup();
        findAvailable();
        switch (state)
        {
            case BotState.stationary:
                findTarget();
                break;
            case BotState.planting:
                plantBomb();
                findAvailable();
                break;
            case BotState.moving:
                checkDestinationReached();
                break;

        } 
        //Debug.Log(state);
        //showAvailable();
    }

    void tableSetup()
    {
        findWalls();
        findCubes();
        //findPowerups();
        findPlayer();
        findMe();
    }

    private void findWalls()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "wall";
        }
    }

    private void findCubes()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x/2+1);
            int y = ((int)obj.transform.position.z/2+1);
            elementMap[x, y] = "cube";
        }
    }
    /*
    private void findPowerups()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "powerup";
        }
    }*/

    private void findPlayer()
    {
        if (player != null)
        {
            playerX = ((int)player.transform.position.x / 2 + 1);
            playerY = ((int)player.transform.position.z / 2 + 1);
            elementMap[playerX, playerY] = "player";
        }
    }

    private void findMe()
    {
        int x = ((int)transform.position.x / 2 + 1);
        int y = ((int)transform.position.z / 2 + 1);
        elementMap[x, y] = "bot";

    }

    private void findAvailable()
    {
        NavMeshPath path = new NavMeshPath();
        for (int i = 0; i < elementMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j < elementMap.GetUpperBound(1); j++)
            {
                if (elementMap[i, j] == "a")
                {
                    Vector3 destination = new Vector3(i, 0, j);
                    agent.CalculatePath(destination, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        bool different = true;
                        foreach (Vector3 pos in availablePositions)
                        {
                            if (pos == destination)
                                different = false;
                        }
                        if (different)
                            availablePositions.Add(destination);
                    }
                }
            }
        }
        /*if (x > 1 && y > 1 && x < 16 && y < 10)
        {
            
            if ((elementMap[x - 1, y] != "wall" || elementMap[x - 1, y] != "block" || elementMap[x - 1, y] != "a"))
            {
                //Debug.Log("lewo");
                //findAvailable(x - 1, y);
            }
        }*/
        
    }

    void showAvailable()
    {
        foreach(Vector3 pos in availablePositions)
        {
            Debug.Log(pos);
        }
    }

    private void findTarget()
    {
        Vector3 destination = new Vector3();
        int minX = 1000, minY = 1000, tempX, tempY;
        foreach(Vector3 pos in availablePositions)
        {
            tempX = (int)Mathf.Abs(playerX - pos.x);
            tempY = (int)Mathf.Abs(playerY - pos.z);
            if (tempY < minY)
            {
                minY = tempY;
                destination.z = pos.z*2-1;
            }
            if (tempX < minX)
            {
                minX = tempX;
                destination.x = pos.x*2-1;
            }
        }
        Debug.Log(destination.x);
        Debug.Log(destination.z);
        agent.SetDestination(destination);
        state = BotState.moving;
    }



    void checkDestinationReached()
    {
        float distanceToTarget = Vector3.Distance(transform.position, agent.destination);
        if (distanceToTarget < 0.1f)
        {
            state = BotState.planting;
        }
    }

    void plantBomb()
    {
        if (cooldown == 0)
        {
            Vector3Int intVector = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
            GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
            bomb.GetComponent<Bomb>().maxLifetime = bombLifetime;
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
            bomb.transform.GetChild(1).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
            bomb.transform.GetChild(2).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
            bomb.transform.GetChild(3).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.GetComponent<Bomb>().is3D = false;
            bomb.GetComponent<Bomb>().range = range;
            cooldown = 100;
        }
        state = BotState.stationary;
    }
    void escape()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "bomb";
        }
    }
}
