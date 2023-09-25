using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public int Damage = 1;
    float ScaleValue = 1.0f;

    CapsuleCollider Col;
    private void Awake()
    {
        Col = GetComponent<CapsuleCollider>();
        if (Col == null)
            Debug.Log(null);
    }
    private void OnDisable()
    {
        MonRootScript[] Mons = GameObject.FindObjectsOfType<MonRootScript>();

        for(int i=0; i<Mons.Length; i++)
        {
            Mons[i].OnHitReady = true;
        }
    }

    public void ColScaling(float _ScaleValue)
    {
        
        Vector3 ColCen = Col.center;
        ColCen.y += _ScaleValue*0.5f;
        Col.center = ColCen;


        float ColHeight = Col.height;
        ColHeight += _ScaleValue;
        Col.height = ColHeight;

    }
}
