using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.RegularExpressions;

[RequireComponent(typeof(BoardDeadlock))]
[RequireComponent(typeof(BoardShuffler))]
public class Board : MonoBehaviour
{
    CollectionGoal collection;
    public LevelGoalCollected level;
    // dimensions of board
    public int width;
    public int height;
    public int maxHeight = 15;

    float verticalSizeGlobal;

    public bool bombsStillRemaining = true;
    public static bool meteorsImpacting;

    // margin outside Board for calculating camera field of view
    public int borderSizeSides;
    public int borderSizeTopBottom;
    float globalAspectRatio;

    public static bool isWateringSoundPlaying;

    // Prefab representing a single tile
    public GameObject tileNormalPrefab;
    public GameObject tileFalloutPrefab;
    public GameObject tileBrokenBarrelPrefab;
    public GameObject tileObstaclePrefab;
    public GameObject tileFactoryPrefab;
    public GameObject tileCityPrefab;
    public GameObject tileMinePrefab;
    public GameObject tilePlantsPrefab;
    public GameObject tileTrashPrefab;
    public GameObject tileNuclearRuinsPrefab;
    public GameObject tileLitterPrefab;
    public GameObject tileBarrelPrefab;
    public GameObject tileBreakablePrefab;
    public GameObject tileDoubleBreakablePrefab;
    public GameObject tileTripleBreakablePrefab;
    public GameObject tileSolarPrefab;
    public GameObject tileNuclearPPPrefab;
    public GameObject tileAlarmNuclearPPPrefab;
    public GameObject tileOceanPrefab;
    public GameObject tileWindmillPrefab;
    public GameObject tilePipelinePrefab1;
    public GameObject tilePipelinePrefab2;
    public GameObject tilePipelinePrefab3;
    public GameObject tilePipelinePrefab4;
    public GameObject tilePipelinePrefab5;
    public GameObject tilePipelinePrefab6;
    public GameObject tileWaterDamPrefab;
    public GameObject tileWindmillOnPrefab;
    public GameObject meteorPrefab;
    public GameObject tilewasteDump0Prefab;
    public GameObject tilewasteDump1Prefab;
    public GameObject tilewasteDump2Prefab;
    public GameObject tileOil;
    public GameObject backgroundPrefab;
    public GameObject forestPrefab;
    public bool clearRemainingBombs = true;

    public GameObject specialBombWaitingBluePrefab;
    public GameObject specialBombJumpingBluePrefab;
    public GameObject specialBombWaitingGreenPrefab;
    public GameObject specialBombJumpingGreenPrefab;
    public GameObject specialBombWaitingPurplePrefab;
    public GameObject specialBombJumpingPurplePrefab;
    public GameObject specialBombWaitingRedPrefab;
    public GameObject specialBombJumpingRedPrefab;
    public GameObject specialBombWaitingWhitePrefab;
    public GameObject specialBombJumpingWhitePrefab;
    public GameObject specialBombWaitingYellowPrefab;
    public GameObject specialBombJumpingYellowPrefab;
    //List<GameObject> specialBombsWaiting;
    GameObject[,] specialBombsWaiting;
    int specialBombsOnBoard;
    bool lookForAllColB;

    public GameObject jumpingBombBlue;
    public GameObject jumpingBombGreen;
    public GameObject jumpingBombPurple;
    public GameObject jumpingBombRed;
    public GameObject jumpingBombWhite;
    public GameObject jumpingBombYellow;

    public GameObject bigBombBlue;
    public GameObject bigBombGreen;
    public GameObject bigBombPurple;
    public GameObject bigBombRed;
    public GameObject bigBombWhite;
    public GameObject bigBombYellow;

    //bools for decreasing available pregame boosters 
    bool decreaseColorB;
    bool decreaseJumpingB;
    bool decreaseBigB;

    //road prefabs
    public GameObject roadPrefab1;
    public GameObject roadPrefab2;
    public GameObject roadPrefab3;
    public GameObject roadPrefab4;
    public GameObject roadPrefab5;
    public GameObject roadPrefab6;
    public GameObject roadPrefab7;
    public GameObject roadPrefab8;
    public GameObject roadPrefab9;
    public GameObject roadPrefab10;
    public GameObject roadPrefab11;
    public GameObject roadPrefab12;

    public static int factoriesOnBoard = 0;
    public static int citiesOnBoard = 0;
    public static int oilMinesOnBoard = 0;
    int falloutOnBoard = 0;

    int plantsOnBoard = 0;

    int nuclearRuinsOnBoard = 0;

    int breakableTOnBoard = 0;
    int doubleBreakableTOnBoard = 0;
    int tripleBreakableTOnBoard = 0;
    int solarPanelsOnBoard = 0;

    int windmillsOnBoard = 0;

    int pipelinesOnBoard = 0;
    int waterDamsOnBoard = 0;
    int roadTilesOnBoard = 0;
    //int oiledTilePrefab = 0;

    //float boardShuffleWait = 0;

    public static int boardShuffles = -1;

    public static bool boosterMade = false;

    bool switchBoosterMade;

    List<GamePiece> filledPieces;

    List<GamePiece> usedMatches;

    //Trash cans for different waste types
    public StartingObject[] trashCans;

    public GameObject eWasteCanPrefab;

    List<int> eWasteColumns;

    public GameObject organicCanPrefab;

    List<int> organicColumns;

    public GameObject plasticCanPrefab;

    List<int> plasticColumns;

    public GameObject glassCanPrefab;

    List<int> glassColumns;

    // array of dot Prefabs
    public GameObject[] gamePiecePrefabs;

    // Prefab array for adjacent bombs
    public GameObject[] adjacentBombPrefabs;

    // Prefab array for column clearing bombs
    public GameObject[] columnBombPrefabs;

    // Prefab array for row clearing bombs
    public GameObject[] rowBombPrefabs;

    //variable to hold data either to produce litter or not (every second move)
    int lastLitter = 1;

    // Prefab bomb FX for clearing a single color from the Board
    public GameObject colorBombPrefab;

    public GameObject ColorStrikeprefab;

    // the maximum number of Collectible game pieces allowed per Board
    public int maxCollectibles = 3;

    // the current number of Collectibles on the Board
    public int collectibleCount = 0;

    // this is the percentage for a top-row tile to get a collectible
    [Range(0, 1)]
    public float chanceForCollectible = 0.1f;

    // an array of our Collectible game objects
    //public GameObject[] collectiblePrefabs;

    // reference to a Bomb created on the clicked Tile (first Tile clicked by mouse or finger)
    GameObject m_clickedTileBomb;

    GameObject currentGamePiece;

    List<GameObject> allBombs;

    public static bool barrelBroken = false;

    // reference to a Bomb created on the target Tile (Tile dragged into by mouse or finger)
    GameObject m_targetTileBomb;

    // the time required to swap GamePieces between the Target and Clicked Tile
    public float swapTime = 0.2f;

    // array of all the Board's Tile pieces
    public Tile[,] m_allTiles;

    Tiles2ndLayer[,] m_all2ndLayerTiles;

    // array of all of the Board's GamePieces
    GamePiece[,] m_allGamePieces;
    GamePiece[,] m_tempGamePieces;

    //array of all Earths ?
    Earth[,] m_allEarths;

    public static int nuclearDisasters = 0;

    public static List<int> nuclearListX = new List<int>();
    public static List<int> nuclearListY = new List<int>();

    bool canBeMovedOnRoad = true;

    bool canBreakBarrel = true;

    bool canProduceBarrel = true;

    public static bool locked = false;

    bool canproduceLitterTrash = true;

    bool delayAfterNuclearBlast = false;

    bool twoColors = false;

    public static bool delayAfterMeteorBooster = false;

    Coroutine matchRoutine;

    // Tile first clicked by mouse or finger
    Tile m_clickedTile;

    // adjacent Tile dragged into by mouse or finger
    Tile m_targetTile;

    // whether user input is currently allowed
    public static bool m_playerInputEnabled = true;

    // manually positioned Tiles, placed before Board is filled
    public StartingObject[] startingTiles;
    public StartingObject[] starting2ndTiles;

    public BoardCollectibles[] bCollectibles;

    // manually positioned GamePieces, placed before the Board is filled
    public StartingObject[] startingGamePieces;

    // manually positioned Earths, placed before Board is filled
    public StartingObject[] startingForests;

    // manually positioned Oceans, placed before Board is filled
    public StartingObject[] startingPlanets;

    // manager class for particle effects
    public ParticleManager m_particleManager;

    // Y Offset used to make the pieces "fall" into place to fill the Board
    public int fillYOffset = 10;

    // time used to fill the Board
    public float fillMoveTime = 0.5f;

    // the current score multiplier, depending on how many chain reactions we have caused
    int m_scoreMultiplier = 0;

    List<MatchValue> allValuesOnBoard;

    public bool isRefilling = false;

    BoardDeadlock m_boardDeadlock;

    BoardShuffler m_boardShuffler;

    List<int> ColumnsToCollapse;

    //list of oceans
    Ocean[,] m_allOceans;

    List<MatchValue> matchValues;

    //clear board after nuclear blast
    bool clearBoardAfterBlast;
    bool clearBoardAfterPlanetCooled;
    int routine = 0;

    // this is a generic GameObject that can be positioned at coordinate (x,y,z) when the game begins
    [System.Serializable]
    public class StartingObject
    {
        public GameObject prefab;
        public int x;
        public int y;
        public int z;
    }

    [System.Serializable]
    public class BoardCollectibles
    {
        public GameObject prefab;
        public int NoToColl;
        public int NoToCollcopy;
    }

    // invoked when we start the level
    void Start()
    {
        // initialize array of Tiles
        m_allTiles = new Tile[width, height];
        matchValues = new List<MatchValue>();
        m_all2ndLayerTiles = new Tiles2ndLayer[width, height];
        // initialize array of Earths
        m_allEarths = new Earth[width - 1, height - 1];

        // initialize array of Earths
        m_allOceans = new Ocean[width - 1, height - 1];

        // initial array of GamePieces
        m_allGamePieces = new GamePiece[width, height];
        m_tempGamePieces = new GamePiece[width, height];

        // find the ParticleManager by Tag
        m_particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();

        m_boardDeadlock = GetComponent<BoardDeadlock>();

        m_boardShuffler = GetComponent<BoardShuffler>();

        allBombs = new List<GameObject>();

        eWasteColumns = new List<int>();

        organicColumns = new List<int>();

        glassColumns = new List<int>();

        plasticColumns = new List<int>();

        ColumnsToCollapse = new List<int>();

        filledPieces = new List<GamePiece>();

        usedMatches = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            ColumnsToCollapse.Add(i);
        }

        allValuesOnBoard = new List<MatchValue>();

        specialBombsWaiting = new GameObject[width, height]; ;

        collection = FindObjectOfType<CollectionGoal>();

        level = FindObjectOfType<LevelGoalCollected>();
        nuclearListX.Clear();
        nuclearListY.Clear();

        //level.collectionGoals.Clear();

        #region 
        factoriesOnBoard = 0;
        citiesOnBoard = 0;
        oilMinesOnBoard = 0;
        falloutOnBoard = 0;

        plantsOnBoard = 0;

        nuclearRuinsOnBoard = 0;

        breakableTOnBoard = 0;
        doubleBreakableTOnBoard = 0;
        tripleBreakableTOnBoard = 0;
        solarPanelsOnBoard = 0;

        windmillsOnBoard = 0;

        nuclearDisasters = 0;
        boardShuffles = -1;
        m_scoreMultiplier = 0;
        pipelinesOnBoard = 0;
        waterDamsOnBoard = 0;
        verticalSizeGlobal = 0;
        globalAspectRatio = 0;
        roadTilesOnBoard = 0;

        clearBoardAfterBlast = false;
        isRefilling = false;
        m_playerInputEnabled = true;
        locked = false;
        boosterMade = false;
        delayAfterNuclearBlast = false;
        delayAfterMeteorBooster = false;
        twoColors = false;
        clearBoardAfterPlanetCooled = false;
        bombsStillRemaining = true;
        switchBoosterMade = false;
        decreaseColorB = true;
        decreaseJumpingB = true;
        decreaseBigB = true;
        meteorsImpacting = false;
        isWateringSoundPlaying = false;
        specialBombsOnBoard = 0;
        lookForAllColB = false;
        #endregion

        FindAllMatchValuesOnBoard();

    }

    private void FindAllMatchValuesOnBoard()
    {
        for (int i = 0; i < gamePiecePrefabs.Length; i++)
        {
            GamePiece piece = gamePiecePrefabs[i].GetComponent<GamePiece>();
            if (piece != null)
            {
                if (piece.matchValue != MatchValue.None && piece.matchValue != MatchValue.Litter && piece.matchValue != MatchValue.Barrel && piece.matchValue != MatchValue.BrokenBarrel)
                {
                    MatchValue match = piece.matchValue;
                    allValuesOnBoard.Add(match);
                }
            }
        }
    }

    // This function sets up the Board.
    public void SetupBoard()
    {
        // sets up any manually placed Tiles
        SetupTiles();

        SetupOceans();

        Setup2ndLayerTiles();

        // sets up any manually placed Earths
        SetupEarths();

        // sets up any manually placed GamePieces
        //SetupGamePieces();

        // check the Board for Collectibles 
        List<GamePiece> startingCollectibles = FindAllCollectibles();
        collectibleCount = startingCollectibles.Count;

        //SetupBoosters();

        // fill the empty Tiles of the Board with GamePieces
        InitialFillBoard(fillYOffset, 0f);

        if (bCollectibles.Length > 0)
        {
            SetupTrashCans();
        }

        // place our Camera to frame the Board with a certain border
        SetupCamera();

        CollectibleGoals();
        UIManager.Instance.SetupCollectionGoalLayout(level.collectionGoals);

        matchRoutine = StartCoroutine(CheckMatches());

        SetupBackgroundTiles();
    }


    void SetupBoosters()
    {
        if (GameManager.useColorBooster == true)
        {

            int randomX = UnityEngine.Random.Range(0, width);
            int randomY = UnityEngine.Random.Range(0, height);
            int loops = 0;
            bool execute = true;



            while (!NotOccupiedByObstacle(randomX, randomY) || IsOccupiedByStartingPieces(randomX, randomY) || m_allGamePieces[randomX, randomY] != null)
            {
                randomX = UnityEngine.Random.Range(0, width);
                randomY = UnityEngine.Random.Range(0, height);

                loops++;
                if (loops > 100)
                {
                    execute = false;
                    break;
                }
            }

            if (execute)
            {
                ClearPieceAt(randomX, randomY);
                GameObject bombObject = MakeBomb(colorBombPrefab, randomX, randomY);
                ActivateBomb(bombObject);
                //Debug.Log("color");
                if (decreaseColorB == true)
                {
                    decreaseColorB = false;
                    int boosterNo = Mathf.Clamp(--LoadInfo.booster3No, 0, 999);
                    string boostertype = "colorBomb";
                    --LoadInfo.booster3No;
                    LoadInfo.Save(boosterNo, boostertype);
                }
            }
        }

        if (GameManager.useJumpingBooster == true)
        {

            int randomX = UnityEngine.Random.Range(0, width);
            int randomY = UnityEngine.Random.Range(0, height);
            int loops = 0;
            bool execute = true;



            while (!NotOccupiedByObstacle(randomX, randomY) || IsOccupiedByStartingPieces(randomX, randomY) || m_allGamePieces[randomX, randomY] != null)
            {
                randomX = UnityEngine.Random.Range(0, width);
                randomY = UnityEngine.Random.Range(0, height);

                loops++;
                if (loops > 100)
                {
                    execute = false;
                    break;
                }
            }

            if (execute)
            {
                int randomIdx = UnityEngine.Random.Range(0, gamePiecePrefabs.Length);
                if (gamePiecePrefabs[randomIdx] != null)
                {
                    GamePiece piece = gamePiecePrefabs[randomIdx].GetComponent<GamePiece>();
                    if (piece != null)
                    {
                        MatchValue match = piece.matchValue;
                        GameObject bombObject = null;

                        switch (match)
                        {
                            case MatchValue.Blue:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombBlue, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;
                            case MatchValue.Green:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombGreen, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;
                            case MatchValue.Purple:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombPurple, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;
                            case MatchValue.Red:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombRed, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;
                            case MatchValue.White:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombWhite, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;
                            case MatchValue.Yellow:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(jumpingBombYellow, randomX, randomY);
                                ActivateBomb(bombObject);
                                break;

                        }
                        //Debug.Log("jumping");

                        if (decreaseJumpingB == true)
                        {
                            decreaseJumpingB = false;
                            int boosterNo = Mathf.Clamp(--LoadInfo.booster6No, 0, 999);
                            string boostertype = "jumping";
                            --LoadInfo.booster6No;
                            LoadInfo.Save(boosterNo, boostertype);
                            specialBombsOnBoard++;
                        }
                    }
                }
            }
        }

        if (GameManager.useBigBooster == true)
        {

            int randomX = UnityEngine.Random.Range(0, width);
            int randomY = UnityEngine.Random.Range(0, height);
            int loops = 0;
            bool execute = true;



            while (!NotOccupiedByObstacle(randomX, randomY) || IsOccupiedByStartingPieces(randomX, randomY) || m_allGamePieces[randomX, randomY] != null)
            {
                randomX = UnityEngine.Random.Range(0, width);
                randomY = UnityEngine.Random.Range(0, height);

                loops++;
                if (loops > 100)
                {
                    execute = false;
                    break;
                }
            }

            if (execute)
            {
                int randomIdx = UnityEngine.Random.Range(0, gamePiecePrefabs.Length);
                if (gamePiecePrefabs[randomIdx] != null)
                {
                    GamePiece piece = gamePiecePrefabs[randomIdx].GetComponent<GamePiece>();
                    if (piece != null)
                    {
                        MatchValue match = piece.matchValue;
                        GameObject bombObject = null;

                        switch (match)
                        {
                            case MatchValue.Blue:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombBlue, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;
                            case MatchValue.Green:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombGreen, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;
                            case MatchValue.Purple:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombPurple, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;
                            case MatchValue.Red:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombRed, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;
                            case MatchValue.White:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombWhite, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;
                            case MatchValue.Yellow:
                                ClearPieceAt(randomX, randomY);
                                bombObject = MakeBomb(bigBombYellow, randomX, randomY);
                                ActivateBomb(bombObject);

                                break;

                        }
                        //Debug.Log("big");
                        if (decreaseBigB == true)
                        {
                            decreaseBigB = false;
                            int boosterNo = Mathf.Clamp(--LoadInfo.booster7No, 0, 999);
                            string boostertype = "big";
                            --LoadInfo.booster7No;
                            LoadInfo.Save(boosterNo, boostertype);
                        }
                    }
                }
            }
        }
    }

    bool IsOccupiedByStartingPieces(int x, int y)
    {
        for (int i = 0; i < startingGamePieces.Length; i++)
        {
            if (startingGamePieces[i] != null)
            {
                if (startingGamePieces[i].x == x && startingGamePieces[i].y == y)
                {
                    return true;
                }

            }
        }

        return false;
    }

    void SetupBackgroundTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allTiles[i, j] != null && m_allTiles[i, j].tileType != TileType.Wall && m_allTiles[i, j].tileType != TileType.Teleport)
                {
                    if (backgroundPrefab != null && IsWithinBounds(i, j))
                    {

                        GameObject bcktile = Instantiate(backgroundPrefab, new Vector3(i, j, 6), Quaternion.identity) as GameObject;
                        BackgroundTiles bT = bcktile.GetComponent<BackgroundTiles>();
                        bT.transform.parent = transform;
                        bT.Init(i, j, this);
                    }
                }
            }
        }
    }

    void SetupOceans()
    {
        foreach (StartingObject sOcean in startingPlanets)
        {
            if (sOcean != null)
            {
                GameObject ocean = Instantiate(sOcean.prefab, new Vector3(sOcean.x, sOcean.y, -1), Quaternion.identity) as GameObject;

                m_allOceans[sOcean.x, sOcean.y] = ocean.GetComponent<Ocean>();
                ocean.name = "Planet (" + sOcean.x + "," + sOcean.y + ")";
                ocean.transform.parent = transform;

                m_allOceans[sOcean.x, sOcean.y].PlaceOcean(sOcean.x, sOcean.y, this);

                for (int xOffset = 0; xOffset < 2; xOffset++)
                {
                    for (int yOffset = 0; yOffset < 2; yOffset++)
                    {
                        if (tileOceanPrefab != null)
                        {
                            MakeNewTile(tileOceanPrefab, sOcean.x + xOffset, sOcean.y + yOffset, 5);
                        }
                    }
                }

            }
        }
    }

    private void CollectibleGoals()
    {
        foreach (BoardCollectibles boardCollectibles in bCollectibles)
        {
            CollectionGoal collectionGoal = Instantiate(collection) as CollectionGoal;
            collectionGoal.MakeNewCollection(boardCollectibles.prefab, boardCollectibles.NoToCollcopy);
            level.collectionGoals.Add(collectionGoal);
        }
    }

    private void SetupEarths()
    {
        int forestOnBoard = 0;
        foreach (StartingObject sEarth in startingForests)
        {
            if (sEarth != null)
            {
                if (sEarth.prefab != null)
                {
                    // create a Earth at position (x,y,z) with no rotations; rename the Tile and parent it 

                    // to the Board, then initialize the Earth into the m_allEarths array

                    GameObject earth = Instantiate(sEarth.prefab, new Vector3(sEarth.x, sEarth.y, 5), Quaternion.identity) as GameObject;

                    m_allEarths[sEarth.x, sEarth.y] = earth.GetComponent<Earth>();
                    earth.name = "Forest (" + sEarth.x + "," + sEarth.y + ")";
                    earth.transform.parent = transform;

                    m_allEarths[sEarth.x, sEarth.y].PlaceEarth(sEarth.x, sEarth.y, this);
                    forestOnBoard++;
                    foreach (CollectionGoal collectionGoal in level.collectionGoals)
                    {
                        if (collectionGoal != null)
                        {
                            if (collectionGoal.prefabToCollect == tileBreakablePrefab)
                            {
                                level.collectionGoals.Remove(collectionGoal);

                                if (UIManager.Instance != null)
                                {
                                    UIManager.Instance.UpdateCollectionGoalLayout1();
                                }

                                break;
                            }
                        }
                    }

                }
            }

        }

        if (forestOnBoard > 0)
        {
            MakeCollectionGoal(forestPrefab, forestOnBoard);
        }

    }

    private void SetupTrashCans()
    {
        foreach (StartingObject sCan in trashCans)
        {
            if (sCan != null)
            {
                if (sCan.prefab == eWasteCanPrefab)
                {
                    eWasteColumns.Add(sCan.x);
                }

                if (sCan.prefab == organicCanPrefab)
                {
                    organicColumns.Add(sCan.x);
                }

                if (sCan.prefab == plasticCanPrefab)
                {
                    plasticColumns.Add(sCan.x);
                }

                if (sCan.prefab == glassCanPrefab)
                {
                    glassColumns.Add(sCan.x);
                }

                if (sCan.prefab != null)
                {
                    // create a Tile at position (x,y,z) with no rotations; rename the Tile and parent it 

                    // to the Board, then initialize the Tile into the m_allTiles array

                    GameObject can = Instantiate(sCan.prefab, new Vector3(sCan.x, -1, -3), Quaternion.identity) as GameObject;

                    can.transform.parent = transform;

                }
            }

        }
    }

    // Creates a GameObject prefab at certain (x,y,z) coordinate
    void MakeTile(GameObject prefab, int x, int y, int z = 1)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y))
        {
            // create a Tile at position (x,y,z) with no rotations; rename the Tile and parent it 

            // to the Board, then initialize the Tile into the m_allTiles array

            GameObject tile = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            tile.name = "Tile (" + x + "," + y + ")";
            m_allTiles[x, y] = tile.GetComponent<Tile>();
            tile.transform.parent = transform;
            m_allTiles[x, y].Init(x, y, this);
        }
    }

    void Make2ndLayerTile(GameObject prefab, int x, int y, int z = 0)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y))
        {
            // create a Tile at position (x,y,z) with no rotations; rename the Tile and parent it 

            // to the Board, then initialize the Tile into the m_allTiles array

            GameObject tile = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            tile.name = "Tile2nd (" + x + "," + y + ")";
            m_all2ndLayerTiles[x, y] = tile.GetComponent<Tiles2ndLayer>();
            tile.transform.parent = transform;
            m_all2ndLayerTiles[x, y].Init(x, y, this);
        }
    }

    public void MakeNewTile(GameObject prefab, int x, int y, int z = 1)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y))
        {
            //Clear old tile
            Tile tileOld = m_allTiles[x, y];

            if (tileOld != null)
            {

                //GameManager.Instance.UpdateCollectionGoalsTiles(tileOld);
                m_allTiles[x, y] = null;
                Destroy(tileOld.gameObject);
            }
            // create a Tile at position (x,y,z) with no rotations; rename the Tile and parent it 

            // to the Board, then initialize the Tile into the m_allTiles array

            GameObject tile = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            tile.name = "Tile (" + x + "," + y + ")";
            m_allTiles[x, y] = tile.GetComponent<Tile>();
            tile.transform.parent = transform;
            m_allTiles[x, y].Init(x, y, this);
        }
    }

    public void MakeNew2ndLayerTile(GameObject prefab, int x, int y, int z = -2)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y))
        {
            //Clear old tile
            Tiles2ndLayer tileOld = m_all2ndLayerTiles[x, y];

            if (tileOld != null)
            {
                m_all2ndLayerTiles[x, y] = null;
                Destroy(tileOld.gameObject);
            }
            // create a Tile at position (x,y,z) with no rotations; rename the Tile and parent it 

            // to the Board, then initialize the Tile into the m_allTiles array

            GameObject tile = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            tile.name = "Tile2nd (" + x + "," + y + ")";
            m_all2ndLayerTiles[x, y] = tile.GetComponent<Tiles2ndLayer>();
            tile.transform.parent = transform;
            m_all2ndLayerTiles[x, y].Init(x, y, this);
        }
    }

    // Creates a GamePiece prefab at a certain (x,y,z) coordinate
    void MakeGamePiece(GameObject prefab, int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y) && m_allGamePieces[x, y] == null)
        {
            prefab.GetComponent<GamePiece>().Init(this);
            PlaceGamePiece(prefab.GetComponent<GamePiece>(), x, y);

            // allows the GamePiece to be placed higher than the Board, so it can be moved into place


            if (falseYOffset != 0)
            {
                prefab.transform.position = new Vector3(x, y + falseYOffset, 0);
                //Debug.Log("move time:" + moveTime);
                prefab.GetComponent<GamePiece>().Move(x, y, moveTime);
            }

            // parent the GamePiece to the Board
            prefab.transform.parent = transform;
        }
    }

    void MakeBrokenBarrelPiece(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        GameObject prefab = Instantiate(tileBrokenBarrelPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y) && m_allGamePieces[x, y] == null)
        {
            prefab.GetComponent<GamePiece>().Init(this);
            PlaceGamePiece(prefab.GetComponent<GamePiece>(), x, y);

            // allows the GamePiece to be placed higher than the Board, so it can be moved into place


            if (falseYOffset != 0)
            {
                prefab.transform.position = new Vector3(x, y + falseYOffset, 0);
                //Debug.Log("move time:" + moveTime);
                prefab.GetComponent<GamePiece>().Move(x, y, moveTime);
            }

            // parent the GamePiece to the Board
            prefab.transform.parent = transform;
        }
    }

    // creat a Bomb prefab at location (x,y)
    GameObject MakeBomb(GameObject prefab, int x, int y)
    {
        // only run the logic on valid GameObject and if we are within the boundaries of the Board
        if (prefab != null && IsWithinBounds(x, y))
        {
            // create a Bomb and initialize it; parent it to the Board

            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<Bomb>().Init(this);
            bomb.GetComponent<Bomb>().SetCoord(x, y);
            bomb.transform.parent = transform;
            return bomb;
        }
        return null;
    }

    // setup the manually placed Tiles
    void SetupTiles()
    {
        foreach (StartingObject sTile in startingTiles)
        {
            if (sTile != null)
            {

                if (sTile.prefab == tileFactoryPrefab)
                {
                    factoriesOnBoard++;
                }
                if (sTile.prefab == tileCityPrefab)
                {
                    citiesOnBoard++;
                }
                if (sTile.prefab == tileMinePrefab)
                {
                    oilMinesOnBoard++;
                }

                if (sTile.prefab == tileNuclearRuinsPrefab)
                {
                    nuclearRuinsOnBoard++;
                }

                if (sTile.prefab == tileBreakablePrefab)
                {
                    breakableTOnBoard++;
                }

                if (sTile.prefab == tileDoubleBreakablePrefab)
                {
                    doubleBreakableTOnBoard++;
                }

                if (sTile.prefab == tileTripleBreakablePrefab)
                {
                    tripleBreakableTOnBoard++;
                }

                if (sTile.prefab == tileFalloutPrefab)
                {
                    falloutOnBoard++;
                }

                if (sTile.prefab == tileSolarPrefab)
                {
                    solarPanelsOnBoard++;
                }

                if (sTile.prefab == tileWindmillPrefab)
                {
                    windmillsOnBoard++;
                }

                if (sTile.prefab == tilePipelinePrefab1 || sTile.prefab == tilePipelinePrefab2 || sTile.prefab == tilePipelinePrefab3
                    || sTile.prefab == tilePipelinePrefab4 || sTile.prefab == tilePipelinePrefab5 || sTile.prefab == tilePipelinePrefab6)
                {
                    pipelinesOnBoard++;
                }

                if (sTile.prefab == tileWaterDamPrefab)
                {
                    waterDamsOnBoard++;
                }

                if (sTile.prefab == tilePlantsPrefab)
                {
                    plantsOnBoard++;
                }

                if (sTile.prefab == tilePipelinePrefab1 || sTile.prefab == tilePipelinePrefab2 || sTile.prefab == tilePipelinePrefab3
                    || sTile.prefab == tilePipelinePrefab4 || sTile.prefab == tilePipelinePrefab5 || sTile.prefab == tilePipelinePrefab6
                    || sTile.prefab == tileDoubleBreakablePrefab || sTile.prefab == tileTripleBreakablePrefab || sTile.prefab == tileBreakablePrefab || sTile.prefab == tilePlantsPrefab)
                {
                    MakeTile(sTile.prefab, sTile.x, sTile.y, 4);
                }

                else if (sTile.prefab == roadPrefab1 || sTile.prefab == roadPrefab2 || sTile.prefab == roadPrefab3 || sTile.prefab == roadPrefab4
                    || sTile.prefab == roadPrefab5 || sTile.prefab == roadPrefab6 || sTile.prefab == roadPrefab7 || sTile.prefab == roadPrefab8
                    || sTile.prefab == roadPrefab9 || sTile.prefab == roadPrefab10 || sTile.prefab == roadPrefab11 || sTile.prefab == roadPrefab12)
                {
                    MakeTile(sTile.prefab, sTile.x, sTile.y, 4);
                    roadTilesOnBoard++;
                }

                else if (sTile.prefab == tileFalloutPrefab)
                {
                    MakeTile(sTile.prefab, sTile.x, sTile.y, -2);
                }

                else
                {
                    MakeTile(sTile.prefab, sTile.x, sTile.y, 0);
                }

            }

        }

        if (solarPanelsOnBoard > 0)
        {
            MakeCollectionGoal(tileSolarPrefab, solarPanelsOnBoard);
        }

        if (falloutOnBoard > 0)
        {
            MakeCollectionGoal(tileFalloutPrefab, falloutOnBoard);

        }

        if (nuclearRuinsOnBoard > 0)
        {
            MakeCollectionGoal(tileNuclearRuinsPrefab, nuclearRuinsOnBoard);

        }

        if (doubleBreakableTOnBoard > 0)
        {
            breakableTOnBoard += doubleBreakableTOnBoard;

        }

        if (tripleBreakableTOnBoard > 0)
        {
            breakableTOnBoard += tripleBreakableTOnBoard;

        }

        if (breakableTOnBoard > 0)
        {
            MakeCollectionGoal(tileBreakablePrefab, breakableTOnBoard);

        }

        if (plantsOnBoard > 0)
        {
            MakeCollectionGoal(tilePlantsPrefab, plantsOnBoard);

        }

        if (windmillsOnBoard > 0)
        {
            MakeCollectionGoal(tileWindmillPrefab, windmillsOnBoard);

        }

        if (waterDamsOnBoard > 0)
        {
            MakeCollectionGoal(tileWaterDamPrefab, waterDamsOnBoard);

        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allTiles[i, j] == null)
                {
                    MakeTile(tileNormalPrefab, i, j, 0);
                }
            }
        }
    }

    void Setup2ndLayerTiles()
    {
        foreach (StartingObject sTile in starting2ndTiles)
        {
            if (sTile != null)
            {
                if (sTile.prefab == tileOil || sTile.prefab == tilewasteDump0Prefab || sTile.prefab == tilewasteDump1Prefab || sTile.prefab == tilewasteDump2Prefab || sTile.prefab == tileTrashPrefab)
                {
                    Make2ndLayerTile(sTile.prefab, sTile.x, sTile.y, -2);
                }
            }

        }
    }

    private void MakeCollectionGoal(GameObject prefab, int number)
    {
        if (collection != null && prefab != null)
        {
            CollectionGoal collectionGoal = Instantiate(collection) as CollectionGoal;
            collectionGoal.MakeNewCollection(prefab, number);
            level.collectionGoals.Add(collectionGoal);
        }
    }

    // setup the manually placed GamePieces
    void SetupGamePieces()
    {
        foreach (StartingObject sPiece in startingGamePieces)
        {
            if (sPiece != null)
            {
                GameObject piece = Instantiate(sPiece.prefab, new Vector3(sPiece.x, sPiece.y, 0), Quaternion.identity) as GameObject;
                MakeGamePiece(piece, sPiece.x, sPiece.y, fillYOffset, fillMoveTime);

            }
        }
    }

    // set the Camera position and parameters to center the Board onscreen with a border
    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, (float)(height - 1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)borderSizeTopBottom;

        float horizontalSize = ((float)width / 2f + (float)borderSizeSides) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;

        if (verticalSize >= horizontalSize)
        {
            verticalSizeGlobal = verticalSize;
            globalAspectRatio = aspectRatio;
        }
        else
        {
            verticalSizeGlobal = horizontalSize;
            globalAspectRatio = aspectRatio;
        }


    }

    // return a random object from an array of GameObjects
    GameObject GetRandomObject(GameObject[] objectArray)
    {
        int randomIdx = UnityEngine.Random.Range(0, objectArray.Length);
        if (objectArray[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }
        return objectArray[randomIdx];
    }

    GameObject GetRandomCollectibleFromArray(BoardCollectibles[] collectiblesArray)
    {
        int randomIdx = UnityEngine.Random.Range(0, collectiblesArray.Length);
        int loops = 0;
        int maxLoops = 50;

        if (collectiblesArray[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }

        while (collectiblesArray[randomIdx].NoToColl == 0)
        {
            randomIdx = UnityEngine.Random.Range(0, collectiblesArray.Length);
            if (collectiblesArray[randomIdx] == null)
            {
                Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
            }
            loops++;
            if (loops >= maxLoops)
            {
                break;
            }
        }
        collectiblesArray[randomIdx].NoToColl--;
        return collectiblesArray[randomIdx].prefab;
    }


    // return a random GamePiece
    GameObject GetRandomGamePiece()
    {
        return GetRandomObject(gamePiecePrefabs);
    }

    // return a random Collectible
    GameObject GetRandomCollectible()
    {
        return GetRandomCollectibleFromArray(bCollectibles);
    }

    // place a GamePiece onto the Board at position (x,y)
    public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD:  Invalid GamePiece!");
            return;
        }

        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;

        if (IsWithinBounds(x, y))
        {
            m_allGamePieces[x, y] = gamePiece;
        }

        gamePiece.SetCoord(x, y);

        Bomb bomb = gamePiece.GetComponent<Bomb>();
        if (bomb != null)
        {
            if (IsSpecialBomb(gamePiece))
            {
                specialBombsOnBoard++;
            }
        }
    }

    // returns true if within the boundaries of the Board, otherwise returns false
    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    // creates a random GamePiece at position (x,y)
    GamePiece FillRandomGamePieceAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

            MakeGamePiece(randomPiece, x, y, falseYOffset, moveTime);
            return randomPiece.GetComponent<GamePiece>();
        }
        return null;
    }

    GamePiece FillLitterGamePieceAt(int x, int y, int falseYOffset = 0, float moveTime = 0f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(tileLitterPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            MakeGamePiece(randomPiece, x, y, falseYOffset, moveTime);
            return randomPiece.GetComponent<GamePiece>();
        }
        return null;
    }

    GamePiece FillBarrelGamePieceAt(int x, int y, int falseYOffset = 0, float moveTime = 0f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(tileBarrelPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            MakeGamePiece(randomPiece, x, y, falseYOffset, moveTime);
            return randomPiece.GetComponent<GamePiece>();
        }
        return null;
    }

    // create a random Collectible at position (x,y) with optional Y Offset
    GamePiece FillRandomCollectibleAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomCollectible(), Vector3.zero, Quaternion.identity) as GameObject;
            MakeGamePiece(randomPiece, x, y, falseYOffset, moveTime);

        }
        return null;
    }

    // fill the Board using a known list of GamePieces instead of Instantiating new prefabs
    void FillBoardFromList(List<GamePiece> gamePieces, GamePiece[,] positions)
    {
        // create a first in-first out Queue to store the GamePieces in a pre-set order
        Queue<GamePiece> unusedPieces = new Queue<GamePiece>(gamePieces);

        // iterations to prevent infinite loop
        int maxIterations = 100;
        int iterations = 0;

        // loop through each position on the Board
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {


                if (unusedPieces.Count > 0)
                {
                    // only fill in a GamePiece if 
                    if (m_allGamePieces[i, j] == null && NotOccupiedByObstacle(i, j) && positions[i, j] != null)
                    {

                        // grab a new GamePiece from the Queue
                        m_allGamePieces[i, j] = unusedPieces.Dequeue();

                        // reset iteration count
                        iterations = 0;

                        // while a match forms when filling in a GamePiece...
                        while (HasMatchOnFill(i, j))
                        {
                            // put the GamePiece back into the Queue at the end of the line
                            unusedPieces.Enqueue(m_allGamePieces[i, j]);

                            // grab a new GamePiece from the Queue
                            m_allGamePieces[i, j] = unusedPieces.Dequeue();

                            // increment iterations each time we try to place a piece
                            iterations++;

                            // if our iterations exceeds limit, break out of the loop and move to next position
                            if (iterations >= maxIterations)
                            {
                                break;
                            }
                        }
                    }
                }

            }
        }

    }

    // fills the empty spaces in the Board with an optional Y offset to make the pieces drop into place
    List<GamePiece> FillBoard()
    {
        float moveTime = 0.07f;
        List<GamePiece> refilledPieces = new List<GamePiece>();
        int maxInterations = 200;
        int iterations = 0;
        int j = height - 1;
        // loop through all spaces of the board
        for (int i = 0; i < width; i++)
        {
            // if the space is unoccupied and does not contain an Obstacle tile 			
            if (m_allGamePieces[i, j] == null && NotOccupiedByOwithTeleport(i, j))
            {

                //Debug.Log(newMoveTime + " for j = " + j);
                // if we are at the top row, check if we can drop a collectible...
                if (CanAddCollectible())
                {
                    if (m_allTiles[i, j].tileType != TileType.Teleport)
                    {
                        // add a random collectible prefab
                        FillRandomCollectibleAt(i, j, 1, moveTime);
                        refilledPieces.Add(m_allGamePieces[i, j]);
                        collectibleCount++;
                    }

                    else
                    {
                        for (int h = 1; h < height; h++)
                        {
                            if (IsWithinBounds(i, j - h))
                            {
                                if (m_allTiles[i, j - h].tileType != TileType.Teleport)
                                {
                                    if (m_allGamePieces[i, j - h] == null && NotOccupiedByObstacle(i, j - h))
                                    {
                                        FillRandomCollectibleAt(i, j - h, 1 + h, moveTime);
                                        refilledPieces.Add(m_allGamePieces[i, j - h]);
                                        collectibleCount++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                // ...otherwise, fill in a game piece prefab
                else
                {
                    if (m_allTiles[i, j].tileType != TileType.Teleport)
                    {
                        FillRandomGamePieceAt(i, j, 1, moveTime);
                        refilledPieces.Add(m_allGamePieces[i, j]);
                        iterations = 0;

                        // if we form a match while filling in the new piece...
                        while (HasMatchOnFill(i, j))
                        {
                            // remove the piece and try again
                            refilledPieces.Remove(m_allGamePieces[i, j]);
                            ClearPieceAt(i, j);

                            FillRandomGamePieceAt(i, j, 1, moveTime);
                            refilledPieces.Add(m_allGamePieces[i, j]);


                            // check to prevent infinite loop
                            iterations++;

                            if (iterations >= maxInterations)
                            {
                                break;
                            }
                        }
                    }

                    else
                    {
                        for (int h = 1; h < height; h++)
                        {
                            if (IsWithinBounds(i, j - h))
                            {
                                if (m_allTiles[i, j - h].tileType != TileType.Teleport)
                                {
                                    if (m_allGamePieces[i, j - h] == null && NotOccupiedByObstacle(i, j - h))
                                    {
                                        FillRandomGamePieceAt(i, j - h, 1 + h, moveTime);
                                        refilledPieces.Add(m_allGamePieces[i, j - h]);
                                        iterations = 0;

                                        // if we form a match while filling in the new piece...
                                        while (HasMatchOnFill(i, j - h))
                                        {
                                            // remove the piece and try again
                                            refilledPieces.Remove(m_allGamePieces[i, j - h]);
                                            ClearPieceAt(i, j - h);

                                            FillRandomGamePieceAt(i, j - h, 1 + h, moveTime);
                                            refilledPieces.Add(m_allGamePieces[i, j - h]);


                                            // check to prevent infinite loop
                                            iterations++;

                                            if (iterations >= maxInterations)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return refilledPieces;
    }

    void InitialFillBoard(int falseYOffset = 0, float moveTime = 0.1f)
    {

        int maxInterations = 100;
        int iterations = 0;
        int loops = 0;
        GamePiece[,] m_Pieces = new GamePiece[width, height];

        do
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    ClearPieceAt(i, j);
                    m_allGamePieces[i, j] = null;
                }
            }

            SetupBoosters();
            SetupGamePieces();

            // loop through all spaces of the board
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    // if the space is unoccupied and does not contain an Obstacle tile 			
                    if (m_allGamePieces[i, j] == null && NotOccupiedByObstacle(i, j))
                    {

                        // if we are at the top row, check if we can drop a collectible...
                        if (j == height - 1 && CanAddCollectible())
                        {

                            // add a random collectible prefab
                            FillRandomCollectibleAt(i, j, falseYOffset, moveTime);
                            collectibleCount++;
                        }

                        // ...otherwise, fill in a game piece prefab
                        else
                        {
                            FillRandomGamePieceAt(i, j, falseYOffset, moveTime);
                            iterations = 0;

                            List<GamePiece> matches = FindMatchesAt(i, j);
                            // if we form a match while filling in the new piece...
                            while (matches.Count > 0)
                            {
                                // remove the piece and try again
                                ClearPieceAt(i, j);
                                FillRandomGamePieceAt(i, j, falseYOffset, moveTime);

                                // check to prevent infinite loop
                                iterations++;

                                //Debug.Log("onFill");

                                if (iterations >= maxInterations)
                                {
                                    break;
                                }

                                matches = FindMatchesAt(i, j);
                            }
                        }

                    }
                }
            }
            loops++;

            //Debug.Log(loops);

            if (loops > maxInterations)
            {
                break;
            }

            foreach (GamePiece gp in m_allGamePieces)
            {
                if (gp != null)
                {
                    if (m_all2ndLayerTiles[gp.xIndex, gp.yIndex] == null)
                    {
                        m_Pieces[gp.xIndex, gp.yIndex] = gp;
                    }
                }

            }

        } while (m_boardDeadlock.IsDeadlocked(m_Pieces, 3));

    }

    // check if we form a match down or to the left when filling the Board
    // note: this does not take into account StartingGamePieces
    bool HasMatchOnFill(int x, int y, int minLength = 3)
    {
        // find matches to the left
        List<GamePiece> leftMatches = FindMatches(x, y, new Vector2(-1, 0), minLength);

        // find matches downward
        List<GamePiece> downwardMatches = FindMatches(x, y, new Vector2(0, -1), minLength);

        List<GamePiece> rightMatches = FindMatches(x, y, new Vector2(1, 0), minLength);

        // find matches downward
        List<GamePiece> upwardMatches = FindMatches(x, y, new Vector2(0, 1), minLength);

        if (leftMatches == null)
        {
            leftMatches = new List<GamePiece>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<GamePiece>();
        }

        if (rightMatches == null)
        {
            rightMatches = new List<GamePiece>();
        }

        if (upwardMatches == null)
        {
            upwardMatches = new List<GamePiece>();
        }

        // return whether matches were found
        return (leftMatches.Count > 0 || downwardMatches.Count > 0 || rightMatches.Count > 0 || upwardMatches.Count > 0);

    }

    // set our clicked tile
    public void ClickTile(Tile tile)
    {
        if (m_clickedTile == null)
        {
            m_clickedTile = tile;
        }
    }

    // set our target tile
    public void DragToTile(Tile tile)
    {
        if (m_clickedTile != null && IsNextTo(tile, m_clickedTile))
        {
            m_targetTile = tile;
        }
    }

    // Swap Tiles if we release the touch/mouse and have valid clicked and target Tiles
    public void ReleaseTile()
    {
        if (m_clickedTile != null && m_targetTile != null)
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }

        m_clickedTile = null;
        m_targetTile = null;
    }

    // swap two tiles
    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
    }

    // coroutine for swapping two Tiles
    IEnumerator SwitchTilesRoutine(Tile clickedTile, Tile targetTile)
    {
        StopCoroutine(matchRoutine);

        // if the player input is enabled...
        if (m_playerInputEnabled && !GameManager.Instance.IsGameOver)
        {
            // set the corresponding GamePieces to the clicked Tile and target Tile
            GamePiece clickedPiece = m_allGamePieces[clickedTile.xIndex, clickedTile.yIndex];
            GamePiece targetPiece = m_allGamePieces[targetTile.xIndex, targetTile.yIndex];
            bool colorBombImpactedC = false;
            bool colorBombImpactedT = false;
            switchBoosterMade = false;

            if (targetPiece != null && clickedPiece != null)
            {
                // Debug.Log("1");

                m_boardDeadlock.removeMatchGlow();
                // move the clicked GamePiece to the target GamePiece and vice versa
                clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);

                m_playerInputEnabled = false;
                // wait for the swap time
                while (clickedPiece.m_isMoving || targetPiece.m_isMoving)
                {
                    yield return null;
                }
                //yield return new WaitForSeconds(swapTime);             

                // find all matches for each GamePiece after the swap
                List<GamePiece> clickedPieceMatches = new List<GamePiece>();
                List<GamePiece> targetPieceMatches = new List<GamePiece>();
                List<GamePiece> clickedPieceMatchesA = new List<GamePiece>();
                List<GamePiece> targetPieceMatchesA = new List<GamePiece>();

                List<GamePiece> rowMatches = new List<GamePiece>();
                List<GamePiece> columnMatches = new List<GamePiece>();
                List<GamePiece> adjacentMatches = new List<GamePiece>();

                #region color bombs
                // create a new List to hold potential color matches
                List<GamePiece> colorMatches = new List<GamePiece>();
                List<GamePiece> doubleColorMatches = new List<GamePiece>();
                List<GamePiece> newBombs = new List<GamePiece>();

                // if the clicked GamePiece is a Color Bomb, set the color matches to the first color
                if (IsColorBomb(clickedPiece) && !IsColorBomb(targetPiece))
                {
                    colorBombImpactedC = true;
                    clickedPieceMatchesA = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
                    if (IsRowBomb(targetPiece) || IsColumnBomb(targetPiece))
                    {
                        // Debug.Log("RorC");
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        clickedPiece.matchValue = targetPiece.matchValue;
                        colorMatches = FindAllMatchValue(clickedPiece.matchValue);
                        int randomBomb;

                        foreach (GamePiece gamePiece in colorMatches)
                        {
                            Bomb bomb1 = gamePiece.GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                randomBomb = UnityEngine.Random.Range(0, 2);

                                GameObject bomb = DropCertainBomb(gamePiece.xIndex, gamePiece.yIndex, clickedPiece.matchValue, randomBomb);
                                ClearPieceAt(gamePiece.xIndex, gamePiece.yIndex);
                                //allBombs.Add(bomb);
                                ActivateBomb(bomb);
                                GamePiece piece = bomb.GetComponent<GamePiece>();
                                newBombs.Add(piece);

                                if (m_particleManager != null)
                                {
                                    m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex, 0, 0f);
                                }
                            }

                        }
                        colorMatches = FindAllMatchValue(clickedPiece.matchValue);

                    }

                    else if (IsAdjacentBomb(targetPiece))
                    {
                        // Debug.Log("A");
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        clickedPiece.matchValue = targetPiece.matchValue;
                        colorMatches = FindAllMatchValue(clickedPiece.matchValue);
                        int randomBomb = 2;

                        foreach (GamePiece gamePiece in colorMatches)
                        {
                            Bomb bomb1 = gamePiece.GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                GameObject bomb = DropCertainBomb(gamePiece.xIndex, gamePiece.yIndex, clickedPiece.matchValue, randomBomb);
                                ClearPieceAt(gamePiece.xIndex, gamePiece.yIndex);
                                //allBombs.Add(bomb);
                                ActivateBomb(bomb);
                                GamePiece piece = bomb.GetComponent<GamePiece>();
                                newBombs.Add(piece);


                                if (m_particleManager != null)
                                {
                                    m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex, 0, 0f);
                                }
                            }

                        }

                        colorMatches = FindAllMatchValue(clickedPiece.matchValue);

                    }

                    else
                    {
                        //Debug.Log("none");
                        if (targetPiece.matchValue != MatchValue.None && targetPiece.matchValue != MatchValue.Litter && targetPiece.matchValue != MatchValue.Barrel && targetPiece.matchValue != MatchValue.BrokenBarrel)
                        {
                            clickedPiece.matchValue = targetPiece.matchValue;
                            matchValues.Add(clickedPiece.matchValue);
                            colorMatches = FindAllMatchValue(clickedPiece.matchValue);
                        }
                    }

                }
                //... if the target GamePiece is a Color Bomb, set the color matches to the second color
                else if (!IsColorBomb(clickedPiece) && IsColorBomb(targetPiece))
                {
                    colorBombImpactedT = true;
                    targetPieceMatchesA = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);

                    if (IsRowBomb(clickedPiece) || IsColumnBomb(clickedPiece))
                    {
                        //Debug.Log("RorC2");
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);
                        targetPiece.matchValue = clickedPiece.matchValue;
                        colorMatches = FindAllMatchValue(targetPiece.matchValue);
                        int randomBomb;

                        foreach (GamePiece gamePiece in colorMatches)
                        {
                            Bomb bomb1 = gamePiece.GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                randomBomb = UnityEngine.Random.Range(0, 2);
                                GameObject bomb = DropCertainBomb(gamePiece.xIndex, gamePiece.yIndex, targetPiece.matchValue, randomBomb);
                                ClearPieceAt(gamePiece.xIndex, gamePiece.yIndex);
                                //allBombs.Add(bomb);
                                ActivateBomb(bomb);
                                GamePiece piece = bomb.GetComponent<GamePiece>();
                                newBombs.Add(piece);

                                if (m_particleManager != null)
                                {
                                    m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex, 0, 0f);
                                }
                            }

                        }
                        colorMatches = FindAllMatchValue(targetPiece.matchValue);
                    }

                    else if (IsAdjacentBomb(clickedPiece))
                    {
                        // Debug.Log("A2");
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);
                        targetPiece.matchValue = clickedPiece.matchValue;
                        colorMatches = FindAllMatchValue(targetPiece.matchValue);
                        int randomBomb = 2;

                        foreach (GamePiece gamePiece in colorMatches)
                        {
                            Bomb bomb1 = gamePiece.GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                GameObject bomb = DropCertainBomb(gamePiece.xIndex, gamePiece.yIndex, targetPiece.matchValue, randomBomb);
                                ClearPieceAt(gamePiece.xIndex, gamePiece.yIndex);
                                //allBombs.Add(bomb);
                                ActivateBomb(bomb);
                                GamePiece piece = bomb.GetComponent<GamePiece>();
                                newBombs.Add(piece);

                                if (m_particleManager != null)
                                {
                                    m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex, 0, 0f);
                                }
                            }

                        }
                        colorMatches = FindAllMatchValue(targetPiece.matchValue);
                    }

                    else
                    {
                        //Debug.Log("none2");
                        if (clickedPiece.matchValue != MatchValue.None && clickedPiece.matchValue != MatchValue.Litter && clickedPiece.matchValue != MatchValue.Barrel && clickedPiece.matchValue != MatchValue.BrokenBarrel)
                        {
                            targetPiece.matchValue = clickedPiece.matchValue;
                            matchValues.Add(targetPiece.matchValue);
                            colorMatches = FindAllMatchValue(targetPiece.matchValue);
                        }
                    }
                }
                //... otherwise, if they are both Color bombs, choose the whole Board!
                else if (IsColorBomb(clickedPiece) && IsColorBomb(targetPiece))
                {
                    if (m_particleManager != null)
                    {
                        float x;
                        float y;
                        if (clickedPiece.xIndex == targetPiece.xIndex)
                        {
                            x = clickedPiece.xIndex;
                        }
                        else if (clickedPiece.xIndex < targetPiece.xIndex)
                        {
                            x = clickedPiece.xIndex + 0.5f;
                        }
                        else
                        {
                            x = targetPiece.xIndex + 0.5f;
                        }

                        if (clickedPiece.yIndex == targetPiece.yIndex)
                        {
                            y = clickedPiece.yIndex;
                        }
                        else if (clickedPiece.yIndex < targetPiece.yIndex)
                        {
                            y = clickedPiece.yIndex + 0.5f;
                        }
                        else
                        {
                            y = targetPiece.yIndex + 0.5f;
                        }

                        m_particleManager.DoubleColorFXAt(x, y);
                    }


                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            if (m_allGamePieces[i, j] != null)
                            {
                                //delay = delay + (interpolation);
                                if (!doubleColorMatches.Contains(m_allGamePieces[i, j]))
                                {
                                    //Debug.Log(piece.xIndex + ", " + piece.yIndex);                         
                                    doubleColorMatches.Add(m_allGamePieces[i, j]);
                                    if (m_particleManager != null)
                                    {
                                        m_particleManager.ColorPickedFXAt(m_allGamePieces[i, j].xIndex, m_allGamePieces[i, j].yIndex, -1, 2f);
                                    }
                                }
                            }
                            else
                            {
                                if (m_allTiles[i, j] != null)
                                {
                                    if (m_all2ndLayerTiles[i, j] != null)
                                    {
                                        BreakBombedTileAt(i, j, 2f);
                                    }
                                    else
                                    {
                                        if (m_allTiles[i, j].tileType == TileType.Breakable || m_allTiles[i, j].tileType == TileType.Fallout
                                            || m_allTiles[i, j].tileType == TileType.DoubleBreakable || m_allTiles[i, j].tileType == TileType.TripleBreakable)
                                        {
                                            BreakTileAt(i, j, 2f);
                                        }
                                        else
                                        {
                                            BreakBombedTileAt(i, j, 2f);
                                        }
                                    }

                                }

                            }
                        }
                    }

                    twoColors = true;
                }
                #endregion

                else if (!IsColorBomb(clickedPiece) && !IsColorBomb(targetPiece))
                {
                    #region row bombs
                    // create a new List to hold potential row matches


                    // if they are both Row bombs
                    if (IsRowBomb(clickedPiece) && IsRowBomb(targetPiece))
                    {
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("R&R");
                        for (int column = 0; column < width; column++)
                        {
                            if (IsWithinBounds(column, clickedPiece.yIndex))
                            {
                                if (m_allGamePieces[column, clickedPiece.yIndex] != null)
                                {
                                    rowMatches.Add(m_allGamePieces[column, clickedPiece.yIndex]);
                                }
                                BreakBombedTileAt(column, clickedPiece.yIndex);
                            }
                        }

                        for (int row = 0; row < height; row++)
                        {
                            if (IsWithinBounds(clickedPiece.xIndex, row))
                            {
                                if (m_allGamePieces[clickedPiece.xIndex, row] != null)
                                {
                                    if (!rowMatches.Contains(m_allGamePieces[clickedPiece.xIndex, row]))
                                    {
                                        rowMatches.Add(m_allGamePieces[clickedPiece.xIndex, row]);
                                    }

                                }
                                BreakBombedTileAt(clickedPiece.xIndex, row);
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.CrossFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue);
                        }
                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;

                    }

                    else if (IsRowBomb(clickedPiece) && IsColumnBomb(targetPiece))
                    {

                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("R&C");
                        for (int column = 0; column < width; column++)
                        {
                            if (IsWithinBounds(column, clickedPiece.yIndex))
                            {
                                if (m_allGamePieces[column, clickedPiece.yIndex] != null)
                                {
                                    rowMatches.Add(m_allGamePieces[column, clickedPiece.yIndex]);
                                }
                                BreakBombedTileAt(column, clickedPiece.yIndex);
                            }
                        }

                        for (int row = 0; row < height; row++)
                        {
                            if (IsWithinBounds(clickedPiece.xIndex, row))
                            {
                                if (m_allGamePieces[clickedPiece.xIndex, row] != null)
                                {
                                    if (!rowMatches.Contains(m_allGamePieces[clickedPiece.xIndex, row]))
                                    {
                                        rowMatches.Add(m_allGamePieces[clickedPiece.xIndex, row]);
                                    }

                                }
                                BreakBombedTileAt(clickedPiece.xIndex, row);
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.CrossFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue);
                        }
                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;

                    }


                    #endregion

                    #region column bombs
                    // create a new List to hold potential row matches


                    // if they are both Row bombs
                    else if (IsColumnBomb(clickedPiece) && IsColumnBomb(targetPiece))
                    {
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("C&C");
                        for (int column = 0; column < width; column++)
                        {
                            if (IsWithinBounds(column, clickedPiece.yIndex))
                            {
                                if (m_allGamePieces[column, clickedPiece.yIndex] != null)
                                {
                                    columnMatches.Add(m_allGamePieces[column, clickedPiece.yIndex]);
                                }
                                BreakBombedTileAt(column, clickedPiece.yIndex);
                            }
                        }

                        for (int row = 0; row < height; row++)
                        {
                            if (IsWithinBounds(clickedPiece.xIndex, row))
                            {
                                if (m_allGamePieces[clickedPiece.xIndex, row] != null)
                                {
                                    if (!columnMatches.Contains(m_allGamePieces[clickedPiece.xIndex, row]))
                                    {
                                        columnMatches.Add(m_allGamePieces[clickedPiece.xIndex, row]);
                                    }

                                }
                                BreakBombedTileAt(clickedPiece.xIndex, row);
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.CrossFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue);
                        }

                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;

                    }

                    else if (IsColumnBomb(clickedPiece) && IsRowBomb(targetPiece))
                    {
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("C&R");
                        for (int column = 0; column < width; column++)
                        {
                            if (IsWithinBounds(column, clickedPiece.yIndex))
                            {
                                if (m_allGamePieces[column, clickedPiece.yIndex] != null)
                                {
                                    columnMatches.Add(m_allGamePieces[column, clickedPiece.yIndex]);
                                }
                                BreakBombedTileAt(column, clickedPiece.yIndex);
                            }
                        }

                        for (int row = 0; row < height; row++)
                        {
                            if (IsWithinBounds(clickedPiece.xIndex, row))
                            {
                                if (m_allGamePieces[clickedPiece.xIndex, row] != null)
                                {
                                    if (!columnMatches.Contains(m_allGamePieces[clickedPiece.xIndex, row]))
                                    {
                                        columnMatches.Add(m_allGamePieces[clickedPiece.xIndex, row]);
                                    }

                                }
                                BreakBombedTileAt(clickedPiece.xIndex, row);
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.CrossFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue);
                        }
                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;
                    }

                    #endregion

                    #region adjacent bombs
                    // create a new List to hold potential row matches


                    // if they are both Row bombs
                    else if (IsAdjacentBomb(clickedPiece) && IsAdjacentBomb(targetPiece))
                    {
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("A&A");
                        for (int column = -2; column < 3; column++)
                        {
                            for (int row = -2; row < 3; row++)
                            {
                                if (IsWithinBounds(clickedPiece.xIndex + column, clickedPiece.yIndex + row))
                                {
                                    if (m_allGamePieces[clickedPiece.xIndex + column, clickedPiece.yIndex + row] != null)
                                    {
                                        adjacentMatches.Add(m_allGamePieces[clickedPiece.xIndex + column, clickedPiece.yIndex + row]);
                                    }

                                    BreakBombedTileAt(clickedPiece.xIndex + column, clickedPiece.yIndex + row, 1f);

                                }
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.AdjacentBigFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue, 0, 0f);
                        }

                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlayBigBombSound();
                        }

                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;
                    }

                    else if ((IsAdjacentBomb(clickedPiece) && IsRowBomb(targetPiece) || IsAdjacentBomb(clickedPiece) && IsColumnBomb(targetPiece))
                        || (IsAdjacentBomb(targetPiece) && IsRowBomb(clickedPiece) || IsAdjacentBomb(targetPiece) && IsColumnBomb(clickedPiece)))
                    {
                        GameManager.Instance.UpdateCollectionGoals(clickedPiece);
                        GameManager.Instance.UpdateCollectionGoals(targetPiece);

                        //Debug.Log("A&R or A&C or A&R or A&C");
                        for (int column = -1; column < 2; column++)
                        {
                            for (int row = 0; row < height; row++)
                            {
                                if (IsWithinBounds(clickedPiece.xIndex + column, row))
                                {
                                    if (m_allGamePieces[clickedPiece.xIndex + column, row] != null)
                                    {
                                        adjacentMatches.Add(m_allGamePieces[clickedPiece.xIndex + column, row]);
                                    }

                                    BreakBombedTileAt(clickedPiece.xIndex + column, row, 1f);

                                }
                            }
                        }

                        for (int row = -1; row < 2; row++)
                        {
                            for (int column = 0; column < width; column++)
                            {
                                if (IsWithinBounds(column, clickedPiece.yIndex + row) && !adjacentMatches.Contains(m_allGamePieces[column, clickedPiece.yIndex + row]))
                                {
                                    if (m_allGamePieces[column, clickedPiece.yIndex + row] != null)
                                    {
                                        adjacentMatches.Add(m_allGamePieces[column, clickedPiece.yIndex + row]);
                                    }

                                    BreakBombedTileAt(column, clickedPiece.yIndex + row, 1f);

                                }
                            }
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.MixedBigFXAt(clickedPiece.xIndex, clickedPiece.yIndex, clickedPiece.matchValue, 0, 0f);
                        }

                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlayBigBombSound();
                        }

                        Bomb bomb = clickedPiece.GetComponent<Bomb>();
                        bomb.bombType = BombType.None;
                        Bomb bomb2 = targetPiece.GetComponent<Bomb>();
                        bomb2.bombType = BombType.None;
                    }

                    #endregion

                    else
                    {
                        clickedPieceMatches = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
                        targetPieceMatches = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);
                    }
                }


                // if we don't make any matches, then swap the pieces back
                if (targetPieceMatches.Count == 0 && clickedPieceMatches.Count == 0 && colorMatches.Count == 0 && rowMatches.Count == 0 && columnMatches.Count == 0 && adjacentMatches.Count == 0 && doubleColorMatches.Count == 0
                    && Booster.switchBoosterAnabled == false)
                {
                    clickedPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
                    targetPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);

                    //yield return new WaitForSeconds(swapTime);
                    while (clickedPiece.m_isMoving || targetPiece.m_isMoving)
                    {
                        yield return null;
                    }

                    matchRoutine = StartCoroutine(CheckMatches());
                    m_playerInputEnabled = true;
                    // Debug.Log("1.5");
                }

                else
                {          //Debug.Log("2");         
                    //m_playerInputEnabled = true;
                    canBeMovedOnRoad = true;
                    //Debug.Log("switch");
                    canBreakBarrel = true;
                    canProduceBarrel = true;
                    canproduceLitterTrash = true;
                    boardShuffles = -1;
                    if (Booster.switchBoosterAnabled == true)
                    {
                        switchBoosterMade = true;
                        Booster.ActiveBooster = null;
                        GameManager.Instance.SwitchBoosterDone();
                    }

                    //boardShuffleWait = 0;


                    if (lastLitter == 1)
                    {
                        lastLitter = 2;
                    }
                    else
                    {
                        lastLitter = 1;
                    }

                    #region drop bombs
                    // record the general vector of our swipe
                    Vector2 swipeDirection = new Vector2(targetTile.xIndex - clickedTile.xIndex, targetTile.yIndex - clickedTile.yIndex);

                    // convert the clicked GamePiece or target GamePiece to a bomb depending on matches and swipe direction
                    m_clickedTileBomb = DropBomb(clickedTile.xIndex, clickedTile.yIndex, swipeDirection, clickedPieceMatches);
                    m_targetTileBomb = DropBomb(targetTile.xIndex, targetTile.yIndex, swipeDirection, targetPieceMatches);

                    // if the clicked GamePiece is a non-color Bomb, then change its color to the correct target color
                    if (m_clickedTileBomb != null && targetPiece != null)
                    {
                        GamePiece clickedBombPiece = m_clickedTileBomb.GetComponent<GamePiece>();
                        if (!IsColorBomb(clickedBombPiece))
                        {
                            clickedBombPiece.ChangeColor(targetPiece);
                        }
                    }

                    // if the target GamePiece is a non-color Bomb, then change its color to the correct clicked color
                    if (m_targetTileBomb != null && clickedPiece != null)
                    {
                        GamePiece targetBombPiece = m_targetTileBomb.GetComponent<GamePiece>();

                        if (!IsColorBomb(targetBombPiece))
                        {
                            targetBombPiece.ChangeColor(clickedPiece);
                        }
                    }
                    #endregion

                    // clear matches and refill the Board
                    List<GamePiece> piecesToClear = clickedPieceMatches.Union(targetPieceMatches).ToList().Union(colorMatches).ToList();

                    List<GamePiece> bombedPiecesToClear = adjacentMatches.Union(newBombs).ToList().Union(doubleColorMatches).ToList();

                    List<GamePiece> crossPiecesToClear = rowMatches.Union(columnMatches).ToList();

                    if ((colorBombImpactedC == false && Booster.switchBoosterAnabled == true && colorBombImpactedT == false) || (colorBombImpactedC == true && Booster.switchBoosterAnabled == false)
                        || (colorBombImpactedT == true && Booster.switchBoosterAnabled == false)
                        || (colorBombImpactedC == false && Booster.switchBoosterAnabled == false && colorBombImpactedT == false))
                    {
                        //Debug.Log(piecesToClear.Count);
                        yield return StartCoroutine(ClearAndRefillBoardRoutine(piecesToClear, bombedPiecesToClear, crossPiecesToClear));
                    }

                    else if ((colorBombImpactedC == true && clickedPieceMatchesA.Count > 0 && Booster.switchBoosterAnabled == true))
                    {
                        yield return StartCoroutine(ClearAndRefillBoardRoutine(clickedPieceMatchesA));
                    }

                    else if ((colorBombImpactedT == true && targetPieceMatchesA.Count > 0 && Booster.switchBoosterAnabled == true))
                    {
                        yield return StartCoroutine(ClearAndRefillBoardRoutine(targetPieceMatchesA));
                    }

                    else
                    {
                        m_playerInputEnabled = true;
                    }

                    if (Booster.switchBoosterAnabled == false)
                    {
                        // otherwise, we decrement our moves left
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.UpdateMoves();
                        }
                    }

                    else
                    {
                        Booster.switchBoosterAnabled = false;

                    }

                    matchRoutine = StartCoroutine(CheckMatches());

                }



            }
        }

    }

    // return true if one Tile is adjacent to another, otherwise returns false
    bool IsNextTo(Tile start, Tile end)
    {
        if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex)
        {
            return true;
        }

        if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
        {
            return true;
        }

        return false;
    }

    // general method to find matches, defaulting to a minimum of three-in-a-row, passing in an (x,y) position and direction

    List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        // keep a running list of GamePieces
        List<GamePiece> matches = new List<GamePiece>();

        GamePiece startPiece = null;

        // get a starting piece at an (x,y) position in the array of GamePieces
        if (IsWithinBounds(startX, startY) && m_all2ndLayerTiles[startX, startY] == null)
        {
            startPiece = m_allGamePieces[startX, startY];
        }

        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }

        // use the search direction to increment to the next space to look...
        int nextX;
        int nextY;

        int maxValue = (width > height) ? width : height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            // ... find the adjacent GamePiece and check its MatchValue...
            GamePiece nextPiece = m_allGamePieces[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }

            // ... if it matches then add it our running list of GamePieces
            else
            {
                if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece) && nextPiece.matchValue != MatchValue.None
                    && nextPiece.matchValue != MatchValue.Litter && nextPiece.matchValue != MatchValue.Barrel && nextPiece.matchValue != MatchValue.BrokenBarrel && m_all2ndLayerTiles[nextPiece.xIndex, nextPiece.yIndex] == null)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }

        // if our list is greater than our minimum (usually 3), then return the list...
        if (matches.Count >= minLength)
        {
            return matches;
        }

        //...otherwise return nothing
        return null;

    }

    // find all vertical matches given a position (x,y) in the Board
    List<GamePiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<GamePiece> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);

        if (upwardMatches == null)
        {
            upwardMatches = new List<GamePiece>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<GamePiece>();
        }

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;

    }

    // find all horizontal matches given a position (x,y) in the Board
    List<GamePiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<GamePiece> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        if (rightMatches == null)
        {
            rightMatches = new List<GamePiece>();
        }

        if (leftMatches == null)
        {
            leftMatches = new List<GamePiece>();
        }

        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;

    }

    // find horizontal and vertical matches at a position (x,y) in the Board
    List<GamePiece> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<GamePiece> horizMatches = FindHorizontalMatches(x, y, minLength);
        List<GamePiece> vertMatches = FindVerticalMatches(x, y, minLength);

        if (horizMatches == null)
        {
            horizMatches = new List<GamePiece>();
        }

        if (vertMatches == null)
        {
            vertMatches = new List<GamePiece>();
        }
        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    // find all matches given a list of GamePieces
    List<GamePiece> FindMatchesAt(List<GamePiece> gamePieces, int minLength = 3)
    {
        List<GamePiece> matches = new List<GamePiece>();

        foreach (GamePiece piece in gamePieces)
        {
            matches = matches.Union(FindMatchesAt(piece.xIndex, piece.yIndex, minLength)).ToList();
        }
        return matches;

    }

    // find all matches in the game Board
    List<GamePiece> FindAllMatches()
    {
        List<GamePiece> combinedMatches = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var matches = FindMatchesAt(i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        return combinedMatches;
    }

    // turn off the temporary highlight
    void HighlightTileOff(int x, int y)
    {
        if (m_allTiles[x, y].tileType != TileType.Breakable && m_allTiles[x, y].tileType != TileType.DoubleBreakable && m_allTiles[x, y].tileType != TileType.TripleBreakable)
        {
            SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }

    // temporary method to draw a highlight around a Tile
    void HighlightTileOn(int x, int y, Color col)
    {
        if (m_allTiles[x, y].tileType != TileType.Breakable && m_allTiles[x, y].tileType != TileType.DoubleBreakable && m_allTiles[x, y].tileType != TileType.TripleBreakable)
        {
            SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriteRenderer.color = col;
        }
    }
    // highlight all matching tiles at position (x,y) in the Board
    void HighlightMatchesAt(int x, int y)
    {
        HighlightTileOff(x, y);
        var combinedMatches = FindMatchesAt(x, y);
        if (combinedMatches.Count > 0)
        {
            foreach (GamePiece piece in combinedMatches)
            {
                HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    // highlight all matching tiles in the Board
    void HighlightMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                HighlightMatchesAt(i, j);

            }
        }
    }

    // highlight Tiles that correspond to a list of GamePieces
    void HighlightPieces(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    // clear the GamePiece at position (x,y) in the Board
    public void ClearPieceAt(int x, int y)
    {
        GamePiece pieceToClear = m_allGamePieces[x, y];

        if (pieceToClear != null)
        {
            m_allGamePieces[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }

    }

    // clear a list of GamePieces (plus a potential sublist of GamePieces destroyed by bombs)
    void ClearPieceAt(List<GamePiece> gamePieces)
    {

        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                if (m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null)
                {
                    // play particle effects for pieces...
                    if (m_particleManager != null && !usedMatches.Contains(piece))
                    {
                        m_particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex, piece.matchValue);
                    }
                    // clear the GamePiece
                    ClearPieceAt(piece.xIndex, piece.yIndex);

                    // add a score bonus if we clear four or more pieces
                    int bonus = 0;
                    if (gamePieces.Count >= 4)
                    {
                        bonus = 20;
                    }

                    if (GameManager.Instance != null)
                    {
                        if (m_particleManager != null)
                        {
                            m_particleManager.PointsFXAt(piece.xIndex, piece.yIndex, piece.scoreValue * m_scoreMultiplier, piece.matchValue);
                        }
                        GameManager.Instance.ScorePoints(piece, m_scoreMultiplier, bonus);

                        GameManager.Instance.UpdateCollectionGoals(piece);
                    }
                }

                else
                {
                    BreakTilesProcedure(piece.xIndex, piece.yIndex, m_allTiles[piece.xIndex, piece.yIndex]);
                }

            }
        }
    }

    // damage a Breakable Tile
    void BreakTileAt(int x, int y, float waitTime = 0f)
    {
        Tile tileToBreak = m_allTiles[x, y];

        if (tileToBreak != null && (tileToBreak.tileType == TileType.Breakable || tileToBreak.tileType == TileType.Fallout
            || tileToBreak.tileType == TileType.DoubleBreakable || tileToBreak.tileType == TileType.TripleBreakable))
        {
            // play appropriate particle effect
            if (m_particleManager != null)
            {
                if (tileToBreak.tileType == TileType.Breakable)
                {
                    m_particleManager.Smoke1FXAt(x, y, 0);
                }

                else if (tileToBreak.tileType == TileType.DoubleBreakable)
                {
                    if (tileToBreak.breakableValue == 1)
                    {
                        m_particleManager.Smoke1FXAt(x, y, 0);
                    }
                    if (tileToBreak.breakableValue == 2)
                    {
                        m_particleManager.Smoke2FXAt(x, y, 0);
                    }

                }

                else if (tileToBreak.tileType == TileType.TripleBreakable)
                {
                    if (tileToBreak.breakableValue == 3)
                    {
                        m_particleManager.Smoke3FXAt(x, y, 0);
                    }

                }

                else if (tileToBreak.tileType == TileType.Fallout)
                {
                    m_particleManager.TileFalloutFXAt(x, y, 0);
                }

            }

            if (tileToBreak.breakableValue == 1)
            {
                if (tileToBreak.tileType != TileType.Fallout)
                {
                    tileToBreak.tileType = TileType.Breakable;
                }
            }

            if (tileToBreak.breakableValue == 2)
            {
                if (tileToBreak.tileType != TileType.DoubleBreakable)
                {
                    tileToBreak.tileType = TileType.DoubleBreakable;
                }
            }

            tileToBreak.BreakTile(waitTime);
        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.Pipeline)
        {
            if (IsWithinBounds(x, y + 1) && m_allTiles[x, y + 1].tileType == TileType.Pipeline)
            {
                if (m_allTiles[x, y + 1].breakableValue == 0 && m_allTiles[x, y].breakableValue == 1)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.ChainBrakeFXAt(x, y, 0);
                    }
                    tileToBreak.TurnGasFlow(waitTime);
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ScorePointsTiles(tileToBreak);
                    }
                }
            }

            if (IsWithinBounds(x - 1, y) && m_allTiles[x - 1, y].tileType == TileType.Pipeline)
            {
                if (m_allTiles[x - 1, y].breakableValue == 0 && m_allTiles[x, y].breakableValue == 1)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.ChainBrakeFXAt(x, y, 0);
                    }
                    tileToBreak.TurnGasFlow(waitTime);
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ScorePointsTiles(tileToBreak);
                    }
                }
            }

            if (IsWithinBounds(x + 1, y) && m_allTiles[x + 1, y].tileType == TileType.Pipeline)
            {
                if (m_allTiles[x + 1, y].breakableValue == 0 && m_allTiles[x, y].breakableValue == 1)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.ChainBrakeFXAt(x, y, 0);
                    }
                    tileToBreak.TurnGasFlow(waitTime);
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ScorePointsTiles(tileToBreak);
                    }
                }
            }

            if (!IsWithinBounds(x, y + 1))
            {
                if (m_allTiles[x, y].breakableValue == 1)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.ChainBrakeFXAt(x, y, 0);
                    }
                    tileToBreak.TurnGasFlow(waitTime);
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ScorePointsTiles(tileToBreak);
                    }
                }

            }

        }

    }

    void BreakBombedTileAt(int x, int y, float waitTime = 0f)
    {
        Tile tileToBreak = m_allTiles[x, y];
        BreakTilesProcedure(x, y, tileToBreak, waitTime);
    }

    GamePiece BreakTilesProcedure(int x, int y, Tile tileToBreak, float waitTime = 0f)
    {
        GamePiece additionalPiece = null;

        if (tileToBreak != null && (tileToBreak.tileType == TileType.FossilFuels || tileToBreak.tileType == TileType.House
            || tileToBreak.tileType == TileType.OilMine))
        {
            // play appropriate particle effect
            if (m_particleManager != null)
            {
                if (tileToBreak.tileType == TileType.FossilFuels)
                {

                    if (tileToBreak.breakableValue == 1)
                    {
                        m_particleManager.TileFossilGoneFXAt(x, y, 0, waitTime);
                    }
                    else
                    {
                        m_particleManager.TileBreakFossilFXAt(x, y, 0, waitTime);
                    }

                }

                else if (tileToBreak.tileType == TileType.OilMine)
                {
                    if (tileToBreak.breakableValue == 1)
                    {
                        m_particleManager.TileOilMineGoneFXAt(x, y, 0, waitTime);
                    }
                    else
                    {
                        m_particleManager.TileBreakOilPumpFXAt(x, y, 0, waitTime);
                    }

                }
                else if (tileToBreak.tileType == TileType.House)
                {
                    if (tileToBreak.breakableValue == 1)
                    {
                        m_particleManager.TileCityGoneFXAt(x, y, 0, waitTime);
                    }
                    else
                    {
                        m_particleManager.WasteDumpWithTrashFXAt(x, y, 0, waitTime);
                    }

                }
            }

            tileToBreak.BreakTile(waitTime);

        }

        Tiles2ndLayer tile = m_all2ndLayerTiles[x, y];
        if (tile != null && (tile.tileType == Tile2ndLayerType.Oil || tile.tileType == Tile2ndLayerType.MiningPit || tile.tileType == Tile2ndLayerType.WasteDump))
        {
            // play appropriate particle effect
            if (m_particleManager != null)
            {
                if (tile.tileType == Tile2ndLayerType.MiningPit)
                {
                    m_particleManager.TileBreakMiningPitFXAt(x, y, -4, waitTime);

                }

                else if (tile.tileType == Tile2ndLayerType.Oil)
                {
                    m_particleManager.ClearOiledPieceFXAt(x, y, -4, waitTime);
                }

                else if (tile.tileType == Tile2ndLayerType.WasteDump)
                {
                    if (tile.breakableValue > 1)
                    {
                        if (m_particleManager != null)
                        {
                            m_particleManager.WasteDumpWithTrashFXAt(x, y, -4, waitTime);
                        }
                    }

                    if (tile.breakableValue <= 1)
                    {
                        if (m_particleManager != null)
                        {
                            m_particleManager.WasteDumpFXAt(x, y, -4, waitTime);
                        }
                    }
                }

                else
                {
                    m_particleManager.BreakTileFXAt(tileToBreak.breakableValue, x, y, 0, waitTime);
                }

            }

            if (tile.tileType == Tile2ndLayerType.WasteDump)
            {
                if (tile.breakableValue == 1)
                {
                    tile.BreakTile(waitTime);
                    m_all2ndLayerTiles[x, y] = null;
                }

                else
                {
                    tile.BreakTile(waitTime);
                }

            }
            else
            {
                tile.BreakTile(waitTime);
                m_all2ndLayerTiles[x, y] = null;
            }

        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.Ocean)
        {
            StartCoroutine(OceanRoutine(tileToBreak, waitTime));
        }

        if (tileToBreak != null && tileToBreak.tileType == TileType.NuclearRuins)
        {
            if (tileToBreak.breakableValue > 1)
            {
                if (m_particleManager != null)
                {
                    m_particleManager.TileNuclearRBreakFXAt(x, y, 0, waitTime);
                }

                tileToBreak.BreakNuclearRuinsTile(x, y, waitTime);
            }
            else
            {
                // play appropriate particle effect
                if (m_particleManager != null)
                {
                    m_particleManager.TileNuclearGoneFXAt(x, y, 0, waitTime);
                }

                tileToBreak.BreakNuclearRuinsTile(x, y, waitTime);

            }


        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.Windmill)
        {
            if (m_particleManager != null)
            {
                if (tileToBreak.windmillRotation != 1)
                {
                    m_particleManager.RotationFXAt(x, y, 0, waitTime);

                }

                else if (tileToBreak.windmillRotation == 1)
                {
                    m_particleManager.RotationOnFXAt(x, y, 0, waitTime);
                }

            }
            tileToBreak.RotateTile(x, y, waitTime);
        }


        else if (tileToBreak != null && tileToBreak.tileType == TileType.WindmillOn && GameManager.Instance.IsGameOver == false)
        {
            if (m_particleManager != null)
            {
                m_particleManager.RotationFXAt(x, y, 0, waitTime);
            }

            tileToBreak.TurnOffWindmillTile(x, y, waitTime);
        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.SolarPanel)
        {
            if (tileToBreak.breakableValue == 1)
            {
                if (m_particleManager != null)
                {
                    m_particleManager.SolarOnFXAt(x, y, 0, waitTime);
                }
                tileToBreak.TurnOnSolarPanel(waitTime);

            }

        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.Plants)
        {
            if (tileToBreak.breakableValue > 0)
            {

                if (tileToBreak.breakableValue == 3)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.WaterPlant1FXAt(x, y, -4, waitTime);
                    }
                }

                else if (tileToBreak.breakableValue == 2)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.WaterPlant2FXAt(x, y, -4, waitTime);
                    }
                }

                else if (tileToBreak.breakableValue == 1)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.WaterPlant3FXAt(x, y, -4, waitTime);
                    }
                }

                tileToBreak.GrowPlants(waitTime);
            }

        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.NuclearPP)
        {
            if (m_particleManager != null)
            {
                m_particleManager.TileBreakNuclearFXAt(x, y, 0, waitTime);
            }
            tileToBreak.BreakNuclearTile(x, y, waitTime);
        }

        else if (tileToBreak != null && tileToBreak.tileType == TileType.NuclearPPAlarm)
        {
            if (m_particleManager != null)
            {
                m_particleManager.TileBreakNuclearFXAt(x, y, 0, waitTime);
            }

            tileToBreak.BreakAlarmNuclearTile(x, y, waitTime);
        }

        GamePiece litterPiece = m_allGamePieces[x, y];
        if (litterPiece != null)
        {
            if (litterPiece.matchValue == MatchValue.Litter || litterPiece.matchValue == MatchValue.Barrel || litterPiece.matchValue == MatchValue.BrokenBarrel)
            {
                additionalPiece = litterPiece;
            }
        }

        return additionalPiece;
    }

    IEnumerator OceanRoutine(Tile tileToBreak, float waitTime)
    {
        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }
        foreach (Ocean ocean in m_allOceans)
        {
            if (ocean != null)
            {
                for (int xOffset = 0; xOffset < 2; xOffset++)
                {
                    for (int yOffset = 0; yOffset < 2; yOffset++)
                    {
                        if (tileToBreak.xIndex == ocean.xIndex + xOffset && tileToBreak.yIndex == ocean.yIndex + yOffset)
                        {
                            if (m_particleManager != null)
                            {
                                if (ocean.breakableValue > 1)
                                {
                                    m_particleManager.GreenHouseGasesFXAt(ocean.xIndex + 0.5f, ocean.yIndex + 0.5f, -7, waitTime);
                                }

                                else if (ocean.breakableValue == 1)
                                {
                                    m_particleManager.GreenHouseGoneFXAt(ocean.xIndex + 0.5f, ocean.yIndex + 0.5f, -7, waitTime);
                                }

                            }

                            if (GameManager.Instance != null)
                            {
                                GameManager.Instance.ScorePointsPlanetTiles(ocean);
                                if (m_particleManager != null)
                                {
                                    m_particleManager.PointsFXAt(tileToBreak.xIndex, tileToBreak.yIndex, tileToBreak.scoreValue, MatchValue.None, -4, waitTime);
                                }
                            }

                            ocean.CleanOcean(waitTime);

                            if (ocean.breakableValue <= 0)
                            {
                                m_allOceans[ocean.xIndex, ocean.yIndex] = null;

                                if (m_particleManager != null)
                                {
                                    m_particleManager.FireworkFXAt(width, height);
                                }

                                if (SoundManager.Instance != null)
                                {
                                    SoundManager.Instance.PlayFireworksSound();
                                }

                                if (GameManager.Instance != null)
                                {
                                    GameManager.Instance.ScorePointsPlanetTiles(ocean);

                                    GameManager.Instance.UpdateCollectionGoalsPlanet(ocean);
                                }

                                clearBoardAfterPlanetCooled = true;

                            }
                        }
                    }
                }
            }
        }
    }

    List<GamePiece> BreakSTileAt(int x, int y)
    {
        GamePiece additionalPiece = null;
        List<GamePiece> PiecesList = new List<GamePiece>();

        if (IsWithinBounds(x + 1, y))
        {
            Tile tileToBreak = m_allTiles[x + 1, y];

            additionalPiece = BreakTilesProcedure(x + 1, y, tileToBreak);
            PiecesList.Add(additionalPiece);

        }

        if (IsWithinBounds(x - 1, y))
        {
            Tile tileToBreak = m_allTiles[x - 1, y];

            additionalPiece = BreakTilesProcedure(x - 1, y, tileToBreak);
            PiecesList.Add(additionalPiece);

        }

        if (IsWithinBounds(x, y + 1))
        {
            Tile tileToBreak = m_allTiles[x, y + 1];

            additionalPiece = BreakTilesProcedure(x, y + 1, tileToBreak);
            PiecesList.Add(additionalPiece);

        }

        if (IsWithinBounds(x, y - 1))
        {
            Tile tileToBreak = m_allTiles[x, y - 1];

            additionalPiece = BreakTilesProcedure(x, y - 1, tileToBreak);
            PiecesList.Add(additionalPiece);
        }

        return PiecesList;
    }

    public void SpreadOil(int x, int y)
    {
        CoverPieceWithOil(x - 1, y + 1);
        CoverPieceWithOil(x - 1, y);
        CoverPieceWithOil(x - 1, y - 1);
        CoverPieceWithOil(x, y + 1);
        CoverPieceWithOil(x, y - 1);
        CoverPieceWithOil(x + 1, y + 1);
        CoverPieceWithOil(x + 1, y);
        CoverPieceWithOil(x + 1, y - 1);
    }

    private void CoverPieceWithOil(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            GamePiece gp = m_allGamePieces[x, y];
            if (m_allGamePieces[x, y] != null && m_allGamePieces[x, y].matchValue != MatchValue.Barrel && m_allGamePieces[x, y].matchValue != MatchValue.BrokenBarrel && m_allGamePieces[x, y].matchValue != MatchValue.Litter
                && m_allTiles[x, y].tileType != TileType.RoadDown && m_allTiles[x, y].tileType != TileType.RoadUp && m_allTiles[x, y].tileType != TileType.RoadRight && m_allTiles[x, y].tileType != TileType.RoadLeft
                && m_all2ndLayerTiles[x, y] == null && !IsBigBomb(m_allGamePieces[x, y]) && !IsColumnBomb(m_allGamePieces[x, y]) && !IsColorBomb(m_allGamePieces[x, y]) && !IsRowBomb(m_allGamePieces[x, y])
                && !IsSpecialBomb(m_allGamePieces[x, y]))
            {
                //ClearPieceAt(x, y);
                MakeNew2ndLayerTile(tileOil, x, y, -5);

            }

        }
    }

    public void NuclearDisaster()
    {
        bool goalExists = false;
        int falloutPieces = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allTiles[i, j].tileType == TileType.Normal)
                {
                    MakeNewTile(tileFalloutPrefab, i, j, -5);
                    falloutPieces++;
                }
            }
        }

        foreach (CollectionGoal goal in level.collectionGoals)
        {
            //Debug.Log(goal.prefabToCollect);
            if (goal.prefabToCollect == tileFalloutPrefab)
            {
                int newNumberToCollect = goal.numberToCollect + falloutPieces;
                goal.numberToCollect = newNumberToCollect;
                UIManager.Instance.UpdateCollectionGoalLayout1();
                goalExists = true;
                break;
            }
        }

        if (!goalExists)
        {
            MakeCollectionGoal(tileFalloutPrefab, falloutPieces);

            UIManager.Instance.SetupCollectionGoalLayout(level.collectionGoals);
            //UIManager.Instance.UpdateCollectionGoalLayout();
        }

        clearBoardAfterBlast = true;

    }

    public void NuclearBlast(int x, int y)
    {
        if (m_particleManager != null)
        {
            m_particleManager.ExplosionFXAt(x, y);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayExplosionSound();
        }
    }


    public void NuclearRuins(int x, int y)
    {
        MakeNewTile(tileNuclearRuinsPrefab, x, y);
    }


    // break Tiles corresponding to a list of gamePieces
    void BreakTileAt(List<GamePiece> gamePieces)
    {
        List<GamePiece> pipelinetiles = new List<GamePiece>();
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                if (piece.matchValue != MatchValue.Barrel && piece.matchValue != MatchValue.BrokenBarrel && piece.matchValue != MatchValue.Litter)
                {
                    BreakTileAt(piece.xIndex, piece.yIndex);
                    if (m_allTiles[piece.xIndex, piece.yIndex] != null
                        && m_allTiles[piece.xIndex, piece.yIndex].tileType == TileType.Pipeline)
                    {
                        pipelinetiles.Add(piece);
                        //Debug.Log("added");
                    }
                }

            }
        }
        if (pipelinetiles.Count > 0)
        {
            for (int i = 1; i < 10; i++)
            {
                foreach (GamePiece piece in pipelinetiles)
                { BreakTileAt(piece.xIndex, piece.yIndex); }
            }
        }
    }

    List<GamePiece> BreakSTileAt(List<GamePiece> gamePieces)
    {
        List<GamePiece> bigList = new List<GamePiece>();
        List<GamePiece> smallList = null;
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                smallList = BreakSTileAt(piece.xIndex, piece.yIndex);
                bigList = bigList.Union(smallList).ToList();
            }
        }

        return bigList;
    }

    // compresses a given column to remove any empty Tile spaces


    private void MoveDown(int column, float collapseTime, int i, int j)
    {
        // move the GamePiece downward to fill in the space and update the GamePiece array
        m_allGamePieces[column, j].Move(column, i, collapseTime * (j - i));
        m_allGamePieces[column, i] = m_allGamePieces[column, j];
        m_allGamePieces[column, i].SetCoord(column, i);
        m_allGamePieces[column, j] = null;
    }

    private void MoveLeft(int column, float collapseTime, int i, int j)
    {
        // move the GamePiece downward to fill in the space and update the GamePiece array
        m_allGamePieces[column, j].Move(column - 1, i, collapseTime);
        m_allGamePieces[column - 1, i] = m_allGamePieces[column, j];
        m_allGamePieces[column - 1, i].SetCoord(column - 1, i);
        m_allGamePieces[column, j] = null;
    }

    private void MoveRight(int column, float collapseTime, int i, int j)
    {
        // move the GamePiece downward to fill in the space and update the GamePiece array
        m_allGamePieces[column, j].Move(column + 1, i, collapseTime);
        m_allGamePieces[column + 1, i] = m_allGamePieces[column, j];
        m_allGamePieces[column + 1, i].SetCoord(column + 1, i);
        m_allGamePieces[column, j] = null;
    }


    // clear and refill the Board
    void ClearAndRefillBoard(List<GamePiece> gamePieces)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
    }

    public void ClearAndRefillBoard(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            GamePiece pieceToClear = m_allGamePieces[x, y];
            List<GamePiece> listOfOne = new List<GamePiece>();
            listOfOne.Add(pieceToClear);
            ClearAndRefillBoard(listOfOne);
        }
    }

    // coroutine to clear GamePieces and collapse empty spaces, then refill the Board
    IEnumerator ClearAndRefillBoardRoutine(List<GamePiece> gamePieces, List<GamePiece> bombedPieces = null, List<GamePiece> crossPieces = null, bool finalBoardClear = false)
    {

        // disable player input so we cannot swap pieces while the Board is collapsing/refilling
        m_playerInputEnabled = false;

        isRefilling = true;

        if ((bombedPieces != null && bombedPieces.Count != 0))
        {

            yield return new WaitForSeconds(1f);
        }

        if (delayAfterNuclearBlast)
        {
            yield return new WaitForSeconds(2f);
            delayAfterNuclearBlast = false;
        }

        // create a new List of GamePieces, using our initial list as a starting point
        List<GamePiece> matches = gamePieces;
        //List<GamePiece> pieceMatches = new List<GamePiece>();

        // store a score multiplier for chain reactions
        m_scoreMultiplier = 0;

        //  increment our score multiplier by 1 for each subsequent recursive call of ClearAndCollapseRoutine
        m_scoreMultiplier++;




        // run the coroutine to clear the Board and collapse any columns to fill in the spaces
        yield return StartCoroutine(ClearAndCollapseRoutine(matches, bombedPieces, crossPieces));

        if (usedMatches.Count > 0)
        {
            usedMatches.Clear();
        }

        boardShuffles = -1;

        if (m_scoreMultiplier > 1)
        {
            int newMultiplier = 0;
            if (m_scoreMultiplier <= 6)
            {
                newMultiplier = m_scoreMultiplier;
            }
            else
            {
                newMultiplier = 7;
            }

            if (m_particleManager != null)
            {

                if ((width % 2) == 0)
                {
                    if ((height % 2) == 0)
                    {
                        m_particleManager.ComboFXAt((width / 2) - 0.5f, (height / 2) - 0.5f, (verticalSizeGlobal * globalAspectRatio) / 2.8125f, newMultiplier);
                    }
                    else
                    {
                        m_particleManager.ComboFXAt((width / 2) - 0.5f, (height / 2), (verticalSizeGlobal * globalAspectRatio) / 2.8125f, newMultiplier);
                    }

                }
                else
                {
                    if ((height % 2) == 0)
                    {
                        m_particleManager.ComboFXAt((width / 2), (height / 2) - 0.5f, (verticalSizeGlobal * globalAspectRatio) / 2.8125f, newMultiplier);
                    }
                    else
                    {
                        m_particleManager.ComboFXAt((width / 2), (height / 2), (verticalSizeGlobal * globalAspectRatio) / 2.8125f, newMultiplier);
                    }

                }

            }
        }

        if (bombedPieces != null)
        {
            bombedPieces.Clear();
        }

        /* if (routine >= 1)
         {
             clearBoardAfterBlast = false;
             routine = 0;
         } */


        if (pipelinesOnBoard > 0)
        {
            foreach (Tile tile in m_allTiles)
            {
                if (tile != null && tile.tileType == TileType.BiogasPlant)
                {
                    if (tile.breakableValue != 0)
                    {
                        if (IsWithinBounds(tile.xIndex, tile.yIndex + 1))
                        {
                            if (m_allTiles[tile.xIndex, tile.yIndex + 1].breakableValue == 0 && m_allTiles[tile.xIndex, tile.yIndex + 1].tileType == TileType.Pipeline)
                            {
                                tile.TurnOnBiogasPlant();
                                if (GameManager.Instance != null)
                                {
                                    GameManager.Instance.ScorePointsTiles(tile);
                                }
                                if (m_particleManager != null)
                                {
                                    m_particleManager.BiogasPlantFXAt(tile.xIndex, tile.yIndex);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (nuclearDisasters > 0 && GameManager.Instance.IsGameOver == false)
        {
            //Debug.Log(nuclearDisasters);
            for (int i = 0; i <= nuclearDisasters; i++)
            {
                int x = nuclearListX[i];
                int y = nuclearListY[i];

                NuclearBlast(x, y);
                NuclearRuins(x, y);
                nuclearDisasters--;
            }

            nuclearDisasters = 0;
            nuclearListX.Clear();
            nuclearListY.Clear();
            NuclearDisaster();
            List<GamePiece> allPieces = new List<GamePiece>();
            //routine++;

            foreach (GamePiece gamePiece in m_allGamePieces)
            {
                allPieces.Add(gamePiece);
            }
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(ClearAndRefillBoardRoutine(allPieces));
            yield break;
        }

        if (clearBoardAfterPlanetCooled)
        {
            List<GamePiece> emptyList = new List<GamePiece>();
            List<GamePiece> piecesToClear = new List<GamePiece>();

            piecesToClear = ClearTheBoard();
            clearBoardAfterPlanetCooled = false;
            yield return StartCoroutine(ClearAndRefillBoardRoutine(emptyList, piecesToClear));
            yield break;
        }


        if (canBeMovedOnRoad == true && switchBoosterMade == false && roadTilesOnBoard > 0)
        {
            List<GamePiece> movedP = new List<GamePiece>();
            movedP = MovePiecesOnTheRoad();
            canBeMovedOnRoad = false;
            matches = FindAllMatches();

            while (!IsCollapsed(movedP))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);
            if (matches.Count != 0)
            {
                List<GamePiece> usedMatches = new List<GamePiece>();

                usedMatches = CreateNewBombs(usedMatches);

                //usedMatches.Clear();

                yield return StartCoroutine(ClearAndRefillBoardRoutine(matches));

            }
        }

        if (canproduceLitterTrash && switchBoosterMade == false)
        {
            if (factoriesOnBoard > 0)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (m_allTiles[i, j] != null && m_allTiles[i, j].tileType == TileType.FossilFuels && m_allTiles[i, j].wasAttacked == WasFactoryAttacked.NotAttacked)
                        {

                            // Debug.Log(factoriesOnBoard);
                            CreateTrashTile(i, j);

                        }
                        if (m_allTiles[i, j] != null && m_allTiles[i, j].tileType == TileType.FossilFuels && m_allTiles[i, j].wasAttacked == WasFactoryAttacked.Attacked)
                        {
                            m_allTiles[i, j].wasAttacked = WasFactoryAttacked.NotAttacked;
                        }

                    }
                }
            }


            if (citiesOnBoard > 0)
            {
                if (lastLitter == 2)
                {
                    for (int i = 1; i <= citiesOnBoard; i++)
                    {
                        // Debug.Log(citiesOnBoard);
                        CreateLitterTile();
                    }
                }
            }

            canproduceLitterTrash = false;
        }

        if (canProduceBarrel && switchBoosterMade == false)
        {

            if (oilMinesOnBoard > 0)
            {
                for (int i = 1; i <= oilMinesOnBoard; i++)
                {
                    // Debug.Log(factoriesOnBoard);
                    CreateBarrelTile();
                }
            }
            canProduceBarrel = false;
        }

        if (canBreakBarrel && switchBoosterMade == false)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (m_allGamePieces[i, j] != null && m_allGamePieces[i, j].matchValue == MatchValue.Barrel)
                    {
                        GamePiece gp = m_allGamePieces[i, j];
                        gp.BreakBarrel(i, j);
                        if (barrelBroken)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.BarrelExplodeFXAt(i, j, -5);
                            }
                            ClearPieceAt(i, j);
                            MakeBrokenBarrelPiece(i, j, 0, 0f);
                            SpreadOil(i, j);
                            //matches.Add(m_allGamePieces[i, j]);
                            barrelBroken = false;

                        }
                    }
                }
            }

            canBreakBarrel = false;
            // yield return StartCoroutine(ClearAndRefillBoardRoutine(matches));
        }


        if (specialBombsOnBoard > 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (specialBombsWaiting[x, y] != null)
                    {
                        List<Tile> impactedTiles = new List<Tile>();
                        yield return StartCoroutine(JumpSpecialRoutine(x, y, impactedTiles));
                        yield break;
                    }
                }
            }
        }

        List<GamePiece> collectedPieces = FindCollectiblesAt(0, true);

        if (collectedPieces.Count > 0)
        {
            yield return StartCoroutine(ClearAndRefillBoardRoutine(collectedPieces));
            yield break;
        }

        //yield return new WaitForSeconds(0.2f);

        GamePiece[,] m_Pieces = new GamePiece[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                m_Pieces[i, j] = null;
            }
        }


        foreach (GamePiece gp in m_allGamePieces)
        {
            if (gp != null)
            {
                if (m_all2ndLayerTiles[gp.xIndex, gp.yIndex] == null)
                {
                    /* Collectible collectible = gp.GetComponent<Collectible>();
                     if(collectible == null)
                     {*/
                    m_Pieces[gp.xIndex, gp.yIndex] = gp;
                    // }

                }
            }

        }


        if (!finalBoardClear)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.m_levelGoal.movesLeft > 1)
                {
                    if (m_boardDeadlock.IsDeadlocked(m_Pieces, 3))
                    {
                        boardShuffles++;
                        yield return new WaitForSeconds(0.5f);
                        int orangeMatch = 0;
                        int blueMatch = 0;
                        int greenMatch = 0;
                        int purpleMatch = 0;
                        int yellowMatch = 0;
                        int whiteMatch = 0;

                        foreach (GamePiece piece in m_allGamePieces)
                        {
                            if (piece != null)
                            {
                                Bomb bomb = piece.GetComponent<Bomb>();

                                if (piece.matchValue == MatchValue.Red && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    orangeMatch++;
                                }

                                if (piece.matchValue == MatchValue.Blue && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    blueMatch++;
                                }

                                if (piece.matchValue == MatchValue.Green && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    greenMatch++;
                                }

                                if (piece.matchValue == MatchValue.Purple && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    purpleMatch++;
                                }

                                if (piece.matchValue == MatchValue.Yellow && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    yellowMatch++;
                                }

                                if (piece.matchValue == MatchValue.White && m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && bomb == null)
                                {
                                    whiteMatch++;
                                }
                            }
                        }

                        if ((yellowMatch >= 3 || orangeMatch >= 3 || blueMatch >= 3 || greenMatch >= 3 || purpleMatch >= 3 || yellowMatch >= 3 || whiteMatch >= 3) && AreThreeNeighboringInLine(m_Pieces) == true)
                        {
                            if (boardShuffles < 1)
                            {

                                foreach (GamePiece gamePiece in m_Pieces)
                                {
                                    if (gamePiece != null)
                                    {
                                        if (m_particleManager != null)
                                        {
                                            m_particleManager.ShuffleFXAt(gamePiece.xIndex, gamePiece.yIndex);
                                        }
                                    }
                                }

                                yield return StartCoroutine(ShuffleBoardRoutine());
                                yield break;

                            }
                        }

                        else
                        {
                            locked = true;
                        }

                    }
                }
            }

        }


        // re-enable player input
        m_playerInputEnabled = true;

        isRefilling = false;

        if (boardShuffles == 1 || locked == true)
        {
            yield return new WaitForSeconds(1f);

        }
    }

    IEnumerator JumpSpecialRoutine(int x, int y, List<Tile> impactedTiles)
    {
        if (specialBombsWaiting[x, y] != null)
        {
            bool execute = true;
            bool exists = false;
            GameObject special = null;
            List<GamePiece> newBombs = new List<GamePiece>();
            int initialX = x;
            int initialY = y;
            MatchValue value = MatchValue.None;
            switch (specialBombsWaiting[x, y].name)
            {
                case "Blue":
                    value = MatchValue.Blue;
                    break;
                case "Green":
                    value = MatchValue.Green;
                    break;
                case "Purple":
                    value = MatchValue.Purple;
                    break;
                case "Red":
                    value = MatchValue.Red;
                    break;
                case "White":
                    value = MatchValue.White;
                    break;
                case "Yellow":
                    value = MatchValue.Yellow;
                    break;

            }


            for (int j = 0; j < 3; j++)
            {
                int randomGamePieceX = UnityEngine.Random.Range(0, width);
                int randomGamePieceY = UnityEngine.Random.Range(0, height);
                int loops = 0;
                execute = true;
                //Debug.Log("here");
                while (m_allGamePieces[randomGamePieceX, randomGamePieceY] == null || IsColorBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsColumnBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
               || IsRowBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsAdjacentBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsBigBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
               || impactedTiles.Contains(m_allTiles[randomGamePieceX, randomGamePieceY])
               || (randomGamePieceX == initialX && randomGamePieceY == initialY) || IsSpecialBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Barrel
               || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.BrokenBarrel || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Litter
               || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.None || m_all2ndLayerTiles[randomGamePieceX, randomGamePieceY] != null)
                {
                    randomGamePieceX = UnityEngine.Random.Range(0, width);
                    randomGamePieceY = UnityEngine.Random.Range(0, height);

                    if (loops > 100)
                    {
                        execute = false;
                        break;
                    }
                }

                if (execute)
                {

                    if (exists == false)
                    {
                        switch (value)
                        {
                            case MatchValue.Blue:
                                special = Instantiate(specialBombJumpingBluePrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                            case MatchValue.Green:
                                special = Instantiate(specialBombJumpingGreenPrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                            case MatchValue.Purple:
                                special = Instantiate(specialBombJumpingPurplePrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                            case MatchValue.Red:
                                special = Instantiate(specialBombJumpingRedPrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                            case MatchValue.White:
                                special = Instantiate(specialBombJumpingWhitePrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                            case MatchValue.Yellow:
                                special = Instantiate(specialBombJumpingYellowPrefab, new Vector3(x, y, -8f), Quaternion.identity) as GameObject;
                                break;
                        }


                        if (special != null)
                        {
                            special.transform.parent = transform;
                            if (m_allGamePieces[x, y] != null
                                && !IsColorBomb(m_allGamePieces[x, y])
                                && !IsColumnBomb(m_allGamePieces[x, y])
                               && !IsRowBomb(m_allGamePieces[x, y])
                               && !IsAdjacentBomb(m_allGamePieces[x, y])
                               && !impactedTiles.Contains(m_allTiles[x, y])
                               && !IsSpecialBomb(m_allGamePieces[x, y]))
                            {

                                GameObject bomb = DropCertainBomb(x, y, m_allGamePieces[x, y].matchValue, 2);
                                if (bomb != null)
                                {
                                    List<Tile> allAdjacentTiles = GetAllAdjacentTiles(x, y);
                                    impactedTiles = impactedTiles.Union(allAdjacentTiles).ToList();
                                    ClearPieceAt(x, y);
                                    //allBombs.Add(bomb);
                                    ActivateBomb(bomb);
                                    GamePiece piece = bomb.GetComponent<GamePiece>();
                                    newBombs.Add(piece);
                                    if (SoundManager.Instance != null)
                                    {
                                        SoundManager.Instance.PlayBounceSound();
                                    }
                                }

                            }

                            GameObject pieceToClear = specialBombsWaiting[x, y];

                            if (pieceToClear != null)
                            {
                                specialBombsWaiting[x, y] = null;
                                Destroy(pieceToClear.gameObject);
                                specialBombsOnBoard--;
                                //Debug.Log("destroyed");
                            }

                            exists = true;
                        }

                    }

                    //specialBombsWaiting[i] = special;
                    if (special != null)
                    {
                        List<Tile> allAdjacentTiles = GetAllAdjacentTiles(randomGamePieceX, randomGamePieceY);
                        impactedTiles = impactedTiles.Union(allAdjacentTiles).ToList();
                        float scaleIndex = Vector3.Distance(special.transform.position, new Vector3(randomGamePieceX, randomGamePieceY, -8f)) / 14;
                        //Debug.Log(scaleIndex);
                        yield return StartCoroutine(MoveSpecialRoutine(new Vector3(randomGamePieceX, randomGamePieceY, -8f), 2f, special, scaleIndex));
                        GameObject bomb = DropCertainBomb(randomGamePieceX, randomGamePieceY, m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue, 2);
                        ClearPieceAt(randomGamePieceX, randomGamePieceY);
                        //allBombs.Add(bomb);
                        ActivateBomb(bomb);
                        GamePiece piece = bomb.GetComponent<GamePiece>();
                        newBombs.Add(piece);
                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlayBounceSound();
                        }

                        if (m_particleManager != null)
                        {
                            m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex, 0, 0f);
                        }
                    }

                }
            }

            if (newBombs.Count > 0)
            {
                Destroy(special.gameObject);
                StartCoroutine(ClearAndRefillBoardRoutine(newBombs));
                yield break;
            }
            else
            {
                yield return null;
            }
        }

    }

    // coroutine to handle movement
    IEnumerator MoveSpecialRoutine(Vector3 destination, float timeToMove, GameObject tile, float scaleIndex)
    {
        Vector3 startScale = tile.transform.localScale;

        // store our starting position
        Vector3 startPosition = tile.transform.position;
        float calculatedScale = 8f * scaleIndex;
        float calculatedTimeToMove = timeToMove * scaleIndex;
        if (calculatedTimeToMove < 1f)
        {
            calculatedTimeToMove = 1f;
        }
        if (calculatedScale < 2f)
        {
            calculatedScale = 2f;
        }


        if (calculatedScale > 4.5)
        {
            startScale.z = 30f;
        }
        else if (calculatedScale > 3)
        {
            startScale.z = 20f;
        }
        else if (calculatedScale > 1.5)
        {
            startScale.z = 10f;
        }
        // have we reached our destination?
        bool reachedDestination = false;

        // how much time has passed since we started moving
        float elapsedTime = 0f;

        // while we have not reached the destination, check to see if we are close enough
        while (!reachedDestination)
        {
            // if we are close enough to destination
            if (Vector3.Distance(tile.transform.position, destination) < 0.01f)
            {
                // we have reached the destination
                reachedDestination = true;
                break;
            }

            // increment the total running time by the Time elapsed for this frame
            elapsedTime += Time.deltaTime;

            // calculate the Lerp value
            float t = Mathf.Clamp(elapsedTime / calculatedTimeToMove, 0f, 1f);

            double z = 0.5 - Math.Sin(Math.Asin(1.0 - 2.0 * t) / 3.0);
            // move the game piece
            tile.transform.position = Vector3.Lerp(startPosition, destination, t);
            if (t < 0.5)
            {
                tile.transform.localScale = Vector3.Lerp(startScale, startScale * (1 + calculatedScale), (float)z);
            }
            else
            {
                tile.transform.localScale = Vector3.Lerp(startScale * (1 + calculatedScale), startScale, (float)z);
            }

            // wait until next frame
            yield return null;
        }


    }

    public void MeteorImpact()
    {
        StartCoroutine(MeteorRoutine());
    }

    IEnumerator MeteorRoutine()
    {
        meteorsImpacting = true;
        bool stillImpact = true;
        int offset = 15;
        float timeBeforeNextImpact = 0.3f;
        List<GamePiece> allPiecesImpacted = new List<GamePiece>();
        List<GamePiece> emptyList = new List<GamePiece>();
        List<Tile> allTilesImpacted = new List<Tile>();
        List<Tile> targetTiles = new List<Tile>();

        for (int m = 0; m < width; m++)
        {
            for (int n = 0; n < height; n++)
            {
                if (m_allTiles[m, n].tileType == TileType.FossilFuels
                    || m_all2ndLayerTiles[m, n] != null
                    || m_allTiles[m, n].tileType == TileType.House
                    || m_allTiles[m, n].tileType == TileType.Windmill
                    || m_allTiles[m, n].tileType == TileType.OilMine
                    || m_allTiles[m, n].tileType == TileType.NuclearRuins
                    || m_allTiles[m, n].tileType == TileType.Ocean
                    || m_allTiles[m, n].tileType == TileType.Breakable
                    || m_allTiles[m, n].tileType == TileType.DoubleBreakable
                    || m_allTiles[m, n].tileType == TileType.TripleBreakable
                    || m_allTiles[m, n].tileType == TileType.Fallout)
                {
                    //some code
                    targetTiles.Add(m_allTiles[m, n]);
                }

                if (m_allTiles[m, n].tileType == TileType.SolarPanel && m_allTiles[m, n].breakableValue > 0)
                {
                    //some code
                    targetTiles.Add(m_allTiles[m, n]);
                }

                if (m_allTiles[m, n].tileType == TileType.Plants && m_allTiles[m, n].breakableValue > 0)
                {
                    //some code
                    targetTiles.Add(m_allTiles[m, n]);
                }

                if (m_allTiles[m, n].tileType == TileType.Pipeline && m_allTiles[m, n].breakableValue > 0)
                {
                    //some code
                    targetTiles.Add(m_allTiles[m, n]);
                }

                if (m_allGamePieces[m, n] != null)
                {
                    Collectible collectibleComponent = m_allGamePieces[m, n].GetComponent<Collectible>();

                    if (collectibleComponent != null)
                    {
                        if (IsWithinBounds(m, n - 2))
                        {
                            for (int Yoffset = 0; Yoffset <= (n - 2); Yoffset++)
                            {
                                if (m_allTiles[m, n - 2 - Yoffset] != null && !targetTiles.Contains(m_allTiles[m, n - 2 - Yoffset]) && m_allTiles[m, n - 2 - Yoffset].tileType != TileType.Teleport)
                                {
                                    targetTiles.Add(m_allTiles[m, n - 2 - Yoffset]);
                                    break;
                                }
                            }

                        }

                        else if (IsWithinBounds(m, n - 1))
                        {
                            for (int Yoffset = 0; Yoffset <= (n - 1); Yoffset++)
                            {
                                if (m_allTiles[m, n - 1 - Yoffset] != null && !targetTiles.Contains(m_allTiles[m, n - 1 - Yoffset]) && m_allTiles[m, n - 1 - Yoffset].tileType != TileType.Teleport)
                                {
                                    targetTiles.Add(m_allTiles[m, n - 1 - Yoffset]);
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(timeBeforeNextImpact);
            }

            if (targetTiles.Count > 0)
            {

                while (targetTiles.Count > 0)
                {
                    if (i == 2 && allPiecesImpacted.Count == 0)
                    {
                        int iterations = 0;
                        int randomGamePieceX = UnityEngine.Random.Range(0, width);
                        int randomGamePieceY = UnityEngine.Random.Range(0, height);

                        while (m_allGamePieces[randomGamePieceX, randomGamePieceY] == null)
                        {
                            randomGamePieceX = UnityEngine.Random.Range(0, width);
                            randomGamePieceY = UnityEngine.Random.Range(0, height);
                            iterations++;

                            if (iterations >= 100)
                            {
                                break;
                            }
                        }
                        List<Tile> meteorTiles2 = GetAdjacentTiles(randomGamePieceX, randomGamePieceY, 1);
                        allTilesImpacted = allTilesImpacted.Union(meteorTiles2).ToList();
                        List<GamePiece> meteorPieces2 = GetAdjacentPieces(randomGamePieceX, randomGamePieceY, 1, 2.6f - (i * timeBeforeNextImpact));
                        allPiecesImpacted = allPiecesImpacted.Union(meteorPieces2).ToList();

                        if (m_particleManager != null)
                        {
                            GameObject meteor = Instantiate(meteorPrefab, new Vector3(randomGamePieceX + offset, randomGamePieceY + offset, -5f), Quaternion.identity) as GameObject;

                            Meteor comet = meteor.GetComponent<Meteor>();

                            meteor.transform.parent = transform;
                            comet.MoveMeteor(randomGamePieceX, randomGamePieceY, 0.5f);
                            if (SoundManager.Instance != null)
                            {
                                SoundManager.Instance.PlayMeteorSound();
                            }
                            if (m_particleManager != null)
                            {
                                m_particleManager.ClearPieceFXAt(randomGamePieceX, randomGamePieceY, MatchValue.Green, -4, 2.6f - (i * timeBeforeNextImpact));
                            }
                        }

                        stillImpact = false;
                        break;
                    }

                    else
                    {
                        int randomListIdx = UnityEngine.Random.Range(0, targetTiles.Count);
                        if (!allTilesImpacted.Contains(targetTiles[randomListIdx]))
                        {
                            List<Tile> meteorTiles = GetAdjacentTiles(targetTiles[randomListIdx].xIndex, targetTiles[randomListIdx].yIndex, 1);
                            allTilesImpacted = allTilesImpacted.Union(meteorTiles).ToList();
                            List<GamePiece> meteorPieces = GetAdjacentPieces(targetTiles[randomListIdx].xIndex, targetTiles[randomListIdx].yIndex, 1, 2.6f - (i * timeBeforeNextImpact));
                            allPiecesImpacted = allPiecesImpacted.Union(meteorPieces).ToList();

                            if (m_particleManager != null)
                            {
                                GameObject meteor = Instantiate(meteorPrefab, new Vector3(targetTiles[randomListIdx].xIndex + offset, targetTiles[randomListIdx].yIndex + offset, -5f), Quaternion.identity) as GameObject;

                                Meteor comet = meteor.GetComponent<Meteor>();

                                meteor.transform.parent = transform;
                                comet.MoveMeteor(targetTiles[randomListIdx].xIndex, targetTiles[randomListIdx].yIndex, 0.5f);
                                if (SoundManager.Instance != null)
                                {
                                    SoundManager.Instance.PlayMeteorSound();
                                }
                                if (m_particleManager != null)
                                {
                                    m_particleManager.ClearPieceFXAt(targetTiles[randomListIdx].xIndex, targetTiles[randomListIdx].yIndex, MatchValue.Green, -4, 2.6f - (i * timeBeforeNextImpact));
                                }
                            }

                            targetTiles.RemoveAt(randomListIdx);

                            i++;
                            if (i > 0)
                            {
                                yield return new WaitForSeconds(timeBeforeNextImpact);
                            }

                            if (i > 2)
                            {
                                stillImpact = false;
                                break;
                            }
                        }

                        else
                        {
                            targetTiles.RemoveAt(randomListIdx);
                        }
                    }

                }

            }

            if (stillImpact)
            {

                int iterations = 0;
                int randomGamePieceX = UnityEngine.Random.Range(0, width);
                int randomGamePieceY = UnityEngine.Random.Range(0, height);

                while (m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Teleport || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Wall || allTilesImpacted.Contains(m_allTiles[randomGamePieceX, randomGamePieceY]))
                {
                    randomGamePieceX = UnityEngine.Random.Range(0, width);
                    randomGamePieceY = UnityEngine.Random.Range(0, height);
                    iterations++;

                    if (iterations >= 100)
                    {
                        break;
                    }
                }
                List<Tile> meteorTiles2 = GetAdjacentTiles(randomGamePieceX, randomGamePieceY, 1);
                allTilesImpacted = allTilesImpacted.Union(meteorTiles2).ToList();
                List<GamePiece> meteorPieces2 = GetAdjacentPieces(randomGamePieceX, randomGamePieceY, 1, 2.6f - (i * timeBeforeNextImpact));
                allPiecesImpacted = allPiecesImpacted.Union(meteorPieces2).ToList();

                if (m_particleManager != null)
                {
                    GameObject meteor = Instantiate(meteorPrefab, new Vector3(randomGamePieceX + offset, randomGamePieceY + offset, -5f), Quaternion.identity) as GameObject;

                    Meteor comet = meteor.GetComponent<Meteor>();

                    meteor.transform.parent = transform;
                    comet.MoveMeteor(randomGamePieceX, randomGamePieceY, 0.5f);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayMeteorSound();
                    }
                    if (m_particleManager != null)
                    {
                        m_particleManager.ClearPieceFXAt(randomGamePieceX, randomGamePieceY, MatchValue.Green, -4, 2.6f - (i * timeBeforeNextImpact));
                    }

                }
            }

        }
        yield return new WaitForSeconds(0.5f);
        meteorsImpacting = false;
        StartCoroutine(ClearAndRefillBoardRoutine(emptyList, allPiecesImpacted));
        yield break;
    }

    private void CreateBarrelTile()
    {
        int randomGamePieceX = UnityEngine.Random.Range(0, width);
        int randomGamePieceY = UnityEngine.Random.Range(0, height);
        int iterations = 0;
        bool execute = true;

        // Debug.Log("function works");

        while (m_allGamePieces[randomGamePieceX, randomGamePieceY] == null || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Barrel
            || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.BrokenBarrel
            || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Litter || m_all2ndLayerTiles[randomGamePieceX, randomGamePieceY] != null
            || IsColorBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsColumnBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
            || IsRowBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsSpecialBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
            || IsBigBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]))
        {
            randomGamePieceX = UnityEngine.Random.Range(0, width);
            randomGamePieceY = UnityEngine.Random.Range(0, height);
            if (iterations > 100)
            {
                execute = false;
                break;
            }
            iterations++;
        }

        if (execute)
        {
            if (m_allGamePieces[randomGamePieceX, randomGamePieceY] != null)
            {
                ClearPieceAt(randomGamePieceX, randomGamePieceY);
                FillBarrelGamePieceAt(randomGamePieceX, randomGamePieceY);
                if (m_particleManager != null)
                {
                    m_particleManager.TileBarrelAppearFXAt(randomGamePieceX, randomGamePieceY);
                }
            }
        }

    }

    List<GamePiece> MovePiecesOnTheRoad()
    {
        List<GamePiece> movedPiecesOnRoad = new List<GamePiece>();
        float moveTime = 0.3f;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayRoadSound();
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allTiles[i, j].tileType == TileType.RoadDown || m_allTiles[i, j].tileType == TileType.RoadUp
                    || m_allTiles[i, j].tileType == TileType.RoadLeft || m_allTiles[i, j].tileType == TileType.RoadRight)
                {
                    m_tempGamePieces[i, j] = m_allGamePieces[i, j];
                    //Debug.Log(m_allGamePieces[i, j].matchValue);
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //down and left
                if (m_allTiles[i, j].tileType == TileType.RoadDown)
                {
                    MoveOnRoadVert(i, moveTime, j, j + 1);
                    movedPiecesOnRoad.Add(m_allGamePieces[i, j]);

                    if (IsWithinBounds(i, j - 1))
                    {
                        if (m_allTiles[i, j - 1] != null && m_allTiles[i, j - 1].tileType == TileType.RoadDown)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 4);
                            }
                        }
                    }

                    if (IsWithinBounds(i - 1, j))
                    {
                        if (m_allTiles[i - 1, j] != null && m_allTiles[i - 1, j].tileType == TileType.RoadLeft)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 5);
                            }
                        }
                    }

                    if (IsWithinBounds(i + 1, j))
                    {
                        if (m_allTiles[i + 1, j] != null && m_allTiles[i + 1, j].tileType == TileType.RoadRight)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 6);
                            }
                        }
                    }
                }

                if (m_allTiles[i, j].tileType == TileType.RoadLeft)
                {
                    MoveOnRoadHoriz(j, moveTime, i, i + 1);
                    movedPiecesOnRoad.Add(m_allGamePieces[i, j]);

                    if (IsWithinBounds(i, j - 1))
                    {
                        if (m_allTiles[i, j - 1] != null && m_allTiles[i, j - 1].tileType == TileType.RoadDown)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 9);
                            }
                        }
                    }

                    if (IsWithinBounds(i - 1, j))
                    {
                        if (m_allTiles[i - 1, j] != null && m_allTiles[i - 1, j].tileType == TileType.RoadLeft)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 7);
                            }
                        }
                    }

                    if (IsWithinBounds(i, j + 1))
                    {
                        if (m_allTiles[i, j + 1] != null && m_allTiles[i, j + 1].tileType == TileType.RoadUp)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 8);
                            }
                        }
                    }
                }
            }
        }

        for (int i = width - 1; i >= 0; i--)
        {
            for (int j = height - 1; j >= 0; j--)
            {
                //up and right
                if (m_allTiles[i, j].tileType == TileType.RoadUp)
                {
                    MoveOnRoadVert(i, moveTime, j, j - 1);
                    movedPiecesOnRoad.Add(m_allGamePieces[i, j]);

                    if (IsWithinBounds(i + 1, j))
                    {
                        if (m_allTiles[i + 1, j] != null && m_allTiles[i + 1, j].tileType == TileType.RoadRight)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 3);
                            }
                        }
                    }

                    if (IsWithinBounds(i - 1, j))
                    {
                        if (m_allTiles[i - 1, j] != null && m_allTiles[i - 1, j].tileType == TileType.RoadLeft)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 2);
                            }
                        }
                    }

                    if (IsWithinBounds(i, j + 1))
                    {
                        if (m_allTiles[i, j + 1] != null && m_allTiles[i, j + 1].tileType == TileType.RoadUp)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 1);
                            }
                        }
                    }
                }

                if (m_allTiles[i, j].tileType == TileType.RoadRight)
                {
                    MoveOnRoadHoriz(j, moveTime, i, i - 1);
                    movedPiecesOnRoad.Add(m_allGamePieces[i, j]);

                    if (IsWithinBounds(i + 1, j))
                    {
                        if (m_allTiles[i + 1, j] != null && m_allTiles[i + 1, j].tileType == TileType.RoadRight)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 10);
                            }
                        }
                    }

                    if (IsWithinBounds(i, j - 1))
                    {
                        if (m_allTiles[i, j - 1] != null && m_allTiles[i, j - 1].tileType == TileType.RoadDown)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 11);
                            }
                        }
                    }

                    if (IsWithinBounds(i, j + 1))
                    {
                        if (m_allTiles[i, j + 1] != null && m_allTiles[i, j + 1].tileType == TileType.RoadUp)
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.MoveOnRoadFXAt(i, j, 12);
                            }
                        }
                    }
                }
            }
        }

        return movedPiecesOnRoad;
    }

    private void MoveOnRoadVert(int column, float collapseTime, int i, int j)
    {
        m_allGamePieces[column, i] = null;
        if (m_tempGamePieces[column, j] != null)
        {
            m_tempGamePieces[column, j].Move(column, i, collapseTime, 1);
            m_allGamePieces[column, i] = m_tempGamePieces[column, j];
            m_allGamePieces[column, i].SetCoord(column, i);
            if (m_allGamePieces[column, j] == m_tempGamePieces[column, j])
            {
                m_allGamePieces[column, j] = null;
            }

            // add our piece to the list of pieces that we are moving

            m_tempGamePieces[column, j] = null;
        }
    }

    private void MoveOnRoadHoriz(int row, float collapseTime, int i, int j)
    {
        m_allGamePieces[i, row] = null;
        if (m_tempGamePieces[j, row] != null)
        {
            m_tempGamePieces[j, row].Move(i, row, collapseTime, 1);
            m_allGamePieces[i, row] = m_tempGamePieces[j, row];
            m_allGamePieces[i, row].SetCoord(i, row);
            if (m_allGamePieces[j, row] == m_tempGamePieces[j, row])
            {
                m_allGamePieces[j, row] = null;
            }

            // add our piece to the list of pieces that we are moving

            m_tempGamePieces[j, row] = null;
        }

    }

    private void CreateLitterTile()
    {
        int randomGamePieceX = UnityEngine.Random.Range(0, width);
        int randomGamePieceY = UnityEngine.Random.Range(0, height);
        int iterations = 0;
        bool canAdd = true;

        // Debug.Log("function works");

        while (m_allGamePieces[randomGamePieceX, randomGamePieceY] == null || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Barrel || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.BrokenBarrel
            || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Litter || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.None
            || IsColumnBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsColorBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsRowBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
            || IsSpecialBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsBigBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]))
        {
            randomGamePieceX = UnityEngine.Random.Range(0, width);
            randomGamePieceY = UnityEngine.Random.Range(0, height);
            if (iterations > 100)
            {
                canAdd = false;
                break;
            }
            iterations++;
        }

        if (canAdd)
        {
            if (m_allGamePieces[randomGamePieceX, randomGamePieceY] != null)
            {
                ClearPieceAt(randomGamePieceX, randomGamePieceY);
                FillLitterGamePieceAt(randomGamePieceX, randomGamePieceY);
                if (m_particleManager != null)
                {
                    m_particleManager.TileLitterAppearFXAt(randomGamePieceX, randomGamePieceY);
                }
            }
        }
    }

    private void CreateTrashTile(int i, int j)
    {

        int randomSide = UnityEngine.Random.Range(0, 4);

        for (int z = 1; z < 20; z++)
        {
            randomSide = UnityEngine.Random.Range(0, 4);

            if (randomSide == 0)
            {
                if (IsWithinBounds(i - 1, j) && CanAddTrash(i - 1, j))
                {
                    AddTrash(i - 1, j);
                    return;
                }
            }

            if (randomSide == 1)
            {
                if (IsWithinBounds(i + 1, j) && CanAddTrash(i + 1, j))
                {
                    AddTrash(i + 1, j);
                    return;
                }
            }

            if (randomSide == 2)
            {
                if (IsWithinBounds(i, j - 1) && CanAddTrash(i, j - 1))
                {
                    AddTrash(i, j - 1);
                    return;
                }
            }

            if (randomSide == 3)
            {
                if (IsWithinBounds(i, j + 1) && CanAddTrash(i, j + 1))
                {
                    AddTrash(i, j + 1);
                    return;
                }
            }
        }

        for (int iterations = 0; iterations < 100; iterations++)
        {
            int k = UnityEngine.Random.Range(0, width);
            int l = UnityEngine.Random.Range(0, height);

            if (m_all2ndLayerTiles[k, l] != null && m_all2ndLayerTiles[k, l].tileType == Tile2ndLayerType.MiningPit)
            {
                if (IsWithinBounds(k - 1, l) && CanAddTrash(k - 1, l))
                {
                    AddTrash(k - 1, l);
                    return;
                }

                if (IsWithinBounds(k + 1, l) && CanAddTrash(k + 1, l))
                {
                    AddTrash(k + 1, l);
                    return;
                }

                if (IsWithinBounds(k, l - 1) && CanAddTrash(k, l - 1))
                {
                    AddTrash(k, l - 1);
                    return;
                }

                if (IsWithinBounds(k, l + 1) && CanAddTrash(k, l + 1))
                {
                    AddTrash(k, l + 1);
                    return;
                }
            }
        }

    }

    private void AddTrash(int randomGamePieceX, int randomGamePieceY)
    {

        MakeNew2ndLayerTile(tileTrashPrefab, randomGamePieceX, randomGamePieceY);
        if (m_particleManager != null)
        {
            m_particleManager.TileMiningAppearFXAt(randomGamePieceX, randomGamePieceY);
        }

    }

    private bool CanAddTrash(int randomGamePieceX, int randomGamePieceY)
    {
        bool canAdd = true;

        if (
                    m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.RoadDown || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.RoadLeft
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.RoadRight || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.RoadUp
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Fallout || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.House
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.FossilFuels
                    || m_all2ndLayerTiles[randomGamePieceX, randomGamePieceY] != null || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.SolarPanel
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.OilMine || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Plants
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.NuclearPP || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.NuclearPPAlarm
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Teleport || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Wall
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Ocean || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.BiogasPlant
                    || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.Windmill || m_allTiles[randomGamePieceX, randomGamePieceY].tileType == TileType.WindmillOn)
        {
            canAdd = false;
        }

        if (m_allGamePieces[randomGamePieceX, randomGamePieceY] != null && (m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Litter
                    || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Barrel || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.BrokenBarrel
                    || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.None || IsSpecialBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
                    || IsColumnBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsRowBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsColorBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
                    || IsBigBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsAdjacentBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])))
        {
            canAdd = false;
        }

        return canAdd;
    }

    // coroutine to clear GamePieces from the Board and collapse any empty spaces
    IEnumerator ClearAndCollapseRoutine(List<GamePiece> gamePieces, List<GamePiece> bombedPieces2 = null, List<GamePiece> crossPieces = null)
    {

        // list of GamePieces that will be moved
        List<GamePiece> movingPieces = new List<GamePiece>();

        // list of GamePieces that form matches
        List<GamePiece> matches = new List<GamePiece>();
        List<GamePiece> allMatches = new List<GamePiece>();

        //yield return new WaitForSeconds(0.5f);

        bool isFinished = false;

        if (crossPieces != null && crossPieces.Count > 0)
        {
            bombedPieces2 = bombedPieces2.Union(crossPieces).ToList();
        }

        while (!isFinished)
        {
            yield return null;

            bool thereIsColorBomb = false;

            if (twoColors == false)
            {
                thereIsColorBomb = GetColorFX(gamePieces);
            }
            else if (twoColors == true)
            {
                yield return new WaitForSeconds(1f);
                //code here
            }

            if (thereIsColorBomb)
            {
                foreach (GamePiece gp in gamePieces)
                {

                    if (IsColorBomb(gp))
                    {
                        //Debug.Log("color");
                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlayColorBombSound();
                        }
                        //yield return new WaitForSeconds(0.5f);
                        foreach (GamePiece piece in gamePieces)
                        {
                            if (piece != null)
                            {
                                if (!IsColorBomb(piece))
                                {

                                    if (m_particleManager != null)
                                    {
                                        if (IsBigBomb(piece))
                                        {
                                            m_particleManager.AdjacentBigFastFXAt(piece.xIndex, piece.yIndex, piece.matchValue);
                                        }
                                        else
                                        {
                                            //m_particleManager.ColorPickedFXAt(piece.xIndex, piece.yIndex);

                                            GameObject strike = Instantiate(ColorStrikeprefab, new Vector3(gp.xIndex, gp.yIndex, -1f), Quaternion.identity) as GameObject;

                                            strike.transform.parent = transform;
                                            MovePieceMakingBomb(piece.xIndex, piece.yIndex, 0.3f, strike, 0.7f);
                                        }

                                    }
                                }
                            }

                        }
                        if (lookForAllColB == false)
                        {
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    }
                }

                if (lookForAllColB == true)
                {
                    yield return new WaitForSeconds(1f);
                    lookForAllColB = false;
                }
            }
            else
            {
                foreach (GamePiece game in gamePieces)
                {
                    bool wait = false;
                    if (game != null)
                    {
                        if (IsBigBomb(game))
                        {
                            if (m_particleManager != null)
                            {
                                m_particleManager.AdjacentBigFXAt(game.xIndex, game.yIndex, game.matchValue);
                            }
                            if (wait == false)
                            {
                                yield return new WaitForSeconds(1f);
                                wait = true;
                            }
                        }
                    }
                }
            }


            //Debug.Log("clear and collapse inside loop");
            // check the original list for bombs and append any pieces affected by these bombs

            List<GamePiece> bombedPieces = new List<GamePiece>();

            if (twoColors == false)
            {
                if (gamePieces.Count == 0)
                {
                    if (bombedPieces2 != null)
                    {
                        bombedPieces = GetBombedPieces(bombedPieces2);
                    }

                }
                else
                {
                    if (gamePieces != null)
                    {
                        bombedPieces = GetBombedPieces(gamePieces);
                    }

                }


            }
            else
            {
                twoColors = false;
            }


            matchValues.Clear();
            List<GamePiece> NoBombMatches = gamePieces;


            if (bombedPieces2 != null)
            {
                bombedPieces = bombedPieces.Union(bombedPieces2).ToList();
            }


            gamePieces = gamePieces.Union(bombedPieces).ToList();

            // find any collectibles that have reached the bottom and decrement the number of collectibles needed
            List<GamePiece> collectedPieces = FindCollectiblesAt(0, true);
            if (m_particleManager != null)
            {
                foreach (GamePiece gamePiece in collectedPieces)
                {
                    m_particleManager.RecycleFXAt(gamePiece.xIndex, gamePiece.yIndex, -1);
                }
            }

            // add blockers to list of collected pieces
            //collectedPieces = collectedPieces.Union(blockers).ToList();

            // decrement cleared collectibles/blockers
            collectibleCount -= collectedPieces.Count;

            // add these collectibles to the list of GamePieces to clear
            gamePieces = RemoveCollectibles(gamePieces);

            List<GamePiece> additionalPieces = new List<GamePiece>();

            if (clearBoardAfterBlast == false)
            {
                // break any tiles under the cleared GamePieces and around
                BreakTileAt(gamePieces);
                if (gamePieces.Count != 1)
                {
                    additionalPieces = BreakSTileAt(NoBombMatches);
                }
            }
            else
            {
                foreach (Tile tile in m_allTiles)

                    if (tile != null)
                    {
                        if (tile.tileType != TileType.Normal && tile.tileType != TileType.Teleport
                        && tile.tileType != TileType.Wall && tile.tileType != TileType.Fallout && tile.tileType != TileType.NuclearRuins)
                        {
                            BreakTileAt(tile.xIndex, tile.yIndex);
                            GamePiece piece = BreakTilesProcedure(tile.xIndex, tile.yIndex, tile);
                            additionalPieces.Add(piece);
                        }
                    }
                clearBoardAfterBlast = false;
            }

            gamePieces = gamePieces.Union(collectedPieces).ToList();
            gamePieces = gamePieces.Union(additionalPieces).ToList();

            // clear the GamePieces, pass in the list of GamePieces affected by bombs as a separate list
            ClearPieceAt(gamePieces);

            usedMatches.Clear();

            // activate any bombs in our clicked or target Tiles
            if (m_clickedTileBomb != null)
            {
                ActivateBomb(m_clickedTileBomb);
                m_clickedTileBomb = null;
            }

            if (m_targetTileBomb != null)
            {
                ActivateBomb(m_targetTileBomb);
                m_targetTileBomb = null;
            }

            if (allBombs != null)
            {
                int a = 0;
                foreach (GameObject go in allBombs)
                {
                    if (allBombs[a] != null)
                    {
                        ActivateBomb(allBombs[a]);
                    }
                    a++;
                }

                allBombs.Clear();
            }

            yield return new WaitForSeconds(0.3f);

            if (m_allEarths.Length > 0)
            {
                ReleaseEarths();
            }
            // after a delay, collapse the columns to remove any empty spaces

            filledPieces = FillBoard();

            while (!IsCollapsed(filledPieces))
            {
                yield return null;
            }

            filledPieces.Clear();

            yield return StartCoroutine(CollapsePiecesRoutine());

            filledPieces.Clear();
            filledPieces = FillBoard();

            while (!IsCollapsed(filledPieces))
            {
                yield return null;
            }

            filledPieces.Clear();

            // find any matches that form from collapsing...
            matches = FindAllMatches();

            //...and any collectibles that hit the bottom row...
            collectedPieces = FindCollectiblesAt(0, true);

            //... and add them to our list of GamePieces to clear
            matches = matches.Union(collectedPieces).ToList();

            isWateringSoundPlaying = false;

            // if we didn't make any matches from the collapse, then we're done
            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }

            // otherwise, increase our score multiplier for the chair reaction... 
            else
            {
                m_scoreMultiplier++;

                // ...play a bonus sound for making a chain reaction...
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayBonusSound();
                }

                usedMatches = CreateNewBombs(usedMatches);

                //usedMatches.Clear();

                // ...and run ClearAndCollapse again

                gamePieces = matches;

            }
        }
        yield return null;
    }

    private List<GamePiece> CreateNewBombs(List<GamePiece> usedMatches)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (m_allGamePieces[i, j] != null && m_all2ndLayerTiles[i, j] == null && !usedMatches.Contains(m_allGamePieces[i, j]))
                {
                    List<GamePiece> newMatches = new List<GamePiece>();
                    newMatches = FindMatchesAt(i, j);
                    if (newMatches != null && newMatches.Count >= 5)
                    {
                        usedMatches = usedMatches.Union(newMatches).ToList();

                        currentGamePiece = DropBomb(i, j, new Vector2(0, 1), newMatches);

                        if (currentGamePiece != null)
                        {
                            GamePiece BombPiece = currentGamePiece.GetComponent<GamePiece>();
                            allBombs.Add(currentGamePiece);
                            currentGamePiece = null;
                        }
                    }
                }
            }
        }

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (m_allGamePieces[i, j] != null && m_all2ndLayerTiles[i, j] == null && !usedMatches.Contains(m_allGamePieces[i, j]))
                {
                    List<GamePiece> vertMatches = new List<GamePiece>();
                    List<GamePiece> horizMatches = new List<GamePiece>();
                    horizMatches = FindHorizontalMatches(i, j, 3);
                    vertMatches = FindVerticalMatches(i, j, 3);

                    if (vertMatches != null && vertMatches.Count >= 4)
                    {
                        usedMatches = usedMatches.Union(vertMatches).ToList();
                        currentGamePiece = DropBomb(i, j, new Vector2(1, 0), vertMatches);
                        if (currentGamePiece != null)
                        {
                            GamePiece BombPiece = currentGamePiece.GetComponent<GamePiece>();
                            allBombs.Add(currentGamePiece);
                            currentGamePiece = null;
                        }
                    }

                    else if (horizMatches != null && horizMatches.Count >= 4)
                    {
                        usedMatches = usedMatches.Union(horizMatches).ToList();
                        currentGamePiece = DropBomb(i, j, new Vector2(0, 1), horizMatches);
                        if (currentGamePiece != null)
                        {
                            GamePiece BombPiece = currentGamePiece.GetComponent<GamePiece>();
                            allBombs.Add(currentGamePiece);
                            currentGamePiece = null;
                        }
                    }
                }
            }
        }

        return usedMatches;
    }

    IEnumerator CollapsePiecesRoutine()
    {
        float moveTime = 0.07f;
        bool moved = true;

        while (moved == true)
        {
            moved = false;

            for (int y = 1; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (m_allGamePieces[x, y] != null && NotOccupiedByObstacle(x, y))
                    {
                        if (m_allGamePieces[x, y - 1] == null && NotOccupiedByObstacle(x, y - 1))
                        {
                            /*if (y > 1)
                            {
                                for (int y2 = 1; y2 <= y - 1; y2++)
                                {
                                    if(y - 1 - y2 == 0 && m_allGamePieces[x, y - 1 - y2] == null && NotOccupiedByObstacle(x, y - 1 - y2))
                                    {
                                        MoveDown(x, moveTime, y - 1 - y2, y);
                                    }
                                    
                                    else if (m_allGamePieces[x, y - 1 - y2] != null || !NotOccupiedByObstacle(x, y - 1 - y2))
                                    {
                                        MoveDown(x, moveTime, y - y2, y);
                                        break;
                                    }
                                }
                                //MoveDown(x, moveTime, y - 1, y);
                                moved = true;
                            }*/
                           
                            MoveDown(x, moveTime, y - 1, y);
                            moved = true;
                          
                        }

                        //check left
                        else if (IsWithinBounds(x - 1, y - 1) && m_allGamePieces[x - 1, y - 1] == null && NotOccupiedByObstacle(x - 1, y - 1)
                            && IsObstacleAbove(x - 1, y - 1) && (m_allGamePieces[x - 1, y] == null || m_all2ndLayerTiles[x - 1, y] != null))
                        {
                            MoveLeft(x, moveTime, y - 1, y);

                            moved = true;
                        }

                        //check right
                        else if (IsWithinBounds(x + 1, y - 1) && m_allGamePieces[x + 1, y - 1] == null && NotOccupiedByObstacle(x + 1, y - 1)
                            && IsObstacleAbove(x + 1, y - 1) && (m_allGamePieces[x + 1, y] == null || m_all2ndLayerTiles[x + 1, y] != null))
                        {
                            MoveRight(x, moveTime, y - 1, y);

                            moved = true;
                        }

                        else if (m_allTiles[x, y - 1].tileType == TileType.Teleport)
                        {
                            for (int i = 1; i < height; i++)
                            {
                                if (IsWithinBounds(x, y - 1 - i))
                                {
                                    if (m_allTiles[x, y - 1 - i].tileType != TileType.Teleport)
                                    {
                                        if (m_allGamePieces[x, y - 1 - i] == null && NotOccupiedByObstacle(x, y - 1 - i))
                                        {
                                            MoveDown(x, moveTime, y - 1 - i, y);

                                            moved = true;
                                        }
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            if (moved)
            {
                FillBoard();
            }

            List<GamePiece> allPieces = new List<GamePiece>();
            foreach (GamePiece gamePiece in m_allGamePieces)
            {
                if (gamePiece != null)
                {
                    allPieces.Add(gamePiece);
                }
            }

            while (!IsCollapsed(allPieces))
            {
                yield return null;
            }

            //yield return null;
        }

        //yield return null;
    }

    bool IsObstacleAbove(int x, int y)
    {
        bool isObstacle = false;

        for (int i = y; i < height; i++)
        {
            if (m_allTiles[x, i].tileType == TileType.Teleport)
            {
                for (int h = 1; h < height; h++)
                {
                    if (m_allTiles[x, h].tileType != TileType.Teleport && NotOccupiedByObstacle(x, h))
                    {
                        isObstacle = false;
                        return isObstacle;
                    }
                }
            }

            else if (!NotOccupiedByObstacle(x, i))
            {
                isObstacle = true;
                return isObstacle;
            }
        }

        return isObstacle;
    }

    private void ReleaseEarths()
    {
        foreach (Earth earth in m_allEarths)
        {
            if (earth != null)
            {
                int x = earth.xIndex;
                int y = earth.yIndex;
                int freeTiles = 0;

                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {

                        if (IsWithinBounds(i, j))
                        {

                            if ((m_allTiles[i, j].tileType == TileType.Normal || m_allTiles[i, j].tileType == TileType.Teleport
                                || m_allTiles[i, j].tileType == TileType.RoadLeft || m_allTiles[i, j].tileType == TileType.RoadRight
                                || m_allTiles[i, j].tileType == TileType.RoadUp || m_allTiles[i, j].tileType == TileType.RoadDown) && (m_all2ndLayerTiles[i, j] == null))
                            {
                                freeTiles++;

                            }

                        }
                    }
                }

                if (freeTiles >= 9)
                {
                    if (m_particleManager != null)
                    {
                        m_particleManager.ReleaseForestFXAt(earth.xIndex, earth.yIndex);
                        m_particleManager.PointsFXAt(earth.xIndex, earth.yIndex, earth.scoreValue, MatchValue.None);
                    }
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ScorePointsForestTiles(earth);

                        GameManager.Instance.UpdateCollectionGoalsForest(earth);
                    }

                    ClearEarthAt(earth.xIndex, earth.yIndex);
                }
            }
        }
    }

    public void ClearEarthAt(int x, int y)
    {
        Earth earth = m_allEarths[x, y];

        if (earth != null)
        {
            m_allEarths[x, y] = null;
            Destroy(earth.gameObject);
        }

        //HighlightTileOff(x,y);
    }

    // checks if the GamePieces have reached their destination positions on collapse
    bool IsCollapsed(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                if (piece.transform.position.y - (float)piece.yIndex > 0f)
                {
                    return false;
                }

                if (piece.transform.position.x - (float)piece.xIndex > 0f)
                {
                    return false;
                }
                if (piece.m_isMoving)
                {
                    return false;
                }

            }
        }
        return true;
    }

    // gets a list of GamePieces in a specified row
    List<GamePiece> GetRowPieces(int row)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePieces[i, row] != null && m_all2ndLayerTiles[i, row] == null)
            {
                gamePieces.Add(m_allGamePieces[i, row]);

                Collectible collectible = m_allGamePieces[i, row].GetComponent<Collectible>();

                if (collectible != null)
                {
                    BreakTileAt(i, row);
                }
            }

            if (m_allTiles[i, row].tileType != TileType.Normal
                || m_allTiles[i, row].tileType != TileType.RoadDown
                || m_allTiles[i, row].tileType != TileType.RoadUp
                || m_allTiles[i, row].tileType != TileType.RoadRight
                || m_allTiles[i, row].tileType != TileType.RoadLeft
                || m_allTiles[i, row].tileType != TileType.Teleport)
            {
                BreakBombedTileAt(i, row);
            }
        }
        return gamePieces;
    }

    // gets a list of GamePieces in a specified column
    List<GamePiece> GetColumnPieces(int column)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            if (m_allGamePieces[column, i] != null && m_all2ndLayerTiles[column, i] == null)
            {
                gamePieces.Add(m_allGamePieces[column, i]);

                Collectible collectible = m_allGamePieces[column, i].GetComponent<Collectible>();

                if (collectible != null)
                {
                    BreakTileAt(column, i);
                }
            }

            if (m_allTiles[column, i] != null && (m_allTiles[column, i].tileType != TileType.Normal
                || m_allTiles[column, i].tileType != TileType.RoadDown
                || m_allTiles[column, i].tileType != TileType.RoadUp
                || m_allTiles[column, i].tileType != TileType.RoadRight
                || m_allTiles[column, i].tileType != TileType.RoadLeft
                || m_allTiles[column, i].tileType != TileType.Teleport))
            {
                BreakBombedTileAt(column, i);
            }
        }


        return gamePieces;
    }

    // get all GamePieces adjacent to a position (x,y)
    List<GamePiece> GetAdjacentPieces(int x, int y, int offset = 1, float waitTime = 0f)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = y - offset; j <= y + offset; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    if (m_allGamePieces[i, j] != null && m_all2ndLayerTiles[i, j] == null)
                    {
                        gamePieces.Add(m_allGamePieces[i, j]);

                        Collectible collectible = m_allGamePieces[i, j].GetComponent<Collectible>();

                        if (collectible != null)
                        {
                            BreakTileAt(i, j, waitTime);
                        }
                    }

                    else
                    {
                        if (m_allTiles[i, j].tileType != TileType.RoadDown
                || m_allTiles[i, j].tileType != TileType.RoadUp
                || m_allTiles[i, j].tileType != TileType.RoadRight
                || m_allTiles[i, j].tileType != TileType.RoadLeft
                || m_allTiles[i, j].tileType != TileType.Teleport
                || m_allTiles[i, j].tileType != TileType.Wall)
                        {
                            if (m_all2ndLayerTiles[i, j] != null)
                            {
                                BreakBombedTileAt(i, j, waitTime);
                            }

                            else if (m_allTiles[i, j].tileType != TileType.Breakable && m_allTiles[i, j].tileType != TileType.DoubleBreakable
                                && m_allTiles[i, j].tileType != TileType.TripleBreakable)
                            {
                                BreakBombedTileAt(i, j, waitTime);
                            }
                            else
                            {
                                BreakTileAt(i, j, waitTime);

                            }

                        }
                    }

                }

            }
        }


        return gamePieces;
    }

    List<GamePiece> ClearTheBoard(float waitTime = 0f)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    if (m_allGamePieces[i, j] != null && m_all2ndLayerTiles[i, j] == null)
                    {
                        gamePieces.Add(m_allGamePieces[i, j]);
                    }

                    else
                    {
                        if (m_allTiles[i, j].tileType != TileType.RoadDown
                && m_allTiles[i, j].tileType != TileType.RoadUp
                && m_allTiles[i, j].tileType != TileType.RoadRight
                && m_allTiles[i, j].tileType != TileType.RoadLeft
                && m_allTiles[i, j].tileType != TileType.Teleport
                && m_allTiles[i, j].tileType != TileType.Wall
                && m_allTiles[i, j].tileType != TileType.Ocean)
                        {
                            if (m_allTiles[i, j].tileType != TileType.Breakable && m_allTiles[i, j].tileType != TileType.DoubleBreakable
                                && m_allTiles[i, j].tileType != TileType.TripleBreakable)
                            {
                                BreakBombedTileAt(i, j, waitTime);
                            }
                            else
                            {
                                if (m_all2ndLayerTiles[i, j] == null)
                                {
                                    BreakTileAt(i, j, waitTime);
                                }
                                else
                                {
                                    BreakBombedTileAt(i, j, waitTime);
                                }

                            }

                        }
                    }

                }

            }
        }

        return gamePieces;
    }

    List<Tile> GetAdjacentTiles(int x, int y, int offset = 1)
    {
        List<Tile> tilePieces = new List<Tile>();

        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = y - offset; j <= y + offset; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    if (m_allTiles[i, j] != null && m_allTiles[i, j].tileType != TileType.RoadDown
                && m_allTiles[i, j].tileType != TileType.RoadUp
                && m_allTiles[i, j].tileType != TileType.RoadRight
                && m_allTiles[i, j].tileType != TileType.RoadLeft
                && m_allTiles[i, j].tileType != TileType.Teleport)
                    {
                        tilePieces.Add(m_allTiles[i, j]);
                    }

                }

            }
        }

        return tilePieces;
    }

    List<Tile> GetAllAdjacentTiles(int x, int y, int offset = 1, float waitTime = 0f)
    {
        List<Tile> tilePieces = new List<Tile>();

        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = y - offset; j <= y + offset; j++)
            {
                if (IsWithinBounds(i, j))
                {

                    tilePieces.Add(m_allTiles[i, j]);


                }

            }
        }

        return tilePieces;
    }

    // given a list of GamePieces, returns a new List of GamePieces that would be destroyed by bombs from the original list
    List<GamePiece> GetBombedPieces(List<GamePiece> gamePieces)
    {
        // list of GamePieces to clear
        List<GamePiece> allPiecesToClear = new List<GamePiece>();

        // loop through the original list of GamePieces
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                // list of GamePieces to be cleared by bombs
                List<GamePiece> piecesToClear = new List<GamePiece>();

                // check each GamePiece if it has a Bomb
                Bomb bomb = piece.GetComponent<Bomb>();

                // if so, get a list of GamePieces affected
                if (bomb != null)
                {
                    switch (bomb.bombType)
                    {
                        case BombType.Column:
                            piecesToClear = GetColumnPieces(bomb.xIndex);
                            if (m_particleManager != null)
                            {
                                m_particleManager.VerticalFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                            }
                            break;
                        case BombType.Row:
                            piecesToClear = GetRowPieces(bomb.yIndex);
                            if (m_particleManager != null)
                            {
                                m_particleManager.HorizontalFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                            }
                            break;
                        case BombType.Adjacent:

                            piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 1);
                            if (m_particleManager != null)
                            {
                                m_particleManager.AdjacentFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                            }
                            break;
                        case BombType.Color:
                            if (piece.matchValue == MatchValue.None)
                            {
                                int random = UnityEngine.Random.Range(1, 6);
                                MatchValue matchValue = getRandomMatchValue(random);
                                int loops = 0;
                                while (!allValuesOnBoard.Contains(matchValue))
                                {
                                    random = UnityEngine.Random.Range(1, 6);
                                    matchValue = getRandomMatchValue(random);

                                    loops++;
                                    if (loops > 50)
                                    {
                                        break;
                                    }
                                }
                                //Debug.Log("match value: " + matchValue);
                                piecesToClear = FindAllMatchValue(matchValue);

                                foreach (GamePiece gamePiece in piecesToClear)
                                {
                                    if (m_particleManager != null)
                                    {
                                        m_particleManager.ColorPickedFXAt(gamePiece.xIndex, gamePiece.yIndex);
                                    }
                                }
                            }
                            else
                            {
                                piecesToClear = FindAllMatchValue(piece.matchValue);
                            }

                            break;

                        case BombType.BigBomb:
                            piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 2);

                            break;

                        case BombType.Special:
                            MatchValue match = bomb.matchValue;
                            GameObject special = null;

                            if (match == MatchValue.Blue && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingBluePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "Blue";
                            }
                            else if (match == MatchValue.Green && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingGreenPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "Green";
                            }
                            else if (match == MatchValue.Purple && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingPurplePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "Purple";
                            }
                            else if (match == MatchValue.Red && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingRedPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "Red";
                            }

                            else if (match == MatchValue.White && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingWhitePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "White";
                            }

                            else if (match == MatchValue.Yellow && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                            {
                                special = Instantiate(specialBombWaitingYellowPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                special.gameObject.name = "Yellow";
                            }


                            if (special != null)
                            {
                                special.transform.parent = transform;
                                if (specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    specialBombsWaiting[bomb.xIndex, bomb.yIndex] = special;
                                }
                            }

                            break;
                    }

                    // keep a running list of all GamePieces affected by bombs
                    allPiecesToClear = allPiecesToClear.Union(piecesToClear).ToList();

                    // remove any collectibles from our list
                    allPiecesToClear = RemoveCollectibles(allPiecesToClear);

                }
            }
        }
        int listCount = allPiecesToClear.Count;

        allPiecesToClear = GetNextBombedPieces(gamePieces, allPiecesToClear);

        while (listCount != allPiecesToClear.Count)
        {
            allPiecesToClear = GetNextBombedPieces(gamePieces, allPiecesToClear);
            listCount = allPiecesToClear.Count;
        }
        // return a list of all GamePieces that would be affected by any bombs from the original list
        return allPiecesToClear;
    }

    private List<GamePiece> GetNextBombedPieces(List<GamePiece> gamePieces, List<GamePiece> allPiecesToClear)
    {
        foreach (GamePiece piece in allPiecesToClear)
        {
            if (!gamePieces.Contains(piece))
            {
                if (piece != null)
                {
                    // list of GamePieces to be cleared by bombs
                    List<GamePiece> piecesToClear = new List<GamePiece>();

                    // check each GamePiece if it has a Bomb
                    Bomb bomb = piece.GetComponent<Bomb>();

                    // if so, get a list of GamePieces affected
                    if (bomb != null)
                    {
                        switch (bomb.bombType)
                        {
                            case BombType.Column:
                                piecesToClear = GetColumnPieces(bomb.xIndex);
                                if (m_particleManager != null)
                                {
                                    m_particleManager.VerticalFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                                }
                                break;
                            case BombType.Row:
                                piecesToClear = GetRowPieces(bomb.yIndex);
                                if (m_particleManager != null)
                                {
                                    m_particleManager.HorizontalFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                                }
                                break;
                            case BombType.Adjacent:
                                piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 1);
                                if (m_particleManager != null)
                                {
                                    m_particleManager.AdjacentFXAt(bomb.xIndex, bomb.yIndex, piece.matchValue);
                                }
                                break;
                            case BombType.Color:

                                int iterations = 0;
                                while (iterations < 100)
                                {
                                    iterations++;
                                    int random = UnityEngine.Random.Range(1, 6);
                                    MatchValue matchValue = getRandomMatchValue(random);
                                    //Debug.Log("match value: " + matchValue);
                                    piecesToClear = FindAllMatchValue(matchValue);

                                    if (!matchValues.Contains(matchValue) && allValuesOnBoard.Contains(matchValue))
                                    {

                                        foreach (GamePiece gamePiece in piecesToClear)
                                        {
                                            if (m_particleManager != null)
                                            {
                                                m_particleManager.ColorPickedFXAt(gamePiece.xIndex, gamePiece.yIndex);
                                            }
                                        }


                                        break;
                                    }
                                    iterations++;
                                }

                                break;

                            case BombType.BigBomb:
                                piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 2);
                                if (m_particleManager != null)
                                {
                                    m_particleManager.AdjacentBigFastFXAt(bomb.xIndex, bomb.yIndex, bomb.matchValue);
                                }

                                break;

                            case BombType.Special:

                                MatchValue match = bomb.matchValue;
                                GameObject special = null;

                                if (match == MatchValue.Blue && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingBluePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "Blue";
                                }
                                else if (match == MatchValue.Green && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingGreenPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "Green";
                                }
                                else if (match == MatchValue.Purple && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingPurplePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "Purple";
                                }
                                else if (match == MatchValue.Red && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingRedPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "Red";
                                }

                                else if (match == MatchValue.White && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingWhitePrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "White";
                                }

                                else if (match == MatchValue.Yellow && specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                {
                                    special = Instantiate(specialBombWaitingYellowPrefab, new Vector3(bomb.xIndex, bomb.yIndex, -4f), Quaternion.identity) as GameObject;
                                    special.gameObject.name = "Yellow";
                                }


                                if (special != null)
                                {
                                    special.transform.parent = transform;
                                    if (specialBombsWaiting[bomb.xIndex, bomb.yIndex] == null)
                                    {
                                        specialBombsWaiting[bomb.xIndex, bomb.yIndex] = special;

                                    }

                                }

                                break;
                        }

                        // keep a running list of all GamePieces affected by bombs
                        allPiecesToClear = allPiecesToClear.Union(piecesToClear).ToList();

                        // remove any collectibles from our list
                        allPiecesToClear = RemoveCollectibles(allPiecesToClear);

                    }
                }
            }
        }

        return allPiecesToClear;
    }

    bool GetColorFX(List<GamePiece> gamePieces)
    {
        bool thereIsColorBomb = false;
        // loop through the original list of GamePieces
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {

                Bomb bomb = piece.GetComponent<Bomb>();

                // if so, get a list of GamePieces affected
                if (bomb != null)
                {
                    switch (bomb.bombType)
                    {
                        case BombType.Color:
                            if (m_particleManager != null)
                            {
                                m_particleManager.ColorFXAt(bomb.xIndex, bomb.yIndex);
                                thereIsColorBomb = true;
                            }
                            break;
                    }

                }
            }
        }

        return thereIsColorBomb;

    }

    private MatchValue getRandomMatchValue(int random)
    {
        switch (random)
        {
            case 1:
                return MatchValue.Green;

            case 2:
                return MatchValue.Red;

            case 3:
                return MatchValue.Blue;

            case 4:
                return MatchValue.Purple;

            case 5:
                return MatchValue.White;

            case 6:
                return MatchValue.Yellow;
        }

        return MatchValue.Green;

    }

    // check if List of matching GamePieces forms an L shaped match
    bool IsCornerMatch(List<GamePiece> gamePieces)
    {
        bool vertical = false;
        bool horizontal = false;
        int xStart = -1;
        int yStart = -1;

        // loop through all of the pieces
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                // if this is the very first piece we are checking, save its x and y index
                if (xStart == -1 || yStart == -1)
                {
                    xStart = piece.xIndex;
                    yStart = piece.yIndex;
                    continue;
                }

                // otherwise, see if GamePiece is in line horizontally with the first piece
                if (piece.xIndex != xStart && piece.yIndex == yStart)
                {
                    horizontal = true;
                }

                // check if are in line vertically with the first piece
                if (piece.xIndex == xStart && piece.yIndex != yStart)
                {
                    vertical = true;
                }
            }
        }


        // return true only if pieces align both horizontally and vertically with first piece
        return (horizontal && vertical);

    }

    // drops a Bomb at a position (x,y) in the Board, given a list of matching GamePieces
    GameObject DropBomb(int x, int y, Vector2 swapDirection, List<GamePiece> gamePieces)
    {
        GameObject prefab = null;
        GameObject bomb = null;
        MatchValue matchValue = MatchValue.None;

        if (gamePieces != null)
        {
            matchValue = FindMatchValue(gamePieces);
        }

        // check if the GamePieces are four or more in a row
        if (gamePieces.Count >= 5 && matchValue != MatchValue.None && matchValue != MatchValue.Litter && matchValue != MatchValue.Barrel && matchValue != MatchValue.BrokenBarrel)
        {
            usedMatches = usedMatches.Union(gamePieces).ToList();

            switch (matchValue)
            {
                case MatchValue.Blue:
                    prefab = gamePiecePrefabs[0];
                    break;
                case MatchValue.Green:
                    prefab = gamePiecePrefabs[1];
                    break;
                case MatchValue.Red:
                    prefab = gamePiecePrefabs[2];
                    break;
                case MatchValue.Purple:
                    prefab = gamePiecePrefabs[3];
                    break;
                case MatchValue.White:
                    prefab = gamePiecePrefabs[4];
                    break;
                case MatchValue.Yellow:
                    prefab = gamePiecePrefabs[5];
                    break;
            }

            if (prefab != null)
            {
                foreach (GamePiece game in gamePieces)
                {
                    GameObject gamePiece = Instantiate(prefab, new Vector3(game.xIndex, game.yIndex, 0f), Quaternion.identity) as GameObject;

                    gamePiece.transform.parent = transform;
                    MovePieceMakingBomb(x, y, 0.3f, gamePiece);
                }
            }

            /* if(m_particleManager != null)
             {
                 m_particleManager.ColorPickedFXAt(x, y, -1, 0f);
             }*/

            if (gamePieces.Count >= 7)
            {
                if (colorBombPrefab != null)
                {
                    bomb = MakeBomb(colorBombPrefab, x, y);

                }
            }

            // check if we form a corner match and create an adjacent bomb
            else if (IsCornerMatch(gamePieces))
            {
                GameObject adjacentBomb = FindGamePieceByMatchValue(adjacentBombPrefabs, matchValue);

                if (adjacentBomb != null)
                {
                    bomb = MakeBomb(adjacentBomb, x, y);
                }
            }

            else
            {
                // if have five or more in a row, form a color bomb - note we probably should swap this upward to 
                // give it priority over an adjacent bomb

                if (colorBombPrefab != null)
                {
                    bomb = MakeBomb(colorBombPrefab, x, y);

                }
            }
        }
        else if (gamePieces.Count == 4 && matchValue != MatchValue.None && matchValue != MatchValue.Litter && matchValue != MatchValue.Barrel && matchValue != MatchValue.BrokenBarrel)
        {
            usedMatches = usedMatches.Union(gamePieces).ToList();

            switch (matchValue)
            {
                case MatchValue.Blue:
                    prefab = gamePiecePrefabs[0];
                    break;
                case MatchValue.Green:
                    prefab = gamePiecePrefabs[1];
                    break;
                case MatchValue.Red:
                    prefab = gamePiecePrefabs[2];
                    break;
                case MatchValue.Purple:
                    prefab = gamePiecePrefabs[3];
                    break;
                case MatchValue.White:
                    prefab = gamePiecePrefabs[4];
                    break;
                case MatchValue.Yellow:
                    prefab = gamePiecePrefabs[5];
                    break;
            }

            if (prefab != null)
            {
                foreach (GamePiece game in gamePieces)
                {
                    GameObject gamePiece = Instantiate(prefab, new Vector3(game.xIndex, game.yIndex, 0f), Quaternion.identity) as GameObject;

                    gamePiece.transform.parent = transform;
                    MovePieceMakingBomb(x, y, 0.3f, gamePiece);
                }
            }

            /* if (m_particleManager != null)
             {
                 m_particleManager.ColorPickedFXAt(x, y, -1, 0f);
             }*/

            // otherwise, drop a row bomb if we are swiping sideways
            if (swapDirection.x != 0)
            {
                GameObject rowBomb = FindGamePieceByMatchValue(rowBombPrefabs, matchValue);
                if (rowBomb != null)
                {
                    bomb = MakeBomb(rowBomb, x, y);
                }

            }
            else
            {
                GameObject columnBomb = FindGamePieceByMatchValue(columnBombPrefabs, matchValue);
                // or drop a vertical bomb if we are swiping upwards
                if (columnBomb != null)
                {
                    bomb = MakeBomb(columnBomb, x, y);
                }
            }
        }
        // return the Bomb object
        return bomb;
    }

    GameObject DropCertainBomb(int x, int y, MatchValue matchValue, int VorH)
    {

        GameObject bomb = null;

        if (VorH == 0)
        {
            GameObject rowBomb = FindGamePieceByMatchValue(rowBombPrefabs, matchValue);
            if (rowBomb != null)
            {
                bomb = MakeBomb(rowBomb, x, y);
            }

        }

        if (VorH == 1)
        {
            GameObject columnBomb = FindGamePieceByMatchValue(columnBombPrefabs, matchValue);
            if (columnBomb != null)
            {
                bomb = MakeBomb(columnBomb, x, y);
            }

        }

        if (VorH == 2)
        {
            GameObject adjacentBomb = FindGamePieceByMatchValue(adjacentBombPrefabs, matchValue);
            if (adjacentBomb != null)
            {
                bomb = MakeBomb(adjacentBomb, x, y);
            }

        }


        // return the Bomb object
        return bomb;
    }

    // puts the bomb into the game Board and treats it as a normal GamePiece
    void ActivateBomb(GameObject bomb)
    {
        int x = (int)bomb.transform.position.x;
        int y = (int)bomb.transform.position.y;


        if (IsWithinBounds(x, y))
        {
            m_allGamePieces[x, y] = bomb.GetComponent<GamePiece>();
        }
    }

    // find all GamePieces on the Board with a certain MatchValue
    List<GamePiece> FindAllMatchValue(MatchValue mValue)
    {
        List<GamePiece> foundPieces = new List<GamePiece>();
        if (mValue != MatchValue.Barrel && mValue != MatchValue.BrokenBarrel && mValue != MatchValue.Litter && mValue != MatchValue.None)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (m_allGamePieces[i, j] != null)
                    {

                        if (m_allGamePieces[i, j].matchValue == mValue)
                        {
                            foundPieces.Add(m_allGamePieces[i, j]);
                        }

                    }
                }
            }
        }

        return foundPieces;
    }

    // return if the Bomb is a Color Bomb
    bool IsColorBomb(GamePiece gamePiece)
    {
        if (gamePiece != null)
        {
            Bomb bomb = gamePiece.GetComponent<Bomb>();

            if (bomb != null)
            {
                return (bomb.bombType == BombType.Color);
            }
            return false;
        }
        return false;
    }

    bool IsRowBomb(GamePiece gamePiece)
    {
        Bomb bomb = gamePiece.GetComponent<Bomb>();

        if (bomb != null)
        {
            return (bomb.bombType == BombType.Row);
        }
        return false;
    }

    bool IsColumnBomb(GamePiece gamePiece)
    {
        Bomb bomb = gamePiece.GetComponent<Bomb>();

        if (bomb != null)
        {
            return (bomb.bombType == BombType.Column);
        }
        return false;
    }
    bool IsAdjacentBomb(GamePiece gamePiece)
    {
        Bomb bomb = gamePiece.GetComponent<Bomb>();

        if (bomb != null)
        {
            return (bomb.bombType == BombType.Adjacent);
        }
        return false;
    }

    bool IsBigBomb(GamePiece gamePiece)
    {
        if (gamePiece != null)
        {
            Bomb bomb = gamePiece.GetComponent<Bomb>();

            if (bomb != null)
            {
                return (bomb.bombType == BombType.BigBomb);
            }
            return false;
        }
        return false;
    }

    bool IsSpecialBomb(GamePiece gamePiece)
    {
        if (gamePiece != null)
        {
            Bomb bomb = gamePiece.GetComponent<Bomb>();

            if (bomb != null)
            {
                return (bomb.bombType == BombType.Special);
            }
            return false;
        }
        return false;
    }


    // find all Collectibles at a certain row
    List<GamePiece> FindCollectiblesAt(int row, bool clearedAtBottomOnly = true)
    {
        List<GamePiece> foundCollectibles = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePieces[i, row] != null)
            {
                Collectible collectibleComponent = m_allGamePieces[i, row].GetComponent<Collectible>();

                if (collectibleComponent != null)
                {
                    // only return the Collectible if it can be cleared by Bomb OR it can be cleared at the bottom of the Board
                    // and has reached the bottom

                    if (collectibleComponent.collType == CollectibleType.Paper)
                    {
                        for (int c = 0; c < eWasteColumns.Count; c++)
                        {
                            if (i == eWasteColumns[c])
                            {
                                foundCollectibles.Add(m_allGamePieces[i, row]);
                                break;
                            }
                        }
                    }

                    if (collectibleComponent.collType == CollectibleType.Metal)
                    {
                        for (int c = 0; c < organicColumns.Count; c++)
                        {
                            if (i == organicColumns[c])
                            {
                                foundCollectibles.Add(m_allGamePieces[i, row]);
                                break;
                            }
                        }
                    }

                    if (collectibleComponent.collType == CollectibleType.Glass)
                    {
                        for (int c = 0; c < glassColumns.Count; c++)
                        {
                            if (i == glassColumns[c])
                            {
                                foundCollectibles.Add(m_allGamePieces[i, row]);
                                break;
                            }
                        }
                    }

                    if (collectibleComponent.collType == CollectibleType.Plastic)
                    {
                        for (int c = 0; c < plasticColumns.Count; c++)
                        {
                            if (i == plasticColumns[c])
                            {
                                foundCollectibles.Add(m_allGamePieces[i, row]);
                                break;
                            }
                        }
                    }

                }
            }
        }
        return foundCollectibles;
    }

    List<GamePiece> FindBombedCollectiblesAt(int row, bool clearedAtBottomOnly = false)
    {
        List<GamePiece> foundCollectibles = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePieces[i, row] != null)
            {
                Collectible collectibleComponent = m_allGamePieces[i, row].GetComponent<Collectible>();

                if (collectibleComponent != null)
                {
                    foundCollectibles.Add(m_allGamePieces[i, row]);
                }
            }
        }
        return foundCollectibles;
    }

    // find all Collectibles in the Board
    List<GamePiece> FindAllCollectibles()
    {
        List<GamePiece> foundCollectibles = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            List<GamePiece> collectibleRow = FindCollectiblesAt(i);
            foundCollectibles = foundCollectibles.Union(collectibleRow).ToList();
        }

        return foundCollectibles;
    }

    List<GamePiece> FindAllBombedCollectibles()
    {
        List<GamePiece> foundCollectibles = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            List<GamePiece> collectibleRow = FindBombedCollectiblesAt(i);
            foundCollectibles = foundCollectibles.Union(collectibleRow).ToList();
        }

        return foundCollectibles;
    }

    // determines if we can add a Collectible based on probability
    bool CanAddCollectible()
    {
        bool canAddMore = false;
        for (int i = 0; i < bCollectibles.Length; i++)
        {
            if (bCollectibles[i].NoToColl > 0)
            {
                canAddMore = true;
                break;
            }
        }
        return (UnityEngine.Random.Range(0f, 1f) <= chanceForCollectible && canAddMore == true && collectibleCount < maxCollectibles);
    }

    // removes any Collectibles if they can cleared by Bombs
    List<GamePiece> RemoveCollectibles(List<GamePiece> bombedPieces)
    {

        List<GamePiece> collectiblePieces = FindAllBombedCollectibles();
        List<GamePiece> piecesToRemove = new List<GamePiece>();

        foreach (GamePiece piece in collectiblePieces)
        {
            Collectible collectibleComponent = piece.GetComponent<Collectible>();
            if (collectibleComponent != null)
            {


                piecesToRemove.Add(piece);

            }
        }
        return bombedPieces.Except(piecesToRemove).ToList();
    }

    // given a list of GamePieces, return the first valid MatchValue found
    MatchValue FindMatchValue(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                return piece.matchValue;
            }
        }

        return MatchValue.None;
    }

    // given an array of prefabs, find one whose GamePiece component has a given matchValue
    GameObject FindGamePieceByMatchValue(GameObject[] gamePiecePrefabs, MatchValue matchValue)
    {
        if (matchValue == MatchValue.None)
        {
            return null;
        }

        foreach (GameObject go in gamePiecePrefabs)
        {
            GamePiece piece = go.GetComponent<GamePiece>();

            if (piece != null)
            {
                if (piece.matchValue == matchValue)
                {
                    return go;
                }
            }
        }

        return null;

    }

    public void TestDeadlock()
    {
        GamePiece[,] m_Pieces = new GamePiece[width, height];


        foreach (GamePiece gp in m_allGamePieces)
        {
            if (gp != null)
            {
                if (m_all2ndLayerTiles[gp.xIndex, gp.yIndex] == null)
                {
                    m_Pieces[gp.xIndex, gp.yIndex] = gp;
                }
            }

        }


        m_boardDeadlock.IsDeadlocked(m_Pieces, 3);
    }

    // invoke the ShuffleBoardRoutine (called by a button for testing)
    public void ShuffleBoard()
    {
        // only shuffle if the Board permits user input
        if (m_playerInputEnabled)
        {
            StartCoroutine(ShuffleBoardRoutine());
        }

    }

    // shuffle non-bomb and non-collectible GamePieces
    IEnumerator ShuffleBoardRoutine()
    {
        int iterations = 0;
        GamePiece[,] m_Pieces = new GamePiece[width, height];
        GamePiece[,] positionArray = new GamePiece[width, height];

        if (m_particleManager != null)
        {

            if ((width % 2) == 0)
            {
                if ((height % 2) == 0)
                {
                    m_particleManager.ShufflingNoteFXAt((width / 2) - 0.5f, (height / 2) - 0.5f, (verticalSizeGlobal * globalAspectRatio) / 2.8125f);
                }
                else
                {
                    m_particleManager.ShufflingNoteFXAt((width / 2) - 0.5f, (height / 2), (verticalSizeGlobal * globalAspectRatio) / 2.8125f);
                }

            }
            else
            {
                if ((height % 2) == 0)
                {
                    m_particleManager.ShufflingNoteFXAt((width / 2), (height / 2) - 0.5f, (verticalSizeGlobal * globalAspectRatio) / 2.8125f);
                }
                else
                {
                    m_particleManager.ShufflingNoteFXAt((width / 2), (height / 2), (verticalSizeGlobal * globalAspectRatio) / 2.8125f);
                }

            }

        }

        yield return new WaitForSeconds(2f);

        do
        {


            //Debug.Log(iterations);
            List<GamePiece> allPieces = new List<GamePiece>();

            foreach (GamePiece piece in m_allGamePieces)
            {
                if (piece != null)
                {
                    if (m_all2ndLayerTiles[piece.xIndex, piece.yIndex] == null && piece.matchValue != MatchValue.None && piece.matchValue != MatchValue.Barrel
                        && piece.matchValue != MatchValue.BrokenBarrel && piece.matchValue != MatchValue.Litter)
                    { allPieces.Add(piece); }
                }

            }

            // wait for any GamePieces that have not settled into place
            while (!IsCollapsed(allPieces))
            {
                yield return null;
            }

            // remove any normalPieces from m_allGamePieces and store them in a List
            List<GamePiece> normalPieces = m_boardShuffler.RemoveNormalPieces(m_allGamePieces, m_all2ndLayerTiles, positionArray);

            // shuffle the list of normal pieces
            m_boardShuffler.ShuffleList(normalPieces);

            // use the shuffled list to fill the Board
            FillBoardFromList(normalPieces, positionArray);

            iterations++;
            //Debug.Log(iterations);



            // move the pieces to their correct onscreen positions
            m_boardShuffler.MovePieces(m_allGamePieces, 0f);

            while (!IsCollapsed(allPieces))
            {
                yield return null;
            }

            allPieces.Clear();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    m_Pieces[i, j] = null;
                    positionArray[i, j] = null;
                }
            }

            foreach (GamePiece gamePiece in m_allGamePieces)
            {
                if (gamePiece != null)
                {
                    if (m_all2ndLayerTiles[gamePiece.xIndex, gamePiece.yIndex] == null && gamePiece.matchValue != MatchValue.None && gamePiece.matchValue != MatchValue.Barrel
                        && gamePiece.matchValue != MatchValue.BrokenBarrel && gamePiece.matchValue != MatchValue.Litter)
                    {
                        m_Pieces[gamePiece.xIndex, gamePiece.yIndex] = gamePiece;
                    }
                }

            }

            if (iterations > 100)
            {
                break;
            }

        }
        while (m_boardDeadlock.IsDeadlocked(m_Pieces, 3));

        //boardShuffleWait = 0.21f * iterations;
        yield return new WaitForSeconds(0.2f);
        // in the event some matches form, clear and refill the Board
        List<GamePiece> matches = FindAllMatches();

        StartCoroutine(ClearAndRefillBoardRoutine(matches));



    }

    IEnumerator CheckMatches()
    {
        for (int i = 1; i < 1000; i++)
        {
            yield return new WaitForSeconds(5f);
            if (!isRefilling)
            {
                GamePiece[,] m_Pieces = new GamePiece[width, height];

                foreach (GamePiece gp in m_allGamePieces)
                {
                    if (gp != null)
                    {
                        if (m_all2ndLayerTiles[gp.xIndex, gp.yIndex] == null)
                        {
                            m_Pieces[gp.xIndex, gp.yIndex] = gp;
                        }
                    }

                }

                BoardDeadlock.lastGlow = 0;
                m_boardDeadlock.HighligthMatch(m_Pieces, 3);
            }

        }
    }

    bool NotOccupiedByObstacle(int i, int j)
    {
        if (m_allTiles[i, j].tileType != TileType.Teleport
            && m_allTiles[i, j].tileType != TileType.FossilFuels
            && m_all2ndLayerTiles[i, j] == null
            && m_allTiles[i, j].tileType != TileType.House
            && m_allTiles[i, j].tileType != TileType.Windmill
            && m_allTiles[i, j].tileType != TileType.WindmillOn
            && m_allTiles[i, j].tileType != TileType.NuclearPP
            && m_allTiles[i, j].tileType != TileType.NuclearPPAlarm
            && m_allTiles[i, j].tileType != TileType.SolarPanel
            && m_allTiles[i, j].tileType != TileType.OilMine
            && m_allTiles[i, j].tileType != TileType.Plants
            && m_allTiles[i, j].tileType != TileType.NuclearRuins
            && m_allTiles[i, j].tileType != TileType.Wall
            && m_allTiles[i, j].tileType != TileType.Ocean
            && m_allTiles[i, j].tileType != TileType.BiogasPlant)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool NotOccupiedByOwithTeleport(int i, int j)
    {
        if (m_allTiles[i, j].tileType != TileType.FossilFuels
            && m_all2ndLayerTiles[i, j] == null
            && m_allTiles[i, j].tileType != TileType.House
            && m_allTiles[i, j].tileType != TileType.Windmill
            && m_allTiles[i, j].tileType != TileType.WindmillOn
            && m_allTiles[i, j].tileType != TileType.NuclearPP
            && m_allTiles[i, j].tileType != TileType.NuclearPPAlarm
            && m_allTiles[i, j].tileType != TileType.SolarPanel
            && m_allTiles[i, j].tileType != TileType.OilMine
            && m_allTiles[i, j].tileType != TileType.Plants
            && m_allTiles[i, j].tileType != TileType.NuclearRuins
            && m_allTiles[i, j].tileType != TileType.Wall
            && m_allTiles[i, j].tileType != TileType.Ocean
            && m_allTiles[i, j].tileType != TileType.BiogasPlant)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    bool AreThreeNeighboringInLine(GamePiece[,] allPieces)
    {

        int neighbors = 0;
        foreach (GamePiece gamePiece in allPieces)
        {
            //Debug.Log("inside foreach");
            if (gamePiece != null && m_all2ndLayerTiles[gamePiece.xIndex, gamePiece.yIndex] == null)
            {
                Bomb bomb = gamePiece.GetComponent<Bomb>();
                if (bomb == null)
                {
                    // check left
                    for (int i = gamePiece.xIndex - 1; i >= 0; i--)
                    {
                        if (allPieces[i, gamePiece.yIndex] != null && m_all2ndLayerTiles[i, gamePiece.yIndex] == null)
                        {
                            Bomb bomb1 = allPieces[i, gamePiece.yIndex].GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                neighbors++;
                            }
                        }

                        else
                        {
                            break;
                        }

                    }

                    if (neighbors >= 2)
                    {
                        return true;
                    }

                    if (neighbors < 2)
                    {
                        neighbors = 0;
                    }

                    // check right
                    for (int i = gamePiece.xIndex + 1; i < width; i++)
                    {
                        if (allPieces[i, gamePiece.yIndex] != null && m_all2ndLayerTiles[i, gamePiece.yIndex] == null)
                        {
                            Bomb bomb1 = allPieces[i, gamePiece.yIndex].GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                neighbors++;
                            }
                        }

                        else
                        {
                            break;
                        }

                    }

                    if (neighbors >= 2)
                    {
                        return true;
                    }

                    if (neighbors < 2)
                    {
                        neighbors = 0;
                    }

                    // check down
                    for (int i = gamePiece.yIndex - 1; i >= 0; i--)
                    {
                        if (allPieces[gamePiece.xIndex, i] != null && m_all2ndLayerTiles[gamePiece.xIndex, i] == null)
                        {
                            Bomb bomb1 = allPieces[gamePiece.xIndex, i].GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                neighbors++;
                            }
                        }

                        else
                        {
                            break;
                        }

                    }

                    if (neighbors >= 2)
                    {
                        return true;
                    }

                    if (neighbors < 2)
                    {
                        neighbors = 0;
                    }

                    // check up
                    for (int i = gamePiece.yIndex + 1; i < height; i++)
                    {
                        if (allPieces[gamePiece.xIndex, i] != null && m_all2ndLayerTiles[gamePiece.xIndex, i] == null)
                        {
                            Bomb bomb1 = allPieces[gamePiece.xIndex, i].GetComponent<Bomb>();
                            if (bomb1 == null)
                            {
                                neighbors++;
                            }
                        }

                        else
                        {
                            break;
                        }

                    }

                    if (neighbors >= 2)
                    {
                        return true;
                    }

                    if (neighbors < 2)
                    {
                        neighbors = 0;
                    }
                }

            }
        }

        return false;
    }

    public bool ClearTheTile(int x, int y)
    {

        boosterMade = false;
        if (m_allGamePieces[x, y] != null && m_all2ndLayerTiles[x, y] == null)
        {
            Collectible collectible = m_allGamePieces[x, y].GetComponent<Collectible>();

            if (collectible != null && m_allTiles[x, y].tileType != TileType.Normal && m_allTiles[x, y].tileType != TileType.Teleport && m_allTiles[x, y].tileType != TileType.Wall)
            {
                BreakTileAt(x, y);

                if (m_particleManager != null)
                {
                    m_particleManager.ThunderFXAt(x, y);
                }
                boosterMade = true;
                return boosterMade;
            }

            else
            {
                if (m_particleManager != null)
                {
                    m_particleManager.ThunderFXAt(x, y);
                }

                ClearAndRefillBoard(x, y);
                boosterMade = true;
                return boosterMade;
            }
        }

        else if (m_allTiles[x, y] != null && m_allGamePieces[x, y] == null && m_all2ndLayerTiles[x, y] == null)
        {
            if (m_allTiles[x, y].tileType != TileType.Normal && m_allTiles[x, y].tileType != TileType.Teleport && m_allTiles[x, y].tileType != TileType.Wall)
            {
                //bool Nuclearexplosion = false;
                if (m_allTiles[x, y].tileType == TileType.NuclearPPAlarm)
                {
                    //Nuclearexplosion = true;
                    if (m_particleManager != null)
                    {
                        m_particleManager.ThunderFXAt(x, y);
                    }
                    BreakBombedTileAt(x, y);
                    NuclearExplosion();
                }
                else
                {

                    if (m_particleManager != null)
                    {
                        m_particleManager.ThunderFXAt(x, y);
                    }
                    BreakBombedTileAt(x, y);
                    BreakTileAt(x, y);
                    if (clearBoardAfterPlanetCooled)
                    {
                        List<GamePiece> emptyList = new List<GamePiece>();
                        List<GamePiece> piecesToClear = new List<GamePiece>();

                        piecesToClear = ClearTheBoard();
                        clearBoardAfterPlanetCooled = false;
                        StartCoroutine(ClearAndRefillBoardRoutine(emptyList, piecesToClear));
                    }
                    else
                    {
                        Refill(x);
                    }

                }
                //BreakBombedTileAt(x, y);

                //Refill(x);
                boosterMade = true;
                /*if (Nuclearexplosion)
                {
                    NuclearExplosion();
                }*/
            }

            return boosterMade;
        }

        else if (m_all2ndLayerTiles[x, y] != null && m_allGamePieces[x, y] == null)
        {
            if (m_particleManager != null)
            {
                m_particleManager.ThunderFXAt(x, y);
            }

            //Debug.Log("2ndLayer");
            BreakBombedTileAt(x, y);
            Refill(x);
            boosterMade = true;
            return boosterMade;
        }

        else if (m_all2ndLayerTiles[x, y] != null && m_allGamePieces[x, y] != null)
        {
            if (m_particleManager != null)
            {
                m_particleManager.ThunderFXAt(x, y);
            }

            BreakBombedTileAt(x, y);
            boosterMade = true;
            return boosterMade;
        }

        else
        {
            return boosterMade;
        }
    }

    void Refill(int x)
    {
        StartCoroutine(ClearAndCollapseAfterBooster(x));
    }

    IEnumerator ClearAndCollapseAfterBooster(int x)
    {
        // list of GamePieces that will be moved
        List<GamePiece> movingPieces = new List<GamePiece>();

        // list of GamePieces that form matches
        List<GamePiece> matches = new List<GamePiece>();
        List<GamePiece> allMatches = new List<GamePiece>();
        List<GamePiece> NoBombMatches = new List<GamePiece>();

        //yield return new WaitForSeconds(0.5f);

        bool isFinished = false;

        while (!isFinished)
        {


            // find any collectibles that have reached the bottom and decrement the number of collectibles needed
            List<GamePiece> collectedPieces = FindCollectiblesAt(0, true);

            // decrement cleared collectibles/blockers
            collectibleCount -= collectedPieces.Count;

            yield return new WaitForSeconds(0.5f);

            if (m_allEarths.Length > 0)
            {
                ReleaseEarths();
            }
            // after a delay, collapse the columns to remove any empty spaces

            // collapse any columns with empty spaces and keep track of what pieces moved as a result

            filledPieces = FillBoard();

            while (!IsCollapsed(filledPieces))
            {
                yield return null;
            }

            filledPieces.Clear();

            yield return StartCoroutine(CollapsePiecesRoutine());

            filledPieces.Clear();
            filledPieces = FillBoard();

            while (!IsCollapsed(filledPieces))
            {
                yield return null;
            }

            filledPieces.Clear();

            // find any matches that form from collapsing...
            matches = FindAllMatches();

            //...and any collectibles that hit the bottom row...
            collectedPieces = FindCollectiblesAt(0, true);

            //... and add them to our list of GamePieces to clear
            matches = matches.Union(collectedPieces).ToList();


            // if we didn't make any matches from the collapse, then we're done
            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }

            // otherwise, increase our score multiplier for the chair reaction... 
            else
            {
                m_scoreMultiplier++;
                allBombs.Clear();
                // ...play a bonus sound for making a chain reaction...
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayBonusSound();
                }

                usedMatches = CreateNewBombs(usedMatches);

                //usedMatches.Clear();

                // ...and run ClearAndCollapse again

                yield return StartCoroutine(ClearAndCollapseRoutine(matches));
                yield break;

            }
        }
        yield return null;
    }

    private void NuclearExplosion()
    {
        if (nuclearDisasters > 0)
        {

            //Debug.Log(nuclearDisasters);
            for (int i = 0; i <= nuclearDisasters; i++)
            {
                int x = nuclearListX[i];
                int y = nuclearListY[i];

                NuclearBlast(x, y);
                NuclearRuins(x, y);
                nuclearDisasters--;
            }

            nuclearDisasters = 0;
            nuclearListX.Clear();
            nuclearListY.Clear();
            NuclearDisaster();
            List<GamePiece> allPieces = new List<GamePiece>();
            //routine++;

            foreach (GamePiece gamePiece in m_allGamePieces)
            {
                allPieces.Add(gamePiece);
            }
            delayAfterNuclearBlast = true;
            StartCoroutine(ClearAndRefillBoardRoutine(allPieces));
        }
    }

    public IEnumerator ConvertMovesForBombs()
    {
        if (GameManager.Instance != null)
        {
            int moves = GameManager.Instance.m_levelGoal.movesLeft;
            List<GamePiece> allPieces = new List<GamePiece>();

            if (moves > 0)
            {
                List<GamePiece> boardPieces = new List<GamePiece>();

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (m_allGamePieces[i, j] != null)
                        {
                            boardPieces.Add(m_allGamePieces[i, j]);
                        }
                    }
                }

                while (!IsCollapsed(boardPieces))
                {
                    yield return null;
                }

                for (int i = 0; i < moves; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    int randomGamePieceX = UnityEngine.Random.Range(0, width);
                    int randomGamePieceY = UnityEngine.Random.Range(0, height);
                    int randomBombType = UnityEngine.Random.Range(0, 3);
                    int loops = 0;
                    bool execute = true;

                    //Debug.Log("here");
                    while (m_allGamePieces[randomGamePieceX, randomGamePieceY] == null || IsColorBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsColumnBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
                   || IsRowBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsAdjacentBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || IsBigBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY])
                   || IsSpecialBomb(m_allGamePieces[randomGamePieceX, randomGamePieceY]) || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Barrel
                   || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.BrokenBarrel || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.Litter
                   || m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue == MatchValue.None || m_all2ndLayerTiles[randomGamePieceX, randomGamePieceY] != null)
                    {
                        randomGamePieceX = UnityEngine.Random.Range(0, width);
                        randomGamePieceY = UnityEngine.Random.Range(0, height);

                        if (loops > 100)
                        {
                            execute = false;
                            break;
                        }
                        loops++;
                    }
                    if (execute)
                    {
                        GameObject bomb = DropCertainBomb(randomGamePieceX, randomGamePieceY, m_allGamePieces[randomGamePieceX, randomGamePieceY].matchValue, randomBombType);
                        if (bomb != null)
                        {

                            ClearPieceAt(randomGamePieceX, randomGamePieceY);
                            //allBombs.Add(bomb);
                            ActivateBomb(bomb);
                            GamePiece piece = bomb.GetComponent<GamePiece>();
                            allPieces.Add(piece);
                            if (m_particleManager != null)
                            {
                                m_particleManager.ColorPickedFXAt(randomGamePieceX, randomGamePieceY, -4);
                                m_particleManager.PointsFXAt(piece.xIndex, piece.yIndex, piece.scoreValue, piece.matchValue);
                            }
                            if (SoundManager.Instance != null)
                            {
                                SoundManager.Instance.PlaySpawnBombSound();
                            }
                        }
                    }

                    GameManager.Instance.UpdateMoves();
                }

                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(ClearAndRefillBoardRoutine(allPieces, null, null, true));
                yield break;

            }
            else
            {
                foreach (GamePiece gamePiece in m_allGamePieces)
                {
                    allPieces.Add(gamePiece);
                }
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(ClearAndRefillBoardRoutine(allPieces, null, null, true));
                yield break;
            }
        }
    }

    public IEnumerator ExplodeRemainingBombs()
    {
        List<GamePiece> remainingBoms = new List<GamePiece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allGamePieces[i, j] != null)
                {
                    Bomb bomb = m_allGamePieces[i, j].GetComponent<Bomb>();

                    if (bomb != null)
                    {
                        remainingBoms.Add(m_allGamePieces[i, j]);
                    }
                }
            }
        }

        if (remainingBoms.Count > 0)
        {
            lookForAllColB = true;
            bombsStillRemaining = true;
            yield return StartCoroutine(ClearAndRefillBoardRoutine(remainingBoms, null, null, true));
            yield break;
        }
        else
        {
            bombsStillRemaining = false;
        }

    }

    public void MovePieceMakingBomb(int destX, int destY, float timeToMove, GameObject piece, float waitTimeBD = 0f)
    {

        // only move if the GamePiece is not already moving

        StartCoroutine(MovePieceMakingBombRoutine(new Vector3(destX, destY, 0), timeToMove, piece, waitTimeBD));

    }

    IEnumerator MovePieceMakingBombRoutine(Vector3 destination, float timeToMove, GameObject piece, float waitTimeBD = 0f)
    {
        // store our starting position
        Vector3 startPosition = piece.transform.position;

        // have we reached our destination?
        bool reachedDestination = false;

        // how much time has passed since we started moving
        float elapsedTime = 0f;

        // while we have not reached the destination, check to see if we are close enough
        while (!reachedDestination)
        {
            // if we are close enough to destination
            if (Vector3.Distance(piece.transform.position, destination) < 0.01f)
            {
                // we have reached the destination
                reachedDestination = true;
                if (waitTimeBD > 0f)
                {
                    yield return new WaitForSeconds(waitTimeBD);
                }
                Destroy(piece);
                break;
            }

            // increment the total running time by the Time elapsed for this frame
            elapsedTime += Time.deltaTime;

            // calculate the Lerp value
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);


            t = Mathf.Sin(t * Mathf.PI * 0.5f);


            // move the game piece
            piece.transform.position = Vector3.Lerp(startPosition, destination, t);


            // wait until next frame
            yield return null;
        }
    }
}
