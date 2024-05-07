using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 처음에 이벤트의 종류를 나타내는 class.
public class EventOriginal
{ // 이벤트 종류.
    public enum TYPE
    {
        NONE = -1, // 없음.
        ROCKET = 0, // 우주선 수리.
        NUM, // 이벤트가 몇 종류 있는지 나타낸다(=1).
    };
};

public class EventRootOriginal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EventOriginal.TYPE getEventType(GameObject event_go)
    {
        EventOriginal.TYPE type = EventOriginal.TYPE.NONE;
        if (event_go != null)
        { // 인수의 GameObject가 비어있지 않으면.
            if (event_go.tag == "Rocket")
            {
                type = EventOriginal.TYPE.ROCKET;
            }
        }
        return (type);
    }
    // 철광석이나 식물을 든 상태에서 우주선에 접촉했는지 확인
    public bool isEventIgnitable(ItemOriginal.TYPE carried_item, GameObject event_go)
    {
        bool ret = false;
        EventOriginal.TYPE type = EventOriginal.TYPE.NONE;
        if (event_go != null)
        {
            type = this.getEventType(event_go); // 이벤트 타입을 구한다.
        }
        switch (type)
        {
            case EventOriginal.TYPE.ROCKET:
                if (carried_item == ItemOriginal.TYPE.IRON)
                { // 가지고 있는 것이 철광석이라면.
                    ret = true; // '이벤트할 수 있어요！'라고 응답한다.
                }
                if (carried_item == ItemOriginal.TYPE.PLANT)
                { // 가지고 있는 것이 식물이라면.
                    ret = true; // '이벤트할 수 있어요！'라고 응답한다.
                }
                break;
        }
        return (ret);
    }
    // 지정된 게임 오브젝트의 이벤트 타입 반환
    public string getIgnitableMessage(GameObject event_go)
    {
        string message = "";
        EventOriginal.TYPE type = EventOriginal.TYPE.NONE;
        if (event_go != null)
        {
            type = this.getEventType(event_go);
        }
        switch (type)
        {
            case EventOriginal.TYPE.ROCKET:
                message = "수리한다";
                break;
        }
        return (message);
    }
}
