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
    void AttackA1EndFunc()
    {
        
        if (AttackACombo==1)
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
            AttackACombo = 0;
        }
       
    }

    void AttackA2EndFunc()
    {
       
        if (AttackACombo == 2)
        {
            GValue.PlayerDamage = AttackA3Damage;
            Ani.SetTrigger("AttackA3");
            PrevAniName = "AttackA3";
            State = PLAYERSTATE.ATTACKA3;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
            AttackACombo = 0;
        }
    }

    void AttackA3EndFunc()
    {
        
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
        AttackACombo = 0;
    }


    void AttackB1EndFunc()
    {
      
        if (AttackBCombo == 1)
        {
            GValue.PlayerDamage = AttackB2Damage;
            Ani.SetTrigger("AttackB2");
            PrevAniName = "AttackB2";
            State = PLAYERSTATE.ATTACKB2;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
            AttackBCombo = 0;
        }

    }

    void AttackB2EndFunc()
    {
      
        if (AttackBCombo == 2)
        {
            GValue.PlayerDamage = AttackB3Damage;
            Ani.SetTrigger("AttackB3");
            PrevAniName = "AttackB3";
            State = PLAYERSTATE.ATTACKB3;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PrevAniName = "Idle";
            State = PLAYERSTATE.IDLE;
            AttackBCombo = 0;
        }
    }

    void AttackB3EndFunc()
    {
        
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
        AttackBCombo = 0;
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
