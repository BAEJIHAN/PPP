using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HPbarScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject InfoUI;
    public Text InfoUIText;
  
    public void OnPointerEnter(PointerEventData eventData)
    {

        InfoUI.SetActive(true);
        InfoUIText.text =  "HP "+GValue.CurPlayerHP.ToString() + "/" + GValue.MaxPlayerHP.ToString();
       

    }

    public void OnPointerExit(PointerEventData eventData)
    {
      
        InfoUI.SetActive(false);
        

    }
}
