using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    [Header("iOS game ID")]
    public string gameId = "4085452";
#elif UNITY_ANDROID
    [Header("Android game ID")]
    public string gameId = "4085453";
#endif

    [Header("Interstitial ad name")]
    public string adsName_1 = "video";

    [Header("Banner ad name")]
    public string adsName_2 = "banner";

    [Header("Ads are enabled or disabled throughout the game.")]
    [Header("ADS SETTINGS")]
    public bool ads = true;

    [Header("Interstitial are enabled or disabled throughout the game.")]
    public bool interstitial = true;

    [Header("Banner are enabled or disabled throughout the game.")]
    public bool banner  = true;

    [Header("Advertising after passing the level, that is, winning.")]
    [Header("INTERSTITIAL SETTINGS")]
    public bool everyVictory;

    [Header("Advertising after every loss. You can enable both options.")]
    public bool everyLose;

    [Header("At what level to start showing ads. If level now > levelId - ads showing")]
    public int levelId;

    void Start()
    {

        if (PlayerPrefs.GetInt("NoAds") == 1) //If you have purchased disabling advertising, disable it.
        {
            ads = false;
        }

        if (ads) //If advertising is enabled and the banner is enabled, we launch the banner ad
        {
            Advertisement.Initialize(gameId, true); //Advertising initialization 
            Advertisement.AddListener(this); //Add listener

            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER); //Set banner position
            if (banner) //If banner on
            {
                StartCoroutine(ShowBannerWhenInitialized()); //Show banner
            }
        }
    
    }

    IEnumerator ShowBannerWhenInitialized() //Banner coroutine
    {
        while (!Advertisement.isInitialized) //If ads not initialized
        {
            yield return new WaitForSeconds(0.5f); //Wait 0.5 second and try again
        }
        Advertisement.Banner.Show(adsName_2);
    }

    public void ShowInterstitialAd() //Method for calling video advertisements
    {
        if (ads == true && interstitial == true) //If advertising is enabled and interstitial is enabled, launch the ad if it is ready
        {
            if (Advertisement.IsReady(adsName_1)) //If ad ready to show
            {
                Advertisement.Show(adsName_1); //Show ad
                Advertisement.Banner.Hide();//Turn off the banner
            }
            else
            {
                Debug.Log("Interstitial ad not ready at the moment! Please try again later!"); 
            }
        }
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide(); //Method so that you can turn off the banner from another script
    }

    public void NoAds_PurchaseComplete() //If the payment operation was successful, we disable advertising.
    {
        ads = false; //Ads turn off
        PlayerPrefs.SetInt("NoAds", 1); //Save data

        Advertisement.Banner.Hide(); //Hide banner
    }

    public void NoAds_PurchaseFailed() //If the operation fails, you can paste your code.
    {
        //Add here you code
    }

    public void OnUnityAdsDidFinish(string interstitial, ShowResult showResult)
    {
        
        if (showResult == ShowResult.Finished)
        {
            if (banner) //After the end of the advertisement, no matter what, we turn on the banner.
            {
                StartCoroutine(ShowBannerWhenInitialized()); //Open banner
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            if (banner) //After the end of the advertisement, no matter what, we turn on the banner.
            {
                StartCoroutine(ShowBannerWhenInitialized());  //Open banner 
            }
        }
        else if (showResult == ShowResult.Failed)
        {
            if (banner) //After the end of the advertisement, no matter what, we turn on the banner.
            {
                StartCoroutine(ShowBannerWhenInitialized());  //Open banner
            }
        }
    }
    public void OnUnityAdsReady(string interstitial)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (interstitial == adsName_1)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string interstitial)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
