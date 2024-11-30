using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    public AudioClip backGroundMusic = null;

    private MainMenuControl mainMenu;
    private GameStatus game_status = null;
    private PlayerControl player_control = null;
    private GameObject[] enemies = null;
    public enum STEP
    { // 게임 상태.
        NONE = -1, // 상태 정보 없음.
        PLAY = 0, // 플레이 중.
        CLEAR, // 클리어 상태.
        GAMEOVER, // 게임 오버 상태.
        PAUSE,
        NUM, // 상태가 몇 종류인지 나타낸다(=3).
    };
    public STEP step = STEP.NONE; // 현대 단계.
    public STEP next_step = STEP.NONE; // 다음 단계.

    public GUIStyle guistyle; // 폰트 스타일.
    public GUIStyle boxGuiStyle;
    
    private float clearScore = 0.0f;

    public float soundSize;

    public AudioSource audio;

    public Canvas resultCanvasPrefab = null;
    public Canvas resultCanvas;
    public Canvas pauseCanvasPrefab = null;
    public Canvas pauseCanvas;

    private bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        this.guistyle.fontSize = 64;
        boxGuiStyle = new GUIStyle(guistyle);
        boxGuiStyle.fontSize = 23;

        if (SceneManager.GetActiveScene().name != "GameScene") return;
        this.resultCanvasPrefab = GameObject.Find("MainMenuCanvas").GetComponent<SceneControl>().resultCanvasPrefab;
        this.resultCanvas = Canvas.Instantiate(resultCanvasPrefab) as Canvas;
        this.resultCanvas.gameObject.SetActive(false);
        this.pauseCanvasPrefab = GameObject.Find("MainMenuCanvas").GetComponent<SceneControl>().pauseCanvasPrefab;
        this.pauseCanvas = Canvas.Instantiate(pauseCanvasPrefab) as Canvas;
        this.pauseCanvas.gameObject.SetActive(false);

        this.soundSize = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuControl>().soundSize;
        this.game_status = this.gameObject.GetComponent<GameStatus>();
        this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
        this.step = STEP.PLAY;
        this.next_step = STEP.PLAY;
        this.player_control = GameObject.Find("Player").GetComponent<PlayerControl>();
        GameObject.Find("EnemyDeadEffectSound").GetComponent<AudioSource>().volume = soundSize;
        this.audio = GameObject.Find("BackGroundMusic").GetComponent<AudioSource>();
        audio.volume = soundSize;
        audio.clip = backGroundMusic;
        audio.Play();

        Destroy(GameObject.Find("MainMenuCanvas"));
    }

    // Update is called once per frame
    void Update()
    {
        this.getKey();

        if (this.next_step == STEP.NONE)
        {
            
            switch (this.step)
            {
                case STEP.NONE:
                    break;
                case STEP.PLAY:
                    if (this.game_status.isGameClear())
                    {
                        // 클리어 상태로 이동.
                        this.next_step = STEP.CLEAR;
                    }
                    if (this.game_status.isGameOver())
                    {
                        // 게임 오버 상태로 이동.
                        this.next_step = STEP.GAMEOVER;
                    }
                    if (isPause)
                    {
                        this.next_step = STEP.PAUSE;
                    }

                    // 시간 제한과 관계는 clear_rate로 표현 가능하므로 삭제 (삭제)
                    /*
                    if (this.step_timer > GAME_OVER_TIME)
                    {
                        // 제한 시간을 넘었으면 게임 오버.
                        this.next_step = STEP.GAMEOVER;
                    }
                    */
                    break;

                // 클리어 시 및 게임 오버 시의 처리.
                case STEP.CLEAR:
                case STEP.GAMEOVER:
                    break;
            }
        }
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;
            switch (this.step)
            {
                case STEP.CLEAR:
                    // PlayerControl을 제어 불가로.
                    this.player_control.enabled = false;
                    // 현재의 경과 시간으로 클리어 시간을 갱신.
                    this.clearScore = game_status.Score;
                    game_status.step_timer = 0.0f;
                    break;
                case STEP.GAMEOVER:
                    // PlayerControl를 제어 불가.
                    this.player_control.enabled = false;
                    Time.timeScale = 0.0f;
                    game_status.step_timer = 0.0f;
                    break;
                case STEP.PAUSE:
                    Time.timeScale = 0.0f;
                    this.player_control.enabled = false;
                    break;
                case STEP.PLAY:
                    this.player_control.enabled = true;
                    break;

            }
        }
    }

    void OnGUI()
    {
        float pos_x = Screen.width * 0.1f;
        float pos_y = Screen.height * 0.5f;
        switch (this.step)
        {
            case STEP.PLAY:
                GUI.color = Color.black;
                /*
                GUI.Label(new Rect(pos_x, pos_y, 200, 20), // 경과 시간을 표시.
                game_status.step_timer.ToString("0.00"), guistyle);
                */
                GUI.Label(new Rect(pos_x, pos_y, 200, 20),
                    ((int)(game_status.Score)).ToString() + "점", guistyle);

                break;
            case STEP.CLEAR:
                
                /*
                GUI.color = Color.black;
                // 클리어 메시지와 클리어 시간 표시.
                GUI.Label(new Rect(pos_x, pos_y, 200, 60),
                "성공 : " + this.clearScore.ToString() + "점", guistyle);
                clearSelectMenu();
                */
                break;
            case STEP.GAMEOVER:
                resultCanvas.GetComponent<ResultControl>().resultOnText();
                resultCanvas.gameObject.SetActive(true);
                /*
                GUI.color = Color.black;
                // 게임 오버 메시지를 표시.
                GUI.Label(new Rect(Screen.width * 0.3f, Screen.height * 0.5f, 200, 60),
                "게임 오버", boxGuiStyle);
                gameOverSelectMenu();
                */
                break;
            case STEP.PAUSE:
                this.pauseCanvas.gameObject.SetActive(true);
                break;
                
        }
    }

    private void getKey()
    {
        if (SceneManager.GetActiveScene().name != "GameScene") return;
        this.isPause = Input.GetKeyDown(KeyCode.Escape);
    }

    /*
    private void gameOverSelectMenu()
    {
        float pos_x = Screen.width * 0.2f;
        float pos_y = Screen.height * 0.6f;
        GUI.Box(new Rect(Screen.width * 0.15f, Screen.height * 0.15f,
            Screen.width * 0.7f, Screen.height * 0.7f),
            "Result");
        GUI.Label(new Rect(pos_x, pos_y, 200, 20),
                    ("현재 기록 : " + (int)(game_status.Score)).ToString() + "점", guistyle);
        bool isClickRetryButton =
                    GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.35f,
                        Screen.width * 0.25f, Screen.height * 0.2f),
                        "Retry", boxGuiStyle);
        bool isClickMainMenuButton =
            GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.65f,
                Screen.width * 0.25f, Screen.height * 0.2f),
                "Main Menu", boxGuiStyle);

        if (isClickRetryButton)
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (isClickMainMenuButton)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void clearSelectMenu()
    {
        bool isClickRetryButton =
            GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.35f,
                        Screen.width * 0.15f, Screen.height * 0.07f),
                        "Next");
        bool isClickMainMenuButton =
            GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.65f,
                Screen.width * 0.15f, Screen.height * 0.07f),
                "Main Menu", new GUIStyle(guistyle));

        if (isClickRetryButton)
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (isClickMainMenuButton)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    */
}