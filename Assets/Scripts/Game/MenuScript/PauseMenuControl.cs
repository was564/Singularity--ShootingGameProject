using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuControl : MonoBehaviour
{
    private SceneControl sceneControl = null;

    // Start is called before the first frame update
    void Start()
    {
        sceneControl = GameObject.Find("GameRoot").GetComponent<SceneControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickMainMenu()
    {
        DontDestroyOnLoad(GameObject.Find("GameRoot"));
        SceneManager.LoadScene("MainMenu");
    }

    public void onClickResume()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);

        //GameObject.Find("GameRoot").GetComponent<SceneControl>().pauseCanvas.gameObject.SetActive(false);
        sceneControl.next_step = SceneControl.STEP.PLAY;
    }
}
