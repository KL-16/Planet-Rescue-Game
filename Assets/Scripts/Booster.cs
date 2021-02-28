using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[RequireComponent(typeof(Image))]
public class Booster : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	// the UI.Image component
    Image m_image;

    // the RectTransform component
    RectTransform m_rectXform;

    // reset position
    Vector3 m_startPosition;

    // Board component
    Board m_board;

    // the Tile to apply the booster effect
    Tile m_tileTarget;

    // the one active Booster GameObject
    public static GameObject ActiveBooster;

    // UI.Text component for instructions
    public Text instructionsText;

    public GameObject textObject;

    // text instructions 
    public string instructions = "drag over game piece to remove";

    // is the Booster enabled? (has the button been clicked once?)
    public bool isEnabled = false;

    // is this Booster intended to draggable (currently the only implemented behavior)
    public bool isDraggable = true;

    // has the Booster been locked (for use with another manager script)
    public bool isLocked = false;

    public static bool switchBoosterAnabled = false;

    // useful for UI elements that may be colliding with drag event / add a CanvasGroup and add to List
    public List<CanvasGroup> canvasGroups;

    // actions to invoke when the drag is complete
    public UnityEvent boostEvent;

    // time bonus
    public int boostMoves = 5;

    // number of boosters
    public Text booster;
    int boosterNo;
    Text boostertext;

    LevelGoal m_levelGoal;

    public string boostertype = "onePieceRemove";

    //buttons to buy boosters
    public Button boosterBuy;
    Button buttonBuy;

    // initialize components
    void Awake()
    {
        m_image = GetComponent<Image>();
        m_rectXform = GetComponent<RectTransform>();
        m_board = UnityEngine.Object.FindObjectOfType<Board>().GetComponent<Board>();
        m_levelGoal = GetComponent<LevelGoal>();
        buttonBuy = boosterBuy.GetComponent<Button>();
    }

    void Start()
    {
        EnableBooster(false);
        boostertext = booster.GetComponent<Text>();

        if(boostertype == "onePieceRemove")
        {
            if(LoadInfo.booster1No > 0)
            {
                boostertext.text = LoadInfo.booster1No.ToString();
                boosterNo = LoadInfo.booster1No;
                buttonBuy.gameObject.SetActive(false);
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
                boosterNo = LoadInfo.booster1No;
            }
            
        }

        if (boostertype == "switch")
        {
            if (LoadInfo.booster4No > 0)
            {
                boostertext.text = LoadInfo.booster4No.ToString();
                boosterNo = LoadInfo.booster4No;
                buttonBuy.gameObject.SetActive(false);
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
                boosterNo = LoadInfo.booster4No;
            }
        }

        if (boostertype == "meteor")
        {
            if (LoadInfo.booster5No > 0)
            {
                boostertext.text = LoadInfo.booster5No.ToString();
                boosterNo = LoadInfo.booster5No;
                buttonBuy.gameObject.SetActive(false);
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
                boosterNo = LoadInfo.booster5No;
            }
        }


        GameManager.Instance.boosterEvent.AddListener(OnBoosterDown);
        GameManager.Instance.switchBoosterE.AddListener(SwitchTwoPieces);
        textObject.gameObject.SetActive(false);
        switchBoosterAnabled = false;
    }

    public void UpdateGameBoosters()
    {
        if (boostertype == "onePieceRemove")
        {
                boostertext.text = LoadInfo.booster1No.ToString();
                boosterNo = LoadInfo.booster1No;
                buttonBuy.gameObject.SetActive(false);          
        }

        if (boostertype == "switch")
        {
                boostertext.text = LoadInfo.booster4No.ToString();
                boosterNo = LoadInfo.booster4No;
                buttonBuy.gameObject.SetActive(false);
        }

        if (boostertype == "meteor")
        {
                boostertext.text = LoadInfo.booster5No.ToString();
                boosterNo = LoadInfo.booster5No;
                buttonBuy.gameObject.SetActive(false);
        }
    }

    // toggle the Booster on/off
    public void EnableBooster(bool state)
    {
        isEnabled = state;

        if (state)
        {
            DisableOtherBoosters();
            Booster.ActiveBooster = gameObject;
            GameManager.Instance.BoosterIsActive = true;
        }
        else if (gameObject == Booster.ActiveBooster)
        {
            Booster.ActiveBooster = null;
            GameManager.Instance.BoosterIsActive = false;
            ChangeBoostersToWhite();
        }

       
        if (instructionsText != null && textObject != null)
        {
            instructionsText.gameObject.SetActive(Booster.ActiveBooster != null);
            textObject.gameObject.SetActive(Booster.ActiveBooster != null);
            if (gameObject == Booster.ActiveBooster)
            {
                instructionsText.text = instructions;
            }
        }
    }

    // disable all other boosters
    void DisableOtherBoosters()
    {
        Booster[] allBoosters = UnityEngine.Object.FindObjectsOfType<Booster>();

        foreach (Booster b in allBoosters)
        {
            if (b != this)
            {
                b.EnableBooster(false);
                
            }
        }

        foreach (Booster b in allBoosters)
        {
            if (b != this)
            {
                b.m_image.color = Color.gray;
            }
        }
    }

    void ChangeBoostersToWhite()
    {
        Booster[] allBoosters = UnityEngine.Object.FindObjectsOfType<Booster>();

        foreach (Booster b in allBoosters)
        {
            b.m_image.color = Color.white;
        }
    }

    // toggle Booster state
    public void ToggleBooster()
    {
        if (boosterNo > 0 && m_board != null && m_board.isRefilling == false)
        {
            if (boostertype == "switch")
            {
                if (!m_board.isRefilling)
                {
                    switchBoosterAnabled = !switchBoosterAnabled;
                    EnableSwitchBooster(!isEnabled);
                }               
            }

            else
            {
                switchBoosterAnabled = false;
                EnableBooster(!isEnabled);
            }
        }    
    }

    public void EnableSwitchBooster(bool state)
    {
        isEnabled = state;

        if (state)
        {
            //Debug.Log("here2");
            DisableOtherBoosters();
            Booster.ActiveBooster = gameObject;
        }
        else if (gameObject == Booster.ActiveBooster)
        {
            //Debug.Log("here");
            Booster.ActiveBooster = null;
            GameManager.Instance.BoosterIsActive = false;
            ChangeBoostersToWhite();
        }

        if (instructionsText != null && textObject != null)
        {
            instructionsText.gameObject.SetActive(Booster.ActiveBooster != null);
            textObject.gameObject.SetActive(Booster.ActiveBooster != null);

            if (gameObject == Booster.ActiveBooster)
            {
                instructionsText.text = instructions;
            }
        }
    }

    // frame where we begin dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable && !isLocked)
        {
            m_startPosition = gameObject.transform.position;
            EnableCanvasGroups(false);
        }
    }

    // still dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable && !isLocked && Camera.main != null)
        {
            Vector3 onscreenPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectXform, eventData.position, 
                                                                    Camera.main, out onscreenPosition);
            gameObject.transform.position = onscreenPosition;

            RaycastHit2D hit2D = Physics2D.Raycast(onscreenPosition, Vector3.forward, Mathf.Infinity);

            if (hit2D.collider != null )
            {
                m_tileTarget = hit2D.collider.GetComponent<Tile>();
            }
            else
            {
                m_tileTarget = null;
            }
        }
    }

    // frame where we end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable && !isLocked)
        {
            gameObject.transform.position = m_startPosition;
            EnableCanvasGroups(true);

            if (m_board != null && m_board.isRefilling)
            {
                return;
            }

            if (m_tileTarget != null)
            {
                if (boostEvent != null)
                {
                    boostEvent.Invoke();
                }

                EnableBooster(false);

                m_tileTarget = null;
                Booster.ActiveBooster = null;
            }
        }
    }

    public void OnBoosterDown(Tile tile)
    {
        if (isEnabled && !isDraggable && !isLocked && PauseMenu.gameIsPaused == false)
        {
            EnableCanvasGroups(true);

            if (m_board != null && m_board.isRefilling)
            {
                return;
            }
           
            m_tileTarget = tile;

            if (m_tileTarget != null)
            {
                if (boostEvent != null)
                {
                    boostEvent.Invoke();
                }

                EnableBooster(false);

                m_tileTarget = null;
                Booster.ActiveBooster = null;
                GameManager.Instance.BoosterIsActive = false;
            }
        }
    }

    // enable/disable blocksRaycasts for CanvasGroup components
    void EnableCanvasGroups(bool state)
    {
        if (canvasGroups != null && canvasGroups.Count > 0)
        {
            foreach (CanvasGroup cGroup in canvasGroups)
            {
                if (cGroup != null)
                {
                    cGroup.blocksRaycasts = state;
                }
            }
        }
    }

    // action to switch tiles
    public void SwitchTwoPieces()
    {
        if (GameManager.Instance != null && isEnabled && !isDraggable && !isLocked && !GameManager.Instance.IsGameOver)
        {
            EnableCanvasGroups(true);

            if (m_board != null && m_board.isRefilling)
            {
                return;
            }
            //switchBoosterAnabled = false;

            EnableSwitchBooster(false);
            ChangeBoostersToWhite();
            boosterNo = Mathf.Clamp(--boosterNo, 0, 999);
            if (boosterNo > 0)
            {
                boostertext.text = boosterNo.ToString();
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
            }

            LoadInfo.Save(boosterNo, boostertype);
            --LoadInfo.booster4No;
            Booster.ActiveBooster = null;
            
        }
                   
    }

    public void MeteorRain()
    {
        if(GameManager.Instance != null && isEnabled && !isDraggable && !isLocked && !GameManager.Instance.IsGameOver)
        {
            if (m_board != null && !m_board.isRefilling)
            {
                Board.delayAfterMeteorBooster = true;

                m_board.MeteorImpact();

                boosterNo = Mathf.Clamp(--boosterNo, 0, 999);
                if(boosterNo > 0)
                {
                    boostertext.text = boosterNo.ToString();
                }
                else
                {
                    buttonBuy.gameObject.SetActive(true);
                }

                LoadInfo.Save(boosterNo, boostertype);
                --LoadInfo.booster5No;
            }
        }
        
    }

    // action to remove one GamePiece
    public void RemoveOneGamePiece()
    {
        if (GameManager.Instance != null && isEnabled && !isDraggable && !isLocked && !GameManager.Instance.IsGameOver)
        {
            if (m_board != null && m_tileTarget != null && !m_board.isRefilling)
            {

                m_board.ClearTheTile(m_tileTarget.xIndex, m_tileTarget.yIndex);
                if (Board.boosterMade == true)
                {
                    Board.boosterMade = false;
                    boosterNo = Mathf.Clamp(--boosterNo, 0, 999);
                    if (boosterNo > 0)
                    {
                        boostertext.text = boosterNo.ToString();
                    }
                    else
                    {
                        buttonBuy.gameObject.SetActive(true);
                    }

                    LoadInfo.Save(boosterNo, boostertype);
                    --LoadInfo.booster1No;
                }
            }
        }    
    }
}

[Serializable]
class BoosterData
{
    public int numberOfBooster;
}

