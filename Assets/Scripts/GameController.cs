using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public GameObject[] powerupsPrefabs;
    private Queue<Vector3> powerups;
    public Terrain terrain;
    public int mapXSize;
    public int mapYSize;
    public int mapZSize = 1;
    public int blockChance = 25;
    public int anyPowerupChance = 50;
    public int[] powerupWeights;
    public int initRange = 1;
    public int initSpeed = 10;
    public int initLifes = 3;
    public int initBombs = 1;
    public int bombLifetime = 500;
    private GameObject[] players;
    int[] powerupDropTable;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        powerups = new Queue<Vector3>();
        terrain.terrainData.size = new Vector3(2 * mapXSize, 0, 2 * mapYSize);
        if (mapZSize == 1)
            Fill2D();
        else
            Fill3D();
        FillDropTable();
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            var playerScript = player.GetComponent<Character>();
            playerScript.range = initRange;
            playerScript.speed = initSpeed;
            playerScript.bombs = initBombs;
            playerScript.lifes = initLifes;
            playerScript.bombLifetime = bombLifetime;
            playerScript.mapHeight = mapZSize;
            if (player.GetComponent<CharacterController>() != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                playerScript.moveToPlace();
                player.GetComponent<CharacterController>().enabled = true;
            }
            else
                playerScript.moveToPlace();
        }
    }
    void FillDropTable()
    {
        powerupDropTable = new int[100];
        int weightSum = 0;
        foreach (int weight in powerupWeights)
            weightSum+=weight;
        int index = 0;
        int actualPowerup = 0;
        foreach (int weight in powerupWeights)
        {
            int newWeight = anyPowerupChance * weight / weightSum;
            for(int i=0;i<newWeight;i++)
            {
                powerupDropTable[index] = actualPowerup;
                index++;
            }
            actualPowerup++;
        }
        while(index < 100)
        {
            powerupDropTable[index] = -1;
            index++;
        }
    }
    void Fill2D()
    {
        for (int i = 0; i < mapXSize; i++)
        {
            for (int j = 0; j < mapYSize; j++)
            {
                if (i == 0 || j == 0 || i == mapXSize - 1 || j == mapYSize - 1)
                    Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                else if ((i > 2 || j > 2)
                    && (i < mapXSize - 3 || j < mapYSize - 3)
                    && (i > 2 || j < mapYSize - 3)
                    && (i < mapXSize - 3 || j > 2))
                {
                    if (i % 2 == 0 && j % 2 == 0)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                    else if (blockChance >= Random.Range(1, 100))
                        Instantiate(blockPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                }

            }
        }
    }
    void Fill3D()
    {
        //top wall along x axis
        for (int i = 0; i < mapXSize; i++)
        {
            for (int k = mapZSize; k < mapZSize + 2; k++)
            {
                Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 1), Quaternion.identity);
                Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * mapYSize - 1), Quaternion.identity);
            }
        }
        //top wall along y axis
        for (int j = 0; j < mapYSize; j++)
        {
            for (int k = mapZSize; k < mapZSize + 2; k++)
            {
                Instantiate(wallPrefab, new Vector3(1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                Instantiate(wallPrefab, new Vector3(2 * mapXSize - 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
            }
        }
        //creating main playing zone
        for (int i = 0; i < mapXSize; i++)
        {
            for (int j = 0; j < mapYSize; j++)
            {
                for (int k = 0; k < mapZSize; k++)
                    if (i == 0 || j == 0 || i == mapXSize - 1 || j == mapYSize - 1)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                    else
                    {
                        if (i % 2 == 0 && j % 2 == 0)
                            Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                        else if (blockChance >= Random.Range(1, 100))
                            Instantiate(blockPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                    }

            }
        }
        //creating walls for players to stand on
        Instantiate(wallPrefab, new Vector3(3, 2 * mapZSize - 1, 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(3, 2 * mapZSize - 1, 2 * mapYSize - 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(2 * mapXSize - 3, 2 * mapZSize - 1, 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(2 * mapXSize - 3, 2 * mapZSize - 1, 2 * mapYSize - 3), Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        while (powerups.Count > 0)
        {
            Vector3 position = powerups.Dequeue();
            int drop = powerupDropTable[Random.Range(0, 99)];

            if (drop != -1)
            {
                Instantiate(powerupsPrefabs[drop], position, Quaternion.identity);
                break;
            }
        }
    }

    public void AddPowerup(Vector3 position)
    {
        powerups.Enqueue(position);
    }
}
