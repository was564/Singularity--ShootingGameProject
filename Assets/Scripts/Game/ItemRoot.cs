using System.Collections;
using System.Collections.Generic;// List를 사용하기 위해서.
using UnityEngine;

public class Item
{
    public enum TYPE
    { // 아이템 종류.
        //NONE = -1, IRON = 0, APPLE, PLANT, // 없음, 철광석, 사과, 식물.
        NONE = -1, Key = 0, SlowItem, DashItem, Heart,
        NUM,
    }; // 아이템이 몇 종류인가 나타낸다(=3).
};
public class ItemRoot : MonoBehaviour
{
    public GameObject keyPrefab = null; // Prefab 'Key'
    public GameObject slowItemPrefab = null; // prefab 'slowItem'
    public GameObject dashItemPrefab = null; // prefab 'dashItem'
    public GameObject HeartItemPrefab = null;

    private EnemySpawn spawnInfo = null;

    private int spawnHeartBySet;
    
    protected List<Vector3> key_respawn_points; // 출현 지점 List.
    protected List<Vector3> dash_respawn_points; // 출현 지점 List.
    protected List<Vector3> slow_respawn_points; // 출현 지점 List.

    public float step_timer = 0.0f;
    public static float RESPAWN_TIME_DASHITEM = 20.0f; // 사과 출현 시간 상수.
    public static float RESPAWN_TIME_KEY = 30.0f; // 철광석 출현 시간 상수.
    public static float RESPAWN_TIME_SLOWITEM = 30.0f; // 식물 출현 시간 상수.
    private float respawn_timer_dashItem = 0.0f; // 대시 아이템의 출현 시간.
    private float respawn_timer_key = 0.0f; // 열쇠의 출현 시간.
    private float respawn_timer_slowItem = 0.0f; // 슬로우 모션 아이템의 출현 시간.

    // Start is called before the first frame update
    void Start()
    {

        // 메모리 영역 확보.
        this.key_respawn_points = new List<Vector3>();
        this.dash_respawn_points = new List<Vector3>();
        this.slow_respawn_points = new List<Vector3>();

        addAllRespawnPointToVector("DashSpawner", dash_respawn_points);
        addAllRespawnPointToVector("SlowSpawner", slow_respawn_points);

        spawnInfo = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        spawnHeartBySet = 3;
    }

    // Update is called once per frame
    void Update()
    {
        respawn_timer_dashItem += Time.deltaTime;
        //respawn_timer_key += Time.deltaTime;
        respawn_timer_slowItem += Time.deltaTime;
        
        /*
        if (respawn_timer_key > RESPAWN_TIME_KEY)
        {
            respawn_timer_key = 0.0f;
            this.respawnKey(); // 철광석을 출현시킨다.
        }
        */
        if (respawn_timer_slowItem > RESPAWN_TIME_SLOWITEM)
        {
            respawn_timer_slowItem = 0.0f;
            this.respawnSlowItem(); // 식물을 출현시킨다.
        }
        if (respawn_timer_dashItem > RESPAWN_TIME_DASHITEM)
        {
            respawn_timer_dashItem = 0.0f;
            this.respawnDashItem(); // 식물을 출현시킨다.
        }

        if(spawnInfo.set == spawnHeartBySet)
        {
            if (spawnHeartBySet == 3) spawnHeartBySet = 6;
            else if (spawnHeartBySet == 6) spawnHeartBySet = 3;
            this.respawnHeart();

        }
    }

    // 아이템의 종류를 Item.TYPE형으로 반환하는 메소드.
    public Item.TYPE getItemType(GameObject item_go)
    {
        Item.TYPE type = Item.TYPE.NONE;
        if (item_go != null)
        { // 인수로 받은 GameObject가 비어있지 않으면.
            switch (item_go.tag)
            { // 태그로 분기.
                case "Key": type = Item.TYPE.Key; break;
                case "SlowItem": type = Item.TYPE.SlowItem; break;
                case "DashItem": type = Item.TYPE.DashItem; break;
                case "HeartItem": type = Item.TYPE.Heart; break;
            }
        }
        return (type);
    }

    public void respawnKey()
    {
        if (this.key_respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          
          // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.key_respawn_points.Count);
            Vector3 pos = this.key_respawn_points[n];

            // 식물 프리팹을 인스턴스화.
            GameObject go = GameObject.Instantiate(this.keyPrefab) as GameObject;
            
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-5.0f, 5.0f);
            pos.z += Random.Range(-5.0f, 5.0f);
            // 식물의 위치를 이동.
            go.transform.position = pos;
        }
    }

    // 대시 아이템을 출현시킨다.
    public void respawnDashItem()
    {
        if (this.dash_respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          // 식물 프리팹을 인스턴스화.

            GameObject go = GameObject.Instantiate(this.dashItemPrefab) as GameObject;
            // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.dash_respawn_points.Count);
            Vector3 pos = this.dash_respawn_points[n];
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-5.0f, 5.0f);
            pos.z += Random.Range(-5.0f, 5.0f);
            // 식물의 위치를 이동.
            go.transform.position = pos;

            go.transform.Find("Image").Rotate(new Vector3(90, 0, 0));
        }
    }

    // 슬로우 모션 아이템을 출현시킨다.
    public void respawnSlowItem()
    {
        if (this.slow_respawn_points.Count > 0)
        { // List가 비어있지 않으면.
          // 식물 프리팹을 인스턴스화.

            GameObject go = GameObject.Instantiate(this.slowItemPrefab) as GameObject;
            // 식물의 출현 포인트를 랜덤하게 취득.
            int n = Random.Range(0, this.slow_respawn_points.Count);
            Vector3 pos = this.slow_respawn_points[n];
            // 출현 위치를 조정.
            pos.y = 1.0f;
            pos.x += Random.Range(-5.0f, 5.0f);
            pos.z += Random.Range(-5.0f, 5.0f);
            // 식물의 위치를 이동.
            go.transform.position = pos;

            go.transform.Find("Image").Rotate(new Vector3(90, 0, 0));
        }
    }
    public void respawnHeart()
    {
        // 철광석 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.HeartItemPrefab) as GameObject;
        // 철광석의 출현 포인트를 취득.
        Vector3 pos = GameObject.Find("HeartRespawn").transform.position;
        // 출현 위치를 조정.
        pos.y = 1.0f;
        pos.x += Random.Range(-12.0f, 12.0f);
        pos.z += Random.Range(-12.0f, 12.0f);
        // 식물의 위치를 이동.
        go.transform.position = pos;

        go.transform.Find("Image").Rotate(new Vector3(90, 0, 0));
    }

    public float getConsumeGuage(GameObject item_go)
    {
        float consume = 0.0f;
        if (item_go == null)
        {
            consume = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.DashItem:
                    consume = GameStatus.CONSUME_GUAGE_DASH; break;
                case Item.TYPE.SlowItem:
                    consume = GameStatus.CONSUME_GUAGE_SLOW; break;
            }
        }
        return (consume);
    }
    public float getRegainGuage(GameObject item_go)
    {
        float gain = 0.0f;
        if (item_go == null)
        {
            gain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.DashItem:
                    gain = GameStatus.REGAIN_GUAGE_DASH; break;
                case Item.TYPE.SlowItem:
                    gain = GameStatus.REGAIN_GUAGE_SLOW; break;
                case Item.TYPE.Heart:
                    gain = GameStatus.CONSUME_GUAGE_HEART; break;
            }
        }
        return (gain);
    }

    private void addAllRespawnPointToVector(string tagName, List<Vector3> list)
    {
        // "PlantRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장.
        GameObject[] respawns =
            GameObject.FindGameObjectsWithTag(tagName);
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
            list.Add(go.transform.position);
        }
    }

    // 수리 진척 상태를 반환
    /*
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
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.IRON:
                    gain = GameStatus.GAIN_REPAIRMENT_IRON; break;
                case Item.TYPE.PLANT:
                    gain = GameStatus.GAIN_REPAIRMENT_PLANT; break;
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
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.IRON:
                    consume = GameStatus.CONSUME_SATIETY_IRON; break;
                 case Item.TYPE.APPLE:
                     consume = GameStatus.CONSUME_SATIETY_APPLE; break;
                case Item.TYPE.PLANT:
                    consume = GameStatus.CONSUME_SATIETY_PLANT; break;
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
            Item.TYPE type = this.getItemType(item_go);
            switch (type)
            { // 들고 있는 아이템의 종류로 갈라진다.
                case Item.TYPE.APPLE:
                    regain = GameStatus.REGAIN_SATIETY_APPLE; break;
                case Item.TYPE.PLANT:
                    regain = GameStatus.REGAIN_SATIETY_PLANT; break;
            }
        }
        return (regain);
    }
    */
}