using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DrakeScript : MonoBehaviour
{
    void AttackSpawnFunc()
    {        
        if(!CurTarget.GetComponent<NormalMonRootScript>().IsDead)
        {
            AttackSpawner.GetComponent<AttackSpawnerScript>().DrakeAttackSpawn(AttackPos.transform.position, CurTarget);
           
            AClip = Resources.Load<AudioClip>("Sound/DrakeAttack");
            ASource.PlayOneShot(AClip);
        }
        
       
    }
    void Attack1EndFunc()
    {
        if(PLAYERSTATE.MOVE==Player.GetPlayerState())
        {
            Ani.SetTrigger("Move");
            PreAni = "Move";
            State = DRAKESTATE.MOVE;
        }
        else
        {
            Ani.SetTrigger("Idle");
            PreAni = "Idle";
            State = DRAKESTATE.IDLE;
        }
        
    }
    
}
