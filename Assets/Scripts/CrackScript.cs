using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackScript : MonoBehaviour
{
    bool IsFading = false;
    Material mat;
    float FadeSpeed = 1;
    float Alpha = 1;
    // Start is called before the first frame update
    void Start()
    {
        mat=GetComponent<MeshRenderer>().material;
        StartCoroutine(StartFade());
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFading)
        {
            Alpha -= FadeSpeed * Time.deltaTime;
            mat.color = new Color(1, 1, 1, Alpha);
            if(Alpha<0)
            {
                StopCoroutine(StartFade());
                Destroy(gameObject);
            }
        }
    }

    IEnumerator StartFade()
    {
        yield return new WaitForSeconds(3);
        IsFading = true;
    }
}
