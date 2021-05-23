using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//! Klasa obługujaca boty
public class BotBehaviour : Character
{
    private Character character;
    public Animator animator;
    public GameObject bombPrefab; //!< model podstawianej bomby
    int cooldown = 0;   //!< licznik czasu, który musi upłynać, żeby podstawić kolejną bombę
    private List<Vector3> availablePositions = new List<Vector3>(); //!< lista dostępnych pozycji na mapie
    private string[,] elementMap; //!< mapa elementów
    public NavMeshAgent agent;  //!< NavMeshAgent pozwalający przeciwnikom korzystać z NavMesha na mapie
    public GameObject player; //!< wskaźnik na gracza
    int playerX, playerY; //!< współrzędne gracza na mapie
    enum BotState { moving, escaping, planting, stationary, runningAway }; //!< stany, w których może znajdować się bot
    BotState state = BotState.stationary; //!< aktualny stan bota
    // Start is called before the first frame update
    void Start()
    {
        range = 1;
        bombLifetime = 250;
        Init();
        //pobranie obiektu GameControllera i pobranie z niego rozmiarów mapy
        GameObject go = GameObject.Find("GameController");
        GameController gc = go.GetComponent<GameController>();
        // ustawienie odpowiedniego rozmiaru tablicy elementów
        // tablica ma rzmiar 2 razy mniejszy niż rzeczywisty rozmiar w Unity, gdyż elementy na 
        // mapie mają rozmiar 2
        elementMap = new string[gc.mapXSize + 1, gc.mapYSize + 1];
        //wstępne zapełnienie tablicy elementów
        tableSetup();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldown != 0)
        {
            cooldown--;
        }
        checkBomb();
        checkImmunity();
        for (int i = 0; i < elementMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j < elementMap.GetUpperBound(1); j++)
            {
                elementMap[i, j] = "a";

            }
        }
        if (cooldown < 10)
        {
            // wylaczenie animacji stawiania bomby        
            animator.SetBool("isPlanting", false);
        }
        //aktualizacja tabeli
        tableSetup();
        //aktualizacja listy dostępnych współrzędnych
        findAvailable();
        //określenie kolejnego ruchu bota
        switch (state)
        {
            //w stanie stationary bot wyszukuje cel (gracza)
            case BotState.stationary:
                findTarget();
                break;
            //w stanie planting bot podkłada bombę, znajduje ponownie dostępne miejsca i przechodzi w stan escaping
            case BotState.planting:
                plantBomb();
                findAvailable();
                break;
            //w stanie moving bot sprawdza, czy dotarł do miejsca podłożenia bomby
            case BotState.moving:
                checkDestinationReached();
                break;
            //w stanie escaping bot rozpoczyna ucieczkę z miejsca podłożenia bomby
            case BotState.escaping:
                escape();
                break;
            //w stanie runningAway bot sprawdza, czy dtarłdo celu ucieczki
            case BotState.runningAway:
                chooseTarget();
                break;

        }
    }
    /**
     *Funkcja aktualizująca dane w tablicy położenia elementów na mapie 
     */
    void tableSetup()
    {
        findWalls();
        findCubes();
        findPlayer();
        findMe();
        findBombs();
    }
    /**
     * Funkcja wyszukująca niezniszczalne ściany i wstawiająca je w odpowiednie miejsca w tabeli
     */
    private void findWalls()
    {
        //utworzenie listy wszystkich aktywnych obiektów z tagiem Wall (niezniszczalna ściana)
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Wall");
        //dodanie każdego obiektu z listy do tablicy elementów (z odpowiednią konwersją wspólrzędnych)
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "wall";
        }
    }
    /**
     * Funkcja wyszukująca zniszczalne ściany i wstawiająca je w odpowiednie miejsca w tabeli
     */
    private void findCubes()
    {
        //utworzenie listy wszystkich aktywnych obiektów z tagiem Block (zniszczalna ściana)
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Block");
        //dodanie każdego obiektu z listy do tablicy elementów (z odpowiednią konwersją wspólrzędnych)
        foreach (GameObject obj in tempList)
        {
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "cube";
        }
    }
    /**
     * Funkcja wyszukująca gracza i wstawiająca go w odpowiednie miejsca w tabeli
     */
    private void findPlayer()
    {
        //dodanie gracza do tablicy elementów  i zapisanie jego współrzędnych
        // do odpowiednich zmiennych (z odpowiednią konwersją wspólrzędnych)
        playerX = ((int)player.transform.position.x / 2 + 1);
        playerY = ((int)player.transform.position.z / 2 + 1);
        elementMap[playerX, playerY] = "player";
    }
    /**
     * Funkcja wyszukująca bota wstawiająca go w odpowiednie miejsca w tabeli
     */
    private void findMe()
    {
        //dodanie bota do tablicy elementów  i zapisanie jego współrzędnych
        // do odpowiednich zmiennych (z odpowiednią konwersją wspólrzędnych)
        int x = ((int)transform.position.x / 2 + 1);
        int y = ((int)transform.position.z / 2 + 1);
        elementMap[x, y] = "bot";
    }
    /**
     * Funkcja wyszukująca w tabeli miejsca, do których może dotrzeć bot i dodająca je do listy
     */
    private void findAvailable()
    {
        //zmienna pomocnicza do obliczania, czy ścieżka jest dostępna
        NavMeshPath path = new NavMeshPath();
        for (int i = 0; i < elementMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j < elementMap.GetUpperBound(1); j++)
            {
                //dla każdego elementu tablicy, jeśli dany element jest dostępny (czyli komórka zawiera "a")
                if (elementMap[i, j] == "a")
                {
                    //
                    Vector3 destination = new Vector3(i, 0, j);
                    //obliczenie ścieżki do wybranego elementu
                    agent.CalculatePath(destination, path);
                    //jeśli ścieżka została ukończuna (jest możliwa do przejścia)
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
    }
    /**
     * Funkcja wyszukująca miejsce najbliższe do gracza na którym można podstawić bombę i ustawiająca je jako cel dla bota
     */
    private void findTarget()
    {
        Vector3 destination = new Vector3();
        int minX = 1000, minY = 1000, tempX, tempY;
        foreach (Vector3 pos in availablePositions)
        {
            if(elementMap[(int)pos.x, (int)pos.z] != "bomb" || elementMap[(int)pos.x, (int)pos.z] != "explosion")
            {
                //obliczenie wartości bezwzględnej odległosci między graczem a danymi z listy jako zmienna tymczasowa
                tempX = (int)Mathf.Abs(playerX - pos.x);
                tempY = (int)Mathf.Abs(playerY - pos.z);
                if (tempY < minY)
                {
                    //jeśli zmienna tymczasowa jest mniejsza od dotychczasowego minimum to zmieniamy minimum
                    //i ustawiamy wspólrzędną z wektora docelowego odpowiednio przeliczając dane z listy
                    minY = tempY;
                    destination.z = pos.z * 2 - 1;
                }
                if (tempX < minX)
                {
                    //jeśli zmienna tymczasowa jest mniejsza od dotychczasowego minimum to zmieniamy minimum
                    //i ustawiamy wspólrzędną z wektora docelowego odpowiednio przeliczając dane z listy
                    minX = tempX;
                    destination.x = pos.x * 2 - 1;
                }
            }
        }
        //ustawiamy cel bota na wsólrzędne wektora docelowego
        agent.SetDestination(destination);
        //zmiana stanu bota
        state = BotState.moving;
    }
    /**
     * Funkcja sprawdzająca, czy bot dotarł do celu podstawienia bomby
     */
    void checkDestinationReached()
    {
        //obliczenie dystansu między botem a celem
        float distanceToTarget = Vector3.Distance(transform.position, agent.destination);
        //jeśli dystans jest mniejszy niż 0.5, to bot może podłożyć bombę
        if (distanceToTarget < 0.5f)
        {
            //zmiana stanu bota
            state = BotState.planting;
        }
    }
    /**
     * Funkcja podstawiająca bombę przez bota
     */
    void plantBomb()
    {

        if (cooldown == 0)
        {
            // wlaczenie animacji stawiania bomby
            animator.SetBool("isPlanting", true);
            // ustawienie opoznienia na ok. czas stawiania bomby
            cooldown = 150;
        }
        if (cooldown == 75)
        {
            //pobranie zasięgu i czasu życia bomby ze skryptu postaci
            int range = this.range;
            int bombLifetime = this.bombLifetime;
            //obliczenie bazowego wektora (z float na int)
            Vector3Int intVector = new Vector3Int((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
            //obliczenie wektora pozycji stawiającego bombę na środku pola
            Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
            //stworzenie bomby
            GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
            //przekazanie czasu życia
            bomb.GetComponent<Bomb>().maxLifetime = bombLifetime;
            //przekazanie zasięgu i czasów życia do obiektów odpowiadających za zadawanie obrażeń
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
            bomb.transform.GetChild(1).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
            bomb.transform.GetChild(2).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
            bomb.transform.GetChild(3).GetComponent<BombExplosion>().lifetime = bombLifetime;
            //przekazanie wartości trójwymiarowości do bomby
            bomb.GetComponent<Bomb>().is3D = false;
            //przekazanie zasięgu do bomby
            bomb.GetComponent<Bomb>().range = range;
        }
        //zmiana stanu bota
        if (animator.GetBool("isPlanting") == true)
            return;
        state = BotState.escaping;
    }
    /**
     * Funkcja wyszukująca bomby i wstawiająca je w odpowiednie miejsca w tabeli
     */
    void findBombs()
    {
        //utworzenie listy wszystkich aktywnych obiektów z tagiem Block (zniszczalna ściana)
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject obj in tempList)
        {
            //konwersja współrzędnych bomby na odpowiednią pozycję w tablicy
            int x = ((int)obj.transform.position.x / 2 + 1);
            int y = ((int)obj.transform.position.z / 2 + 1);
            elementMap[x, y] = "bomb";
            //zmienna pomocnicza - zasięg bomby
            int rad = obj.GetComponent<Bomb>().range;
            //zmienna pomocnicza
            int a = 1;
            //dopóki a jest mniejsze niż zasięg i nie napotkaliśly ściany wpisujemy
            //w odpowiednie miejsca tabeli elementów ostrzeżenie o ekspolzji w każdym z kierunków
            while (a <= rad && elementMap[x + a, y] != "wall")
            {
                elementMap[x + a, y] = "explosion";
                a++;
            }
            a = 1;
            while (a <= rad && elementMap[x - a, y] != "wall")
            {
                elementMap[x - a, y] = "explosion";
                a++;
            }
            a = 1;
            while (a <= rad && elementMap[x, y + a] != "wall")
            {
                elementMap[x, y + a] = "explosion";
                a++;
            }
            a = 1;
            while (a <= rad && elementMap[x, y - a] != "wall")
            {
                elementMap[x, y - a] = "explosion";
                a++;
            }
            //usuwanie pozycji z bombami z listy dostępnych współrzędnych 
             for (int i = availablePositions.Count - 1; i > -1; i--)
            {
                if (availablePositions[i].x == x && availablePositions[i].z == y)
                {
                    availablePositions.RemoveAt(i);
                }
            }
        }
    }
    /**
     * Funkcja "ucieczki" bota po podstawieniu bomby
     */
    void escape()
    {
        //wybranie losoewgo elementu z listy dostępnych wspólrzędnych
        if (availablePositions.Count > 0)
        {
            int rnd = Random.Range(0, availablePositions.Count - 1);
            Vector3 dest = availablePositions[rnd];
            while (elementMap[(int)dest.x, (int)dest.z] == "bomb" || elementMap[(int)dest.x, (int)dest.z] == "explosion")
            {
                rnd = Random.Range(0, availablePositions.Count - 1);
                dest = availablePositions[rnd];
            }
            //ustawienie celu na wylosowaną pozycję
            agent.SetDestination(dest);
        }
        //wybranie losowo liczby między 0 a 2
        int rnd2 = Random.Range(0, 2);
        //losowe wybranie kolejnego stanu - 33,3% szansy, że bot będzie w następnym ruchu ścigał gracza
        //66,6% szansy, że następny ruch bota będzie losowy
        if (rnd2 == 0)
            state = BotState.runningAway;
        if (rnd2 > 0)
            state = BotState.moving;
    }
    /**
     * Funkcja sprawdzająca, czy bot dotarł do celu ucieczki
     */
    void chooseTarget()
    {
        //obliczenie dysansu od bota do losowo wybrnego celu
        float distanceToTarget = Vector3.Distance(transform.position, agent.destination);
        //jesli dystans jest mniejszy niż 0,5 zmieniamy stan bota na stationary
        if (distanceToTarget < 0.5f)
        {
            state = BotState.stationary;
        }
    }
}
