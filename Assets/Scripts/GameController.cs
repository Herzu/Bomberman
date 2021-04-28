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
    // Start is called before the first frame update
    void Start()
    {
        powerups = new Queue<Vector3>();
        terrain.terrainData.size = new Vector3(2 * mapXSize, 0, 2 * mapYSize);
        if (mapZSize == 1)
            Fill2D();
        else
            Fill3D();
    }
    void Fill2D()
    {
        int chance = 25;
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
                    else if (chance >= Random.Range(1, 100))
                        Instantiate(blockPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                }

            }
        }
    }
    void Fill3D()
    {
        int chance = 25;
        for (int i = 0; i < mapXSize; i++)
        {
            for (int j = 0; j < mapYSize; j++)
            {
                for (int k = 0; k < mapZSize; k++)
                    if (i == 0 || j == 0 || i == mapXSize - 1 || j == mapYSize - 1)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                    else if ((i > 2 || j > 2)
                        && (i < mapXSize - 3 || j < mapYSize - 3)
                        && (i > 2 || j < mapYSize - 3)
                        && (i < mapXSize - 3 || j > 2))
                    {
                        if (i % 2 == 0 && j % 2 == 0)
                            Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                        else if (chance >= Random.Range(1, 100))
                            Instantiate(blockPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                    }

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        while (powerups.Count > 0)
        {
            Vector3 position = powerups.Dequeue();
            int rand = Random.Range(1, 100);
            int perPowerupChance = 100 / (powerupsPrefabs.Length + 1);
            for (int i = 0; i < powerupsPrefabs.Length; i++)
            {
                rand -= perPowerupChance;
                if (rand < 0)
                {
                    Instantiate(powerupsPrefabs[i], position, Quaternion.identity);
                    break;
                }
            }
        }
    }

    public void AddPowerup(Vector3 position)
    {
        powerups.Enqueue(position);
    }
}
