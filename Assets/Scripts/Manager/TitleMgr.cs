using System.Collections;
using System.Collections.Generic;
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
        Character.GetComponent<TItleCharacterScript>().GameStartFunc();
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
}
