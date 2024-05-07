using System.Collections;
using System.Collections.Generic;// List를 사용하기 위해서.
using UnityEngine;

public class ItemOriginal
{
    public enum TYPE
    { // 아이템 종류.
        NONE = -1, IRON = 0, APPLE, PLANT, // 없음, 철광석, 사과, 식물.
        NUM,
    }; // 아이템이 몇 종류인가 나타낸다(=3).
};
public class ItemRootOriginal : MonoBehaviour
{
    public GameObject ironPrefab = null; // Prefab 'Iron'
    public GameObject plantPrefab = null; // Prefab 'Plant'
    public GameObject applePrefab = null; // Prefab 'Apple'
    protected List<Vector3> respawn_points; // 출현 지점 List.

    public float step_timer = 0.0f;
    public static float RESPAWN_TIME_APPLE = 20.0f; // 사과 출현 시간 상수.
    public static float RESPAWN_TIME_IRON = 12.0f; // 철광석 출현 시간 상수.
    public static float RESPAWN_TIME_PLANT = 6.0f; // 식물 출현 시간 상수.
    private float respawn_timer_apple = 0.0f; // 사과의 출현 시간.
    private float respawn_timer_iron = 0.0f; // 철광석의 출현 시간.
    private float respawn_timer_plant = 0.0f; // 식물의 출현 시간.

    // Start is called before the first frame update
    void Start()
    {
        // 메모리 영역 확보.
        this.respawn_points = new List<Vector3>();
        // "PlantRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장.
        GameObject[] respawns =
        GameObject.FindGameObjectsWithTag("PlantRespawn");
        // 배열 respawns 내의 개개의 GameObject를 순서래도 처리한다.
        foreach (GameObject go in respawns)
        {
            // 렌더러 획득.
            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            { // 렌더러가 존재하면.
                renderer.enabled = false; // 그 렌더러를 보이지 않게.
            }
            // 출현 포인트 List에 위치 정보를 추가.
            this.respawn_points.Add(go.transform.position);
        }
        // 사과의 출현 포인트를 취득하고, 렌더러를 보이지 않게.
        GameObject applerespawn = GameObject.Find("AppleRespawn");
        applerespawn.GetComponent<MeshRenderer>().enabled = false;
        // 철광석의 출현 포인트를 취득하고, 렌더러를 보이지 않게.
        GameObject ironrespawn = GameObject.Find("IronRespawn");
        ironrespawn.GetComponent<MeshRenderer>().enabled = false;
        this.respawnIron(); // 철광석을 하나 생성.
        this.respawnPlant(); // 식물을 하나 생성.

        this.respawnPlant();
        this.respawnPlant();
    }

    // Update is called once per frame
    void Update()
    {
        respawn_timer_apple += Time.deltaTime;
        respawn_timer_iron += Time.deltaTime;
        respawn_timer_plant += Time.deltaTime;
        if (respawn_timer_apple > RESPAWN_TIME_APPLE)
        {
            respawn_timer_apple = 0.0f;
            this.respawnApple(); // 사과를 출현시킨다.
        }
        if (respawn_timer_iron > RESPAWN_TIME_IRON)
        {
            respawn_timer_iron = 0.0f;
            this.respawnIron(); // 철광석을 출현시킨다.
        }
        if (respawn_timer_plant > RESPAWN_TIME_PLANT)
        {
            respawn_timer_plant = 0.0f;
            this.respawnPlant(); // 식물을 출현시킨다.
        }
    }

    // 아이템의 종류를 Item.TYPE형으로 반환하는 메소드.
    public ItemOriginal.TYPE getItemType(GameObject item_go)
    {
        ItemOriginal.TYPE type = ItemOriginal.TYPE.NONE;
        if (item_go != null)
        { // 인수로 받은 GameObject가 비어있지 않으면.
            switch (item_go.tag)
            { // 태그로 분기.
                case "Iron": type = ItemOriginal.TYPE.IRON; break;
                case "Apple": type = ItemOriginal.TYPE.APPLE; break;
                case "Plant": type = ItemOriginal.TYPE.PLANT; break;
            }
        }
        return (type);
    }

    public void respawnIron()
    {
        // 철광석 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.ironPrefab) as GameObject;
        // 철광석의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("IronRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);
        // 철광석의 위치를 이동.
        go.transform.position = pos;
    }
    // 사과를 출현시킨다.
    public void respawnApple()
    {
        // 사과 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.applePrefab) as GameObject;
        // 사과의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("AppleRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);
        // 사과의 위치를 이동.
        go.transform.position = pos;
    }
    // 식물을 출현시킨다.
    public void respawnPlant()
    {
        if (this.respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          // 식물 프리팹을 인스턴스화.
            GameObject go = GameObject.Instantiate(this.plantPrefab) as GameObject;
            // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.respawn_points.Count);
            Vector3 pos = this.respawn_points[n];
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-1.0f, 1.0f);
            pos.z += Random.Range(-1.0f, 1.0f);
            // 식물의 위치를 이동.
            go.transform.position = pos;
        }
    }

    // 들고 있는 아이템에 따른 ‘수리 진척 상태’를 반환
    public float getGainRepairment(GameObject item_go)
    {
        float gain = 0.0f;
        if (item_go == null)
        {
            gain = 0.0f;
        }
        else
        {
            ItemOriginal.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case ItemOriginal.TYPE.IRON:
                    gain = GameStatusOriginal.GAIN_REPAIRMENT_IRON; break;
                case ItemOriginal.TYPE.PLANT:
                    gain = GameStatusOriginal.GAIN_REPAIRMENT_PLANT; break;
            }
        }
        return (gain);
    }
    // 들고 있는 아이템에 따른 ‘체력 감소 상태’를 반환
    public float getConsumeSatiety(GameObject item_go)
    {
        float consume = 0.0f;
        if (item_go == null)
        {
            consume = 0.0f;
        }
        else
        {
            ItemOriginal.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case ItemOriginal.TYPE.IRON:
                    consume = GameStatusOriginal.CONSUME_SATIETY_IRON; break;
                case ItemOriginal.TYPE.APPLE:
                    consume = GameStatusOriginal.CONSUME_SATIETY_APPLE; break;
                case ItemOriginal.TYPE.PLANT:
                    consume = GameStatusOriginal.CONSUME_SATIETY_PLANT; break;
            }
        }
        return (consume);
    }
    // 들고 있는 아이템에 따른 ‘체력 회복 상태’를 반환
    public float getRegainSatiety(GameObject item_go)
    {
        float regain = 0.0f;
        if (item_go == null)
        {
            regain = 0.0f;
        }
        else
        {
            ItemOriginal.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case ItemOriginal.TYPE.APPLE:
                    regain = GameStatusOriginal.REGAIN_SATIETY_APPLE; break;
                case ItemOriginal.TYPE.PLANT:
                    regain = GameStatusOriginal.REGAIN_SATIETY_PLANT; break;
            }
        }
        return (regain);
    }
}