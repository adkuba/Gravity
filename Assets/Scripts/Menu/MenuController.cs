using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using static System.Math;

public class MenuController : MonoBehaviour
{
    private int highscore;
    private bool infoIsOpen = false;
    private bool tutorialIsOpen = false;
    private bool textUp = false;
    private bool count = false;
    private bool textDown = false;
    private float animWait = 0;
    private bool playerMainPosition = true;
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

    private GameObject highscoreTextGO;
    private GameObject tutorialInfoText;
    private GameObject infoButtonGO;
    private GameObject tapGroup;
    private GameObject tapTextGO;
    private GameObject soundButtonGO;
    private Text highscoreText;
    private Text infoButtonText;
    private Canvas canvas;
    public Sprite soundOn;
    public Sprite soundOFF;
    private Image soundImage;
    private Image infoImageC;
    private Image scoreImageC;
    private Button soundButton;
    private RectTransform soundButtonRect;
    private GameObject tutorialManager;
    private GameObject tutorialYesGO;
    private GameObject tutorialNo;
    private GameObject scoreImage;
    private GameObject playerGO;
    private GameObject coinsGO;
    private GameObject shopGO;
    private GameObject tutorialBGGO;
    private GameObject tutorialMessGO;
    private Image tutorialBG;
    

    void Start()
    {
        highscoreTextGO = GameObject.FindGameObjectWithTag("Highscore");
        playerGO = GameObject.FindGameObjectWithTag("Player");
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
        soundButtonGO = GameObject.FindGameObjectWithTag("SoundB");
        tutorialBGGO = GameObject.FindGameObjectWithTag("TutorialBG");
        tutorialMessGO = GameObject.FindGameObjectWithTag("TutorialMess");

        //swipe size
        dragDistance = (Screen.width + Screen.height) * 0.1f / 2f;
        canvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        highscoreText = highscoreTextGO.GetComponent<UnityEngine.UI.Text>();
        infoButtonText = infoButtonGO.GetComponentInChildren<UnityEngine.UI.Text>();
        infoImageC = infoButtonGO.GetComponentInChildren<UnityEngine.UI.Image>();
        Button infoButton = infoButtonGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialButton = highscoreTextGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialYesButton = tutorialYesGO.GetComponent<UnityEngine.UI.Button>();
        Button tutorialNoButton = tutorialNo.GetComponent<UnityEngine.UI.Button>();
        soundButton = soundButtonGO.GetComponent<UnityEngine.UI.Button>();
        soundImage = soundButtonGO.GetComponent<UnityEngine.UI.Image>();
        scoreImageC = scoreImage.GetComponent<UnityEngine.UI.Image>();
        tutorialBG = tutorialBGGO.GetComponent<UnityEngine.UI.Image>();
        soundButtonRect = soundButtonGO.GetComponent<RectTransform>();

        tutorialManager.SetActive(false);
        shopGO.SetActive(false);
        coinsGO.SetActive(false);
        infoButton.onClick.AddListener(TaskOnInfoClick);
        tutorialYesButton.onClick.AddListener(YesTut);
        tutorialNoButton.onClick.AddListener(TaskOnTutorialNoClick);
        canvasSize = new Vector2(canvas.GetComponent<RectTransform>().rect.width, canvas.GetComponent<RectTransform>().rect.height);
        soundButton.onClick.AddListener(SoundButton);
        tutorialButton.onClick.AddListener(TaskOnTutorialClick);

        //zmiana jezyka
        if (Application.systemLanguage == SystemLanguage.Polish)
        {
            moreText = "Więcej";
            infoButtonText.text = moreText;
            quitText = "Wyjdź";
            shopGO.GetComponentInChildren<UnityEngine.UI.Text>().text = "Sklep";
            tapTextGO.GetComponentInChildren<UnityEngine.UI.Text>().text = "kliknij aby zagrać";
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
        if (PlayerPrefs.GetInt("hasPlayed1_1", 0) == 0)
        {
            PlayerPrefs.SetInt("hasPlayed1_1", 1);
            TaskOnInfoClick();
        }
    }


    void Update()
    {
        if (!playerMainPosition)
        {
            if (playerGO.transform.position.x < 100)
            {
                playerGO.transform.Translate(Vector3.right * 5);
            }
            
        }
        else
        {
            if (playerGO.transform.position.x > 0)
            {
                playerGO.transform.Translate(Vector3.left * 5);
            }
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

        if (!infoIsOpen)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    firstTouchPos = touch.position;
                    lastTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    lastTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    lastTouchPos = touch.position;

                    //swipe detection
                    if (Math.Abs(lastTouchPos.x - firstTouchPos.x) < dragDistance && Math.Abs(lastTouchPos.y - firstTouchPos.y) < dragDistance)
                    {
                        //not clicking buttons
                        //touch is in pixels!!!
                        float percent = (canvasSize.y + soundButtonRect.anchoredPosition.y - 3 * (soundButtonRect.rect.height / 2) / 2) / canvasSize.y;
                        float pixelValue = Screen.height * percent;
                        if (lastTouchPos.y < pixelValue)
                        {
                            PlayerPrefs.SetInt("addScore", 0);
                            SceneManager.LoadScene("Game");
                        }
                    }
                }
            }
        }
    }

    void TaskOnInfoClick()
    {
        if (!infoIsOpen)
        {
            playerMainPosition = false;
            highscoreText.text = "Tutorial";
            infoButtonText.text = quitText;
            scoreImageC.sprite = tutorialSprite;
            infoButtonText.text = quitText;
            infoImageC.sprite = quitSprite;

            tapGroup.SetActive(false);
            shopGO.SetActive(true);
            coinsGO.SetActive(true);
            soundButtonGO.SetActive(false);
            infoIsOpen = true;
        }
        else{
            playerMainPosition = true;
            highscoreText.text = highscore.ToString();
            scoreImageC.sprite = scoreSprite;
            infoButtonText.text = moreText;
            infoImageC.sprite = tutorialSprite;
            soundButtonGO.SetActive(true);
            tapGroup.SetActive(true);
            shopGO.SetActive(false);
            coinsGO.SetActive(false);
            tutorialManager.SetActive(false);
            infoIsOpen = false;
            tutorialIsOpen = false;
        }
    }

    void TaskOnTutorialClick()
    {
        easterEggCounter++;
        if (easterEggCounter == 10)
        {
            tutorialBG.sprite = easterEggImage;
        }
        if (!tutorialIsOpen && infoIsOpen)
        {
            tutorialManager.SetActive(true);
            tutorialIsOpen = true;
        }
    }

    void TaskOnTutorialNoClick()
    {
        if (tutorialIsOpen)
        {
            tutorialManager.SetActive(false);
            tutorialIsOpen = false;
        }
    }


    void YesTut()
    {
        PlayerPrefs.SetInt("onlyPTut", 0);
        SceneManager.LoadScene("Tutorial");
    }


    void SoundButton()
    {
        if (PlayerPrefs.GetInt("canPlayMusic", 1) == 1)
        {
            PlayerPrefs.SetInt("canPlayMusic", 0);
            soundImage.sprite = soundOFF;
        }
        else
        {
            PlayerPrefs.SetInt("canPlayMusic", 1);
            soundImage.sprite = soundOn;
        }
    }
}