using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    public GameObject messagePromptPopup;
    public Image playerHPBar;
    public Image playerOvershieldBar;
    public GameObject playerFlashUI;
    public GameObject playerDamageBoost;
    public GameObject playerSpeedBoost;
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
    public TextMeshProUGUI messagePromptText;

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

    [SerializeField] string parMasterVol = "MasterVolume";
    [SerializeField] string parMusicVol = "MusicVolume";
    [SerializeField] string parSFXVol = "SFXVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle masterToggle;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    private bool disableMasterToggleEvent;
    private bool disableMusicToggleEvent;
    private bool disableSFXToggleEvent;


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
        masterSlider.onValueChanged.AddListener(MasterSlideValChange);
        masterToggle.onValueChanged.AddListener(ToggleMasterValChange);
        musicSlider.onValueChanged.AddListener(MusicSlideValChange);
        musicToggle.onValueChanged.AddListener(ToggleMMusicValChange);
        sfxSlider.onValueChanged.AddListener(SFXSlideValChange);
        sfxToggle.onValueChanged.AddListener(ToggleSFXValChange);
    }
    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(parMasterVol, masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(parMusicVol, musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat(parSFXVol, sfxSlider.value);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (courseStarted)
            timer();

        if (Input.GetButtonDown("Pause") && activeMenu == null)
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
        PlayerPrefs.SetFloat(parMasterVol, masterSlider.value);
        PlayerPrefs.SetFloat(parMusicVol, musicSlider.value);
        PlayerPrefs.SetFloat(parSFXVol, sfxSlider.value);
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
        if (gameManager.instance.playerController.hasFlag == true && gameManager.instance.player.transform.position.z == GameObject.FindGameObjectWithTag("Blue Flag").transform.position.z
            && gameManager.instance.player.transform.position.x == GameObject.FindGameObjectWithTag("Blue Flag").transform.position.x)
        {
            //win con met
            winMessageText.text = "Flag Captured";
            StartCoroutine(youWin());
        }
    }

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(0.5f);
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

    private void ToggleMasterValChange(bool enableSound)
    {
        if (disableMasterToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            masterSlider.value = 0;
        }
        else
        {
            masterSlider.value = masterSlider.minValue;
        }
    }

    private void MasterSlideValChange(float val)
    {
        mixer.SetFloat(parMasterVol, val);
        disableMasterToggleEvent = true;
        masterToggle.isOn = masterSlider.value > masterSlider.minValue;
        disableMasterToggleEvent = false;
        
    }

    private void ToggleMMusicValChange(bool enableSound)
    {
        if (disableMusicToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            musicSlider.value = 0;
        }
        else
        {
            musicSlider.value = musicSlider.minValue;
        }
    }

    private void MusicSlideValChange(float val)
    {
        mixer.SetFloat(parMusicVol, val);
        disableMusicToggleEvent = true;
        musicToggle.isOn = musicSlider.value > musicSlider.minValue;
        disableMusicToggleEvent = false;
        
    }

    private void ToggleSFXValChange(bool enableSound)
    {
        if (disableSFXToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            sfxSlider.value = 0;
        }
        else
        {
            sfxSlider.value = sfxSlider.minValue;
        }
    }

    private void SFXSlideValChange(float val)
    {
        mixer.SetFloat(parSFXVol, val);
        disableSFXToggleEvent = true;
        sfxToggle.isOn = sfxSlider.value > sfxSlider.minValue;
        disableSFXToggleEvent = false;
        
    }
}