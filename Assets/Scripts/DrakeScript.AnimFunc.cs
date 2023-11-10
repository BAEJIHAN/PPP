using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DrakeScript : MonoBehaviour
{
    void AttackSpawnFunc()
    {        
        AttackSpawner.GetComponent<AttackSpawnerScript>().DrakeAttackSpawn(AttackPos.transform.position, CurTarget);
       
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
