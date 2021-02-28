using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{

    string gameId = "3632969";
    string placementId = "banner";
    bool testMode = true;

    // Start is called before the first frame update

    private void Start()
    {
        if (LoadInfo.adsRemoved == false)
        {
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerAdd());
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
