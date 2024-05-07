using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SubMenuControl : MonoBehaviour
{
    public Slider soundSlier = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickOptionBack()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<MainMenuControl>().soundSize = soundSlier.value;
        this.gameObject.SetActive(false);

    }

    public void onClickTutorialBack()
    {
        this.gameObject.SetActive(false);
    }
}
