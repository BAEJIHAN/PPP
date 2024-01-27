using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingMgr : MonoBehaviour
{
    public Image LoadingBar;
    public Text LoadingText;
    AsyncOperation op;
    bool LoadgingEnd = false;
    float LoadingValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    private void Update()
    {
        if (LoadgingEnd)
        {
            if(Input.GetMouseButtonDown(0))
            {
                op.allowSceneActivation = true;
            }
        }
        
    }
    // Update is called once per frame
    IEnumerator LoadMainScene()
    {
        
        op = SceneManager.LoadSceneAsync("SampleScene");
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;
            LoadingValue=Mathf.MoveTowards(LoadingValue, 1.0f, Time.deltaTime);
           

            LoadingBar.fillAmount = LoadingValue;
            
            if (op.progress>=0.9f && LoadingValue>=1.0f)
            {
                LoadingText.text = "마우스 우클릭으로 진행";
                LoadgingEnd = true;
                
                //yield break;
            }
        }
    }

    

}
