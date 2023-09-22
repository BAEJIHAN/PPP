using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public int Damage = 1;

    private void OnDisable()
    {
        MonRootScript[] Mons = GameObject.FindObjectsOfType<MonRootScript>();

        for(int i=0; i<Mons.Length; i++)
        {
            Mons[i].OnHitReady = true;
        }
    }
}
