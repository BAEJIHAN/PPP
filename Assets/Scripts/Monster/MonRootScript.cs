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

    protected MONSTATE State = MONSTATE.NONE;
    protected float Speed=1;
    protected float RotSpeed = 1;
    protected float AttackDist = 1.5f;


    protected GameObject MonsterAttack;
    GameObject Player;
    protected void Awake()
    {
        Ani = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        MonsterAttack = transform.Find("MonsterAttack").gameObject;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        PreAni = "Idle";
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected void Update()
    {
        DetectPlayer();
        SampleMgr.Inst.DText.text = State.ToString();

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
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        RB.MovePosition(RB.position+MoveStep);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

        
    }

    void DetectPlayer()
    {
        if (MONSTATE.NONE != State)
            return;

        if(Player)
        {
            State = MONSTATE.MOVE;
            Ani.SetTrigger("Move");
            PreAni = "Move";
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        
    }
}
