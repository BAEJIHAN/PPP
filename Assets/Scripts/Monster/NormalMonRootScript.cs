using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTATE
{
    NONE,
    IDLE,
    MOVE,
    HIT,
    DEATH,
    PREATTACK,
    ATTACK1,
}

public class NormalMonRootScript : MonRootScript
{
    // Start is called before the first frame update

    public GameObject MonsterAttack;

    protected MONSTATE State = MONSTATE.NONE;
    protected float Speed = 1;
    protected float RotSpeed = 1;
    protected float AttackDist = 1.5f;

    protected float CurIdleTime = 0;
    protected float MaxIdleTime = 2;

    public GameObject DropItem;

    




    protected float CurDeathTime = 0;
    protected float MaxDeathTime = 5;

    Vector3 TargetPos;

    Vector3 SpinAttackPos;
    protected bool IsSpinHit = false;
    protected float SpinHitSpeed = 5;
    protected float MaxSpinHitTime=0.1f;
    protected float CurSpinHitTime = 0;

    float PreAttackTime;

    bool IsOnEvent = false;
    protected void Awake()
    {
        Ani = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        ASource=GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    protected void Start()
    {

        Player = GameObject.Find("Player");
        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "IDLE";

        MaxHP = 3;
        CurHP = 3;

        PreAttackTime = Random.Range(0.1f, 0.2f);
    }

    // Update is called once per frame
    protected void Update()
    {
        
       
       
    }

    protected void FixedUpdate()
    {
        MoveUpdate();

        PreAttackUpdate();

        SpinHitUpdate();
    }

    private void OnEnable()
    {
        IsDead = false;
        StartCoroutine(IdleCo());
    }
    private void OnDisable()
    {
        CurDeathTime = 0;
        CurIdleTime = 0;
        Ani.SetTrigger("Idle");
        PreAni = "Idle";
        State = MONSTATE.IDLE;
        CurHP = MaxHP;
        CapsuleCollider[] Cols = GetComponentsInChildren<CapsuleCollider>();
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].enabled = true;
        }
    }


    void MoveUpdate()
    {
        if (MONSTATE.MOVE != State)
            return;

        if ((Player.transform.position - transform.position).magnitude < AttackDist)
        {

            State = MONSTATE.PREATTACK;
            Ani.SetTrigger("Idle");
            PreAni = "Idle";
            TargetPos = Player.transform.position;
            return;
        }

        Vector3 Dir = Player.transform.position - transform.position;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        //RB.MovePosition(RB.position + MoveStep);
        transform.position += MoveStep;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

    }
     
   
    protected IEnumerator IdleCo()
    {
        
        if (MONSTATE.DEATH == State)
        {
            yield break;
        }
        
        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "Idle";
        yield return new WaitForSeconds(MaxIdleTime);

        if(MONSTATE.DEATH!=State)
        {
            TakeNextAction();
        }
        else
        {
            yield break;
        }
         

    }
    
    IEnumerator DeathCo()
    {
        CapsuleCollider[] Cols = GetComponentsInChildren<CapsuleCollider>();
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].enabled = false;
        }
       
        IsDead = true;
        GValue.NMonNum--;
        GValue.KilledMon++;
        SampleMgr.Inst.SetKillText();

        if(GValue.KilledMon==15 || GValue.KilledMon == 25)
        {
            SampleMgr.Inst.BossSpawnFunc();
        }
       yield return new WaitForSeconds(MaxDeathTime);

        gameObject.SetActive(false);
    }
        

    void PreAttackUpdate()
    {
        if (MONSTATE.PREATTACK != State)
            return;

        
        Vector3 Dir = TargetPos  - transform.position; 

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

        PreAttackTime-= Time.deltaTime;

        if(PreAttackTime<0)
        {
            PreAttackTime = Random.Range(0.1f, 0.2f);
            State = MONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";
        }
      
    }

    void SpinHitUpdate()
    {
        if (!IsSpinHit)
        {
            return;
        }

        Vector3 Dir = transform.position - SpinAttackPos;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Dir * Time.deltaTime * SpinHitSpeed;
        transform.position += MoveStep;
        

        CurSpinHitTime += Time.deltaTime;
        if (CurSpinHitTime>MaxSpinHitTime)
        {
            CurSpinHitTime = 0;
            IsSpinHit = false;
        }
    }
   
    void TakeNextAction()
    {
        if (State==MONSTATE.DEATH)
        {
            return;
        }
        if (IsOnEvent)
        {
            State = MONSTATE.PREATTACK;
            Ani.SetTrigger("Idle");
            PreAni = "Idle";
            TargetPos = Player.transform.position;
            return;
        }

        Vector3 PPos = Player.transform.position;
        Vector3 MPos = transform.position;
        PPos.y = 0;
        MPos.y = 0;
        if ((PPos - MPos).magnitude < AttackDist)
        {

            State = MONSTATE.PREATTACK;
            Ani.SetTrigger("Idle");
            PreAni = "Idle";
            TargetPos = Player.transform.position;
        }
        else
        {
            State = MONSTATE.MOVE;
            Ani.SetTrigger("Move");
            PreAni = "Move";
        }
    }

    public void EventStart()
    {
        if (IsDead)
            return;
        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "IDLE";
        IsOnEvent = true;
    }

    public void EventEnd()
    {
        if (IsDead)
            return;
        IsOnEvent = false;
        TakeNextAction();
    }
    protected void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerAttack" && OnHitReady)
        {
            HitSoundFunc();

            CurHP -= GValue.PlayerDamage;
            MonsterAttack.SetActive(false);
            OnHitReady = false;
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            EffectSpawnerScript.Inst.SpawnAttackEffect(collisionPoint);
            EffectSpawnerScript.Inst.SpawnDamageText(collisionPoint, GValue.PlayerDamage, Color.red);
            if (CurHP <= 0)//Á×À½
            {
                State = MONSTATE.DEATH;
                Ani.SetTrigger("Death");
                PreAni = "Death";
                StartCoroutine(DeathCo());
                if (ItemSpawnerScript.Inst)
                {
                    ItemSpawnerScript.Inst.SpawnItem(transform.position);
                }              
            }
            else
            {
                if(other.transform.gameObject.GetComponentInParent<PlayerScript>().IsSpin)
                {
                    SpinAttackPos=other.transform.position;
                    IsSpinHit = true;
                }
                State = MONSTATE.HIT;
                Ani.SetTrigger("Hit");
                PreAni = "Hit";
            }

            
        }

        if(other.tag == "DrakeAttack")
        {
            if (other.gameObject.GetComponent<DrakeAttackScript>().IsCol)
                return;

            other.gameObject.GetComponent<DrakeAttackScript>().IsCol = true;
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            EffectSpawnerScript.Inst.SpawnAttackEffect(collisionPoint);
            EffectSpawnerScript.Inst.SpawnDamageText(collisionPoint, GValue.DrakeDamage, Color.red);
            CurHP -= GValue.DrakeDamage;

            AClip = Resources.Load<AudioClip>("Sound/DrakeExplosion");
            ASource.PlayOneShot(AClip);


            if (CurHP <= 0)
            {
                State = MONSTATE.DEATH;
                Ani.SetTrigger("Death");
                PreAni = "Death";
                StartCoroutine(DeathCo());

                if (ItemSpawnerScript.Inst)
                {
                    ItemSpawnerScript.Inst.SpawnItem(transform.position);
                }


            }
            else
            {
                State = MONSTATE.HIT;
                Ani.SetTrigger("Hit");
                PreAni = "Hit";
            }

            
            other.gameObject.GetComponent<DrakeAttackScript>().StartDeath();
        }

        if (other.tag == "SmashAttack")
        {
            CurHP -= GValue.PlayerSmashDamage;
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            EffectSpawnerScript.Inst.SpawnAttackEffect(collisionPoint);
            EffectSpawnerScript.Inst.SpawnDamageText(collisionPoint, GValue.PlayerSmashDamage, Color.red);
            if (CurHP <= 0)
            {
                State = MONSTATE.DEATH;
                Ani.SetTrigger("Death");
                PreAni = "Death";
                StartCoroutine(DeathCo());

                if (ItemSpawnerScript.Inst)
                {
                    ItemSpawnerScript.Inst.SpawnItem(transform.position);
                }


            }
            else
            {
                State = MONSTATE.HIT;
                Ani.SetTrigger("Hit");
                PreAni = "Hit";
            }

           
            
        }

    }

    void HitSoundFunc()
    {
        string temps = "Sound/MHit" + Random.Range(1, 6).ToString();
        AClip = Resources.Load<AudioClip>(temps);
        ASource.PlayOneShot(AClip);
    }

    protected virtual void AttackSoundFunc()
    {

    }
}
