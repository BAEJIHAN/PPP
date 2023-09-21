using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    ///  ////////////////////////////////// 애니메이션 이벤트 함수
    void AttackA1EndFunc()
    {
        if(AttackACombo==1)
        {
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

    void AttackSpinPreEnd()
    {
        Ani.SetTrigger("AttackSpin");
        PrevAniName = "AttackSpin";
    }

    void AttackSpinOnEnd()
    {
        Ani.SetTrigger("AttackSpinPost");
        PrevAniName = "AttackSpinPost";
    }

    void AttackSpinPostEnd()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void AttackSmashStartEnd()
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

    void AttackSmashCastingAEnd()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmashCastingB");
            PrevAniName = "AttackSmashCastingB";
        }
        
    }

    void AttackSmashCastingBEnd()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Ani.SetTrigger("AttackSmashCastingA");
            PrevAniName = "AttackSmashCastingA";
        }
        
    }
    void AttackSmashEnd()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }


    void AttackB3EndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
        AttackBCombo = 0;
    }

    void JumpEndEndFunc()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }

    void JumpDoubleEndFunc()
    {
        Ani.SetTrigger("JumpAir");
        PrevAniName = "JumpAir";
        State = PLAYERSTATE.JUMPAIR;
    }

    void RollStartFunc()
    {
        IsRollMove = true;
        //GetComponent<CapsuleCollider>().enabled = false;
    }
    void RollStopFunc()
    {
        IsRollMove = false;
        //GetComponent<CapsuleCollider>().enabled = true;
       
    }

    void RollEndFunc()
    {
        
        //GetComponent<CapsuleCollider>().enabled = true;
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
    }
}
