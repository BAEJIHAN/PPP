using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MushroomScript : NormalMonRootScript
{
    void Attack1StartFunc()
    {
        MonsterAttack.SetActive(true);

    }

    void Attack1EndFunc()
    {
        MonsterAttack.SetActive(false);
    
        StartCoroutine(IdleCo());
    }

    void HitEndFunc()
    {
        if(State== MONSTATE.DEATH)
        {
            return;
        }

        StartCoroutine(IdleCo());
       
    }

}
