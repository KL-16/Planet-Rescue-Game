using UnityEngine;

//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


// the GameManager is the master controller for the GamePlay

[RequireComponent(typeof(LevelGoal))]
public class GameManager : Singleton<GameManager>
{

    // reference to the Board
    Board m_board;

    // is the player read to play?
    bool m_isReadyToBegin = false;

    // is the game over?
    bool m_isGameOver = false;

    public static Tile boosterTile;

    // Array to store stars in each level
    int numOfStars;
    public int numberOfLevels = 50;
    int starsInArray;

    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    // do we have a winner?
    public bool m_isWinner = false;
    
    // are we ready to load/reload a new level?
    bool m_isReadyToReload = false;

    public bool BoosterIsActive = false;

    public TileEvent boosterEvent = new TileEvent();

    public UnityEvent switchBoosterE = new UnityEvent();

    GameObject myCarrierObject;

    // reference to LevelGoal component
    public LevelGoal m_levelGoal;

    public static bool useColorBooster = false;
    public static bool useJumpingBooster = false;
    public static bool useBigBooster = false;

    // reference to LevelGoalTimed component (null if level is not timed)
    //    LevelGoalTimed m_levelGoalTimed;

    LevelGoalCollected m_levelGoalCollected;

    // public reference to LevelGoalTimed component
    public LevelGoal LevelGoal { get { return m_levelGoal; } }

    public Button pauseButton;
    GameObject adsManager;

    public override void Awake()
    {
        base.Awake();

        LoadInfo.LoadData();
          
        // fill in LevelGoal and LevelGoalTimed components
        m_levelGoal = GetComponent<LevelGoal>();
//        m_levelGoalTimed = GetComponent<LevelGoalTimed>();
        m_levelGoalCollected = GetComponent<LevelGoalCollected>();

        // cache a reference to the Board
        m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
        boosterTile = GetComponent<Tile>();
        IsGameOver = false;
        m_isWinner = false;
    }

    void Start()
    {

        if (UIManager.Instance != null)
        {
            pauseButton.GetComponent<Button>().interactable = false;

            // position ScoreStar horizontally
            if (UIManager.Instance.scoreMeter != null)
            {
                UIManager.Instance.scoreMeter.SetupStars(m_levelGoal);
            }

            // use the Scene name as the Level name
            if (UIManager.Instance.levelNameText != null)
            {
                // get a reference to the current Scene
                Scene scene = SceneManager.GetActiveScene();
                UIManager.Instance.levelNameText.text = scene.name;
            }

            if (m_levelGoalCollected != null)
            {
                UIManager.Instance.EnableCollectionGoalLayout(true);
                //UIManager.Instance.SetupCollectionGoalLayout(m_levelGoalCollected.collectionGoals);
            }
            else
            {
                UIManager.Instance.EnableCollectionGoalLayout(false);
            }

            bool useTimer = (m_levelGoal.levelCounter == LevelCounter.Timer);

            UIManager.Instance.EnableTimer(useTimer);
            UIManager.Instance.EnableMovesCounter(!useTimer);
        }

        IsGameOver = false;
        m_isWinner = false;

        adsManager = GameObject.Find("Add Manager");
        myCarrierObject = GameObject.Find("LoadedInfoHolder");

        if(myCarrierObject != null)
        {
            if (LoadInfo.adsRemoved == false)
            {
                if (myCarrierObject.GetComponent<StaticVariableHolder>().levelsSinceLastAdd > 4 && adsManager != null)
                {
                    myCarrierObject.GetComponent<StaticVariableHolder>().levelsSinceLastAdd = 0;
                    adsManager.GetComponent<AdsManager>().ShowSkipAdd();
                }
            }            
        }
        
        // update the moves left UI
        m_levelGoal.movesLeft++;
        UpdateMoves();
        useColorBooster = false;
        useJumpingBooster = false;
        useBigBooster = false;
        IsGameOver = false;
        m_isWinner = false;
        // start the main game loop
        StartCoroutine("ExecuteGameLoop");
    }

    public void BoosterTile(Tile tile)
    {
        boosterTile = tile;
    }

    public void BoosterGoOff()
    {
        if (boosterEvent != null && boosterTile != null)
        {
            boosterEvent.Invoke(boosterTile);
        }
    }

    public void SwitchBoosterDone()
    {
        switchBoosterE.Invoke();
    }

    // update the Text component that shows our moves left
    public void UpdateMoves()
    {
        // if the LevelGoal is not timed (e.g. LevelGoalScored)...
        if (m_levelGoal.levelCounter == LevelCounter.Moves)
        {

            // decrement a move
            m_levelGoal.movesLeft = Mathf.Clamp(--m_levelGoal.movesLeft, 0, 999); ;

            // update the UI
            if (UIManager.Instance != null && UIManager.Instance.movesLeftText != null)
            {
                UIManager.Instance.movesLeftText.text = m_levelGoal.movesLeft.ToString();
            }
        }
    }

    public void UpdateMovesText()
    {
        // if the LevelGoal is not timed (e.g. LevelGoalScored)...
        if (m_levelGoal.levelCounter == LevelCounter.Moves)
        {
            // update the UI
            if (UIManager.Instance != null && UIManager.Instance.movesLeftText != null)
            {
                UIManager.Instance.movesLeftText.text = m_levelGoal.movesLeft.ToString();
            }
        }
    }

    // this is the main coroutine for the Game, that determines are basic beginning/middle/end

    // each stage of the game must complete before we advance to the next stage
    // add as many stages here as necessary

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");

        // wait for board to refill
        yield return StartCoroutine("WaitForBoardRoutine", 0.5f);

        yield return StartCoroutine("EndGameRoutine");
    }

    public IEnumerator ExtraGameLoop()
    {      
            yield return StartCoroutine("PlayGameRoutine");

            // wait for board to refill
            yield return StartCoroutine("WaitForBoardRoutine", 0.5f);

            yield return StartCoroutine("EndGameRoutine");  
    }

    // switches ready to begin status to true
    public void BeginGame()
    {
        m_isReadyToBegin = true;
        pauseButton.GetComponent<Button>().interactable = true;

    }

    // coroutine for the level introduction
    IEnumerator StartGameRoutine()
    {
        if (UIManager.Instance != null)
        {
            // show the message window with the level goal
            if (UIManager.Instance.messageWindow != null)
            {
                UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
                int maxGoal = m_levelGoal.scoreGoals.Length - 1;
                UIManager.Instance.messageWindow.ShowScoreMessage();

            }
        }

        // wait until the player is ready
        while (!m_isReadyToBegin)
        {
            yield return null;
        }

        // fade off the ScreenFader
        if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
        {
            UIManager.Instance.screenFader.FadeOff();
        }

        // wait half a second
        yield return new WaitForSeconds(1f);

        // setup the Board
        if (m_board != null)
        {
            m_board.SetupBoard();
        }
    }

    // coroutine for game play
    IEnumerator PlayGameRoutine()
    {
        // while the end game condition is not true, we keep playing
        // just keep waiting one frame and checking for game conditions
        //Debug.Log("is game over: " + m_isGameOver);
        while (!m_isGameOver)
        {

            m_isGameOver = m_levelGoal.IsGameOver();
            m_isWinner = m_levelGoal.IsWinner();

            // wait one frame
            yield return null;
        }

    }

    IEnumerator WaitForBoardRoutine(float delay = 0f)
    {
        if (m_levelGoal.levelCounter == LevelCounter.Timer && UIManager.Instance != null
            && UIManager.Instance.timer != null)
        {
            UIManager.Instance.timer.FadeOff();
            UIManager.Instance.timer.paused = true;
        }
        if (m_board != null)
        {
            // this accounts for the swapTime delay in the Board's SwitchTilesRoutine BEFORE ClearAndRefillRoutine is invoked
            yield return new WaitForSeconds(m_board.swapTime);

            // wait while the Board is refilling
            while (m_board.isRefilling)
            {
                yield return null;
            }
        }

        // extra delay before we go to the EndGameRoutine
        yield return new WaitForSeconds(delay);
    }

    // coroutine for the end of the level
    IEnumerator EndGameRoutine()
    {
        // set ready to reload to false to give the player time to read the screen
        m_isReadyToReload = false;

        if(m_board.isRefilling == true)
        {
            yield return null;
        }
        // if player beat the level goals, show the win screen and play the win sound
        if (m_isWinner)
        {
            if (m_board.clearRemainingBombs)
            {
                yield return StartCoroutine(m_board.ExplodeRemainingBombs());

                yield return null;

                while(m_board.isRefilling == true)
                {
                    yield return null;
                }

                while (m_board.bombsStillRemaining == true)
                {
                    yield return StartCoroutine(m_board.ExplodeRemainingBombs());

                    yield return null;

                    while (m_board.isRefilling == true)
                    {
                        yield return null;
                    }
                }
                //method to covert moves for bombs
                yield return StartCoroutine(m_board.ConvertMovesForBombs());

                yield return null;

                while (m_board.isRefilling == true)
                {
                    yield return null;
                }

                yield return StartCoroutine(m_board.ExplodeRemainingBombs());

                yield return null;

                while (m_board.isRefilling == true)
                {
                    yield return null;
                }

                while (m_board.bombsStillRemaining == true)
                {
                    yield return StartCoroutine(m_board.ExplodeRemainingBombs());

                    yield return null;

                    while (m_board.isRefilling == true)
                    {
                        yield return null;
                    }
                }
            }
           
            yield return new WaitForSeconds(1f);

            ShowWinScreen();
        } 
        // otherwise, show the lose screen and play the lose sound
		else
        {   
            ShowLoseScreen();
        }

        // wait one second
        //yield return new WaitForSeconds(1f);

        // fade the screen 
        if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
        {
            UIManager.Instance.screenFader.FadeOn();
        }  

        // wait until read to reload
        while (!m_isReadyToReload)
        {
            yield return null;
        }

        // reload the scene (you would customize this to go back to the menu or go to the next level
        // but we just reload the same scene in this demo

        if (m_isWinner)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            myCarrierObject.GetComponent<StaticVariableHolder>().levelsSinceLastAdd++;
        }
        else
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
		
    }

    void ShowWinScreen()
    {
        int stars = -1;
        int actualStars = 0;
        if (UIManager.Instance != null && UIManager.Instance.messageWindow != null)
        {
            if(m_levelGoal.scoreStars < 1)
            {
                actualStars = 1;
            }
            else
            {
                actualStars = m_levelGoal.scoreStars;
            }
            UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowWinMessage();
            UIManager.Instance.messageWindow.DisplayStars(actualStars);

            if (ScoreManager.Instance != null)
            {
                starsInArray = 0;
                string scoreStr = "you scored " + ScoreManager.Instance.CurrentScore.ToString() + " points!";
                UIManager.Instance.messageWindow.ShowGoalCaption(scoreStr,0, 0);
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                if (numOfStars < actualStars)
                {
                   numOfStars = actualStars;

                    stars = LoadInfo.StarsInLevel(currentSceneIndex - 2);

                    if(stars < actualStars)
                    {
                        SaveStars(numOfStars, currentSceneIndex - 2);
                    }

                }   
            }
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();
        }
    }

    void ShowLoseScreen()
    {
        if (UIManager.Instance != null && UIManager.Instance.messageWindow != null)
        {
            UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowLoseMessage();
            
            string caption = "";
            if (m_levelGoal.movesLeft > 0)
            {
                caption = "No more possible moves!";
                   
            }
            else
            {
                caption = "Out of moves!"; 
            }

            UIManager.Instance.messageWindow.ShowGoalCaption(caption, 0, 0);

        }
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLoseSound();
        }
    }

    // use this to acknowledge that the player is ready to reload
    public void ReloadScene()
    {
        m_isReadyToReload = true;
    }

    // score points and play a sound
    public void ScorePoints(GamePiece piece, int multiplier = 1, int bonus = 0)
    {
        if (piece != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(piece.scoreValue * multiplier + bonus);

                // update the scoreStars in the Level Goal component
                m_levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore, 
                        m_levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
            if (SoundManager.Instance != null && piece.clearSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }

    public void ScorePointsTiles(Tile tile)
    {
        if (tile != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(tile.scoreValue);

                // update the scoreStars in the Level Goal component
                m_levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore,
                        m_levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
            if (SoundManager.Instance != null && tile.clearSound != null)
            {
                SoundManager.Instance.PlayRandom(tile.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            } 
        }
    }

    public void ScorePoints2ndTiles(Tiles2ndLayer tile)
    {
        if (tile != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(tile.scoreValue);

                // update the scoreStars in the Level Goal component
                m_levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore,
                        m_levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
             if (SoundManager.Instance != null && tile.clearSound != null)
             {
                 SoundManager.Instance.PlayRandom(tile.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
             } 
        }
    }

    public void ScorePointsPlanetTiles(Ocean tile)
    {
        if (tile != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(tile.scoreValue);

                // update the scoreStars in the Level Goal component
                m_levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore,
                        m_levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
            /* if (SoundManager.Instance != null && tile.clearSound != null)
             {
                 SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
             } */
        }
    }

    public void ScorePointsForestTiles(Earth tile)
    {
        if (tile != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(tile.scoreValue);

                // update the scoreStars in the Level Goal component
                m_levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore,
                        m_levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
             if (SoundManager.Instance != null && tile.clearSound != null)
             {
                 SoundManager.Instance.PlayClipAtPoint(tile.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
             } 
        }
    }



    public void AddMoves(int movesValue)
    {
        if (m_levelGoal.levelCounter == LevelCounter.Moves)
        {
            m_levelGoal.AddMoves(movesValue);
        }
    }

    public void AddTime(int timeValue)
    {
        if (m_levelGoal.levelCounter == LevelCounter.Timer)
        {
            m_levelGoal.AddTime(timeValue);
        }
    }

    public void UpdateCollectionGoals(GamePiece pieceToCheck)
    {
        if (m_levelGoalCollected != null)
        {
            m_levelGoalCollected.UpdateGoals(pieceToCheck);
        }
    }

    public void UpdateCollectionGoalsTiles(Tile tileToCheck)
    {
        if (m_levelGoalCollected != null)
        {
            m_levelGoalCollected.UpdateGoalsTiles(tileToCheck);
        }
    }

    public void UpdateCollectionGoalsTiles2nd(Tiles2ndLayer tileToCheck)
    {
        if (m_levelGoalCollected != null)
        {
            m_levelGoalCollected.UpdateGoalsTiles2nd(tileToCheck);
        }
    }

    public void UpdateCollectionGoalsForest(Earth earth)
    {
        if (m_levelGoalCollected != null)
        {
            m_levelGoalCollected.UpdateGoalsEarth(earth);
        }
    }

    public void UpdateCollectionGoalsPlanet(Ocean ocean)
    {
        if (m_levelGoalCollected != null)
        {
            m_levelGoalCollected.UpdateGoalsPlanet(ocean);
        }
    }


    private void SaveStars(int stars, int idx)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + idx);

        PlayerData data = new PlayerData();
        data.stars = stars;

        bf.Serialize(file, data);
        file.Close();
    }

}

[Serializable]
class PlayerData
{
    public int stars;
    
}

[Serializable]
class NonConsumableData
{
    public bool adsRemoved;
}
