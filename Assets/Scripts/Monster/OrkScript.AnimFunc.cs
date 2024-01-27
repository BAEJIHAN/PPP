using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OrkScript : NormalMonRootScript
{
    void Attack1SpawnFunc()
    {
        if (MONSTATE.DEATH == State)
            return;
        GameObject temp = Instantiate(MonsterAttack);
        temp.SetActive(true);
        temp.transform.position=AxePos.transform.position;
        temp.GetComponent<AxeScript>().SetDir(Player.transform.position);
    }

    void Attack1EndFunc()
    {
        if (MONSTATE.DEATH == State)
            return;
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
