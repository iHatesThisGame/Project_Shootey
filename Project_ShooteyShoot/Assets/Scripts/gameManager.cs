using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public playerController playerController;
    public GameObject playerSpawnPos;

    [Header("----- UI -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject checkpointPopup;
    public Image playerHPBar;
    public Image playerOvershieldBar;
    public GameObject playerFlashUI;
    public GameObject interactPrompt;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI killGoalText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI ammoMaxText;
    public TextMeshProUGUI ammoCurText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI winMessageText;
    public TextMeshProUGUI loseMessageText;

    [Header("----- Objectives -----")]
    public bool elimination;
    public GameObject enemiesRemainingLabel;
    public int enemiesRemaining;
    public bool survival;
    public GameObject killCountLabel;
    [Range(1, 1000)] [SerializeField] int killGoal = 5;
    public int killCount;
    public bool flagCapture;
    public bool courseStarted;
    public GameObject timerLabel;
    public bool isCaptured;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPauseSound;
    [Range(0, 1)][SerializeField] float audPauseSoundVol;

    public bool isPaused;
    float timescaleOrig;
    public Vector3 playerScaleOrig;
    public float currentTime;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        playerScaleOrig = player.transform.localScale;

        if (elimination)
        {
            enemiesRemainingLabel.SetActive(true);
        }

        if (survival)
        {
            killGoalText.text = killGoal.ToString("F0");
            killCountLabel.SetActive(true);
        }

        playerScoreText.text = scoreKeeper.playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (courseStarted)
            timer();

        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            aud.PlayOneShot(audPauseSound, audPauseSoundVol);
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }

        if (isCaptured)
        {
            winMessageText.text = "Flag Captured";
            StartCoroutine(youWin());
        }
    }

    public void statePaused()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGameGoal(int enemyCount)
    {
        enemiesRemaining += enemyCount;
        killGoalText.text = killGoal.ToString("F0");
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");

        if(survival && killCount >= killGoal)
        {
            //win con met
            winMessageText.text = "You Survived";
            StartCoroutine(youWin());
        }
        if(elimination && enemiesRemaining <= 0)
        {
            //win con met
            winMessageText.text = "Enemies Eliminated";
            StartCoroutine(youWin());
        }
    }

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(2);
        scoreKeeper.playerScore += 500;
        instance.playerScoreText.text = scoreKeeper.playerScore.ToString();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        statePaused();
    }

    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void obstacleCourseFinished()
    {
        scoreKeeper.playerScore += 500;
        playerScoreText.text = scoreKeeper.playerScore.ToString();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        statePaused();
    }

    public void timer()
    {
        timerLabel.SetActive(true);
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("0.00");
    }
}