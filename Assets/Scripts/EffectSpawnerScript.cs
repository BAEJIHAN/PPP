using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawnerScript : MonoBehaviour
{
    public static EffectSpawnerScript Inst;

    [Header("Spawn")]
    public GameObject SpawnEffect;
    int SpawnMaxCount = 6;
    List<GameObject> SpawnEffectPool = new List<GameObject>();

    [Header("Hit")]
    public GameObject AttackEffect1;
    public GameObject AttackEffect2;
    public GameObject GuardEffect;
    public GameObject DamageText;
    List<GameObject> AttackEffectPool1 = new List<GameObject>();
    List<GameObject> AttackEffectPool2 = new List<GameObject>();
    List<GameObject> GuardEffectPool = new List<GameObject>();
    List<GameObject> DamageTextPool = new List<GameObject>();
    int AttackMaxCount = 20;
    int GuardMaxCount = 20;
    int DamageTextMaxCount = 50;


    // Start is called before the first frame update
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        for (int i = 0; i < SpawnMaxCount; i++)
        {            
            GameObject SpanwEffectObj;

            SpanwEffectObj = Instantiate(SpawnEffect);

            SpanwEffectObj.name = "SpawnEffect" + i.ToString();
            SpanwEffectObj.SetActive(false);
            SpawnEffectPool.Add(SpanwEffectObj);
        }

        for (int i = 0; i < AttackMaxCount; i++)
        {
            GameObject AttackEffectObj;

            AttackEffectObj = Instantiate(AttackEffect1);

            AttackEffectObj.name = "AttackEffect1_" + i.ToString();
            AttackEffectObj.SetActive(false);
            AttackEffectPool1.Add(AttackEffectObj);
        }

        for (int i = 0; i < AttackMaxCount; i++)
        {
            GameObject AttackEffectObj;

            AttackEffectObj = Instantiate(AttackEffect2);

            AttackEffectObj.name = "AttackEffect2_" + i.ToString();
            AttackEffectObj.SetActive(false);
            AttackEffectPool2.Add(AttackEffectObj);
        }

        for (int i = 0; i < GuardMaxCount; i++)
        {
            GameObject GuardEffectObj;

            GuardEffectObj = Instantiate(GuardEffect);

            GuardEffectObj.name = "GuardEffect" + i.ToString();
            GuardEffectObj.SetActive(false);
            GuardEffectPool.Add(GuardEffectObj);
        }

        for (int i = 0; i < DamageTextMaxCount; i++)
        {
            GameObject DamageTextObj;

            DamageTextObj = Instantiate(DamageText);

            DamageTextObj.name = "DamageText" + i.ToString();
            DamageTextObj.SetActive(false);
            DamageTextPool.Add(DamageTextObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSpawnEffect(Vector3 SpawnPos)
    {
        for(int i=0; i< SpawnEffectPool.Count; i++)
        {
            if (!SpawnEffectPool[i].activeSelf)
            {
                SpawnEffectPool[i].SetActive(true);
                SpawnPos.y = 0.3f;
                SpawnEffectPool[i].transform.position = SpawnPos;
                break;
            }
        }
    }

    public void SpawnAttackEffect(Vector3 SpawnPos)
    {
        int ranNum = Random.Range(0, 2);
        SpawnPos.y += 0.5f;
        if (ranNum == 0)
        {
            for (int i = 0; i < AttackEffectPool1.Count; i++)
            {
                if (!AttackEffectPool1[i].activeSelf)
                {
                    AttackEffectPool1[i].SetActive(true);
                    
                     AttackEffectPool1[i].transform.position = SpawnPos;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < AttackEffectPool2.Count; i++)
            {
                if (!AttackEffectPool2[i].activeSelf)
                {
                    AttackEffectPool2[i].SetActive(true);
                    
                    AttackEffectPool2[i].transform.position = SpawnPos;
                    break;
                }
            }
        }
        
    }

    public void SpawnGuardEffect(Vector3 SpawnPos)
    {
        SpawnPos.y += 0.5f;
        for (int i = 0; i < GuardEffectPool.Count; i++)
        {
            if (!GuardEffectPool[i].activeSelf)
            {
                GuardEffectPool[i].SetActive(true);
                
                GuardEffectPool[i].transform.position = SpawnPos;
                break;
            }
        }
    }

    public void SpawnDamageText(Vector3 SpawnPos, int Damage, Color color)
    {
        SpawnPos.y += 0.5f;
        for (int i = 0; i < DamageTextPool.Count; i++)
        {
            if (!DamageTextPool[i].activeSelf)
            {
                DamageTextPool[i].SetActive(true);

                DamageTextPool[i].GetComponent<DamageTextScript>().Spawn(SpawnPos, Damage, color);
                
                break;
            }
        }
    }

    public void SpawnDamageGuard(Vector3 SpawnPos)
    {
        SpawnPos.y += 0.5f;
        for (int i = 0; i < DamageTextPool.Count; i++)
        {
            if (!DamageTextPool[i].activeSelf)
            {
                DamageTextPool[i].SetActive(true);

                DamageTextPool[i].GetComponent<DamageTextScript>().SpawnGuard(SpawnPos);

                break;
            }
        }
    }

}
