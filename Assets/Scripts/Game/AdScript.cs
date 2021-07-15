using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;


//form unity tutorial
[RequireComponent(typeof(Button))]
public class AdScript : MonoBehaviour, IUnityAdsListener
{
    private bool testMode = false;

    #if UNITY_IOS
    private string gameId = "3510681";
    #elif UNITY_ANDROID
    private string gameId = "3510680";
    #endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    private GameObject netInfo;
    private string loadingText = "Loading ad...";

    void Start()
    {
        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);

        netInfo = GameObject.FindGameObjectWithTag("NetInfo");
        netInfo.SetActive(false);
        myButton = GetComponent<Button>();

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton)
        {
            myButton.onClick.AddListener(ShowRewardedVideo);
        }

        if (Application.systemLanguage == SystemLanguage.Polish)
        {
            loadingText = "Ładuję reklamę...";
        }

        //tylko raz wyswietlamy reklame
        if (PlayerPrefs.GetInt("ADcounter", 0) == 1)
        {
            this.gameObject.SetActive(false);
        }
        //czy mamy internet?
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            this.gameObject.SetActive(false);
            netInfo.SetActive(true);
        }

        StartCoroutine(CheckAd());
    }

    IEnumerator CheckAd ()
    {
        // If not ready and has internet acces
        while (!Advertisement.IsReady(myPlacementId) && Application.internetReachability != NetworkReachability.NotReachable)
        {
            netInfo.SetActive(true);
            netInfo.GetComponent<UnityEngine.UI.Text>().text = loadingText;
            yield return new WaitForSeconds(1.0f);
        }
        // If ready
        if (Advertisement.IsReady(myPlacementId))
        {
            netInfo.SetActive(false);
        }
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(myPlacementId))
        {
            Advertisement.Show(myPlacementId);
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button:
        /*
        if (placementId == myPlacementId && myButton != null)
        {
            myButton.interactable = true;
        }
        */
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            PlayerPrefs.SetInt("ADdisplayed", 1);
            //tylko raz mozemy wyswietlic reklame
            PlayerPrefs.SetInt("ADcounter", 1);
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}