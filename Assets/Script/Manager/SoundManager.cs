using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class seDeta
{
    public string seName;       //SE名
    public AudioClip seClip;    //SE音
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private seDeta[] seDatas;
    [SerializeField]
    private float seInterval = 0.1f;

    //宣言
    private Dictionary<string, AudioClip> seDictionary;
    private Dictionary<string, float> lastPlayTime = new Dictionary<string, float>();
    private AudioSource audioSource;

    //シングルトン
    public static SoundManager instance;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("audioSourceの取得に失敗");
            return;
        }
        //初期化
        seDictionary = new Dictionary<string, AudioClip>();
        foreach(var se in seDatas)
        {
            //存在するか
            if(!seDictionary.ContainsKey(se.seName))
            {
                //追加
                seDictionary.Add(se.seName,se.seClip);
            }
            else
            {
                Debug.Log("SEが重複");
            }
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //指定したSEを再生する
    public void PlaySE(string seName)
    {
        if (audioSource == null)
        {
            Debug.Log("audioSourceがnull");
            return;
        }
        //SE名に対応するAudioClipを取得
        if (seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            //再生
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SE not found");
        }
    }

    //画面内にある指定したSEを一定時間待って再生する
    public void PlaySEIntervalOnScreen(string seName, Vector3 targetObject)
    {
        if (audioSource == null)
        {
            Debug.Log("audioSourceがnull");
            return;
        }
        //SE名に対応するAudioClipを取得
        if(!seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            Debug.Log("SE not found");
        }
        //一定時間経っていなければ再生しない
        if(lastPlayTime.TryGetValue(seName, out float lastTime))
        {
            if (Time.unscaledTime - lastTime < seInterval) 
            {
                return;
            }
        }
        //画面内になければ再生しない
        if(!IsOnScreen(targetObject))
        {
            return;
        }
        //再生
        audioSource.PlayOneShot(clip);
        lastPlayTime[seName] = Time.unscaledTime;
    }

    //画面内にあるか
    bool IsOnScreen(Vector3 worldPos)
    {
        Camera cam = Camera.main;
        if (cam == null) return false;

        Vector3 vp = cam.WorldToViewportPoint(worldPos);
        return vp.z > 0 && vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
