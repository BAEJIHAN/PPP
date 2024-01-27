using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EndSpawnEffect", 1.0f);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnEnable()
    {
        Invoke("EndSpawnEffect", 1.0f);
    }

    void EndSpawnEffect()
    {       
        gameObject.SetActive(false);
    }
}
