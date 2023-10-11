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
    ATTACK1,
}
public class MonRootScript : MonoBehaviour
{
    protected Animator Ani;
    protected Rigidbody RB;
    protected string PreAni;
    public GameObject MonsterAttack;

    protected MONSTATE State = MONSTATE.NONE;
    protected float Speed=1;
    protected float RotSpeed = 1;
    protected float AttackDist = 1.5f;

    protected float CurIdleTime = 0;
    protected float MaxIdleTime = 2;
   
    GameObject Player;

    int HitTime = 0;
    [HideInInspector]public bool OnHitReady = true;

    protected int MaxHP = 10;
    protected int CurHP = 10;
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
    }

    // Update is called once per frame
    protected void Update()
    {
        SampleMgr.Inst.DText.text = MONSTATE.MOVE.ToString();

        IdleUpdate();

  
    }

    protected void FixedUpdate()
    {
        MoveUpdate();
    }

    void MoveUpdate()
    {
        if (MONSTATE.MOVE != State)
            return;

        if ((Player.transform.position - RB.position).magnitude < AttackDist)
        {
           
            State = MONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";
            return;
        }

        Vector3 Dir = Player.transform.position-RB.position;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        RB.MovePosition(RB.position+MoveStep);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

        
    }

    void IdleUpdate()
    {
        if (MONSTATE.IDLE != State)
            return;

        CurIdleTime+=Time.deltaTime;
        if (CurIdleTime > MaxIdleTime)
        {
            CurIdleTime = 0;
            TakeNextAction();
            
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
            CurHP-=GValue.PlayerDamage;
            MonsterAttack.SetActive(false);
            OnHitReady = false;

            if(CurHP<=0)
            {
                CapsuleCollider[] Cols = GetComponentsInChildren<CapsuleCollider>();
                for(int i=0; i<Cols.Length; i++)
                {
                    Cols[i].enabled = false;
                }
                State = MONSTATE.DEATH;
                Ani.SetTrigger("Death");
                PreAni = "Death";
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
