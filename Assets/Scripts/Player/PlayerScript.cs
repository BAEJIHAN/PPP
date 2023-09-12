using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Animator Ani;
    Rigidbody Rb;


    string PrevAniName = "";
    int PressedKey = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        Ani = GetComponent<Animator>();
        Rb = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PressedKey = 0;

        KeyCheck();
    }

    private void FixedUpdate()
    {
        MoveUpdate();
    }

    void KeyCheck()
    {

        if (Input.GetKey(KeyCode.RightArrow))//������ Ű
        {
            PressedKey = PressedKey | 0b00000001;
        }

        if (Input.GetKey(KeyCode.LeftArrow))//���� Ű
        {
            PressedKey = PressedKey | 0b00000010;
        }

        if (Input.GetKey(KeyCode.UpArrow))//�� Ű
        {
            PressedKey = PressedKey | 0b00000100;
        }

        if (Input.GetKey(KeyCode.DownArrow))//�Ʒ� Ű
        {
            PressedKey = PressedKey | 0b00001000;
        }

        
    }

    void MoveUpdate()//
    {
        
        if(PressedKey == 0b00000100)//�� Ű
        {
            if(PrevAniName!="MoveF")
            {
                Ani.SetTrigger("MoveF");
            }
            
            PrevAniName = "MoveF";
            Rb.AddForce(transform.forward*30.0f);
        }


       

        if (PressedKey == 0b00000110)//��+���� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
            }

            PrevAniName = "MoveF";
            Vector3 tempDir = Vector3.zero;
            tempDir = transform.forward - transform.right;
            tempDir.Normalize();
            Rb.AddForce(tempDir * 30.0f);

        }

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    Ani.SetTrigger("MoveB");
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    Ani.SetTrigger("MoveR");
        //}

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    Ani.SetTrigger("MoveL");
        //}
    }
}
