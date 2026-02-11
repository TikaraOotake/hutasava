using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneShifter : MonoBehaviour
{
    [SerializeField] private string DemoScene;//遷移先のシーン

    [SerializeField] private float Timer = 1.0f;//移行までのタイマー

    [SerializeField] private float DemoSceneShiftTimer = 0.0f;//デモシーンに移行するタイマー
    [SerializeField] private float DemoSceneShiftTime = 10.0f;//デモシーンに移行するタイマー

    [SerializeField] private bool flag;

    [SerializeField] private BlindFadeControl blind;
    void Start()
    {
        //タイマーセット
        DemoSceneShiftTimer = DemoSceneShiftTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (DemoSceneShiftTimer <= 0.0f)
        {
            //タイマーが切れたら

            //Fadeを掛ける
            if (blind != null) blind.SetFadeFlag(true);

            //フラグをtrue
            flag = true;
        }
        else
        {
            //タイマー経過
            DemoSceneShiftTimer = Mathf.Max(0.0f, DemoSceneShiftTimer - Time.deltaTime);
        }

        if (flag)
        {
            Timer = Mathf.Max(0, Timer - Time.deltaTime);

            if (Timer <= 0.0f)
            {
                //シーン読み込み開始
                LoadScene(DemoScene);
            }
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
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("シーンが設定されていないか、存在しないシーン名です");

        }
    }
}
