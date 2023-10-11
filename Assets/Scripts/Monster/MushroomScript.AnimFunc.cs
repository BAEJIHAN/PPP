using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MushroomScript : MonRootScript
{
    void Attack1StartFunc()
    {
        MonsterAttack.SetActive(true);

    }

    void Attack1EndFunc()
    {
        MonsterAttack.SetActive(false);

        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "Idle";

    }

    void HitEndFunc()
    {
        State = MONSTATE.IDLE;
        Ani.SetTrigger("Idle");
        PreAni = "Idle";
    }

}
