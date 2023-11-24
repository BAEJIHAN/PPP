using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.AudioSettings;

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
    ATTACKB,
   
    ATTACKSPIN,
    ATTACKSMASHSTART,
    ATTACKSMASHCASTING,
    ATTACKSMASH,
    ATTACKAIR,
    DEFEND,
    HIT,
}
public partial class PlayerScript : MonoBehaviour
{

    public GameObject PlayerAttack;
    public GameObject AttackTrail;
    public GameObject SmashAttack;
    Animator Ani;
    CharacterController CC;
    GroundCheckScript GC;
    GroundEffectScript GE;
    PLAYERSTATE State = PLAYERSTATE.IDLE;

    DrakeScript Drake;

    float Speed = 4;
    float RotSpeed = 8;
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
    float RollSpeed = 5;

    ////////����A
   
    int AttackA1Damage = 1;
    int AttackA2Damage = 1;
    float AttackA1MoveSpeed=2.0f;
    float AttackA2MoveSpeed=3.0f;
    bool IsAttackA1Move = false;
    bool IsAttackA2Move = false;
    bool IsNextAttackA = false;
    bool IsZBtnOn = false;
    
    ////////����B
    
    int AttackBDamage = 1;
    float AttackBMoveSpeed=4.0f;
    bool IsAttackBMove = false;
    bool IsNextAttackB = false;
    bool IsXBtnOn = false;
    // Start is called before the first frame update

    // /////// ���� Spin
    float CurSpinTime = 0;
    float MaxSpinTime = 1.0f;
    int AttackSpindDamage = 1;
    public bool IsSpin = false;

    //���� Air
    int AttackAirDamage = 1;
    bool IsAttackAirReady = true;

    //���� smash
    bool IsABtnOn = false;
    //���
    bool IsDBtnOn = false;




    //����� �̵�
    float JoyMvLen = 0.0f;
    Vector3 JoyMvDir = Vector3.zero;


    private void Awake()
    {
      
        Ani = GetComponent<Animator>();
        CC = gameObject.GetComponent<CharacterController>();
        GC = gameObject.GetComponent<GroundCheckScript>();
        Drake = gameObject.GetComponentInChildren<DrakeScript>();
        GE = SmashAttack.GetComponent<GroundEffectScript>();
    }
    void Start()
    {
        SetAttackASpeed(1.5f);
        SetAttackBSpeed(1.5f);

        SetAddAttackRange(0);
    }

    // Update is called once per frame
    void Update()
    {
        DebugUpdate();              

        PressedKey = 0;

        MoveKeyCheck();

        KeyCheck();

        SmashCastingUpdate();
    }

    private void FixedUpdate()
    {
        MoveUpdate();

        JoyStickMvUpdate();

        JumpUpdate();

        RollUpdate();

        SpinUpdate();

        AttackUpdate();

        AttackAirUpdate();

        
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

        if(Input.GetKeyDown(KeyCode.D))
        {
            if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
            {
                Ani.SetTrigger("Defend");
                PrevAniName = "Defend";
                State = PLAYERSTATE.DEFEND;
            }
            IsDBtnOn = true;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
            IsDBtnOn = false;

        }

        


        if (Input.GetKeyDown(KeyCode.A))
        {
            if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
            {
                Ani.SetTrigger("AttackSmashStart");
                PrevAniName = "AttackSmashStart";
                State = PLAYERSTATE.ATTACKSMASHSTART;
                SmashAttack.SetActive(true);
                GE.IsCharge = true;
            }
            IsABtnOn = true;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            IsABtnOn = false;
        }


            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X))//Spin
        {
            if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
            {                
                GValue.PlayerDamage = AttackSpindDamage;
                Ani.SetTrigger("AttackSpinPre");
                PrevAniName = "AttackSpinPre";
                State = PLAYERSTATE.ATTACKSPIN;
                IsSpin = true;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if((PLAYERSTATE.JUMPAIR == State|| PLAYERSTATE.JUMPDOUBLE == State)//���� ����
                && IsAttackAirReady)
            {
                GValue.PlayerDamage = AttackAirDamage;
                Ani.SetTrigger("AttackAir");
                PrevAniName = "AttackAir";
                State = PLAYERSTATE.ATTACKAIR;
                IsAttackAirReady = false;
            }

            if (PLAYERSTATE.ATTACKA1 == State || PLAYERSTATE.ATTACKA2 == State)//A���� �ļ�Ÿ
            {
                IsNextAttackA = true;
            }
            
            if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)//1Ÿ
            {

                GValue.PlayerDamage = AttackA1Damage;
                Ani.SetTrigger("AttackA1");
                PrevAniName = "AttackA1";
                State = PLAYERSTATE.ATTACKA1;

            }


        }

        if (Input.GetKeyDown(KeyCode.X))//���� Ű2
        {
            if (PLAYERSTATE.ATTACKA2 == State)//A���� �ļ�Ÿ
            {
                IsNextAttackB = true;
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

        if (SampleMgr.Inst.IsMobile)
            return;

        if (PressedKey == 0b00000100)//�� Ű
        {
            if(PrevAniName!="MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//�Ʒ� 
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//������ Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//���� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//������ + �� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }


            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//���� + �� Ű
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)                
                    Drake.SetAni("Move");
                
               
            }


            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else
        {
            if (PrevAniName != "Idle")
            {
                
                Ani.SetTrigger("Idle");
                PrevAniName = "Idle";
                State = PLAYERSTATE.IDLE;

                if(Drake!=null)                
                    Drake.SetAni("Idle");                
                
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001000)//�Ʒ� 
        {           
            Vector3 MoveStep = -Vector3.forward * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000001)//������ Ű
        {
            Vector3 MoveStep = Vector3.right * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000010)//���� Ű
        {         
            Vector3 MoveStep = Vector3.left * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000101)//������ + �� Ű
        {           
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000110)//���� + �� Ű
        {      
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
        }

        if (GC.IsGrounded())
        {
            Ani.SetTrigger("JumpEnd");
            PrevAniName = "JumpEnd";
            State = PLAYERSTATE.JUMPEND;
            IsDoubleJumped = false;
            IsAttackAirReady = true;
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

        if (!IsSpin)
            return;

        CurSpinTime += Time.deltaTime;
        if(CurSpinTime>MaxSpinTime)
        {
            CurSpinTime = 0;
            TrailOffFunc();
            AttackSpinOnEnd();

        }
       
        if (PressedKey == 0b00000100)//�� Ű
        {     
            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//�Ʒ� 
        {
            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//������ Ű
        {
            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//���� Ű
        {
            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//������ + �� Ű
        {
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//������ + �Ʒ� Ű
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//����  + �Ʒ� Ű
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//���� + �� Ű
        {
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
        }
       
    }

    void SmashCastingUpdate()
    {
        if (!(PLAYERSTATE.ATTACKSMASHSTART==State || 
            PLAYERSTATE.ATTACKSMASHCASTING == State)
            )
            return;

        if(Input.GetKeyUp(KeyCode.A) || !IsABtnOn)
        {
            if(PLAYERSTATE.ATTACKSMASHCASTING == State)
            {
                Ani.SetTrigger("AttackSmash");
                PrevAniName = "AttackSmash";
                State = PLAYERSTATE.ATTACKSMASH;
                GE.IsCharge = false;
            }
            else if(PLAYERSTATE.ATTACKSMASHSTART == State)
            {
                Ani.SetTrigger("Idle");
                PrevAniName = "Idle";
                State = PLAYERSTATE.IDLE;
                GE.IsCharge = false;
                SmashAttack.SetActive(false);
            }
           
        }
       
    }

    void AttackUpdate()
    {
        if(PLAYERSTATE.ATTACKA1 != State && PLAYERSTATE.ATTACKA2 != State && PLAYERSTATE.ATTACKB != State)
        {
            return;
        }

        if(IsAttackA1Move)
        {
            Vector3 MoveStep = transform.forward*AttackA1MoveSpeed * Time.deltaTime;
            CC.Move(MoveStep);
        }
        else if (IsAttackA2Move)
        {
            Vector3 MoveStep = transform.forward * AttackA2MoveSpeed * Time.deltaTime;
            CC.Move(MoveStep);
        }
        else if (IsAttackBMove)
        {
            Vector3 MoveStep = transform.forward * AttackBMoveSpeed * Time.deltaTime;
            CC.Move(MoveStep);
        }
    }

    void AttackAirUpdate()
    {
        if (PLAYERSTATE.ATTACKAIR != State)
            return;

        CurJumpVelocity += Time.deltaTime * JumpAcc * Mass;
        CC.Move(CurJumpVelocity * Time.deltaTime * Vector3.up);

        if (GC.IsGrounded())
        {

            CurJumpVelocity = 0;
        }
        else
        {
            if (JumpPressedKey == 0b00000100)//�� Ű
            {
                Vector3 MoveStep = Vector3.forward * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001000)//�Ʒ� 
            {
                Vector3 MoveStep = -Vector3.forward * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000001)//������ Ű
            {
                Vector3 MoveStep = Vector3.right * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000010)//���� Ű
            {
                Vector3 MoveStep = Vector3.left * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000101)//������ + �� Ű
            {
                Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001001)//������ + �Ʒ� Ű
            {
                Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001010)//����  + �Ʒ� Ű
            {
                Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000110)//���� + �� Ű
            {
                Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
            }
        }

        

        
    }

    public void JoyStickMvUpdate()
    {
        if (!(PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State))
            return;

        if (!SampleMgr.Inst.IsMobile)
            return;


            //--- ���̽�ƽ �̵� �ڵ�
        if (0.0f < JoyMvLen)
        {
            Vector3 MoveStep = JoyMvDir * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(JoyMvDir), RotSpeed * Time.deltaTime);

            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;

                if (Drake != null)
                    Drake.SetAni("Move");
            }
        }
        else
        {
            if (PrevAniName != "Idle")
            {

                Ani.SetTrigger("Idle");
                PrevAniName = "Idle";
                State = PLAYERSTATE.IDLE;

                if (Drake != null)
                    Drake.SetAni("Idle");

            }
        }
    }

    public void SetJoyStickMv(float _JoyMvLen, Vector3 _JoyMvDir)
    {
        JoyMvLen = _JoyMvLen;
        if (0.0f < _JoyMvLen)
        {
            JoyMvDir = new Vector3(_JoyMvDir.x, 0.0f, _JoyMvDir.y);
            JoyMvDir.Normalize();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "MonsterAttack") && (PLAYERSTATE.DEFEND == State))
        {
            other.gameObject.GetComponent<MonsterAttackScript>().CanHit = false;

            if (!IsDefended(other.gameObject.transform.position))
            {
                PlayerAttack.SetActive(false);
                AttackTrail.SetActive(false);

                TakeDamage(0);


                State = PLAYERSTATE.HIT;
                Ani.SetTrigger("Hit");
                PrevAniName = "Hit";

                IsNextAttackA = false;
                IsNextAttackB = false;

                IsAttackA1Move = false;
                IsAttackA2Move = false;
                IsAttackBMove = false;

                SmashAttack.SetActive(false);
            }
            else
            {
                Ani.SetTrigger("DefendHit");
                PrevAniName = "DefendHit";
                
            }
            return;
        }

        if ((other.tag=="MonsterAttack") &&(!IsRollMove||!IsSpin))
        {
            if (!other.gameObject.GetComponent<MonsterAttackScript>().CanHit)
                return;

            other.gameObject.GetComponent<MonsterAttackScript>().CanHit = false;

            PlayerAttack.SetActive(false);
            AttackTrail.SetActive(false);

            TakeDamage(0);
            
            if (State==PLAYERSTATE.ATTACKSMASHCASTING || State == PLAYERSTATE.ATTACKSMASH)
            {
                return;
            }

            State = PLAYERSTATE.HIT;
            Ani.SetTrigger("Hit");
            PrevAniName = "Hit";

            IsNextAttackA = false;
            IsNextAttackB = false;

            IsAttackA1Move = false;
            IsAttackA2Move = false;
            IsAttackBMove = false;

            SmashAttack.SetActive(false);
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

    public void SetAddAttackRange(float fvalue)
    {
        PlayerAttack.SetActive(true);
        PlayerAttack.GetComponent<PlayerAttackScript>().ColScaling(fvalue);
        PlayerAttack.SetActive(false);

        AttackTrail.SetActive(true);
        AttackTrail.transform.position+=AttackTrail.transform.up*fvalue;
        
        
        AttackTrail.SetActive(false);
       

    }

    public void SetStomp()
    {
        if(!(PLAYERSTATE.ATTACKAIR==State||
            PLAYERSTATE.JUMPAIR == State||
            PLAYERSTATE.JUMPDOUBLE == State))
        {
            TakeDamage(0);

            PlayerAttack.SetActive(false);
            AttackTrail.SetActive(false);

            State = PLAYERSTATE.HIT;
            Ani.SetTrigger("Hit");
            PrevAniName = "Hit";

            IsNextAttackA = false;
            IsNextAttackB = false;

            IsAttackA1Move = false;
            IsAttackA2Move = false;
            IsAttackBMove = false;
        }
    }

    public void TakeDamage(int HDamage)
    {
       
    }

    public PLAYERSTATE GetPlayerState()
    {
        return State;
    }

    bool IsDefended(Vector3 EAttackPos)
    {

        EAttackPos.y = 0;
        Vector3 Dir = EAttackPos - transform.position;
        Dir.Normalize();

        float DotResult=Vector3.Dot(Dir, transform.forward);

        //SampleMgr.Inst.DText.text ="���� ���� ���� " + Dir.ToString()
        //   + "\n���� ���� ����" + transform.forward.ToString()
        //    + "\n���� ��" + DotResult.ToString();

        if (DotResult >= 0.5)
            return true;


        return false;

        
    }

    public void ABtnDown()
    {
        if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
        {
            Ani.SetTrigger("AttackSmashStart");
            PrevAniName = "AttackSmashStart";
            State = PLAYERSTATE.ATTACKSMASHSTART;
            SmashAttack.SetActive(true);
            GE.IsCharge = true;
        }
        IsABtnOn = true;


    }

    public void ABtnUp()
    {
        IsABtnOn = false;
    }
    public void DBtnDown()
    {
        if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
        {
            Ani.SetTrigger("Defend");
            PrevAniName = "Defend";
            State = PLAYERSTATE.DEFEND;
        }

        IsDBtnOn = true;
    }

    public void DBtnUp()
    {
        
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;

        IsDBtnOn = false;

    }
    public void ZBtnDown()
    {
        IsZBtnOn = true;

        if (IsZBtnOn && IsXBtnOn)//Spin
        {
            if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
            {
                GValue.PlayerDamage = AttackSpindDamage;
                Ani.SetTrigger("AttackSpinPre");
                PrevAniName = "AttackSpinPre";
                State = PLAYERSTATE.ATTACKSPIN;
                IsSpin = true;
                return;
            }

        }


        if ((PLAYERSTATE.JUMPAIR == State || PLAYERSTATE.JUMPDOUBLE == State)//���� ����
                && IsAttackAirReady)
        {
            GValue.PlayerDamage = AttackAirDamage;
            Ani.SetTrigger("AttackAir");
            PrevAniName = "AttackAir";
            State = PLAYERSTATE.ATTACKAIR;
            IsAttackAirReady = false;
        }

        if (PLAYERSTATE.ATTACKA1 == State || PLAYERSTATE.ATTACKA2 == State)//A���� �ļ�Ÿ
        {
            IsNextAttackA = true;
        }

        if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)//1Ÿ
        {

            GValue.PlayerDamage = AttackA1Damage;
            Ani.SetTrigger("AttackA1");
            PrevAniName = "AttackA1";
            State = PLAYERSTATE.ATTACKA1;

        }
    }

    public void ZBtnUp()
    {
        IsZBtnOn = false;
    }
    public void XBtnDown()
    {
        IsXBtnOn = true;
        if (IsZBtnOn && IsXBtnOn)//Spin
        {
            if (PLAYERSTATE.MOVE == State || PLAYERSTATE.IDLE == State)
            {
                GValue.PlayerDamage = AttackSpindDamage;
                Ani.SetTrigger("AttackSpinPre");
                PrevAniName = "AttackSpinPre";
                State = PLAYERSTATE.ATTACKSPIN;
                IsSpin = true;
                return;
            }

        }

        if (PLAYERSTATE.ATTACKA2 == State)//A���� �ļ�Ÿ
        {
            IsNextAttackB = true;
        }
    }

    public void XBtnUp()
    {
        IsXBtnOn = false;
        if (PLAYERSTATE.ATTACKA2 == State)//A���� �ļ�Ÿ
        {
            IsNextAttackB = true;
        }
    }

    public void CBtn()
    {
        if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)
        {
            Ani.SetTrigger("Roll");
            PrevAniName = "Roll";
            State = PLAYERSTATE.ROLL;
        }
    }

    public void SpaceBtn()
    {
        if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)
        {
            Ani.SetTrigger("JumpAir");
            PrevAniName = "JumpAir";
            State = PLAYERSTATE.JUMPAIR;
            CurJumpVelocity = JumpVelocity;
            JumpPressedKey = PressedKey;
        }
        else if (PLAYERSTATE.JUMPAIR == State && false == IsDoubleJumped)
        {


            Ani.SetTrigger("JumpDouble");
            PrevAniName = "JumpDouble";
            State = PLAYERSTATE.JUMPDOUBLE;
            CurJumpVelocity = JumpVelocity;
            JumpPressedKey = PressedKey;
            IsDoubleJumped = true;
        }
    }

    void DebugUpdate()
    {
        SampleMgr.Inst.DText.text = "PreAni " + PrevAniName
                                    + "\nState " + State;
    }
}
