using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MushroomScript : NormalMonRootScript
{
    void Attack1StartFunc()
    {
        if (MONSTATE.DEATH == State)
            return;
        MonsterAttack.SetActive(true);

    }

    void Attack1EndFunc()
    {
        if (MONSTATE.DEATH == State)
            return;
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
