using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LVUPFUNC
{
    DAMAGE,
    DKDAMAGE,
    RANGE,
    RATE
}

class LVFuncValue
{
    public float value;
    public LVFuncValue(float _value)
    {
        value = _value;
    }

    public LVFuncValue(int _value)
    {
        value = (float)_value;
    }
}
public class SampleMgr : MonoBehaviour
{

    GameObject Player;

    public static SampleMgr Inst=null;
    public GameObject MonSpawner;
    public GameObject[] Boss;
    
    public Text DText;
    public Image ExpBar;
    public Image HPBar;
    public Text KillNumText;
    public GameObject BossEventUI;
    
    [Header("Level Up")]
    public GameObject LevelUpP;
    public Button LevelUpBtn1;
    public Button LevelUpBtn2;
    public Button LevelUpBtn3;
    public Text[] LevelUpTexts;
    delegate void del();
    del LevelUpDel1;
    del LevelUpDel2;
    del LevelUpDel3;

    List<del> LevelUpFuncArr=new List<del>();
    List<string> LevelUpTextArr=new List<string>();
    List<LVFuncValue> LevelUpTextValueArr = new List<LVFuncValue>();
    List<LVFuncValue> LevelUpTextAddValueArr = new List<LVFuncValue>();
    int[] LevelUpIndex = new int[5] { 0, 1, 2, 3, 4};

    [Header("Toggle UI")]
    public Button NormalSpawnToggleBtn;
    public Button BossSpawnBtn;

    public GameObject MobileUI;
    public Button MobileToggleBtn;

    [Header("SpinCool")]
    public GameObject SpinCool;
    public Image SpinBlur;
    public Text SpinCoolText;

    [Header("Manual")]
    public GameObject ManualWindow;
    public Button ManualOpenBtn;
    public Button ManualExitBtn;
    public Button ToTitleBtn;
    public Button GameExitBtn;

    [Header("DeathWindow")]
    public GameObject DeathWindow;
    public Button DeathToTitleBtn;
    public Button DeathExitBtn;

    bool IsNormalSpawn = true;
    [HideInInspector] public bool IsOnBossEvent = false;
    [HideInInspector] public bool IsBoss = false;

    [HideInInspector] public bool IsMobile = false;
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        
        LevelUpFuncArr.Add(AddPlayerDamage);
        LevelUpFuncArr.Add(AddDrakeDamage);
        LevelUpFuncArr.Add(AddPlayerAttakRange);
        LevelUpFuncArr.Add(AddPlayerAttakSpeed);
        LevelUpTextArr.Add("플레이어\n 공격력 상승");
        LevelUpTextArr.Add("펫 공격력 상승");
        LevelUpTextArr.Add("플레이어\n 공격 범위 상승");
        LevelUpTextArr.Add("플레이어\n 공격 속도 상승");
        LevelUpTextValueArr.Add(new LVFuncValue(GValue.PlayerDamage));
        LevelUpTextValueArr.Add(new LVFuncValue(GValue.DrakeDamage));
        LevelUpTextValueArr.Add(new LVFuncValue(GValue.PlayerAttackRange));
        LevelUpTextValueArr.Add(new LVFuncValue(GValue.PlayerAttackSpeed));
        LevelUpTextAddValueArr.Add(new LVFuncValue(1));
        LevelUpTextAddValueArr.Add(new LVFuncValue(1));
        LevelUpTextAddValueArr.Add(new LVFuncValue(0.5f));
        LevelUpTextAddValueArr.Add(new LVFuncValue(0.5f));

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

        if (MobileToggleBtn != null)
            MobileToggleBtn.onClick.AddListener(MobileToggleBtnFunc);

        if (ManualOpenBtn != null)
            ManualOpenBtn.onClick.AddListener(ManualOpenBtnFunc);

        if (ManualExitBtn != null)
            ManualExitBtn.onClick.AddListener(ManualExitBtnFunc);

        if (ToTitleBtn != null)
            ToTitleBtn.onClick.AddListener(ToTitleBtnFunc);

        if (GameExitBtn != null)
            GameExitBtn.onClick.AddListener(GameExitBtnFunc);

        if (DeathToTitleBtn != null)
        {
            DeathToTitleBtn.onClick.AddListener(ToTitleBtnFunc);
        }

        if (DeathExitBtn != null)
        {
            DeathExitBtn.onClick.AddListener(GameExitBtnFunc);
        }
        ;

    }

    // Update is called once per frame
    void Update()
    {
        DText.text = GValue.PlayerDamage.ToString();
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
            SetLevelUp();
            
        }
    }

    public void SetHpBar()
    {
        if (HPBar == null)
            return;

        float Deno = (float)GValue.MaxPlayerHP;
        float Nume = (float)GValue.CurPlayerHP;
        HPBar.fillAmount = Nume / Deno;

       
    }

    public void SetKillText()
    {
        KillNumText.text = "Kill : " + GValue.KilledMon.ToString();
    }

    public void BossSpawnFunc()
    {
        if (!IsBoss)
        {
            if (GValue.BossIdx > 2)
                return;
            IsBoss = true;
            GameObject tBoss = Instantiate(Boss[GValue.BossIdx]);
            //GameObject tBoss = Instantiate(Boss[1]);


            tBoss.transform.position = new Vector3(5, 0, 3);
            GValue.BossIdx++;
            Camera.main.gameObject.GetComponent<CameraScript>().SetFocusBoss(tBoss);
        }
    }

    #region ButtonFunc
    void LevelUpBtn1Func()
    {
        LevelUpDel1();
        EndLevelUp();
    }
    void LevelUpBtn2Func()
    {
        LevelUpDel2();
        EndLevelUp();
    }
    void LevelUpBtn3Func()
    {
        LevelUpDel3();
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
        BossSpawnFunc();
    }


    void MobileToggleBtnFunc()
    {
        if (IsMobile)
        {
            MobileUI.SetActive(false);
            MobileToggleBtn.GetComponentInChildren<Text>().text = "모바일 UI On";
        }
        else
        {
            MobileUI.SetActive(true);
            MobileToggleBtn.GetComponentInChildren<Text>().text = "모바일 UI Off";
        }

        IsMobile = !IsMobile;
    }

    void ManualOpenBtnFunc()
    {
        Time.timeScale = 0.0f;
        ManualWindow.SetActive(true);
    }
    void ManualExitBtnFunc()
    {
        Time.timeScale = 1.0f;
        ManualWindow.SetActive(false);
    }
    void ToTitleBtnFunc()
    {
        SceneManager.LoadScene("TitleScene");
        Time.timeScale = 1.0f;
    }
    void GameExitBtnFunc()
    {
        Application.Quit();
    }

    #endregion


    public void BossEventUIOn()
    {
        BossEventUI.SetActive(true);
    }
    public void BossEventUIOff()
    {
        BossEventUI.SetActive(false);
    }

    void EndLevelUp()
    {
        Time.timeScale = 1.0f;
        LevelUpP.SetActive(false);
        ResetExpBar();
        GValue.CurPlayerHP = GValue.MaxPlayerHP;
        SetHpBar();
        GValue.Level++;
    }

    void ResetExpBar()
    {
        GValue.PlayerCurExp = 0.0f;
        ExpBar.fillAmount = 0;
    }

    void SetLevelUp()
    {
        LevelUpTextValueArr[0].value = GValue.PlayerDamage;
        LevelUpTextValueArr[1].value = GValue.DrakeDamage; 
        LevelUpTextValueArr[2].value = GValue.PlayerAttackRange; 
        LevelUpTextValueArr[3].value = GValue.PlayerAttackSpeed; 



        int[] RanIdx=new int[3];
        int tempRan = -1;
        for(int i=0; i<3; i++)
        {
            while(true)
            {
                RanIdx[i] = Random.Range(0, 4);
                if(RanIdx[i]!=tempRan)
                {
                    tempRan = RanIdx[i];
                    break;
                }
            }
            
        }
        
        LevelUpDel1 = LevelUpFuncArr[RanIdx[0]];
        LevelUpDel2 = LevelUpFuncArr[RanIdx[1]];
        LevelUpDel3 = LevelUpFuncArr[RanIdx[2]];

        LevelUpTexts[0].text = LevelUpTextArr[RanIdx[0]];
        LevelUpTexts[1].text = LevelUpTextArr[RanIdx[1]];
        LevelUpTexts[2].text = LevelUpTextArr[RanIdx[2]];

        LevelUpTexts[0].text +="\n 현재 " + LevelUpTextValueArr[RanIdx[0]].value.ToString("F1")
            + "\n상승량 "+ LevelUpTextAddValueArr[RanIdx[0]].value.ToString("F1");
        LevelUpTexts[1].text += "\n 현재 " + LevelUpTextValueArr[RanIdx[1]].value.ToString("F1")
            + "\n상승량 " + LevelUpTextAddValueArr[RanIdx[1]].value.ToString("F1"); 
        LevelUpTexts[2].text += "\n 현재 " + LevelUpTextValueArr[RanIdx[2]].value.ToString("F1")
            + "\n상승량 " + LevelUpTextAddValueArr[RanIdx[2]].value.ToString("F1"); ;




    }

    public void MonsterEventStart()
    {
        MonSpawner.gameObject.SetActive(false);
        GameObject[] Mons = GameObject.FindGameObjectsWithTag("NormalMonster");
        for (int i = 0; i < Mons.Length; i++)
        {
            Mons[i].GetComponent<NormalMonRootScript>().EventStart();
        }
    }

    public void MonsterEventEnd()
    {
        MonSpawner.gameObject.SetActive(true);
        GameObject[] Mons = GameObject.FindGameObjectsWithTag("NormalMonster");
        for (int i = 0; i < Mons.Length; i++)
        {
            Mons[i].GetComponent<NormalMonRootScript>().EventEnd();
        }
    }

    public void GameOverFunc()
    {
        DeathWindow.SetActive(true);
    }

    //////////////////////////////////
    void AddPlayerDamage()
    {
        GValue.PlayerDamage++;
    }
    
    void AddDrakeDamage()
    {
        GValue.DrakeDamage++;
    }

    void AddPlayerAttakRange()
    {
        GValue.PlayerAttackRange += 0.5f;
        Player.GetComponent<PlayerScript>().SetAttackRange();
    }

    void AddPlayerAttakSpeed()
    {
        GValue.PlayerAttackRange += 0.5f;
        Player.GetComponent<PlayerScript>().SetAttackSpeed();
    }

}
