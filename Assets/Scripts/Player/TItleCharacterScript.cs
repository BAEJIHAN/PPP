using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleCharacterScript : MonoBehaviour
{
    Animator Ani;
    private void Awake()
    {
        Ani= GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStartFunc()
    {
        Ani.SetTrigger("Victory");
    }

    void VictoryEndFunc()
    {
        Ani.SetTrigger("Idle");
    }

 
}
