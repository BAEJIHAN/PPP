using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HPExpInfo : MonoBehaviour
{
    PointerEventData a_EDCurPos;
    public Text InfoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //IsPointerOverUIObject();
    }

    private void FixedUpdate()
    {
        transform.position = Input.mousePosition + new Vector3(55, -25, 0); ;
        
    }

   // using UnityEngine.EventSystems;
    public bool IsPointerOverUIObject()
    {   //마우스가 UI를 위에 있는지? 아닌지? 를 확인 하는 함수
        a_EDCurPos = new PointerEventData(EventSystem.current);

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

			List<RaycastResult> results = new List<RaycastResult>();
			for (int i = 0; i < Input.touchCount; ++i)
			{
				a_EDCurPos.position = Input.GetTouch(i).position;  
				results.Clear();
				EventSystem.current.RaycastAll(a_EDCurPos, results);
                if (0 < results.Count)
                    return true;
			}

			return false;
#else
        a_EDCurPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(a_EDCurPos, results);
        string s = " ";
        for(int i=0; i<results.Count; i++)
        {
            s += results[i].gameObject.name + " ";
            
        }
        Debug.Log(s);
        if (results[0].gameObject.name=="")
        {

        }
        return (0 < results.Count);
#endif
    }//public bool IsPointerOverUIObject() 
}
