using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using static System.Math;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class MenuController : MonoBehaviour
{
    private int highscore;
    private bool infoIsOpen = false;
    private bool tutorialIsOpen = false;
    private bool textUp = false;
    private bool count = false;
    private int coins = 0;
    private bool textDown = false;
    private float animWait = 0;
    private float playerXMove = 0;
    private float hsCounter = 0;
    private Vector2 canvasSize;
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;
    private float dragDistance;
    private int delta;
    private int easterEggCounter = 0;
    public Sprite easterEggImage;
    public Sprite tutorialSprite;
    public Sprite quitSprite;
    public Sprite scoreSprite;
    private String quitText = "Quit";
    private String moreText = "More";

    public AudioSource audioSource;
    public AudioClip select;
    public AudioClip back;

    private GameObject highscoreTextGO;
    private GameObject tutorialInfoText;
    private GameObject infoButtonGO;
    private GameObject tapGroup;
    private GameObject tapTextGO;
    private GameObject soundButtonGO;
    private Text highscoreText;
    private Text infoButtonText;
    private Text coinText;
    private Canvas canvas;
    public Sprite soundOn;
    public Sprite soundOFF;
    private Image soundImage;
    private Image infoImageC;
    private Image scoreImageC;
    private Button soundButton;
    private GameObject tutorialManager;
    private GameObject tutorialYesGO;
    private GameObject tutorialNo;
    private GameObject scoreImage;
    private GameObject playerGO;
    private GameObject coinGO;
    private GameObject coinsGO;
    private GameObject playAreaGO;
    private GameObject shopGO;
    private GameObject supportGO;
    private GameObject tutorialBGGO;
    private GameObject tutorialMessGO;
    private GameObject shopScrollGO;
    private Image tutorialBG;
    

    void Start()
    {
        highscoreTextGO = GameObject.FindGameObjectWithTag("Highscore");
        supportGO = GameObject.FindGameObjectWithTag("Support");
        coinGO = GameObject.FindGameObjectWithTag("Coin");
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playAreaGO = GameObject.FindGameObjectWithTag("PlayArea");
        coinsGO = GameObject.FindGameObjectWithTag("Coin");
        shopGO = GameObject.FindGameObjectWithTag("Shop");
        tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager");
        tutorialYesGO = GameObject.FindGameObjectWithTag("TutorialYes");
        tutorialNo = GameObject.FindGameObjectWithTag("TutorialNo");
        scoreImage = GameObject.FindGameObjectWithTag("ScoreImage");
        tapGroup = GameObject.FindGameObjectWithTag("TapGroup");
        tutorialInfoText = GameObject.FindGameObjectWithTag("TutorialInfoText");
        infoButtonGO = GameObject.FindGameObjectWithTag("InfoButton");
        tapTextGO = GameObject.FindGameObjectWithTag("TapText");
        shopScrollGO = GameObject.FindGameObjectWithTag("ShopScroll");
        soundButtonGO = GameObject.FindGameObjectWithTag("SoundB");
        tutorialBGGO = GameObject.FindGameObjectWithTag("TutorialBG");
        tutorialMessGO = GameObject.FindGameObjectWithTag("TutorialMess");

        //swipe size
        dragDistance = (Screen.width + Screen.height) * 0.1f / 2f;
        canvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        highscoreText = highscoreTextGO.GetComponent<UnityEngine.UI.Text>();
        infoButtonText = infoButtonGO.GetComponentInChildren<UnityEngine.UI.Text>();
        coinText = coinGO.GetComponentInChildren<UnityEngine.UI.Text>();
        infoImageC = infoButtonGO.GetComponentInChildren<UnityEngine.UI.Image>();
        Button infoButton = infoButtonGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialButton = highscoreTextGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialYesButton = tutorialYesGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialNoButton = tutorialNo.GetComponent<UnityEngine.UI.Button>();
        Button supportButton = supportGO.GetComponent<UnityEngine.UI.Button>();
        Button shopButton = shopGO.GetComponent<UnityEngine.UI.Button>();
        Button playButton = playAreaGO.GetComponent<UnityEngine.UI.Button>();
        soundButton = soundButtonGO.GetComponent<UnityEngine.UI.Button>();
        soundImage = soundButtonGO.GetComponent<UnityEngine.UI.Image>();
        scoreImageC = scoreImage.GetComponent<UnityEngine.UI.Image>();
        tutorialBG = tutorialBGGO.GetComponent<UnityEngine.UI.Image>();

        tutorialManager.SetActive(false);
        shopGO.SetActive(false);
        shopScrollGO.SetActive(false);
        coinsGO.SetActive(false);
        shopButton.onClick.AddListener(ShopClick);
        infoButton.onClick.AddListener(TaskOnInfoClick);
        tutorialYesButton.onClick.AddListener(YesTut);
        supportButton.onClick.AddListener(Support);
        playButton.onClick.AddListener(Play);
        tutorialNoButton.onClick.AddListener(TaskOnTutorialNoClick);
        canvasSize = new Vector2(canvas.GetComponent<RectTransform>().rect.width, canvas.GetComponent<RectTransform>().rect.height);
        soundButton.onClick.AddListener(SoundButton);
        tutorialButton.onClick.AddListener(TaskOnTutorialClick);
        coins = PlayerPrefs.GetInt("coins", 0);
        coinText.text = coins.ToString();

        //zmiana jezyka
        if (Application.systemLanguage == SystemLanguage.Polish)
        {
            moreText = "Więcej";
            infoButtonText.text = moreText;
            quitText = "Wyjdź";
            shopGO.GetComponentInChildren<UnityEngine.UI.Text>().text = "Sklep";
            tapTextGO.GetComponentInChildren<UnityEngine.UI.Text>().text = "kliknij aby zagrać";
            supportGO.GetComponent<UnityEngine.UI.Text>().text = "Wesprzyj nas";
            tutorialInfoText.GetComponentInChildren<UnityEngine.UI.Text>().text = "Otwórz poradnik i dowiedz się jak kontrolować statek.";
            tutorialMessGO.GetComponentInChildren<UnityEngine.UI.Text>().text = "Otwórz poradnik";
            tutorialYesGO.transform.GetChild(0).gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "TAK";
            tutorialNo.transform.GetChild(0).gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "NIE";
        }

        if (PlayerPrefs.GetInt("highscoreChanged", 0) == 0)
        {
            highscore = PlayerPrefs.GetInt("highscore", 0);
            highscoreText.text = highscore.ToString();
        }
        //highscore passed
        else
        {
            hsCounter = PlayerPrefs.GetInt("lastHighscore", 0);
            highscore = PlayerPrefs.GetInt("lastHighscore", 0);
            highscoreText.text = highscore.ToString();
        }

        if (PlayerPrefs.GetInt("canPlayMusic", 1) == 1)
        {
            soundImage.sprite = soundOn;
        }
        else
        {
            soundImage.sprite = soundOFF;
        }
        animWait = Time.time;
        delta = PlayerPrefs.GetInt("highscore", 0) - highscore;

        //first open
        if (PlayerPrefs.GetInt("hasPlayed", 0) == 0)
        {
            PlayerPrefs.SetInt("hasPlayed", 1);
            playerXMove = -120.0f;
            highscoreText.text = "Tutorial";
            infoButtonText.text = quitText;
            scoreImageC.sprite = tutorialSprite;
            infoButtonText.text = quitText;
            infoImageC.sprite = quitSprite;

            tapGroup.SetActive(false);
            supportGO.SetActive(false);
            shopGO.SetActive(true);
            coinsGO.SetActive(true);
            soundButtonGO.SetActive(false);
            infoIsOpen = true;
            tutorialManager.SetActive(true);
            tutorialIsOpen = true;
        }

        #if UNITY_IOS
            // check with iOS to see if the user has accepted or declined tracking
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Debug.Log("Unity iOS Support: Displaying popup.");
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
        #else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
        #endif
    }


    void Update()
    {
        float previousPosition = playerGO.transform.position.x;
        if (playerXMove > 10)
        {
            playerGO.transform.Translate(Vector3.left * 5);
            playerXMove -= (previousPosition - playerGO.transform.position.x);
            
        }
        else if (playerXMove < -10)
        {
            playerGO.transform.Translate(Vector3.right * 5);
            playerXMove += (playerGO.transform.position.x - previousPosition);
        }

        //highscore animation
        if (PlayerPrefs.GetInt("highscoreChanged", 0) == 1)
        {
            if (!textUp && Time.time - animWait > 1)
            {
                if (highscoreText.fontSize < 28)
                {
                    highscoreText.fontSize += 1;
                } 
                else
                {
                    animWait = Time.time;
                    textUp = true;
                }
            }

            if (!count && textUp && Time.time - animWait > 1)
            {
                if (highscore < PlayerPrefs.GetInt("highscore", 0))
                {
                    hsCounter += (delta * 0.24f + 15.6f) * Time.deltaTime;
                    if (hsCounter > PlayerPrefs.GetInt("highscore", 0))
                    {
                        hsCounter = PlayerPrefs.GetInt("highscore", 0);
                    }
                    highscore = Convert.ToInt32(hsCounter);
                    highscoreText.text = highscore.ToString();
                } 
                else
                {
                    animWait = Time.time;
                    count = true;
                }
            }

            if (!textDown && count && textUp && Time.time - animWait > 1)
            {
                if (highscoreText.fontSize > 22)
                {
                    highscoreText.fontSize -= 1;
                }
                else
                {
                    textDown = true;
                }
            }

            if (textUp && count && textDown)
            {
                PlayerPrefs.SetInt("highscoreChanged", 0);
            }
        }
    }

    public void Play()
    {
        if (!infoIsOpen)
        {
            PlayerPrefs.SetInt("addScore", 0);
            SceneManager.LoadScene("Game");
        }
    }

    void TaskOnInfoClick()
    {
        if (!infoIsOpen)
        {
            audioSource.clip = select;
            audioSource.Play();
            playerXMove = -120.0f;
            highscoreText.text = "Tutorial";
            infoButtonText.text = quitText;
            scoreImageC.sprite = tutorialSprite;
            infoButtonText.text = quitText;
            infoImageC.sprite = quitSprite;

            tapGroup.SetActive(false);
            supportGO.SetActive(false);
            shopGO.SetActive(true);
            coinsGO.SetActive(true);
            soundButtonGO.SetActive(false);
            infoIsOpen = true;
        }
        else
        {
            audioSource.clip = back;
            audioSource.Play();
            if (!shopGO.activeSelf)
            {
                shopScrollGO.SetActive(false);
                shopGO.SetActive(true);
            }
            else
            {
                playerXMove = 120.0f;
                highscoreText.text = highscore.ToString();
                scoreImageC.sprite = scoreSprite;
                infoButtonText.text = moreText;
                infoImageC.sprite = tutorialSprite;
                soundButtonGO.SetActive(true);
                tapGroup.SetActive(true);
                supportGO.SetActive(true);
                shopGO.SetActive(false);
                coinsGO.SetActive(false);
                tutorialManager.SetActive(false);
                infoIsOpen = false;
                tutorialIsOpen = false;
            }
        }
    }

    void TaskOnTutorialClick()
    {
        easterEggCounter++;
        if (easterEggCounter == 10)
        {
            tutorialBG.sprite = easterEggImage;
        }
        if (!tutorialIsOpen && infoIsOpen && !shopScrollGO.activeSelf)
        {
            audioSource.clip = select;
            audioSource.Play();
            tutorialManager.SetActive(true);
            tutorialIsOpen = true;
        }
    }

    void ShopClick()
    {
        if (!tutorialIsOpen)
        {
            audioSource.clip = select;
            audioSource.Play();
            shopGO.SetActive(false);
            shopScrollGO.SetActive(true);
        }   
    }

    void TaskOnTutorialNoClick()
    {
        if (tutorialIsOpen)
        {
            audioSource.clip = back;
            audioSource.Play();
            tutorialManager.SetActive(false);
            tutorialIsOpen = false;
        }
    }

    void Support()
    {
        DateTime campaignEnd = new DateTime(2021, 9, 25);
        DateTime now = System.DateTime.Now;

        if (now.Date < campaignEnd.Date)
        {
            Application.OpenURL("https://bit.ly/gravity-kickstarter");
        }
        else
        {
            Application.OpenURL("https://bit.ly/3C7fqcU");
        }
    }


    void YesTut()
    {
        audioSource.clip = select;
        audioSource.Play();
        PlayerPrefs.SetInt("onlyPTut", 0);
        SceneManager.LoadScene("Tutorial");
    }


    void SoundButton()
    {
        if (PlayerPrefs.GetInt("canPlayMusic", 1) == 1)
        {
            audioSource.clip = back;
            audioSource.Play();
            PlayerPrefs.SetInt("canPlayMusic", 0);
            soundImage.sprite = soundOFF;
        }
        else
        {
            audioSource.clip = select;
            audioSource.Play();
            PlayerPrefs.SetInt("canPlayMusic", 1);
            soundImage.sprite = soundOn;
        }
    }
}