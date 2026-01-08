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

    private Dictionary<string, AudioClip> seDictionary;
    private AudioSource audioSource;

    //シングルトン
    public static SoundManager instance;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.Log("audioSourceの取得に失敗");

        seDictionary = new Dictionary<string, AudioClip>();
        foreach(var se in seDatas)
        {
            if(!seDictionary.ContainsKey(se.seName))
            {
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
        if (audioSource == null) Debug.Log("audioSourceがnull");

        //SE名に対応するAudioClipを取得
        if(seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            //再生
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SE not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
