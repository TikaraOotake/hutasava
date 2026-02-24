using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private string targetScene;//遷移先のシーン

    [SerializeField] private float Timer = 1.0f;//移行までのタイマー

    [SerializeField] private float UncontrolTimer = 1.0f;//操作不能時間タイマー

    [SerializeField] private bool flag;

    [SerializeField] private BlindFadeControl blind;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && UncontrolTimer <= 0.0f)
        {
            //Fadeを掛ける
            if (blind != null) blind.SetFadeFlag(true);

            //フラグをtrue
            flag = true;
        }

        if (flag)
        {
            Timer = Mathf.Max(0, Timer - Time.deltaTime);

            if (Timer <= 0.0f)
            {
                //シーン読み込み開始
                LoadScene(targetScene);
            }
        }

        //更新
        UncontrolTimer = Mathf.Max(0.0f, UncontrolTimer - Time.deltaTime);
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
