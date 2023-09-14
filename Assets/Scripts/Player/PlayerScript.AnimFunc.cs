using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    ///  ////////////////////////////////// 애니메이션 이벤트 함수
    void Attack1Func()
    {
        Ani.SetTrigger("Idle");
        PrevAniName = "Idle";
        State = PLAYERSTATE.IDLE;
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
