using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MobileMgr : MonoBehaviour
{
    PlayerScript Player;
    
    [Header("------- JoyStick -------")]
    public GameObject JoySBack;
    public Image JoyStick;
    float Radius = 0.0f;
    Vector3 OriginPos = Vector3.zero;
    Vector3 Axis = Vector3.zero;
    Vector3 JsCacVec = Vector3.zero;
    float JsCacDist = 0.0f;


    [Header("------- Button -------")]
    public Button ZBtn;
    public Button XBtn;
    public Button ABtn;
    public Button DBtn;
    public Button CBtn;
    public Button SpaceBtn;

    // Start is called before the first frame update
    void Start()
    {

        Player = FindObjectOfType<PlayerScript>();

        if (JoySBack != null && JoyStick != null)
        {       

            Vector3[] v = new Vector3[4];
            JoySBack.GetComponent<RectTransform>().GetWorldCorners(v);
           
            Radius = v[2].y - v[0].y;
            Radius = Radius / 3.0f;

            OriginPos = JoyStick.transform.position;
           
            EventTrigger trigger = JoySBack.GetComponent<EventTrigger>();
           
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) =>
            {
                OnDragJoyStick((PointerEventData)data);
            });
            trigger.triggers.Add(entry);



            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) =>
            {
                OnEndDragJoyStick((PointerEventData)data);
            });
            trigger.triggers.Add(entry);
        }

        //////////////////////////////////////////////////////////
        
        if(ZBtn!= null)
        {
            EventTrigger trigger = ZBtn.GetComponent<EventTrigger>();
          
            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                ZBtnDownFunc();
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) =>
            {
                ZBtnUpFunc();
            });
            trigger.triggers.Add(entry);
        }       
                
        if (XBtn != null)
        {
            EventTrigger trigger = XBtn.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                XBtnDownFunc();
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) =>
            {
                XBtnUpFunc();
            });
            trigger.triggers.Add(entry);
        }

        if (CBtn != null)
        {
            EventTrigger trigger = CBtn.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                CBtnDownFunc();
            });
            trigger.triggers.Add(entry);
        }

        if (SpaceBtn != null)
        {
            EventTrigger trigger = SpaceBtn.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                SpaceBtnDownFunc();
            });
            trigger.triggers.Add(entry);
        }

        if (ABtn != null)
        {
            EventTrigger trigger = ABtn.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                ABtnDownFunc();
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) =>
            {
                ABtnUpFunc();
            });
            trigger.triggers.Add(entry);
        }

        if (DBtn != null)
        {
            EventTrigger trigger = DBtn.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                DBtnDownFunc();
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) =>
            {
                DBtnUpFunc();
            });
            trigger.triggers.Add(entry);
        }
    }

   

    // Update is called once per frame
    void Update()
    {

    }

    void OnDragJoyStick(PointerEventData a_data)
    {
        if (JoyStick == null)
            return;

        JsCacVec = (Vector3)a_data.position - OriginPos;
        JsCacVec.z = 0.0f;
        JsCacDist = JsCacVec.magnitude;
        Axis = JsCacVec.normalized;

        //조이스틱 백그라운드를 벗어나지 못하게 막는 부분
        if (Radius < JsCacDist)
        {
            JoyStick.transform.position =
                            OriginPos + Axis * Radius;
        }
        else
        {
            JoyStick.transform.position =
                            OriginPos + Axis * JsCacDist;
        }

        if (1.0f < JsCacDist)
            JsCacDist = 1.0f;

        
         if (Player != null)
            Player.SetJoyStickMv(JsCacDist, Axis);
    }

    void OnEndDragJoyStick(PointerEventData a_data)
    {
        if (JoyStick == null)
            return;

        Axis = Vector3.zero;
        JoyStick.transform.position = OriginPos;
        JsCacDist = 0.0f;

        //캐릭터 정지처리
        if (Player != null)
            Player.SetJoyStickMv(0.0f, Vector3.zero);
    }

    void ZBtnDownFunc()
    {
        Player.ZBtnDown();
    }

    void ZBtnUpFunc()
    {
        Player.ZBtnUp();
    }

    private void XBtnDownFunc()
    {
        Player.XBtnDown();
    }

    private void XBtnUpFunc()
    {
        Player.XBtnUp();
    }
    private void SpaceBtnDownFunc()
    {
        Player.SpaceBtn();
    }

    private void CBtnDownFunc()
    {
        Player.CBtn();
    }

    private void ABtnDownFunc()
    {
        Player.ABtnDown();
    }

    private void ABtnUpFunc()
    {
        Player.ABtnUp();
    }

    private void DBtnDownFunc()
    {
        Debug.Log(1);
        Player.DBtnDown();
    }

    private void DBtnUpFunc()
    {
        Player.DBtnUp();
    }
}
