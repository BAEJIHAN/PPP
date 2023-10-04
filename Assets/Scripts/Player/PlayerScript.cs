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
    Animator Ani;
    CharacterController CC;
    GroundCheckScript GC;
    PLAYERSTATE State = PLAYERSTATE.IDLE;

    float Speed = 4;
    float RotSpeed = 8;
    string PrevAniName = "";
    int PressedKey = 0;



    // ////////점프
    float JumpVelocity = 6;
    float CurJumpVelocity = 6;
    float JumpAcc = -9.8f;
    float Mass = 1.5f;
    float JumpSpeed = 4;
    int JumpPressedKey = 0;     
    bool IsDoubleJumped=false;

    ////////구르기
    bool IsRollMove = false;
    float RollSpeed = 5;

    ////////공격A
   
    int AttackA1Damage = 1;
    int AttackA2Damage = 1;
    float AttackA1MoveSpeed=2.0f;
    float AttackA2MoveSpeed=3.0f;
    bool IsAttackA1Move = false;
    bool IsAttackA2Move = false;
    bool IsNextAttackA = false;

    ////////공격B
    
    int AttackBDamage = 1;
    float AttackBMoveSpeed=4.0f;
    bool IsAttackBMove = false;
    bool IsNextAttackB = false;

    // Start is called before the first frame update

    // /////// 공격 Spin
    float CurSpinTime = 0;
    float MaxSpinTime = 1.0f;
    int AttackSpindDamage = 1;

    //공격 Air
    int AttackAirDamage = 1;
    bool IsAttackAirReady = true;



    int DebugNum = 0;
    private void Awake()
    {
      
        Ani = GetComponent<Animator>();
        CC = gameObject.GetComponent<CharacterController>();
        GC = gameObject.GetComponent<GroundCheckScript>();
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

        AttackUpdate();

        AttackAirUpdate();

        
    }

    void MoveKeyCheck()
    {

        if (Input.GetKey(KeyCode.RightArrow))//오른쪽 키
        {
            PressedKey = PressedKey | 0b00000001;
        }

        if (Input.GetKey(KeyCode.LeftArrow))//왼쪽 키
        {
            PressedKey = PressedKey | 0b00000010;
        }

        if (Input.GetKey(KeyCode.UpArrow))//위 키
        {
            PressedKey = PressedKey | 0b00000100;
        }

        if (Input.GetKey(KeyCode.DownArrow))//아래 키
        {
            PressedKey = PressedKey | 0b00001000;
        }

        
    }

    void KeyCheck()
    {

        

        if(Input.GetKeyDown(KeyCode.D))
        {
            Ani.SetTrigger("Defend");
            PrevAniName = "Defend";
            State = PLAYERSTATE.DEFEND;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (PLAYERSTATE.ATTACKSMASHSTART != State)
            {
                Ani.SetTrigger("AttackSmashStart");
                PrevAniName = "AttackSmashStart";
                State = PLAYERSTATE.ATTACKSMASHSTART;
            }
        }


        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X))//Spin
        {
            if (PLAYERSTATE.ATTACKSPIN != State)
            {
                GValue.PlayerDamage = AttackSpindDamage;
                Ani.SetTrigger("AttackSpinPre");
                PrevAniName = "AttackSpinPre";
                State = PLAYERSTATE.ATTACKSPIN;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if((PLAYERSTATE.JUMPAIR == State|| PLAYERSTATE.JUMPDOUBLE == State)//점프 공격
                && IsAttackAirReady)
            {
                GValue.PlayerDamage = AttackAirDamage;
                Ani.SetTrigger("AttackAir");
                PrevAniName = "AttackAir";
                State = PLAYERSTATE.ATTACKAIR;
                IsAttackAirReady = false;
            }

            if (PLAYERSTATE.ATTACKA1 == State || PLAYERSTATE.ATTACKA2 == State)//A공격 후속타
            {
                IsNextAttackA = true;
            }
            
            if (PLAYERSTATE.IDLE == State || PLAYERSTATE.MOVE == State)//1타
            {

                GValue.PlayerDamage = AttackA1Damage;
                Ani.SetTrigger("AttackA1");
                PrevAniName = "AttackA1";
                State = PLAYERSTATE.ATTACKA1;

            }


        }

        if (Input.GetKeyDown(KeyCode.X))//공격 키2
        {
            if (PLAYERSTATE.ATTACKA2 == State)//A공격 후속타
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

        if (PressedKey == 0b00000100)//위 키
        {
            if(PrevAniName!="MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//아래 
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//오른쪽 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//왼쪽 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//오른쪽 + 위 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//오른쪽 + 아래 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//왼쪽  + 아래 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
            }


            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//왼쪽 + 위 키
        {
            if (PrevAniName != "MoveF")
            {
                Ani.SetTrigger("MoveF");
                PrevAniName = "MoveF";
                State = PLAYERSTATE.MOVE;
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

        if (JumpPressedKey == 0b00000100)//위 키
        {         
            Vector3 MoveStep = Vector3.forward * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001000)//아래 
        {           
            Vector3 MoveStep = -Vector3.forward * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000001)//오른쪽 키
        {
            Vector3 MoveStep = Vector3.right * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000010)//왼쪽 키
        {         
            Vector3 MoveStep = Vector3.left * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000101)//오른쪽 + 위 키
        {           
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001001)//오른쪽 + 아래 키
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00001010)//왼쪽  + 아래 키
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
            CC.Move(MoveStep);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (JumpPressedKey == 0b00000110)//왼쪽 + 위 키
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

        CurSpinTime += Time.deltaTime;
        if(CurSpinTime>MaxSpinTime)
        {
            CurSpinTime = 0;
            Ani.SetTrigger("AttackSpinPost");
            PrevAniName = "AttackSpinPost";
        }
       
        if (PressedKey == 0b00000100)//위 키
        {     
            Vector3 MoveStep = Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001000)//아래 
        {
            Vector3 MoveStep = -Vector3.forward * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000001)//오른쪽 키
        {
            Vector3 MoveStep = Vector3.right * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000010)//왼쪽 키
        {
            Vector3 MoveStep = Vector3.left * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000101)//오른쪽 + 위 키
        {
            Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001001)//오른쪽 + 아래 키
        {
            Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00001010)//왼쪽  + 아래 키
        {
            Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
        }
        else if (PressedKey == 0b00000110)//왼쪽 + 위 키
        {
            Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * Speed * Time.deltaTime;
            CC.Move(MoveStep);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
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
            if (JumpPressedKey == 0b00000100)//위 키
            {
                Vector3 MoveStep = Vector3.forward * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001000)//아래 
            {
                Vector3 MoveStep = -Vector3.forward * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000001)//오른쪽 키
            {
                Vector3 MoveStep = Vector3.right * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(90f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000010)//왼쪽 키
            {
                Vector3 MoveStep = Vector3.left * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(270f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000101)//오른쪽 + 위 키
            {
                Vector3 MoveStep = (Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(45f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001001)//오른쪽 + 아래 키
            {
                Vector3 MoveStep = (Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(135f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00001010)//왼쪽  + 아래 키
            {
                Vector3 MoveStep = (-Vector3.right - Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(225f, Vector3.up), RotSpeed * Time.deltaTime);
            }
            else if (JumpPressedKey == 0b00000110)//왼쪽 + 위 키
            {
                Vector3 MoveStep = (-Vector3.right + Vector3.forward).normalized * JumpSpeed * Time.deltaTime;
                CC.Move(MoveStep);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(315f, Vector3.up), RotSpeed * Time.deltaTime);
            }
        }

        

        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag=="MonsterAttack" &&!IsRollMove)
        {
            if (!other.gameObject.GetComponent<MonsterAttackScript>().CanHit)
                return;

            other.gameObject.GetComponent<MonsterAttackScript>().CanHit = false;

            PlayerAttack.SetActive(false);
            State = PLAYERSTATE.HIT;
            Ani.SetTrigger("Hit");
            PrevAniName = "Hit";

            IsNextAttackA = false;
            IsNextAttackB = false;

            IsAttackA1Move = false;
            IsAttackA2Move = false;
            IsAttackBMove = false;

            AttackTrail.SetActive(false);

            DebugNum++;
            SampleMgr.Inst.DText.text = DebugNum.ToString();
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


}
