﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusOriginal : MonoBehaviour
{
    // 철광석, 식물을 사용했을 때 각각의 수리 정도.
    public static float GAIN_REPAIRMENT_IRON = 0.30f;
    public static float GAIN_REPAIRMENT_PLANT = 0.10f;
    // 철광석, 사과, 식물을 운반했을 때 각각의 체력 소모 정도.
    public static float CONSUME_SATIETY_IRON = 0.20f;
    public static float CONSUME_SATIETY_APPLE = 0.1f;
    public static float CONSUME_SATIETY_PLANT = 0.1f;
    // 사과, 식물을 먹었을 때 각각의 체력 회복 정도.
    public static float REGAIN_SATIETY_APPLE = 0.7f;
    public static float REGAIN_SATIETY_PLANT = 0.3f;
    public float repairment = 0.0f; // 우주선의 수리 정도(0.0f~1.0f).
    public float satiety = 1.0f; // 배고픔,체력(0.0f~1.0f).
    public GUIStyle guistyle; // 폰트 스타일.

    public static float CONSUME_SATIETY_ALWAYS = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        this.guistyle.fontSize = 24; // 폰트 크기를 24로.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnGUI()
    {
        float x = Screen.width * 0.2f;
        float y = 20.0f;
        // 체력을 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f), "체력:" +
        (this.satiety * 100.0f).ToString("000"), guistyle);
        x += 200;
        // 수리 정도를 표시.
        GUI.Label(new Rect(x, y, 200.0f, 20.0f),
        "로켓 :" + (this.repairment * 100.0f).ToString("000"), guistyle);
    }
    // 우주선 수리를 진행
    public void addRepairment(float add)
    {
        this.repairment = Mathf.Clamp01(this.repairment + add); // 0.0~1.0 강제 지정
    }
    // 체력을 늘리거나 줄임
    public void addSatiety(float add)
    {
        this.satiety = Mathf.Clamp01(this.satiety + add);
    }

    // 게임을 클리어했는지 검사
    public bool isGameClear()
    {
        bool is_clear = false;
        if (this.repairment >= 1.0f)
        { // 수리 정도가 100% 이상이면.
            is_clear = true; // 클리어했다.
        }
        return (is_clear);
    }
    // 게임이 끝났는지 검사
    public bool isGameOver()
    {
        bool is_over = false;
        if (this.satiety <= 0.0f)
        { // 체력이 0이하라면.
            is_over = true; // 게임 오버.
        }
        return (is_over);
    }

    // 배를 고프게 하는 메서드 추가
    public void alwaysSatiety()
    {
        this.satiety = Mathf.Clamp01(this.satiety - CONSUME_SATIETY_ALWAYS * Time.deltaTime);
    }

}
