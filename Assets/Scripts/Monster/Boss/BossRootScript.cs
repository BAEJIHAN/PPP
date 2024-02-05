using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BMONSTATE
{
    NONE,
    IDLE,
    MOVE,
    HIT,
    ROAR,
    DEATH,
    PREATTACK,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    ATTACK4,
    ATTACK5,
    ATTACK6,
    JUMP1,
    JUMP2,
    TAKEOFF,
    FLY,
    LAND,
    RUN,
}
public class BossRootScript : MonRootScript
{


    protected BMONSTATE State;
    public GameObject[] HitCols;
    // Start is called before the first frame update
    protected void Awake()
    {
        Ani = GetComponent<Animator>();
        ASource=GetComponent<AudioSource>();
    }
    protected void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {


        for (int i = 0; i < HitCols.Length; i++)
        {
            HitCols[i].GetComponent<BossHitColScript>().OnHitReady = false;
        }
    }

    public virtual void TakeDamage(int HDamage)
    {

    }
}
