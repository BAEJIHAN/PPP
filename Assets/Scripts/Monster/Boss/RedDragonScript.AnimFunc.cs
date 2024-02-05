using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class RedDragonScript : BossRootScript
{
    // Start is called before the first frame update
    

    void RoarEndFunc()
    {
        if ( BMONSTATE.ATTACK4 == State)
        {
            Ani.SetTrigger("Run");
            PreAni = "Run";
            StepSound.Play();
            StartCoroutine(RunEndFunc());
        }
        else
        {
            SetPreAttack();
        }
        
    }

    void SetActiveJawAttack()
    {
        JawAttack.SetActive(true);
    }
    void DeActiveJawAttack()
    {
        JawAttack.SetActive(false);
    }
    void SetActiveTailAttack()
    {
        TailAttack1.SetActive(true);
        TailAttack2.SetActive(true);
    }
    void DeActiveTailAttack()
    {
        TailAttack1.SetActive(false);
        TailAttack2.SetActive(false);
    }

    void Attack1EndFunc()
    {
        ChargePhase = 0;
        SetPreAttack();
    }

    void Attack2EndFunc()
    {


        SetPreAttack();
    }

    void FireFunc1()
    {
        GroundFire.SetActive(true);
    }

    void FireFunc2()
    {
        FireAttackObj1.SetActive(true);
        FireAttackObj2.SetActive(true);       
    }

    void FireFunc3()
    {
        FireAttackObj1.SetActive(false);
        FireAttackObj2.SetActive(false);
        GroundFire.SetActive(false);
    }
    void Attack6EndFunc()
    {

        SetPreAttack();
    }
    void TakeOffEndFunc()
    {        
        Ani.SetTrigger("FlyIdle");
        PreAni = "FlyIdle";
        State = BMONSTATE.FLY;
        
    }

    void LandEndFunc()
    {
        SetPreAttack();
        WingSound.Stop();
    }
    void FlyAttackPre()
    {
        
    }
    void FlyAttackShoot()
    {
       
        GameObject tempObj = Instantiate(Fireball);
        tempObj.transform.position = FireballPos.transform.position;
        Vector3 PlayerPos = Player.transform.position;
        PlayerPos.y = 0;
        tempObj.GetComponent<FireballScript>().SetTargetPos(PlayerPos);
       
    }
    void FlyAttackEndFunc()
    {
        Ani.SetTrigger("FlyIdle");
        PreAni = "FlyIdle";
        State = BMONSTATE.FLY;
    }

    void ChargePhase1Func()
    {
        Ani.speed = 0.3f;
        if (BMONSTATE.ATTACK3== State)
        {
            ChargePhase++;
        }
        
    }

    void ChargePhase2Func()
    {
        Ani.speed = 1.2f;
        if (BMONSTATE.ATTACK3 == State)
        {
            ChargePhase++;
        }

    }

    void ChargePhase3Func()
    {
        Ani.speed = 1f;
        if (BMONSTATE.ATTACK3 == State)
        {
            ChargePhase++;
        }

    }

    void DeathFunc()
    {
        Invoke("LoadEndingScene", 5.0f);
    }

    void LoadEndingScene()
    {
        SceneManager.LoadScene("EndingScene");
    }
}
