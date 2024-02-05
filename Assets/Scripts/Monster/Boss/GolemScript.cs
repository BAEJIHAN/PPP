using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
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
    JUMP1,
    JUMP2,
}
*/
public partial class GolemScript : BossRootScript
{
    
    float Speed = 3;
    float RotSpeed = 10;
    float ThrowRotSpeed = 7;

    float JumpHeight = 3;
    float JumpSpeed = 3;
    float JumpDist;
    float fValue = 0;

    
    public GameObject RightAttack;
    public GameObject LeftAttack;
    public GameObject RockPos;
    public GameObject GolemRock;
    GameObject tRock;

    public GameObject SmashPos;
    GameObject SmashPosObj;

    bool IsAttack1Rot = false;
    bool IsJumping = false;

    Vector3 P1, P2, P3, P4;

    
    Vector3 PreAttackPos = Vector3.zero;
    float PreAttackDist = 0;
    void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        State = BMONSTATE.ROAR;
        PreAni = "Roar";
        MaxHP = 20;
        CurHP = 20;
      

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        MoveUpdate();

        Attack1Update();

        JumpUpdate();

        PreAttackUpdate();
    }

    void MoveUpdate()
    {
        if (BMONSTATE.MOVE != State)
            return;

        Vector3 Dir = Player.transform.position - transform.position;
        Dir.y = 0;
        Dir.Normalize();
        Vector3 MoveStep = Speed * Time.deltaTime * Dir;
        transform.position += MoveStep;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

        float AttackRange = (Player.transform.position - transform.position).magnitude;
        if (AttackRange < 5)
        {          
          
            SetPreAttack();
        }
    }

    void Attack1Update()
    {
        if (BMONSTATE.ATTACK1 != State || IsAttack1Rot == false)
            return;

        Vector3 Dir = Player.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), ThrowRotSpeed * Time.deltaTime);
    }

    void JumpUpdate()
    {
        if (BMONSTATE.JUMP1 != State)
            return;

        if (fValue < 0.1f)
        {
            Vector3 Dir = P4 - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), ThrowRotSpeed * Time.deltaTime);
        }

        if (!IsJumping)
            return;
        float temp = Time.deltaTime / JumpDist;

        fValue += temp * JumpSpeed;
       

        transform.position = MMath.Lerp(P1, P2, P3, P4, fValue);

        if (fValue >= 1.0f)
        {
            JumpEndFunc();
        }
            


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




    /// ///////////
    //attack1 돌 던지기
    //attack2 정권 찌르기
    //attack3 후리기
    //attack4 후리기 길게 
    //attack5 점프
    
    void TakeNextAction()
    {
        Vector3 temp = Player.transform.position;
        temp.y = 0;
        float PBDist = (transform.position - temp).magnitude;
        int Ran = -1;
        
        if(PBDist<3)//가까울 떄
        {
            Ran=Random.Range(0, 4);//0, 1, 2, 3
        }
        else if(3<= PBDist ||  PBDist<= 6)
        {
            Ran = Random.Range(4, 8);//4, 5, 6, 7
        }
        else
        {
            Ran = 0;
        }



        if (Ran == 0 || Ran==4)
        {
            SetPreAttack();
        }
        else if (Ran == 1)
        {
            State = BMONSTATE.ATTACK4;
            Ani.SetTrigger("Attack4");
            PreAni = "Attack4"; 
                   
        }
        else if (Ran == 2)
        {
            State = BMONSTATE.ATTACK2;
            Ani.SetTrigger("Attack2");
            PreAni = "Attack2";
        }
        else if (Ran == 3)
        {

            State = BMONSTATE.ATTACK3;
            Ani.SetTrigger("Attack3");
            PreAni = "Attack3";
        }//////////가까울 떄
        else if (Ran == 5 || Ran == 6)/////멀 때
        {

            State = BMONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            PreAni = "Attack1";

        }
        else if(Ran == 7)
        {
            State = BMONSTATE.JUMP1;
            Ani.SetTrigger("Jump1");
            PreAni = "Jump1";
            SetJumpPos();
        }
        else
        {

        }
    }

    void SetPreAttack()
    {
        State = BMONSTATE.PREATTACK;
        Ani.SetTrigger("Move");
        PreAni = "Move";

        PreAttackPos = Player.transform.position;
        PreAttackPos.y = 0;

        PreAttackPos.x = Random.Range(-5.0f, 5.0f);
        PreAttackPos.z = Random.Range(-5.0f, 5.0f);         
       
        PreAttackDist = (PreAttackPos - transform.position).magnitude;
    }
    void SetJumpPos()
    {
        P1 = transform.position;
        P4 = Player.transform.position;

        if (Random.Range(0, 2) == 0)
        {
            P4.x += Random.Range(-4.0f, -1.0f);
        }
        else
        {
            P4.x += Random.Range(1.0f, 4.0f);
        }
        if (Random.Range(0, 2) == 0)
        {
            P4.z += Random.Range(-4.0f, -1.0f);
        }
        else
        {
            P4.z += Random.Range(1.0f, 4.0f);
        }


        float MidX = (P1.x + P4.x) / 2.0f;
        float MidZ = (P1.z + P4.z) / 2.0f;
        float P2x = (P1.x + MidX) / 2.0f;
        float P3x = (P4.x + MidX) / 2.0f;
        float P2z = (P1.z + MidZ) / 2.0f;
        float P3z = (P4.z + MidZ) / 2.0f;


        P2 = new Vector3(P2x, JumpHeight, P2z);
        P3 = new Vector3(P3x, JumpHeight, P3z);
        JumpDist = (P4 - P1).magnitude;
    }

    override public void TakeDamage(int HDamage)
    {
        if (BMONSTATE.JUMP1 == State)
            return;

        HitSoundFunc();

        CurHP -= HDamage;
        if (CurHP < 0)
        {
            for(int i=0; i<HitCols.Length; i++)
            {
                HitCols[i].SetActive(false);
          
            }
            RightAttack.SetActive(false);
            LeftAttack.SetActive(false);
            
            Destroy(SmashPosObj);
            SmashPosObj = null;
            State = BMONSTATE.DEATH;
            Ani.SetTrigger("Death");
            PreAni = "Death";
            Vector3 temp = transform.position;
            temp.y = 0;
            transform.position = temp;

        }   

    }

    private void OnDestroy()
    {
        StopCoroutine(DeathCo());
    }

    IEnumerator DeathCo()
    {
        ItemSpawnerScript.Inst.BossSpawnItem(transform.position);
        SampleMgr.Inst.IsBoss = false;
        yield return new WaitForSeconds(5.0f);
       
        Destroy(gameObject);
    }

    void SpawnSmashPos()
    {
        SmashPosObj=Instantiate(SmashPos);        
        Vector3 tempV = P4;
        tempV.y = 0.01f;
        SmashPosObj.transform.position = tempV;
    }

    void AttackSoundFunc()
    {
        string temps = "Sound/GolemAttack" + Random.Range(1, 4).ToString();
        AClip = Resources.Load<AudioClip>(temps);
        ASource.PlayOneShot(AClip);
    }

    void RoarSoundFunc()
    {        
        AClip = Resources.Load<AudioClip>("Sound/GolemRoar");
        ASource.PlayOneShot(AClip);
    }

    void StompSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/Stomp");
        ASource.PlayOneShot(AClip);
    }

    void DeathSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/GolemDeath");
        ASource.PlayOneShot(AClip);
    }

    void HitSoundFunc()
    {
        AClip = Resources.Load<AudioClip>("Sound/GolemHit");
        ASource.PlayOneShot(AClip);
    }
}
