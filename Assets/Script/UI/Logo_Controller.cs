using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo_Controller : MonoBehaviour
{
    [SerializeField] private string TitleSceneName;

    [SerializeField] private float EventTimer = 0.0f;

    [SerializeField]
    private float FadeoutTime = 4.5f;
    private bool FadeoutFlag;

    [SerializeField]
    private float ChangeSceneTime = 6.0f;
    private bool ChangeSceneFlag;

    [SerializeField] private BlindFadeControl blind;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeoutTime <= EventTimer && FadeoutFlag == false)
        {
            Debug.Log("フェードアウトします");
            FadeoutFlag = true;


            if (blind != null)
            {
                blind.SetFadeFlag(true);
            }
        }
        if (ChangeSceneTime <= EventTimer && ChangeSceneFlag == false)
        {
            Debug.Log("チェンジシーンを開始します");
            ChangeSceneFlag = true;
            LoadScene(TitleSceneName);
        }

        //タイマー加算
        EventTimer += Time.deltaTime;
    }

    private void LoadScene(string targetScene)
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            if (!Application.CanStreamedLevelBeLoaded(targetScene))
            {
                Debug.LogError(
                $"Scene '{targetScene}' はBuild Profiles（SceneList）に登録されてないよ！"
                );
                return;
            }
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("シーンが設定されていないか、存在しないシーン名です");

        }
    }
}
