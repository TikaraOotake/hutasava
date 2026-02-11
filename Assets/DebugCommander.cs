using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DebugCommander : MonoBehaviour
{
    [SerializeField] private List<string> SceneNames = new List<string>();
    private static DebugCommander instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 重複を削除
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        int InputNum = -1;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // エディタの場合は再生停止
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // ビルド実行時はアプリ終了
            Application.Quit();
#endif
            Debug.Log("Game Exit triggered by ESC key");
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            InputNum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            InputNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            InputNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            InputNum = 3;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            InputNum = 4;
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            InputNum = 5;
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            InputNum = 6;
        }

        if (InputNum >= 0 && InputNum < SceneNames.Count)
        {
            LoadScene(SceneNames[InputNum]);
        }
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

            //時間を元にもどす
            Time.timeScale = 1.0f;

            //シーン読み込み
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("シーンが設定されていないか、存在しないシーン名です");

        }
    }
}
