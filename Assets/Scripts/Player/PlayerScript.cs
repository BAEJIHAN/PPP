using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYERSTATE
{
    IDLE,
    MOVE,
    ATTACK
}
public class PlayerScript : MonoBehaviour
{
    Animator Ani;
    Rigidbody Rb;

    PLAYERSTATE State = PLAYERSTATE.IDLE;

    float Speed = 5;
    float RotateSpeed = 8;
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

        if (Input.GetKeyDown(KeyCode.Z))//���� Ű
        {
            if (PrevAniName != "Attack1")
            {
                Ani.SetTrigger("Attack1");
                PrevAniName = "Attack1";
                State = PLAYERSTATE.ATTACK;
            }
        }
    }

    void MoveUpdate()//
    {
        if (PLAYERSTATE.ATTACK == State)
            return;

        if(PressedKey == 0b00000100)//�� Ű
        {
            if(PrevAniName!="MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }
            
            
            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//�Ʒ� 
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//������ Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//���� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//������ + �� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = (Vector3.right+Vector3.forward).normalized* Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

           
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = (Vector3.right-Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

           
            Vector3 MoveStep = (-Vector3.right-Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//���� + �� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }

            
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);

            
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else
        {
            if (PrevAniName != "Idle")
            {
                Ani.SetTrigger("Idle");
                PrevAniName = "Idle";
                State = PLAYERSTATE.IDLE;
            }
            
        }

        

       






    }



    ///  ////////////////////////////////// �ִϸ��̼� �̺�Ʈ �Լ�
    void Attack1Func()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }
}
