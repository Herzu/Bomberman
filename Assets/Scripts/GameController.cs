using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController: MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public Terrain terrain;
    public int mapXSize;
    public int mapYSize;
    // Start is called before the first frame update
    void Start()
    {
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
                if(chance >= UnityEngine.Random.Range(1, 100))
                    Instantiate(blockPrefab, new Vector3(i, 1, j), Quaternion.identity);
            }
        }
        for (int i = 7; i < realXSize - 7; i += 2)
        {
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, 3), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, 5), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, realYSize-3), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(i, 1, realYSize-5), Quaternion.identity);
        }
        for (int i = 7; i < realYSize - 7; i += 2)
        {
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(3, 1, i), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(5, 1, i), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(realXSize-3, 1, i), Quaternion.identity);
            if (chance >= UnityEngine.Random.Range(1, 100))
                Instantiate(blockPrefab, new Vector3(realXSize-5, 1, i), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
