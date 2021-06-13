using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//! Klasa odpowiedzialna za sterowanie rozgrywką
public class GameController: MonoBehaviour
{
    public GameObject blockPrefab;              //!< prefab zniszczalnej ściany
    public GameObject wallPrefab;               //!< prefab niezniszczalnej ściany
    public GameObject[] classicPowerupsPrefabs; //!< lista prefabów powerupów trybu klasycznego
    public GameObject[] arcadePowerupsPrefabs;  //!< lista prefabów powerupów trybu arcade
    private Queue<Vector3> powerups;            //!< kolejka powerupów do stworzenia
    public Terrain terrain;                     //!< obiekt terenu
    public int mapXSize;                        //!< rozmiar mapy w osi X
    public int mapYSize;                        //!< rozmiar mpay w osi Y
    public int mapZSize = 1;                    //!< wysokość mapy
    public int blockChance = 25;                //!< szansa na postawienie zniszczalnej ściany na każdym polu
    public int anyPowerupChance = 50;           //!< szansa na pojawienie sie powerupu
    public int[] classicPowerupWeights;         //!< wagi szansy na odpowiednie powerupy w trybie klasycznym
    public int[] arcadePowerupsWeights;         //!< wagi szansy na odpowiednie powerupy w trybie arcade
    public int initRange = 1;                   //!< startowy zasięg postaci
    public int initSpeed = 10;                  //!< startowa szybkość postaci
    public int initLifes = 3;                   //!< startowa ilość żyć postaci
    public int initBombs = 1;                   //!< startowa ilość bomb postaci
    public int bombLifetime = 500;              //!< startowy czas życia bomb
    int[] powerupDropTable;                     //!< tablica informująca który powerup został wylosowany

    bool isGameOver;                     //!< czy gra została zakonczona
    void Awake()
    {   
        InitValues();
        anyPowerupChance = PlayerPrefs.GetInt("powerupChanceAmount");
        Time.timeScale = 1;
        powerups = new Queue<Vector3>();
        terrain.terrainData.size = new Vector3(2 * mapXSize, 0, 2 * mapYSize);
        Fill2D();
        FillDropTable();
    }
    void InitValues() {
        //pobranie rozmiaru mapy
        mapXSize = PlayerPrefs.GetInt("xSize");
        mapYSize = PlayerPrefs.GetInt("ySize");
        mapZSize = PlayerPrefs.GetInt("zSize");
    }

    void FillDropTable()
    {
        //inicjalizacja tablicy ze stoma miejscami (miejsca są zajmowanie przez powerupy w zależności od ich wagi)
        powerupDropTable = new int[100];
        int weightSum = 0;      //suma wag powerupów
        int[] powerupWeights;   //tablica wag powerupów do której prrzypisana jest odpowiednia tablica w zalezności od trybu gry
        if (PlayerPrefs.GetString("playerMode") == "TPP")
            powerupWeights = classicPowerupWeights;
        else
            powerupWeights = arcadePowerupsWeights;
        //obliczenie sumy wag
        foreach (int weight in powerupWeights)
            weightSum+=weight;
        int index = 0;          //aktualny index tablicy
        int actualPowerup = 0;  //aktualny index powerupu
        foreach (int weight in powerupWeights)
        {
            //obliczenie liczby zajmowanych przez powerup miejsc
            int newWeight = anyPowerupChance * weight / weightSum;
            //wypełnienie odpowiedniej ilości miejsc aktualnym powerupem
            for(int i=0;i<newWeight;i++)
            {
                powerupDropTable[index] = actualPowerup;
                index++;
            }
            //inkrementacja indexu powerupu
            actualPowerup++;
        }
        //wypełnienie reszty miejsc pustymi miejscami (nie wypadnięcie żadnego powerupu)
        while(index < 100)
        {
            powerupDropTable[index] = -1;
            index++;
        }
    }
    /**  funkcja wypełnieniająca mapę 2D*/
    void Fill2D()
    {
        int offsetX = (int)(this.gameObject.transform.parent.gameObject.transform.position.x)/2;
        int offsetY = (int)(this.gameObject.transform.parent.gameObject.transform.position.y)/2;
        for (int i = offsetX; i < mapXSize+offsetX; i++)
        {
            for (int j = offsetY; j < mapYSize+offsetY; j++)
            {
                //stworzenie zewnętrznych ścian
                if (i == offsetX || j == offsetY || i == mapXSize + offsetX - 1 || j == mapYSize + offsetY - 1)
                    Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                //pozostawienie wolnego miejsca w pozycjach startowych
                else if ((i > offsetX + 2 || j > offsetY + 2)
                    && (i < mapXSize + offsetX - 3 || j < mapYSize + offsetY - 3)
                    && (i > offsetX + 2 || j < mapYSize + offsetY - 3)
                    && (i < mapXSize + offsetX - 3 || j > offsetY + 2))
                {
                    //wypełnienie odpowiednich miejsc niezniszczalnymi ścianami
                    if (i % 2 == 0 && j % 2 == 0)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                    //wypełnienie reszty miejsc zniszczalnymi ścianami zgodnie z ustaloną szansą na stworzenie ściany
                    else if (blockChance >= Random.Range(1, 100)) {}
                        // Instantiate(blockPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                }

            }
        }
    }

    void Fill3D()
    {
        //stworzenie dodatkowej części muru w osi X
        for (int i = 0; i < mapXSize; i++)
        {
            for (int k = mapZSize; k < mapZSize + 2; k++)
            {
                Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 1), Quaternion.identity);
                Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * mapYSize - 1), Quaternion.identity);
            }
        }
        //stworzenie dodatkowej części muru w osi Y
        for (int j = 0; j < mapYSize; j++)
        {
            for (int k = mapZSize; k < mapZSize + 2; k++)
            {
                Instantiate(wallPrefab, new Vector3(1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                Instantiate(wallPrefab, new Vector3(2 * mapXSize - 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
            }
        }
        for (int i = 0; i < mapXSize; i++)
        {
            for (int j = 0; j < mapYSize; j++)
            {
                for (int k = 0; k < mapZSize; k++)
                    //stworzenie zewnętrznych ścian
                    if (i == 0 || j == 0 || i == mapXSize - 1 || j == mapYSize - 1)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                    else
                    {
                        //wypełnienie odpowiednich miejsc niezniszczalnymi ścianami
                        if (i % 2 == 0 && j % 2 == 0)
                            Instantiate(wallPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity);
                        //wypełnienie reszty miejsc zniszczalnymi ścianami zgodnie z ustaloną szansą na stworzenie ściany
                        else if (blockChance >= Random.Range(1, 100))
                            Instantiate(blockPrefab, new Vector3(2 * i + 1, 2 * k + 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                    }

            }
        }
        //stworzenie niezniszcalnych ścian w miejscu pojawienia się postaci żeby nie spadły na dół
        Instantiate(wallPrefab, new Vector3(3, 2 * mapZSize - 1, 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(3, 2 * mapZSize - 1, 2 * mapYSize - 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(2 * mapXSize - 3, 2 * mapZSize - 1, 3), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(2 * mapXSize - 3, 2 * mapZSize - 1, 2 * mapYSize - 3), Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        //jeżeli w kolejce znajdują się powerupy
        while (powerups.Count > 0)
        {
            //usunięcie powerupu z kolejki
            Vector3 position = powerups.Dequeue();
            //wylosowanie miejsca w tablicy powerupów
            int drop = powerupDropTable[Random.Range(0, 99)];
            //jeżeli powerup ma się pojawić
            if (drop != -1)
            {
                //stworzenie odpowiedniego powerupu w zależności od trybu gry
                if (PlayerPrefs.GetString("playerMode") == "TPP")
                    Instantiate(classicPowerupsPrefabs[drop], position, Quaternion.identity);
                else
                    Instantiate(arcadePowerupsPrefabs[drop], position, Quaternion.identity);
                break;
            }
        }
    }

    /**  funkcja dodająca powerupu do kolejki
     * @param position pozycja w której ma zostać stworzony powerup
     */
    public void AddPowerup(Vector3 position) {
        powerups.Enqueue(position);
    }
}
