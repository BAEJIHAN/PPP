using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYERSTATE
{
    IDLE,
    MOVE,
    ATTACK,    
    JUMPAIR,
    JUMPEND,
    JUMPDOUBLE,
    ROLL
}
public partial class PlayerScript : MonoBehaviour
{
    Animator Ani;
    Rigidbody Rb;

    PLAYERSTATE State = PLAYERSTATE.IDLE;

    float Speed = 5;
    float RotateSpeed = 8;
    string PrevAniName = "";
    int PressedKey = 0;



    // ////////����
    float JumpVelocity = 7;
    float CurJumpVelocity = 7;
    float JumpAcc = -9.8f;
    int JumpPressedKey = 0;     
    bool IsDoubleJumped=false;

    ////////������
    bool IsRollMove = false;
    float RollSpeed = 7;
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

        MoveKeyCheck();

        KeyCheck();
    }

    private void FixedUpdate()
    {
        MoveUpdate();

        JumpUpdate();

        RollUpdate();
    }

    void MoveKeyCheck()
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

    void KeyCheck()
    {
        if (Input.GetKeyDown(KeyCode.Z))//���� Ű
        {
            if (PrevAniName != "Attack1")
            {
                Ani.SetTrigger("Attack1");
                PrevAniName = "Attack1";
                State = PLAYERSTATE.ATTACK;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {            

            if (PLAYERSTATE.IDLE==State || PLAYERSTATE.MOVE == State)
            {
                Ani.SetTrigger("JumpAir");
                PrevAniName = "JumpAir";
                State = PLAYERSTATE.JUMPAIR;
                CurJumpVelocity = JumpVelocity;
                JumpPressedKey = PressedKey;
            }
            else if(PLAYERSTATE.JUMPAIR == State && false==IsDoubleJumped)
            {
               

                Ani.SetTrigger("JumpDouble");
                PrevAniName = "JumpDouble";
                State = PLAYERSTATE.JUMPDOUBLE;
                CurJumpVelocity = JumpVelocity;
                JumpPressedKey = PressedKey;
                IsDoubleJumped = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && State != PLAYERSTATE.ROLL)
        {
            Ani.SetTrigger("Roll");
            PrevAniName = "Roll";
            State = PLAYERSTATE.ROLL;
         
        }
    }

    void MoveUpdate()//
    {
        if (!(PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State))
            return;

        

        if (PressedKey == 0b00000100)//�� Ű
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

    void JumpUpdate()
    {
        if(!(PLAYERSTATE.JUMPAIR==State || PLAYERSTATE.JUMPDOUBLE == State))
        {
            return;
        }

        CurJumpVelocity += Time.deltaTime * JumpAcc;
        Rb.MovePosition(Rb.position + CurJumpVelocity*Time.deltaTime*Vector3.up);

        if (JumpPressedKey == 0b00000100)//�� Ű
        {         
            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001000)//�Ʒ� 
        {           
            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000001)//������ Ű
        {
            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000010)//���� Ű
        {         
            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000101)//������ + �� Ű
        {           
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000110)//���� + �� Ű
        {      
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            Rb.MovePosition(Rb.position + MoveStep);
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        


    }

    void RollUpdate()
    {
        if (PLAYERSTATE.ROLL != State)
            return;
        if (!IsRollMove)
            return;
        Vector3 MoveStep = transform.forward * RollSpeed * Time.deltaTime;
        Rb.MovePosition(Rb.position + MoveStep);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            if (PLAYERSTATE.JUMPAIR == State)
            {

                Ani.SetTrigger("JumpEnd");
                PrevAniName = "JumpEnd";
                State = PLAYERSTATE.JUMPEND;
                IsDoubleJumped = false;
            }
        }
    }




}
