using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController: MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public GameObject[] classicPowerupsPrefabs;
    public GameObject[] arcadePowerupsPrefabs;
    private Queue<Vector3> powerups;
    public Terrain terrain;
    public int mapXSize;
    public int mapYSize;
    public int mapZSize = 1;
    private bool isPaused;
    public UnityEvent OnGameOver;
    private GameObject[] players;
    private GameObject[] bots;
    public int blockChance = 25;
    public int anyPowerupChance = 50;
    public int[] classicPowerupWeights;
    public int[] arcadePowerupsWeights;
    public int initRange = 1;
    public int initSpeed = 10;
    public int initLifes = 3;
    public int initBombs = 1;
    public int bombLifetime = 500;
    int[] powerupDropTable;

    void Awake()
    {   
        InitValues();
        anyPowerupChance = PlayerPrefs.GetInt("powerupChanceAmount");
        Time.timeScale = 1;
        isPaused = false;
        Application.targetFrameRate = 60;
        powerups = new Queue<Vector3>();
        terrain.terrainData.size = new Vector3(2 * mapXSize, 0, 2 * mapYSize);
        if (PlayerPrefs.GetString("playerMode") == "TPP")
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
        bots = GameObject.FindGameObjectsWithTag("Bot");
        foreach (GameObject bot in bots)
        {
            var botScript = bot.GetComponent<Character>();
            botScript.range = initRange;
            botScript.speed = initSpeed;
            botScript.bombs = initBombs;
            botScript.lifes = initLifes;
            botScript.bombLifetime = bombLifetime;
            botScript.mapHeight = mapZSize;
            botScript.moveToPlace();
        }
    }

    void InitValues() {
        mapXSize = PlayerPrefs.GetInt("xSize");
        mapYSize = PlayerPrefs.GetInt("ySize");
        mapZSize = PlayerPrefs.GetInt("zSize");
        initBombs = PlayerPrefs.GetInt("bombsAmount");
        initLifes = PlayerPrefs.GetInt("lifesAmount");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            player.SetActive(false);
            if(player.GetComponent<FPS_Player>()) {
                if (PlayerPrefs.GetString("playerMode") == "FPP") {
                    GameObject.FindWithTag("MainCamera").SetActive(false);
                    player.SetActive(true);
                }
            } else if(player.GetComponent<TPS_Player>()) {
                if (PlayerPrefs.GetString("playerMode") == "TPP") {
                    player.SetActive(true);
                }
            }
        }
    }

    void FillDropTable()
    {
        powerupDropTable = new int[100];
        int weightSum = 0;
        int[] powerupWeights;
        if (PlayerPrefs.GetString("playerMode") == "TPP")
            powerupWeights = classicPowerupWeights;
        else
            powerupWeights = arcadePowerupsWeights;
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
                if (PlayerPrefs.GetString("playerMode") == "TPP")
                    Instantiate(classicPowerupsPrefabs[drop], position, Quaternion.identity);
                else
                    Instantiate(arcadePowerupsPrefabs[drop], position, Quaternion.identity);
                break;
            }
        }
        CheckGameover();
    }

    private void CheckGameover() {
        foreach (GameObject player in players) {
            Character playerScript = null;
            if (player){
                playerScript = player.GetComponent<Character>();
            }
            if (playerScript && playerScript.isAlive()) {
                playerScript.handleGameover();
                OnGameOver.Invoke();
            }
        }
        foreach (GameObject bot in bots)
        {
            Character botScript = null;
            if (bot)
            {
                botScript = bot.GetComponent<Character>();
            }
            if (botScript && botScript.isAlive())
            {
                botScript.handleGameover();
            }
        }
    }
    //temporary function, add victory screen here
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
