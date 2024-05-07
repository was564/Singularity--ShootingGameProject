using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    // 대시와 슬로우 모션을 썼을 때 게이지 소모 정도.
    public static float CONSUME_GUAGE_DASH = 1.0f;
    public static float CONSUME_GUAGE_SLOW = 0.1f;
    public static int CONSUME_GUAGE_HEART = 1;

    // 대시와 슬로우 모션 아이템을 먹었을 때 각각의 게이지 회복 정도.
    public static float REGAIN_GUAGE_DASH = 1.0f;
    public static float REGAIN_GUAGE_SLOW = 33.0f;
    public static int REGAIN_GUAGE_HEART = 1;
    public float slowGuage = 100.0f; // 슬로우 모션 게이지(0.0f~100.0f).
    public float dashGuage = 4.0f; // 대시 게이지 (0.0f~2.0f) (1.0f씩)
    public GUIStyle guistyle; // 폰트 스타일.

    public DashGuageControl dashGuageBar = null;
    public HeartGuageControl HeartGuageBar = null;

    public Sprite Heart = null;
    public Sprite Dash = null;
    public Sprite SlowMotion = null;

    /*
    // 열쇠 추가를 위한 변수 (추가)
    public float amountKey = 0.0f; // 가지고 있는 열쇠 개수
    */

    // 체력을 안쓰므로 삭제 (삭제)
    // public static float CONSUME_SATIETY_ALWAYS = 0.03f;

    // hp 추가 (추가)
    public int life = 3;

    public static float CLEAR_CONDITION_AMOUNT_KEY = 5.0f; // 게임 클리어를 위한 열쇠의 개수
    /*
    private float GAME_OVER_TIME = 60.0f; // 제한시간은 60초.
    private float GAME_OVER_RATE = 0.70f; // 클리어 기준은 70% 이상
    */

    public float step_timer { get; set; } = 0.0f; // 타이머

    public float Score = 0;
    public float slow_time { get; set; } = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.guistyle.fontSize = 24; // 폰트 크기를 24로.
        dashGuageBar = GameObject.FindWithTag("DashGuage").GetComponent<DashGuageControl>();
        HeartGuageBar = GameObject.FindWithTag("HeartGuage").GetComponent<HeartGuageControl>();
    }

    // Update is called once per frame
    void Update()
    {
        step_timer += Time.deltaTime;
        Score += Time.deltaTime * 10.0f;
    }

    /*
    void OnGUI()
    {
        float x = Screen.width * 0.2f;
        float y = 20.0f;
        // 체력 게이지를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f), "목숨:" +
        (this.life).ToString("0"), guistyle);
        x += 200;
        // 슬로우 모션 게이지를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f), "슬로우 모션:" +
        (this.slowGuage).ToString("000"), guistyle);
        x += 200;
        // 대시 게이지를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f), "대시:" +
        (this.dashGuage).ToString("0"), guistyle);

        
        // 열쇠 개수를 표시 (추가)
        x += 100;
        GUI.Label(new Rect(x, y, 200.0f, 20.0f), "열쇠:" +
        (this.amountKey).ToString("0"), guistyle);
        
    }
    */

    // 슬로우 모션 게이지를 늘리거나 줄임
    public void addSlowGuage(float add)
    {
        this.slowGuage = Mathf.Clamp(this.slowGuage + add, 0.0f, 100.0f);
    }

    // 대시 게이지를 늘리거나 줄임
    public void addDashGuage(float add)
    {
        this.dashGuage = Mathf.Clamp(this.dashGuage + add, 0.0f, 4.0f);
        this.dashGuageBar.getDashGuage();
        
    }

    public void addHeart(int add)
    {
        this.life = Mathf.Clamp(this.life + add, 0, 5);
        this.HeartGuageBar.getHeartGuage();

    }

    public void useDashGuage()
    {
        this.dashGuage -= 1.0f;
        this.dashGuageBar.useDashGuage();

    }

    public void useSlowMotion()
    {
        this.slowGuage -= 25.0f * Time.deltaTime;
    }

    public float getDashGuage()
    {
        return this.dashGuage;
    }

    public float getSlowGuage()
    {
        return this.slowGuage;
    }

    public float getScore()
    {
        return this.Score;
    }

    public int getLife()
    {
        return this.life;
    }

    /*
    public void rootingKey()
    {
        this.amountKey += 1.0f;
    }
    */

    public void hit()
    {
        this.life -= 1;
        if (life < 0) return;
        HeartGuageBar.useHeartGuage();
    }


    // 게임을 클리어했는지 검사
    public bool isGameClear()
    {
        bool is_clear = false;
        if (false)
        { 
            is_clear = true; // 클리어했다.
        }
        return (is_clear);
    }
    // 게임이 끝났는지 검사
    public bool isGameOver()
    {
        bool is_over = false;
        if (this.life < 1)
        { 
            is_over = true; // 게임 오버.
        }
        return (is_over);
    }
}
