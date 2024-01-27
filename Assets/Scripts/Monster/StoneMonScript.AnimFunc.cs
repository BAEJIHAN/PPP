using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StoneMonScript : NormalMonRootScript
{
    // Start is called before the first frame update
    void Attack1StartFunc()
    {
        if (MONSTATE.DEATH == State)
            return;
        MonsterAttack.SetActive(true);
        IsAttacking = true;

    }

    void Attack1EndFunc()
    {
       
           
        IsAttacking = false;

        AttackTime = 0;

        MonsterAttack.SetActive(false);

        if (MONSTATE.DEATH == State)
        {
            return;
        }

        StartCoroutine(IdleCo());
    }

    void HitEndFunc()
    {
        if (State == MONSTATE.DEATH)
        {
            return;
        }

        StartCoroutine(IdleCo());

    }

}
