using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController: MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public GameObject[] powerupsPrefabs;
    private Queue<Vector3> powerups;
    public Terrain terrain;
    public int mapXSize;
    public int mapYSize;
    // Start is called before the first frame update
    void Start()
    {
        powerups = new Queue<Vector3>();
        int chance = 25;
        int realXSize = 2 * mapXSize;
        int realYSize = 2 * mapYSize;
        terrain.terrainData.size = new Vector3(realXSize, 0, realYSize);
        for (int i = 1; i < realXSize; i += 2)
        {
            Instantiate(wallPrefab, new Vector3(i, 1, 1), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(i, 1, realYSize-1), Quaternion.identity);
        }
        for (int i = 1; i < realYSize; i += 2)
        {
            Instantiate(wallPrefab, new Vector3(1, 1, i), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(realXSize-1, 1, i), Quaternion.identity);
        }
        for (int i = 7;i<realXSize-7;i+=2)
        {
            for (int j = 7; j < realYSize-7; j+=2)
            {
                if(chance >= Random.Range(1, 100))
                    Instantiate(blockPrefab, new Vector3(i, 1, j), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            }
        }
        for (int i = 7; i < realXSize - 7; i += 2)
        {
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, 3), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, 5), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, realYSize-3), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, realYSize-5), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
        }
        for (int i = 7; i < realYSize - 7; i += 2)
        {
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(3, 1, i), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(5, 1, i), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(realXSize-3, 1, i), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
            if (chance >= Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(realXSize-5, 1, i), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        while(powerups.Count>0)
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
