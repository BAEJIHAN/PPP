using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//public enum BMONSTATE
//{
//    NONE,
//    IDLE,
//    MOVE,
//    HIT,
//    ROAR,
//    DEATH,
//    PREATTACK,
//    ATTACK1,
//    ATTACK2,
//    ATTACK3,
//    ATTACK4,
//    ATTACK5,
//    ATTACK6,
//    JUMP1,
//    JUMP2,
//    TAKEOFF,
//    FLY,
//    LAND,
//    RUN,
//}

/*
공격 종류 
물기 꼬리 돌진 달리기 불뿜기 날아서불쏘기 
attack1
attack2
attack3
attack4 roar로 시작
attack5
attack6 takeoff
*/
public partial class RedDragonScript :  BossRootScript
{

    public GameObject JawAttack;
    public GameObject TailAttack1;
    public GameObject TailAttack2;
    public GameObject GroundFire;
    public GameObject FireAttackObj1;
    public GameObject FireAttackObj2;
    public GameObject Fireball;
    public GameObject FireballPos;
    public GameObject WingSoundObj;
    public GameObject StepSoundObj;

    AudioSource WingSound;
    AudioSource StepSound;
    //FlyAttack
    float CurFlyAttackTime=0;
    float MaxFlyAttackTime=3;
    int FlyAttackCount = 0;

    //ChargeAttack
    int ChargePhase = 0;
    float ChargePhase1Speed = 0.5f;
    float ChargePhase2Speed = 8.0f;
    float ChargePhase3Speed = 1.0f;

    //RunAttack
    float RunTime = 3;

    float RunSpeed = 4;
    bool IsRunning = false;

    float Speed = 3;
    float RotSpeed=10;


    int PreAttackCount = 0;
    Vector3 PreAttackPos = Vector3.zero;
    float PreAttackDist = 0;

    float AttackRotSpeed = 8;
    float CurAttackRotTime=0;
    float MaxAttackRotTime=1f;

    bool IsAttackRot = false;

    int PreRanNum = -1;
    new  void Awake()
    {
        base.Awake();

        WingSound = WingSoundObj.GetComponent<AudioSource>();
        StepSound = StepSoundObj.GetComponent<AudioSource>();
    }

    new void Start()
    {
        base.Start();
        //State = BMONSTATE.ROAR;
        //Ani.SetTrigger("Roar");


        State = BMONSTATE.ROAR;
        Ani.SetTrigger("Roar");
        
        PreAni = "Roar";
    }

    // Update is called once per frame
    void Update()
    {
        FlyUpdate();
        
    }

    private void FixedUpdate()
    {
        ChargeUpdate();

        RunUpdate();

        PreAttackUpdate();

        AttackRotUpdate();
    }

    void FlyUpdate()
    {
        if(BMONSTATE.FLY!=State)
        {
            return;
        }

        Vector3 RotDir = Player.transform.position - transform.position;
        RotDir.y = 0;
        RotDir.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RotDir), RotSpeed * Time.deltaTime);


        CurFlyAttackTime += Time.deltaTime;
        if(CurFlyAttackTime > MaxFlyAttackTime )
        {
            if(FlyAttackCount>=3)
            {
                FlyAttackCount = 0;
                CurFlyAttackTime = 0;
                State = BMONSTATE.LAND;
                Ani.SetTrigger("Land");
                PreAni = "Land";
                return;
            }
            FlyAttackCount++;
            CurFlyAttackTime = 0;
            State = BMONSTATE.ATTACK5;
            Ani.SetTrigger("Attack5");
            PreAni = "Attack5";
        }
        
    }

    void ChargeUpdate()
    {
        if (BMONSTATE.ATTACK3 != State)
        {
            return;
        }

        if(ChargePhase==1)
        {

        }
        else if(ChargePhase==2)
        {
            transform.position += transform.forward * Time.deltaTime * ChargePhase2Speed;
        }
        else if (ChargePhase == 3)
        {
        }
       
    }

    void RunUpdate()
    {
        if (!IsRunning)
            return;

        transform.position += transform.forward * Time.deltaTime * RunSpeed;
    }

    void PreAttackUpdate()
    {
        if (BMONSTATE.PREATTACK != State)
            return;

        Vector3 Dir = PreAttackPos - transform.position;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        PreAttackDist -= Speed * Time.deltaTime;
        transform.position += MoveStep;


        Vector3 RotDir = Player.transform.position - transform.position;
        RotDir.y = 0;
        RotDir.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RotDir), RotSpeed * Time.deltaTime);


        if (PreAttackDist < 0.1f)
        {
            
            
            TakeNextAction();
            
            
        }
    }

    void AttackRotUpdate()
    {
        if (!IsAttackRot)
            return;

        CurAttackRotTime += Time.deltaTime;
        if(CurAttackRotTime >MaxAttackRotTime)
        {
            CurAttackRotTime = 0;
            IsAttackRot = false;
        }

        Vector3 RotDir = Player.transform.position - transform.position;
        RotDir.y = 0;
        RotDir.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RotDir), AttackRotSpeed * Time.deltaTime);


    }
    override public void TakeDamage(int HDamage)
    {
        if (BMONSTATE.ATTACK5 == State ||
            BMONSTATE.FLY == State ||
            BMONSTATE.LAND == State)
            return;

        AClip = Resources.Load<AudioClip>("Sound/DragonHit");
        ASource.PlayOneShot(AClip);

        

        CurHP -= HDamage;
        if (CurHP < 0)
        {
            for (int i = 0; i < HitCols.Length; i++)
            {
                HitCols[i].SetActive(false);

            }

            StepSound.Stop();

            AClip = Resources.Load<AudioClip>("Sound/DragonDeath");
            ASource.PlayOneShot(AClip);

            JawAttack.SetActive(false);
            TailAttack1.SetActive(false);
            TailAttack2.SetActive(false);
            GroundFire.SetActive(false);
            FireAttackObj1.SetActive(false);
            FireAttackObj2.SetActive(false);
            Fireball.SetActive(false);                      

            State = BMONSTATE.DEATH;
            Ani.SetTrigger("Death");
            PreAni = "Death";
            Vector3 temp = transform.position;
            temp.y = 0;
            transform.position = temp;

            ItemSpawnerScript.Inst.BossSpawnItem(transform.position);

        }

    }

    /*
공격 종류 
물기 꼬리 돌진 달리기 불뿜기 날아서불쏘기 
attack1 물기
attack2 꼬리
attack3 돌진
attack4 달리기 roar로 시작
attack5 날아서 불 쏘기 takeoff 로 시작
attack6 불뿜기

    돌진, 달리기, 날아서 불 쏘기, 불 뿜기, PreAttack은 멀리서,
    나머지는 가까이서
*/
    void TakeNextAction()
    {
        Vector3 PlayerPos=Player.transform.position;
        PlayerPos.y = 0;
        float Dist = (transform.position - PlayerPos).magnitude;
        int RanNum = 0;
        
        while(true)
        {
            if (Dist > 4)
            {
                RanNum = Random.Range(0, 5);
            }
            else
            {
                RanNum = Random.Range(5, 8);
            }

            if(RanNum!=PreRanNum) 
            {
                PreRanNum= RanNum;
                break;
            }
        }     

        if (RanNum==0)
        {
            IsAttackRot = false;
        }
        else
        {
            IsAttackRot = true;
        }

        if(RanNum==0 || RanNum == 7)//preattack
        {
            SetPreAttack();
        }
        else if(RanNum==1)//돌진
        {
            State = BMONSTATE.ATTACK3;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";
        }
        else if (RanNum == 2)//달리기
        {
            State = BMONSTATE.ATTACK4;
            Ani.SetTrigger("Roar");
            PreAni = "Roar";
        }
        else if (RanNum == 3)//불 뿜기
        {
            State = BMONSTATE.ATTACK6;
            Ani.SetTrigger("Attack6");
            PreAni = "Attack6";
        }
        else if (RanNum == 4)//날아서 불 쏘기 
        {
            State = BMONSTATE.ATTACK5;
            Ani.SetTrigger("TakeOff");
            PreAni = "TakeOff";
        }
        else if (RanNum == 5)//물기
        {
            State = BMONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";
        }
        else if (RanNum == 6)//꼬리
        {
            State = BMONSTATE.ATTACK2;
            Ani.SetTrigger("Attack2");
            PreAni = "Attack2";

        }
       
    }
    void SetPreAttack()
    {
        State = BMONSTATE.PREATTACK;
        Ani.SetTrigger("Move");
        PreAni = "Move";

        PreAttackPos = Player.transform.position;
        PreAttackPos.y = 0;
        
        PreAttackPos.x = Random.Range(-4.0f, 4.0f);               
        
        PreAttackPos.z = Random.Range(-4.0f, 4.0f); 
       



        PreAttackDist = (PreAttackPos - transform.position).magnitude;
    }

    IEnumerator RunEndFunc()
    {
        SetActiveJawAttack();

        IsRunning = true;

        yield return new WaitForSeconds(RunTime);

        StepSound.Stop();

        DeActiveJawAttack();
        IsRunning = false;

        Ani.SetTrigger("Attack2");
        PreAni = "Attack2";
    }

    void JawSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/DragonBite");
        ASource.PlayOneShot(AClip);
    }

    void RoarSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/DragonRoar");
        ASource.PlayOneShot(AClip);
    }

    void FireSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/DragonFire");
        ASource.PlayOneShot(AClip);
    }

    void AirAttackSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/DragonAirAttack");
        ASource.PlayOneShot(AClip);
    }

    void WingSoundStartFunc()
    {
        WingSound.Play();
    }

    void TailSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/DragonTailAttack");
        ASource.PlayOneShot(AClip);
    }



}
