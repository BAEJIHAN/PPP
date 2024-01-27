using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EndEffect", 1.0f);


    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnEnable()
    {
        Invoke("EndEffect", 1.0f);
    }

    void EndEffect()
    {
        gameObject.SetActive(false);
    }
}
