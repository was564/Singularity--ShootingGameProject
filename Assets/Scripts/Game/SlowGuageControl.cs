using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SlowGuageControl : MonoBehaviour
{
    public GameStatus game_status = null;
    public Slider guage = null;

    // Start is called before the first frame update
    void Start()
    {
        game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        guage = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float guage_amount = game_status.getSlowGuage();
        if(guage_amount == 0.0f) { guage.enabled = false; return; }
        guage.value = guage_amount;
    }
}
