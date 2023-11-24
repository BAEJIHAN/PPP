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

    protected void Awake()
    {
        Ani = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();

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

        
    }

    // Update is called once per frame
    protected void Update()
    {
        
       
        PreAttackUpdate();
    }

    protected void FixedUpdate()
    {
        MoveUpdate();

        SpinHitUpdate();
    }

    private void OnEnable()
    {
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

        if ((Player.transform.position - RB.position).magnitude < AttackDist)
        {

            State = MONSTATE.PREATTACK;
            Ani.SetTrigger("Idle");
            PreAni = "Idle";
            TargetPos = Player.transform.position;
            return;
        }

        Vector3 Dir = Player.transform.position - RB.position;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        RB.MovePosition(RB.position + MoveStep);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

    }
     
   
    protected IEnumerator IdleCo()
    {
        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "Idle";
        yield return new WaitForSeconds(MaxIdleTime);

        if(MONSTATE.DEATH!=State)
        {
            TakeNextAction();
        }
         

    }
    
    IEnumerator DeathCo()
    {
        CapsuleCollider[] Cols = GetComponentsInChildren<CapsuleCollider>();
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].enabled = false;
        }
        State = MONSTATE.DEATH;
        Ani.SetTrigger("Death");
        PreAni = "Death";
        IsDead = true;

        yield return new WaitForSeconds(MaxDeathTime);

        gameObject.SetActive(false);
    }
        

    void PreAttackUpdate()
    {
        if (MONSTATE.PREATTACK != State)
            return;

        Vector3 Dir = TargetPos-RB.position; 

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);
        
        if (0.0001f > (Quaternion.LookRotation(Dir).eulerAngles-transform.rotation.eulerAngles).magnitude)
        {
           
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

        Vector3 Dir = RB.position - SpinAttackPos;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Dir * Time.deltaTime * SpinHitSpeed;           
        RB.MovePosition(RB.position + MoveStep);

        CurSpinHitTime += Time.deltaTime;
        if (CurSpinHitTime>MaxSpinHitTime)
        {
            CurSpinHitTime = 0;
            IsSpinHit = false;
        }
    }
   
    void TakeNextAction()
    {
        
        Vector3 PPos = Player.transform.position;
        Vector3 MPos = RB.position;
        PPos.y = 0;
        MPos.y = 0;
        if ((PPos - MPos).magnitude < AttackDist)
        {
           
            State = MONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";
        }
        else
        {
            State = MONSTATE.MOVE;
            Ani.SetTrigger("Move");
            PreAni = "Move";
        }
    }

    
    protected void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerAttack" && OnHitReady)
        {
            CurHP -= GValue.PlayerDamage;
            MonsterAttack.SetActive(false);
            OnHitReady = false;

            if (CurHP <= 0)//Á×À½
            {
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

            CurHP -= GValue.DrakeDamage;

            if (CurHP <= 0)
            {
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

            State = MONSTATE.HIT;
            Ani.SetTrigger("Hit");
            PreAni = "Hit";
            other.gameObject.GetComponent<DrakeAttackScript>().StartSetOff();
        }

        if (other.tag == "SmashAttack")
        {
            CurHP -= GValue.PlayerSmashDamage;

            if (CurHP <= 0)
            {
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
}
