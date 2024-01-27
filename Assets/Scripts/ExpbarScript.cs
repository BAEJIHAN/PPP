using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpbarScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject InfoUI;
    public Text InfoUIText;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
     
        InfoUI.SetActive(true);
        InfoUIText.text = "Lv" + GValue.Level.ToString() + " " + GValue.PlayerCurExp.ToString() + "/" + GValue.PlayerMaxExp.ToString();
        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(1);
        InfoUI.SetActive(false);
        

    }



}
