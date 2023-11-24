using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GroundEffectScript : MonoBehaviour
{
    Material PMat;
    public Material RangeMat;
    

    public GameObject Attack;
    public GameObject Crack;
    public GameObject RangeEffect;

    float CurAddRange = 1;
    float MaxAddRange = 10;
    float RangeSpeed = 2f;

    [HideInInspector]public bool IsCharge = false;
    // Start is called before the first frame update
    private void Awake()
    {
        PMat = GetComponent<Material>();

    }
    void Start()
    {
        PMat = null;
    }

    // Update is called once per frame
    void Update()
    {
        ChargeUpdate();
        
    }

    private void OnEnable()
    {
        RangeEffect.transform.localPosition = new Vector3(0, 0, 0.99f);
    }
    private void OnDisable()
    {
        CurAddRange = 1;
        transform.localScale = Vector3.one;
        Attack.SetActive(false);

    }
    void ChargeUpdate()
    {
        if (!IsCharge)
            return;

        Vector3 temp = Vector3.one;
        CurAddRange += Time.deltaTime * RangeSpeed;
        if(CurAddRange > MaxAddRange)
        {
            CurAddRange = MaxAddRange;
        }
        temp *= CurAddRange;
        temp.z = 1;
        transform.localScale= temp;
    }

    public void SetAttack()
    {
        Attack.SetActive(true);
        StartCoroutine(SetOff());
    }

    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject tCrack = Instantiate(Crack);
        Vector3 tPos= Attack.transform.position;
        float tScaleX = gameObject.transform.localScale.x;
        tScaleX *= 0.1f;
        Vector3 tScale=new Vector3(tScaleX, 0.1f, tScaleX);
        tPos.y = 0.0011f;        
        tCrack.transform.position = tPos;
        tCrack.transform.localScale = tScale;


        Attack.SetActive(false);
        gameObject.SetActive(false);

    }

    
}
