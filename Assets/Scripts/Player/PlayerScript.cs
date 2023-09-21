using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYERSTATE
{
    IDLE,
    MOVE,    
    JUMPAIR,
    JUMPEND,
    JUMPDOUBLE,
    ROLL,
    ATTACKA1,
    ATTACKA2,
    ATTACKA3,
    ATTACKB1,
    ATTACKB2,
    ATTACKB3,
    ATTACKSPIN,
    ATTACKSMASHSTART,
    ATTACKSMASHCASTING,
    ATTACKSMASH
}
public partial class PlayerScript : MonoBehaviour
{

  
    Animator Ani;
    CharacterController CC;

    PLAYERSTATE State = PLAYERSTATE.IDLE;

    float Speed = 5;
    float RotateSpeed = 8;
    string PrevAniName = "";
    int PressedKey = 0;



    // ////////����
    float JumpVelocity = 6;
    float CurJumpVelocity = 6;
    float JumpAcc = -9.8f;
    float Mass = 1.5f;
    float JumpSpeed = 4;
    int JumpPressedKey = 0;     
    bool IsDoubleJumped=false;

    ////////������
    bool IsRollMove = false;
    float RollSpeed = 7;

    ////////����A
    int AttackACombo = 0;

    ////////����B
    int AttackBCombo = 0;
    // Start is called before the first frame update

    // /////// ���� Spin
    float CurSpinTime = 0;
    float MaxSpinTime = 1.0f;
    private void Awake()
    {
      
        Ani = GetComponent<Animator>();
        CC = gameObject.GetComponent<CharacterController>();
    }
    void Start()
    {
        SetAttackASpeed(1.0f);
        SetAttackBSpeed(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    
        
        PressedKey = 0;

        MoveKeyCheck();

        KeyCheck();

        SmashCastingUpdate();
    }

    private void FixedUpdate()
    {
        MoveUpdate();

        JumpUpdate();

        RollUpdate();

        SpinUpdate();
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
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (PLAYERSTATE.ATTACKSMASHSTART != State)//2Ÿ
            {
                Ani.SetTrigger("AttackSmashStart");
                PrevAniName = "AttackSmashStart";
                State = PLAYERSTATE.ATTACKSMASHSTART;
            }
        }


        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X))//���� Ű
        {
            if (PLAYERSTATE.ATTACKSPIN != State)//2Ÿ
            {
                Ani.SetTrigger("AttackSpinPre");
                PrevAniName = "AttackSpinPre";
                State = PLAYERSTATE.ATTACKSPIN;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))//���� Ű1
        {
            if (PLAYERSTATE.ATTACKA1 == State)//2Ÿ
            {
                AttackACombo = 1;
            }
            if (PLAYERSTATE.ATTACKA2 == State)
            {
                AttackACombo = 2;
            }
            if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)//1Ÿ
            {
                if(AttackACombo==0)
                {
                    Ani.SetTrigger("AttackA1");
                    PrevAniName = "AttackA1";
                    State = PLAYERSTATE.ATTACKA1;
                }
            }
            
           
        }

        if (Input.GetKeyDown(KeyCode.X))//���� Ű2
        {
            if (PLAYERSTATE.ATTACKB1 == State)//2Ÿ
            {
                AttackBCombo = 1;
            }
            if (PLAYERSTATE.ATTACKB2 == State)
            {
                AttackBCombo = 2;
            }
            if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)//1Ÿ
            {
                if (AttackBCombo == 0)
                {
                    Ani.SetTrigger("AttackB1");
                    PrevAniName = "AttackB1";
                    State = PLAYERSTATE.ATTACKB1;
                }
            }


        }


        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyDown(KeyCode.C) && (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State))
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
            CC.Move(MoveStep);

            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotateSpeed * Time.deltaTime);
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
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotateSpeed * Time.deltaTime);
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
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotateSpeed * Time.deltaTime);
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
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//������ + �� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotateSpeed * Time.deltaTime);
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
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotateSpeed * Time.deltaTime);
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

       

        CurJumpVelocity += Time.deltaTime * JumpAcc* Mass;
        CC.Move(CurJumpVelocity * Time.deltaTime * Vector3.up);      

        if (JumpPressedKey == 0b00000100)//�� Ű
        {         
            Vector3 MoveStep = Vector3.forward * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001000)//�Ʒ� 
        {           
            Vector3 MoveStep = -Vector3.forward * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000001)//������ Ű
        {
            Vector3 MoveStep = Vector3.right * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000010)//���� Ű
        {         
            Vector3 MoveStep = Vector3.left * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000101)//������ + �� Ű
        {           
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000110)//���� + �� Ű
        {      
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotateSpeed * Time.deltaTime);
        }


       
        if (CC.isGrounded)
        {
            Ani.SetTrigger("JumpEnd");
            PrevAniName = "JumpEnd";
            State = PLAYERSTATE.JUMPEND;
            IsDoubleJumped = false;
            CurJumpVelocity = 0;
        }

    }

    void RollUpdate()
    {
        if (PLAYERSTATE.ROLL != State)
            return;
        if (!IsRollMove)
            return;
        Vector3 MoveStep = transform.forward * RollSpeed * Time.deltaTime;
        CC.Move(MoveStep);
    }

    void SpinUpdate()
    {
        if (PLAYERSTATE.ATTACKSPIN != State)
            return;

        CurSpinTime += Time.deltaTime;
        if(CurSpinTime>MaxSpinTime)
        {
            CurSpinTime = 0;
            Ani.SetTrigger("AttackSpinPost");
            PrevAniName = "AttackSpinPost";
        }
       


        if (PressedKey == 0b00000100)//�� Ű
        {     
            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//�Ʒ� 
        {
            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//������ Ű
        {
            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//���� Ű
        {
            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//������ + �� Ű
        {
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//���� + �� Ű
        {
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotateSpeed * Time.deltaTime);
        }
       
    }

    void SmashCastingUpdate()
    {
        if (PLAYERSTATE.ATTACKSMASHCASTING != State)
            return;

        if(Input.GetKeyUp(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmash");
            PrevAniName = "AttackSmash";
            State = PLAYERSTATE.ATTACKSMASH;
        }
    }
    void SetAttackASpeed(float speed)
    {
        Ani.SetFloat("AttackASpeed", speed);
    }
    void SetAttackBSpeed(float speed)
    {
        Ani.SetFloat("AttackBSpeed", speed);
    }


}
