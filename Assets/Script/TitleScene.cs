using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{/*
    [SerializeField] private SceneAsset targetScene;//遷移先のシーン
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Return))
        {
            string sceneName = " ";

            //シーン名取得
            if (targetScene != null)
            {
                sceneName = targetScene.name;
                if (!string.IsNullOrEmpty(sceneName))
                {
                    if (!Application.CanStreamedLevelBeLoaded(sceneName))
                    {
                        Debug.LogError(
                            $"Scene '{sceneName}' はBuild Profiles（SceneList）に登録されてないよ！"
                        );
                        return;
                    }
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.Log("シーンが設定されていないか、存在しないシーン名です");
                }
            }
        }
    }
  */
}
