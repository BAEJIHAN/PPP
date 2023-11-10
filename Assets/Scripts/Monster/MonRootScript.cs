using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonRootScript : MonoBehaviour
{
    [HideInInspector] public bool OnHitReady = true;
    [HideInInspector] public bool IsDead = false;

    protected GameObject Player;
    protected Animator Ani;
    protected Rigidbody RB;
    protected string PreAni;

    protected int MaxHP = 10;
    protected int CurHP = 10;
  
}
