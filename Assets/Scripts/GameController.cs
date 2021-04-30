using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController: MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public GameObject[] powerupsPrefabs;
    private Queue<Vector3> powerups;
    public Terrain terrain;
    public int mapXSize;
    public int mapYSize;
    private bool isPaused;

    public UnityEvent OnGameOver;

    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        isPaused = false;
        powerups = new Queue<Vector3>();
        int chance = 25;
        int realXSize = 2 * mapXSize;
        int realYSize = 2 * mapYSize;
        terrain.terrainData.size = new Vector3(realXSize, 0, realYSize);
        for(int i=0;i<mapXSize;i++)
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
        CheckGameover();
    }

    private void CheckGameover() {
        if (players == null) {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        foreach (GameObject player in players) {
            TPS_Player playerTPS = null;
            if (player){
                playerTPS = player.GetComponent<TPS_Player>();
            }
            if (playerTPS && playerTPS.isAlive()) {
                playerTPS.handleGameover();
                OnGameOver.Invoke();
            }
        }
    }
    public void AddPowerup(Vector3 position) {
        powerups.Enqueue(position);
    }

    
    public void PauseGame() {
        if(!isPaused) {
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void ResumeGame() {
        if(isPaused) {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
