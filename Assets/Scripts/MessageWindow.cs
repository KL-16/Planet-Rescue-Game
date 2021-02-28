using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Net;

// this is a UI component that can show a message, icon and button
[RequireComponent(typeof(RectXformMover))]
public class MessageWindow : MonoBehaviour 
{
	public Image messageImage;
	public Text messageText;
	public Text buttonText;

    public int lastLevelIdx = 216;

    public Button startBtn;
    Button startButton;

    public Image star1;
    public Image star2;
    public Image star3;

    public Image starBackground1;
    public Image starBackground2;
    public Image starBackground3;

    public ParticlePlayer star1FX;
    public ParticlePlayer star2FX;
    public ParticlePlayer star3FX;

    // sprite for losers
    public Sprite loseIcon;

    // sprite for winners
    public Sprite winIcon;

    public Text goalText;

    public Button mainMenuButton;
    Button mainMenubtn;

    public Button colorBombB;
    Button ColorBomb;
    public Sprite ColorBombPicked;
    public Sprite ColorBombIdle;
    Image colorImage;
    bool useColorBombBooster = false;
    public Text boosterColor;
    Text boosterColortext;

    public Button jumpingBombB;
    Button jumpingBomb;
    public Sprite JumpingBombPicked;
    public Sprite JumpingBombIdle;
    Image jumpingImage;
    bool useJumpingBombBooster = false;
    public Text boosterJumping;
    Text boosterJumpingtext;
    public static bool rewarded = false;


    public Button bigBombB;
    Button bigBomb;
    public Image bigImage;
    bool useBigBombBooster = false;
    public Text boosterBig;
    Text boosterBigtext;

    public int mainMenuIndex = 1;

    bool startButtonMoved = false;
    bool extraMovesUsed = false;

    //read environmental facts
    public Text factText;
    public Image factBck;

    //buy buttons
    public Button buyBtnColor;
    Button buyColor;
    public Button buyBtnJumping;
    Button buyJumping;
    public Button buyBtnBig;
    Button buyBig;

    //extra moves for add
    public Button watchAddButton;
    Button extraMovesButton;

    GameObject myCarrierObject;
    GameObject adsManager;

    private void Awake()
    {
        startButton = startBtn.GetComponent<Button>();

        ColorBomb = colorBombB.GetComponent<Button>();
        colorImage = colorBombB.GetComponent<Image>();

        jumpingBomb = jumpingBombB.GetComponent<Button>();
        jumpingImage = jumpingBombB.GetComponent<Image>();

        bigBomb = bigBombB.GetComponent<Button>();
        bigImage.gameObject.SetActive(false);

        extraMovesButton = watchAddButton.GetComponent<Button>();

        buyColor = buyBtnColor.GetComponent<Button>();
        buyJumping = buyBtnJumping.GetComponent<Button>();
        buyBig = buyBtnBig.GetComponent<Button>();
    }

    private void Start()
    {
        mainMenubtn = mainMenuButton.GetComponent<Button>();
        mainMenubtn.onClick.AddListener(GoToMenu);


        ColorBomb.onClick.AddListener(ToggleColorBooster);
        useColorBombBooster = false;
        boosterColortext = boosterColor.GetComponent<Text>();
        if(LoadInfo.booster3No > 0)
        {
            boosterColortext.text = LoadInfo.booster3No.ToString();
            buyColor.gameObject.SetActive(false);
        }
        else
        {
            buyColor.gameObject.SetActive(true);
        }
        
        jumpingBomb.onClick.AddListener(ToggleJumpingBooster);
        useJumpingBombBooster = false;
        boosterJumpingtext = boosterJumping.GetComponent<Text>();
        if(LoadInfo.booster6No > 0)
        {
            boosterJumpingtext.text = LoadInfo.booster6No.ToString();
            buyJumping.gameObject.SetActive(false);
        }
        else
        {
            buyJumping.gameObject.SetActive(true);
        }
        
        bigBomb.onClick.AddListener(ToggleBigBooster);
        useBigBombBooster = false;
        boosterBigtext = boosterBig.GetComponent<Text>();
        if(LoadInfo.booster7No > 0)
        {
            boosterBigtext.text = LoadInfo.booster7No.ToString();
            buyBig.gameObject.SetActive(false);
        }
        else
        {
            buyBig.gameObject.SetActive(true);
        }

        extraMovesButton.gameObject.SetActive(false);
        extraMovesButton.onClick.AddListener(AddMoves);

        startButtonMoved = false;
        extraMovesUsed = false;
        rewarded = false;

        GameManager.useColorBooster = false;
        GameManager.useJumpingBooster = false;
        GameManager.useBigBooster = false;


        star1FX.gameObject.SetActive(false);
        star2FX.gameObject.SetActive(false);
        star3FX.gameObject.SetActive(false);
        factText.gameObject.SetActive(true);
        factBck.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        myCarrierObject = GameObject.Find("LoadedInfoHolder");
        adsManager = GameObject.Find("Add Manager");

        if (myCarrierObject != null)
        {
            int randomFact = UnityEngine.Random.Range(0, myCarrierObject.GetComponent<StaticVariableHolder>().lines.Count);
            factText.text = myCarrierObject.GetComponent<StaticVariableHolder>().lines[randomFact];
        }

    }

    public void UpdatePreGameBoosters()
    {
        boosterColortext.text = LoadInfo.booster3No.ToString();
        boosterJumpingtext.text = LoadInfo.booster6No.ToString();
        boosterBigtext.text = LoadInfo.booster7No.ToString();
        buyColor.gameObject.SetActive(false);
        buyJumping.gameObject.SetActive(false);
        buyBig.gameObject.SetActive(false);
    }

    public void ShowMessage(Sprite sprite = null, string message = "", string buttonMsg = "start", bool imageActive = true, bool lastLevel = false)
	{
		if (messageImage != null) 
		{
            if (imageActive)
            {
                if (!lastLevel)
                {
                    messageImage.enabled = true;
                    messageImage.sprite = sprite;
                    if (startButtonMoved == false)
                    {
                        Vector3 pos = startButton.transform.position;
                        pos.y += 100f;
                        startButton.transform.position = pos;
                        startButtonMoved = true;
                    }
                }
                else
                {
                    startButton.gameObject.SetActive(false);
                }
                
            }

            else
            {
                messageImage.enabled = false;
                if(startButton == null)
                {
                    //Debug.Log("Button is null");
                }              
                    Vector3 pos = startButton.transform.position;
                    pos.y -= 100f;
                    startButton.transform.position = pos;                              
            }
			
		}

        if (messageText != null)
        {
            messageText.text = message;
        }
			
        if (buttonText != null)
        {
            buttonText.text = buttonMsg;
        }
	}

    public void ShowScoreMessage()
    {
        ShowMessage(null, null, "START", false);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWooshSound();
        }
    }

    public void ShowWinMessage()
    {
        if (SceneManager.GetActiveScene().buildIndex == lastLevelIdx)
        {
            ShowMessage(winIcon, "level complete", "NEXT", true, true);
        }
        else
        {
            ShowMessage(winIcon, "level complete", "NEXT");
        }
        
        mainMenubtn.gameObject.SetActive(true);
        colorBombB.gameObject.SetActive(false);
        jumpingBombB.gameObject.SetActive(false);
        bigBombB.gameObject.SetActive(false);
        factText.gameObject.SetActive(false);
        factBck.gameObject.SetActive(false);
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWooshSound();
        }
    }

    public void ShowLoseMessage()
    {
        ShowMessage(loseIcon, "level failed", "TRY AGAIN");
        mainMenubtn.gameObject.SetActive(true);
        colorBombB.gameObject.SetActive(false);
        jumpingBombB.gameObject.SetActive(false);
        bigBombB.gameObject.SetActive(false);
        factText.gameObject.SetActive(false);
        factBck.gameObject.SetActive(false);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWooshSound();
        }
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            //No connection
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found
        }
        else
        {
            //success
            if (extraMovesUsed == false)
            {
                extraMovesButton.gameObject.SetActive(true);
            }
        }
           
    }

    public void ShowGoalCaption(string caption = "", int offsetX = 0, int offsetY = 0)
    {
        if (goalText != null)
        {
            goalText.text = caption;
            RectTransform rectXform = goalText.GetComponent<RectTransform>();
            rectXform.anchoredPosition += new Vector2(offsetX, offsetY);
        }
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void DisplayStars(int numberOfStars)
    {
       StartCoroutine(DisplayStarsRoutine(numberOfStars));
    }

    IEnumerator DisplayStarsRoutine(int numberOfStars)
    {
        if (numberOfStars > 0)
        {
            starBackground1.gameObject.SetActive(true);
            starBackground2.gameObject.SetActive(true);
            starBackground3.gameObject.SetActive(true);
            
            
            yield return new WaitForSeconds(1f);

            switch (numberOfStars)
            {
                case 1:
                    star1FX.gameObject.SetActive(true);
                    star1.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar1Sound();
                    }

                    StarCollectedFX(1);
                    break;
                case 2:
                    star1FX.gameObject.SetActive(true);
                    star1.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar1Sound();
                    }
                    StarCollectedFX(1);
                    yield return new WaitForSeconds(0.3f);
                    star2FX.gameObject.SetActive(true);
                    star2.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar2Sound();
                    }
                    StarCollectedFX(2);
                    break;
                case 3:
                    star1FX.gameObject.SetActive(true);
                    star1.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar1Sound();
                    }
                    StarCollectedFX(1);
                    yield return new WaitForSeconds(0.3f);
                    star2FX.gameObject.SetActive(true);
                    star2.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar2Sound();
                    }
                    StarCollectedFX(2);
                    yield return new WaitForSeconds(0.3f);
                    star3FX.gameObject.SetActive(true);
                    star3.gameObject.SetActive(true);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayStar3Sound();
                    }
                    StarCollectedFX(3);
                    break;
            }
        }
    }

    void StarCollectedFX(int star)
    {
        switch (star)
        {
            case 1:
                if (star1FX != null)
                {
                    star1FX.Play();
                    
                }
                break;
            case 2:
                if (star2FX != null)
                {
                    star2FX.Play();
                }
                break;
            case 3:
                if (star3FX != null)
                {
                    star3FX.Play();
                }
                break;
        }
    }

    private void ToggleColorBooster()
    {
        if(LoadInfo.booster3No > 0)
        {
            useColorBombBooster = !useColorBombBooster;
            UseColorBombBooster(useColorBombBooster);
        }      
    }

    private void UseColorBombBooster(bool state)
    {
        if (state)
        {
            GameManager.useColorBooster = true;
            colorImage.sprite = ColorBombPicked;
        }
        else
        {
            GameManager.useColorBooster = false;
            colorImage.sprite = ColorBombIdle;
        }             
    }

    private void ToggleJumpingBooster()
    {
        if(LoadInfo.booster6No > 0)
        {
            useJumpingBombBooster = !useJumpingBombBooster;
            UseJumpingBombBooster(useJumpingBombBooster);
        }      
    }

    private void UseJumpingBombBooster(bool state)
    {
        if (state)
        {
            GameManager.useJumpingBooster = true;
            jumpingImage.sprite = JumpingBombPicked;
        }
        else
        {
            GameManager.useJumpingBooster = false;
            jumpingImage.sprite = JumpingBombIdle;
        }
    }

    private void ToggleBigBooster()
    {
        if(LoadInfo.booster7No > 0)
        {
            useBigBombBooster = !useBigBombBooster;
            UseBigBombBooster(useBigBombBooster);
        }      
    }

    private void UseBigBombBooster(bool state)
    {
        if (state)
        {
            GameManager.useBigBooster = true;
            bigImage.gameObject.SetActive(true);
        }
        else
        {
            GameManager.useBigBooster = false;
            bigImage.gameObject.SetActive(false);
        }
    }

    private void AddMoves()
    {
        if(GameManager.Instance != null && adsManager != null)
        {
            StartCoroutine(HideButton());
            rewarded = true;
            adsManager.GetComponent<AdsManager>().ShowRewardedAdd();
        }
    }

    IEnumerator HideButton()
    {
        extraMovesUsed = true;
        yield return new WaitForSeconds(1f);
        extraMovesButton.gameObject.SetActive(false);
    }

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

}
