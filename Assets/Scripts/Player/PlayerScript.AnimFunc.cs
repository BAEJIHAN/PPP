using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    ///  ////////////////////////////////// 애니메이션 이벤트 함수
    ///  
    void AttackActiveFunc()
    {
        PlayerAttack.SetActive(true);
    }

    void AttackDeActiveFunc()
    {
        PlayerAttack.SetActive(false);
    }

    void AttackA1MoveFunc()
    {
        IsAttackA1Move = true;
    }

    void AttackA1StopFunc()
    {
        IsAttackA1Move = false;
    }
    void AttackA1EndFunc()
    {
        
        if (IsNextAttackA)
        {
            GValue.PlayerDamage = AttackA2Damage;
            Ani.SetTrigger("AttackA2");
            PrevAniName = "AttackA2";
            State = PLAYERSTATE.ATTACKA2;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
            
        }
        IsNextAttackA = false;


    }

    void AttackA2MoveFunc()
    {
        IsAttackA2Move = true;
    
    }

    void AttackA2StopFunc()
    {
        IsAttackA2Move = false;

    }

    void AttackA2EndFunc()
    {
        if(IsNextAttackB)
        {
            GValue.PlayerDamage = AttackBDamage;
            Ani.SetTrigger("AttackB");
            PrevAniName = "AttackB";
            State = PLAYERSTATE.ATTACKB;
        }
        else if(IsNextAttackA)
        {
            GValue.PlayerDamage = AttackA1Damage;
            Ani.SetTrigger("AttackA1");
            PrevAniName = "AttackA1";
            State = PLAYERSTATE.ATTACKA1;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
           
        }
        IsNextAttackA = false;
        IsNextAttackB = false;
    }


    void AttackBMoveFunc()
    {
        IsAttackBMove = true;
    }

    void AttackBStopFunc()
    {
        IsAttackBMove = false;
    }

    void AttackBEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;

        IsNextAttackA = false;
        IsNextAttackB = false;

    }

  
    void AttackSpinPreEndFunc()
    {
        Ani.SetTrigger("AttackSpin");
        PrevAniName = "AttackSpin";
    }

    //void AttackSpinOnEnd()
    //{
    //    PlayerAttack.SetActive(false);
    //    Ani.SetTrigger("AttackSpinPost");
    //    PrevAniName = "AttackSpinPost";
    //}

    void AttackSpinPostEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void AttackSmashStartEndFunc()
    {       

        if (Input.GetKey(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmashCastingA");
            PrevAniName = "AttackSmashCastingA";
            State = PLAYERSTATE.ATTACKSMASHCASTING;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
        }
    }

    void AttackSmashCastingAEndFunc()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmashCastingB");
            PrevAniName = "AttackSmashCastingB";
        }
        
    }

    void AttackSmashCastingBEndFunc()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmashCastingA");
            PrevAniName = "AttackSmashCastingA";
        }
        
    }
    void AttackSmashEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void AttackAirEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
        IsAttackAirReady = true;
        IsDoubleJumped = false;
    }
   

    void JumpEndEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void JumpDoubleEndFunc()
    {
        if(IsAttackAirReady)
        {
            Ani.SetTrigger("JumpAir");
            PrevAniName = "JumpAir";
            State = PLAYERSTATE.JUMPAIR;
        }
       
    }

    void RollStartFunc()
    {
        IsRollMove = true;
        
    }
    void RollStopFunc()
    {
        IsRollMove = false;
        
       
    }

    void RollEndFunc()
    {
        
        //GetComponent<CapsuleCollider>().enabled = true;
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void HitEndFunc()
    {
        if(GC.IsGrounded())
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
        }
        else
        {
            Ani.SetTrigger("JumpAir");
            PrevAniName = "JumpAir";
            State = PLAYERSTATE.JUMPAIR;
        }

    }
}
