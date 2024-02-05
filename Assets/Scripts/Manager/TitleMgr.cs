using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    GameObject Character;
    public Button StartBtn;
    public Button ManualBtn;
    public Button ExitBtn;

    public GameObject ManualWindow;
    public Button ManualExitBtn;

    public Texture2D MouseTexture;

    public GValueData GData;

    AudioSource ASource;
    AudioClip AClip;

    bool IsStarted = false;
    private void Awake()
    {
        ASource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Character = GameObject.Find("TitleCharacter");

        if (StartBtn != null)
            StartBtn.onClick.AddListener(StartBtnFunc);

        if (ManualBtn != null)
            ManualBtn.onClick.AddListener(ManualBtnFunc);

        if (ExitBtn != null)
            ExitBtn.onClick.AddListener(ExitBtnFunc);

        if (ManualExitBtn != null)
            ManualExitBtn.onClick.AddListener(ManualExitFunc);

        Cursor.SetCursor(MouseTexture, new Vector2(MouseTexture.width / 3, MouseTexture.height / 3), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBtnFunc()
    {
        if (IsStarted)
            return;
        IsStarted = true;
        Character.GetComponent<TItleCharacterScript>().GameStartFunc();
        RefreshGValue();
        AClip = Resources.Load<AudioClip>("Sound/StartBGM");
        ASource.PlayOneShot(AClip);

        Invoke("StartLoadingFunc", 3.0f);
    }

    void ManualBtnFunc()
    {
        ManualWindow.SetActive(true);
    }

    void ExitBtnFunc()
    {
       Application.Quit();
    }

    void ManualExitFunc()
    {
        if (ManualWindow.activeSelf == true)
        {
            ManualWindow.SetActive(false);
        }
    }

    void StartLoadingFunc()
    {       
        SceneManager.LoadScene("LoadingScene");
    }

    void RefreshGValue()
    {
        if(GData.Data[0].Item== "PlayerDamage")
        {
            GValue.PlayerDamage = (int)GData.Data[0].Value;
        }

        if (GData.Data[1].Item == "DrakeDamage")
        {
            GValue.DrakeDamage = (int)GData.Data[1].Value;
        }

        if (GData.Data[2].Item == "PlayerAttackRange")
        {
            GValue.PlayerAttackRange = GData.Data[2].Value;
        }

        if (GData.Data[3].Item == "PlayerAttackSpeed")
        {
            GValue.PlayerAttackSpeed = GData.Data[3].Value;
        }

        if (GData.Data[4].Item == "MaxNMonNum")
        {
            GValue.MaxNMonNum = (int)GData.Data[4].Value;
        }
      
       




    }
}
