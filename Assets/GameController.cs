using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController: MonoBehaviour
{
    public GameObject cubePrefab;
    const int mapXSize = 100;
    const int mapYSize = 100;
    // Start is called before the first frame update
    void Start()
    {
        int chance = 25;
        for (int i = 1;i<mapXSize;i+=2)
        {
            for (int j = 1; j < mapYSize; j+=2)
            {
                if(chance >= UnityEngine.Random.Range(1, 100))
                    Instantiate(cubePrefab, new Vector3(i, 1, j), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
