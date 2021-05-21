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
    public Camera[] cameras;                    //!< lista kamer
    public GameObject[] PlayerPrefabs;          //!< lista prefabów graczy
    public GameObject[] BotPrefabs;             //!< lista prefabó postaci
    public Terrain terrain;                     //!< obiekt terenu
    public int mapXSize;                        //!< rozmiar mapy w osi X
    public int mapYSize;                        //!< rozmiar mpay w osi Y
    public int mapZSize = 1;                    //!< wysokość mapy
    private bool isPaused;                      //!< czy gra jest spauzowana
    public UnityEvent OnPlayerWin;              //!< wydarzenie wywoływane przy końcu wygranej gracza
    public UnityEvent OnBotWin;                 //!< wydarzenie wywoływane przy końcu wygranej bota
    private GameObject[] players;               //!< lista graczy
    private GameObject[] bots;                  //!< lista botów
    public int[] controlls;                     //!< lista wybranych graczy i wybranych kontrollerów
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

    void Awake()
    {   
        //inicjalizacja wartości startowych
        InitValues();
        //pobranie szansy na powerup
        anyPowerupChance = PlayerPrefs.GetInt("powerupChanceAmount");
        //ustalenie skali czasu
        Time.timeScale = 1;
        //wyłączenie pauzy
        isPaused = false;
        //inicjalizacja kolejki powerupów
        powerups = new Queue<Vector3>();
        //ustalenie romiaru terenu
        terrain.terrainData.size = new Vector3(2 * mapXSize, 0, 2 * mapYSize);
        //jeżeli wybrany tryb to tryb klasyczny
        if (PlayerPrefs.GetString("playerMode") == "TPP")
        {
            //wypełnienie mapy 2D ścianami
            Fill2D();
            //destrukcja starych postaci i dezaktywacja postaci pierwszoosobowej
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<FPS_Player>() == null)
                    Destroy(player.gameObject);
                else
                    player.gameObject.SetActive(false);
            }
            //przyzwanie odpowiednich postaci na mapę
            SummonCharacters();
        }
        else
        {
            //wypełnienie mapy 3D ścianami
            Fill3D();
            //destrukcja starych postaci i aktywacja postaci pierwszoosobwej
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                if (player.GetComponent<FPS_Player>() == null)
                    Destroy(player.gameObject);
                else
                {
                    player.gameObject.SetActive(true);
                }
            }
        }
        //wypełnienie tablicy dropu powerupów
        FillDropTable();
        //ustawienie wartości początkowych dla wszystkich postaci
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            var playerScript = player.GetComponent<Character>();
            playerScript.range = initRange;
            playerScript.speed = initSpeed;
            playerScript.updateSpeed();
            playerScript.bombs = initBombs;
            playerScript.lifes = initLifes;
            playerScript.bombLifetime = bombLifetime;
            playerScript.mapHeight = mapZSize;
            //jeżeli postać ma podpięty character controller przed przemieszczeniem jest wyłączany i włączany gdy postać znajduje się w odpowiednim miejscu
            if (player.GetComponent<CharacterController>() != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                playerScript.moveToPlace();
                player.GetComponent<CharacterController>().enabled = true;
            }
            else    //jeżeli nie ma postać jest po prostu przemieszczana
                playerScript.moveToPlace();
        }
        //ustawienie wartości bazowych dla botów
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
    /** \biref funkcja tworząca postacie*/
    void SummonCharacters()
    {
        int players = 0;        //liczba graczy
        int characters = 0;     //liczba postaci
        foreach(int type in controlls)
        {
            //lista pozycji dla postaci
            Vector3[] pos = { new Vector3(3, 1, 3), new Vector3(3, 1, 2*mapYSize - 3), new Vector3(2 * mapXSize - 3, 1, 3), new Vector3(2 * mapXSize - 3, 1, 2 * mapYSize - 3) };
            GameObject character;   //aktualna postać
            switch (type)
            {
                case 0:     //puste miejsce
                    //dekrementacja liczby postaci (po końcowej inkrementacji wartości pozostaje niezmieniona)
                    characters--;
                break;
                case 1:     //gracz sterowany strzałkami
                    //inkremenetacja liczby graczy
                    players++;
                    //umieszczenie postaci
                    character = Instantiate(PlayerPrefabs[characters], pos[characters], Quaternion.identity);
                    //włączenie kontrolera strałkamie
                    character.GetComponent<ArrowsController>().enabled = true;
                    //przypisanie odpowiedniej kamery
                    character.GetComponent<ArrowsController>().camera = cameras[characters];
                    //przypisanie postaci do kamery
                    character.GetComponent<ArrowsController>().camera
                        .transform.Find("UI").GetComponent<PlayerUIUpdater>().player = character;
                break;
                case 2:     //gracz sterowany WASD
                    //analogicznie do gracza sterowanego strzałkami
                    players++;
                    character = Instantiate(PlayerPrefabs[characters], pos[characters], Quaternion.identity);
                    character.GetComponent<WASDController>().enabled = true;
                    character.GetComponent<WASDController>().camera = cameras[characters];
                    character.GetComponent<WASDController>().camera
                        .transform.Find("UI").GetComponent<PlayerUIUpdater>().player = character;
                    break;
                case 3:     //gracz sterowany kontrollerem
                    //analogicznie do gracza sterowanego strzałkami
                    players++;
                    character = Instantiate(PlayerPrefabs[characters], pos[characters], Quaternion.identity);
                    character.GetComponent<GamepadController>().enabled = true;
                    character.GetComponent<GamepadController>().camera = cameras[characters];
                    character.GetComponent<GamepadController>().camera
                        .transform.Find("UI").GetComponent<PlayerUIUpdater>().player = character;
                    break;
                case 4:     //bot
                    //umieszczenie bota
                    Instantiate(BotPrefabs[characters], pos[characters], Quaternion.identity);
                break;
            }
            //inkrementacja liczny postaci
            characters++;
        }
        //ustawienie rozmiarów kamer
        setupCameras(players);
    }
    /**  funkcja ustawiająca kamery
     *  @param players liczba aktywnych graczy
     */
    void setupCameras(int players)
    {
        //wyłaczenie wszystkich kamer
        foreach (Camera cam in cameras)
            cam.gameObject.SetActive(false);
        //jeżeli jest tylko jeden gracz kamera zajmuje cały ekran
        if (players == 1)
            cameras[0].rect = new Rect(0, 0, 1, 1);
        //jeżeli jest dwoje graczy każda kamera zajmuje pół ekranu
        else if(players == 2)
        {
            cameras[0].rect = new Rect(0, 0, 0.5f, 1);
            cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
        }
        //jeżeli jest więcej niż dwoje graczy każda kamera zajmuje ćwierć ekranu
        else
        {
            cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
            cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            cameras[2].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            cameras[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }
        //włączenie odpowiedniej ilości kamer
        for (int i = 0; i < players; i++)
            cameras[i].gameObject.SetActive(true);
    }
    /**funkcja inicjalizująca wartości bazowe*/
    void InitValues() {
        //pobranie rozmiaru mapy
        mapXSize = PlayerPrefs.GetInt("xSize");
        mapYSize = PlayerPrefs.GetInt("ySize");
        mapZSize = PlayerPrefs.GetInt("zSize");
        //pobranie bazowych wartości bomb i żyć
        initBombs = PlayerPrefs.GetInt("bombsAmount");
        initLifes = PlayerPrefs.GetInt("lifesAmount");
        //pobranie wybranych typów postaci
        controlls = new int [] {
            PlayerPrefs.GetInt("P1Controlls"),
            PlayerPrefs.GetInt("P2Controlls"),
            PlayerPrefs.GetInt("P3Controlls"),
            PlayerPrefs.GetInt("P4Controlls")
        };
        //przypisanie postaci do tablicy postaci
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    }
    /**  funkcja wypełniająca tablicę informującą który powerup ma się pojawić*/
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
        for (int i = 0; i < mapXSize; i++)
        {
            for (int j = 0; j < mapYSize; j++)
            {
                //stworzenie zewnętrznych ścian
                if (i == 0 || j == 0 || i == mapXSize - 1 || j == mapYSize - 1)
                    Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                //pozostawienie wolnego miejsca w pozycjach startowych
                else if ((i > 2 || j > 2)
                    && (i < mapXSize - 3 || j < mapYSize - 3)
                    && (i > 2 || j < mapYSize - 3)
                    && (i < mapXSize - 3 || j > 2))
                {
                    //wypełnienie odpowiednich miejsc niezniszczalnymi ścianami
                    if (i % 2 == 0 && j % 2 == 0)
                        Instantiate(wallPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity);
                    //wypełnienie reszty miejsc zniszczalnymi ścianami zgodnie z ustaloną szansą na stworzenie ściany
                    else if (blockChance >= Random.Range(1, 100))
                        Instantiate(blockPrefab, new Vector3(2 * i + 1, 1, 2 * j + 1), Quaternion.identity).gameObject.GetComponent<BlockDestroy>().GameController = this;
                }

            }
        }
    }
    /**  funkcja wypełnieniająca mapę 3D*/
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
        //sprawdzenie czy gra się nie skończyła
        CheckGameover();
    }
    /**  funkcja sprawdzająca czy gra się skończyła*/
    private void CheckGameover() {
        players = GameObject.FindGameObjectsWithTag("Player");
        //sprawdzenie czy którykolwiek z graczy jest martwy
        foreach (GameObject player in players) {
            Character playerScript = null;
            if (player){
                playerScript = player.GetComponent<Character>();
            }
            if (playerScript && playerScript.isDead()) {
                playerScript.handleGameover();
            }
        }

        bots = GameObject.FindGameObjectsWithTag("Bot");
        //sprawdzenie czy którykolwiek z botów jest martwy
        foreach (GameObject bot in bots) {
            Character botScript = null;
            if (bot) {
                botScript = bot.GetComponent<Character>();
            }
            if (botScript && botScript.isDead()) {
                botScript.handleGameover();
            }
        }

        //sprawdzenie kto wygrał
        if (players.Length == 1 && bots.Length == 0) {
            OnPlayerWin.Invoke();
        } else if (bots.Length == 1 && players.Length == 0) {
            OnBotWin.Invoke();
        }
    }
    /**  funkcja dodająca powerupu do kolejki
     * @param position pozycja w której ma zostać stworzony powerup
     */
    public void AddPowerup(Vector3 position) {
        powerups.Enqueue(position);
    }

    /**  funkcja pauzująca grę*/
    public void PauseGame() {
        if(!isPaused) {
            Time.timeScale = 0;
            isPaused = true;
        }
    }
    /**  funkcja wznawiająca grę*/
    public void ResumeGame() {
        if(isPaused) {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
