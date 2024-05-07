using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DashGuageControl : MonoBehaviour
{
    public Image[] images;

    public GameStatus game_status = null;

    private float dash = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //images = GetComponentsInChildren<Image>();
        game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        dash = game_status.getDashGuage();
        for (int i=(int)dash; i<4; i++)
        {
            images[i].enabled = false;
            images[i].gameObject.SetActive(false);
        }
    }

    public float getDash()
    {
        return dash;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void getDashGuage()
    {
        dash += 1;
        images[(int)(game_status.getDashGuage()) - 1].enabled = true;
        images[(int)(game_status.getDashGuage()) - 1].gameObject.SetActive(true);
    }

    public void useDashGuage()
    {
        images[(int)(game_status.getDashGuage())].enabled = false;
        images[(int)(game_status.getDashGuage())].gameObject.SetActive(false);
    }
}
