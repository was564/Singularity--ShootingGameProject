using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    public float soundSize = 1.0f;
    public Canvas OptionCanvasPrefab = null;
    public Canvas TutorialCanvasPrefab = null;

    private Canvas optionCanvas;
    private Canvas tutorialCanvas;

    // Start is called before the first frame update
    void Start()
    {
        this.optionCanvas = Canvas.Instantiate(OptionCanvasPrefab) as Canvas;
        this.tutorialCanvas = Canvas.Instantiate(TutorialCanvasPrefab) as Canvas;
        optionCanvas.gameObject.SetActive(false);
        tutorialCanvas.gameObject.SetActive(false);
        GameObject go = GameObject.Find("GameRoot");
        if (go != null)
        {
            this.soundSize = go.GetComponent<SceneControl>().soundSize;
            this.optionCanvas.GetComponent<SubMenuControl>().soundSlier.value = soundSize;
            Destroy(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /*
    void OnGUI()
    {
        float pos_x = Screen.width * 0.5f;
        float pos_y = Screen.height * 0.5f;
        GUI.Box(new Rect(Screen.width * 0.2f, Screen.height * 0.2f,
            Screen.width * 0.6f, Screen.height * 0.6f), 
            "Singularity");

        bool isClick = 
            GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.35f,
                Screen.width * 0.2f, Screen.height * 0.1f),
                "Game Start");
        if (isClick) SceneManager.LoadScene("GameScene");

        bool isOption =
            GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.55f,
                Screen.width * 0.2f, Screen.height * 0.1f),
                "Option");
        
    }
    */

    public void onClickStartGame()
    {
        GameObject go = GameObject.Find("MainMenuCanvas");
        DontDestroyOnLoad(go);
        SceneManager.LoadScene("GameScene");
    }

    public void onClickOption()
    {
        this.optionCanvas.gameObject.SetActive(true);

    }

    public void onClickTutorial()
    {
        this.tutorialCanvas.gameObject.SetActive(true);
    }
}
