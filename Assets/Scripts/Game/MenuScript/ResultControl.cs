using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ResultControl : MonoBehaviour
{
    public Text result;
    public Text score;

    private GameStatus game_status;
    private SceneControl info;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickMenu()
    {
        DontDestroyOnLoad(GameObject.Find("GameRoot"));
        SceneManager.LoadScene("MainMenu");
    }

    public void resultOnText()
    {
        game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        result.text = "좋은 결과네요!";
        score.text = "현재 기록 : " + ((int)(game_status.getScore())).ToString();

    }

}
