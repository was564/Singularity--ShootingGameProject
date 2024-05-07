using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeartGuageControl : MonoBehaviour
{
    public Image[] images;

    public GameStatus game_status = null;

    private int heart = 0;
    // Start is called before the first frame update
    void Start()
    {
        //images = GetComponentsInChildren<Image>();
        game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        heart = game_status.getLife();
        for(int i=game_status.life; i<5; i++)
        {
            images[i].enabled = false;
            images[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public float getHeart()
    {
        return heart;
    }

    public void getHeartGuage()
    {
        images[game_status.life - 1].enabled = true;
        images[game_status.life - 1].gameObject.SetActive(true);
    }

    public void useHeartGuage()
    {
        images[game_status.life].enabled = false;
        images[game_status.life].gameObject.SetActive(false);
    }
}
