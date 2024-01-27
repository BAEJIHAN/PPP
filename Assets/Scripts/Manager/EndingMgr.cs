using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingMgr : MonoBehaviour
{
    public Button ToTitleBtn;
    public Button ExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (ToTitleBtn != null)
            ToTitleBtn.onClick.AddListener(ToTitleBtnFunc);

        if (ExitBtn != null)
            ExitBtn.onClick.AddListener(ExitBtnFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToTitleBtnFunc()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void ExitBtnFunc()
    {
        Application.Quit();
    }

}
