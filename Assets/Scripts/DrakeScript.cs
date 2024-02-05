using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum DRAKESTATE
{
    IDLE,
    MOVE,
    ATTACK1,
    ATTACK2,
    DEATH
}
public partial class DrakeScript : MonoBehaviour
{
    Animator Ani;
    DRAKESTATE State;

    string PreAni;

    AudioSource ASource;
    AudioClip AClip;


    [HideInInspector] public float Range=10;
    public LayerMask TargetLayer;
    [HideInInspector] public RaycastHit[] Targets;
    [HideInInspector] public GameObject CurTarget;

    [HideInInspector] public PlayerScript Player;

    float AttackRate = 5;
    float CurAttackRate = 0;

    public GameObject AttackPos;
    public GameObject Fireball;
    GameObject AttackSpawner;

    float CurSearchRate = 0;
    float SearchRate = 3;
    private void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerScript>();
        Ani = GetComponent<Animator>();
        AttackSpawner = GameObject.Find("AttackSpawner");
        ASource=GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        State = DRAKESTATE.IDLE;
        PreAni = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetPlayerState() == PLAYERSTATE.DEATH)
            return;
        CurSearchRate += Time.deltaTime;
        if(SearchRate< CurSearchRate)
        {
            Targets = null;
            CurSearchRate = 0;
            Targets = Physics.SphereCastAll(transform.position, Range, Vector3.up, 0, TargetLayer);
            Attack();
        }
       
    }

   

    void Attack()
    {
        
            CurTarget = SearchTarget();
            if (CurTarget)
            {
                CurAttackRate = 0;
                Ani.SetTrigger("Attack1");
                PreAni = "Attack1";
                State = DRAKESTATE.ATTACK1;
            }


       
      
    }
    GameObject SearchTarget()
    {
        if (Targets.Length == 0)
            return null;

        GameObject RTarget=null;
        RTarget = Targets[0].transform.gameObject;
        float Dist=Vector3.Distance(transform.position, RTarget.transform.position);
        for (int i=1; i<Targets.Length; i++)
        {
            if(Dist>Vector3.Distance(transform.position, Targets[i].transform.position))               
            {
                Dist = Vector3.Distance(transform.position, Targets[i].transform.position);
                
                if(Targets[i].transform.gameObject.GetComponent<MonRootScript>())
                {
                    if(!Targets[i].transform.gameObject.GetComponent<MonRootScript>().IsDead)
                    {
                        RTarget = Targets[i].transform.gameObject;
                    }
                }
                   
            }
        }
        return RTarget;
    }

    public void SetAni(string AniName)
    {

        if (DRAKESTATE.ATTACK1 == State)
            return;

        if (PreAni == AniName)
        {
            return;
        }

        Ani.SetTrigger(AniName);
        PreAni = AniName;
        State = (DRAKESTATE)System.Enum.Parse(typeof(DRAKESTATE), AniName.ToUpper());
       
    }

  
   
}
