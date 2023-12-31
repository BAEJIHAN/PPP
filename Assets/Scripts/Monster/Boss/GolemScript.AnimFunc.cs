using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public partial class GolemScript : BossRootScript
{
    void SetActiveRAttack()
    {
        RightAttack.SetActive(true);
    }
    void DeActiveRAttack()
    {
        RightAttack.SetActive(false);
    }
    void SetActiveLAttack()
    {
        LeftAttack.SetActive(true);
    }       

    void DeActiveLAttack()
    {
        LeftAttack.SetActive(false);
    }
    void RoarEndFunc()
    {
        State = BMONSTATE.MOVE;
        Ani.SetTrigger("Move");
        PreAni = "Move";
    }
    
    void AttackEndFunc()
    {
        State = BMONSTATE.MOVE;
        Ani.SetTrigger("Move");
        PreAni = "Move";
    }

    void JumpStartFunc()
    {
        IsJumping = true;
    }

    void JumpEndFunc()
    {
        IsJumping = false;
        State = BMONSTATE.JUMP2;
        Ani.SetTrigger("Jump2");
        PreAni = "Jump2";
        fValue = 0;
        Player.GetComponent<PlayerScript>().SetStomp();
    }

    void SpawnRockFunc()
    {
        IsAttack1Rot = true;
        tRock = Instantiate(GolemRock, RockPos.transform) as GameObject;
        tRock.transform.localScale=Vector3.one*0.7f;
    }

    void ThrowRockFunc()
    {
        IsAttack1Rot = false;
        tRock.transform.SetParent(null);
        if (tRock == null)
            Debug.Log(1);
        tRock.GetComponent<GolemRockScript>().StartFly(Player.transform.position);
    }

    void DeathFunc()
    {
        StartCoroutine(DeathCo());
    }
}
