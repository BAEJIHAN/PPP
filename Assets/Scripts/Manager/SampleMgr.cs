using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SampleMgr : MonoBehaviour
{
    public static SampleMgr Inst=null;
    public GameObject MonSpawner;
    public GameObject Boss;
    public Text DText;
    public Image ExpBar;
    public GameObject LevelUpP;
    public Button LevelUpBtn1;
    public Button LevelUpBtn2;
    public Button LevelUpBtn3;

    public Button NormalSpawnToggleBtn;
    public Button BossSpawnBtn;

    bool IsNormalSpawn = true;
    public bool IsBoss = false;
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelUpBtn1 != null)
            LevelUpBtn1.onClick.AddListener(LevelUpBtn1Func);

        if (LevelUpBtn2 != null)
            LevelUpBtn2.onClick.AddListener(LevelUpBtn2Func);

        if (LevelUpBtn3 != null)
            LevelUpBtn3.onClick.AddListener(LevelUpBtn3Func);

        if (NormalSpawnToggleBtn != null)
            NormalSpawnToggleBtn.onClick.AddListener(NormalSpawnToggleBtnFunc);

        if(BossSpawnBtn!=null)
            BossSpawnBtn.onClick.AddListener(BossSpawnBtnFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetExpBar(float AddExp)
    {
        if (ExpBar == null)
            return;
        GValue.PlayerCurExp += AddExp;
        ExpBar.fillAmount = GValue.PlayerCurExp / GValue.PlayerMaxExp;

        if (GValue.PlayerCurExp>= GValue.PlayerMaxExp)
        {
            LevelUpP.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void LevelUpBtn1Func()
    {
        EndLevelUp();
    }
    void LevelUpBtn2Func()
    {
        EndLevelUp();
    }
    void LevelUpBtn3Func()
    {
        EndLevelUp();
    }

    void NormalSpawnToggleBtnFunc()
    {
        if(IsNormalSpawn)
        {            
            NormalSpawnToggleBtn.GetComponentInChildren<Text>().text = "일반 몬스터 소환 시작";
            MonSpawner.gameObject.SetActive(false);
        }
        else
        {
            NormalSpawnToggleBtn.GetComponentInChildren<Text>().text = "일반 몬스터 소환 중지";
            MonSpawner.gameObject.SetActive(true);
        }

        IsNormalSpawn = !IsNormalSpawn;

    }

    void BossSpawnBtnFunc()
    {
        if(!IsBoss)
        {
            IsBoss = true;
            GameObject tBoss = Instantiate(Boss);
            tBoss.transform.position = new Vector3(5, 0, 3);
        }
    }

    void EndLevelUp()
    {
        Time.timeScale = 1.0f;
        LevelUpP.SetActive(false);
        ResetExpBar();
    }

    void ResetExpBar()
    {
        GValue.PlayerCurExp = 0.0f;
        ExpBar.fillAmount = 0;
    }
}
