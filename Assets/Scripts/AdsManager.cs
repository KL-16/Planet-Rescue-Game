using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener 
{
    string placement = "rewardedVideo";
    string gameId = "3632969";
    string placementId = "banner";
    //bool testMode = true;
    private static bool isInitialized = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized)
        {
            
                isInitialized = true;
                Advertisement.AddListener(this);
                Advertisement.Initialize(gameId, false);
        }

        if (LoadInfo.adsRemoved == false)
        {
            StartCoroutine(ShowBannerAdd());
        }

        //Advertisement.Initialize(gameId, true);

    }

    public void ShowSkipAdd()
    {
        StartCoroutine(ShowSkipAddRoutine());
    }

    public void ShowRewardedAdd()
    {
        StartCoroutine(ShowRewardedAddRoutine());
    }

    IEnumerator ShowSkipAddRoutine()
    {
        while (!Advertisement.IsReady())
        {
            yield return null;
        }
           
        Advertisement.Show();
    }

    IEnumerator ShowRewardedAddRoutine()
    {
        while (!Advertisement.IsReady(placement))
        {
            yield return null;
        }
           
        Advertisement.Show(placement);
    }

    public void OnUnityAdsReady(string placement)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placement)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placement, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {if(GameManager.Instance != null && MessageWindow.rewarded == true)
            {
                MessageWindow.rewarded = false;
                //Debug.Log("moves added after ad");
                GameManager.Instance.AddMoves(5);
                GameManager.Instance.UpdateMovesText();
                GameManager.Instance.IsGameOver = false;
                GameManager.Instance.m_isWinner = false;
                if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
                {
                    UIManager.Instance.screenFader.FadeOff();
                }
                StartCoroutine(GameManager.Instance.ExtraGameLoop());             
            }
            
        }
    }

    public IEnumerator ShowBannerAdd()
    {

        while (!Advertisement.IsReady())
            yield return null;

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
    }

    public void HideBannerAdd()
    {
        Advertisement.Banner.Hide();
    }

}
