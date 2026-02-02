using UnityEngine;

public class PairChakramBullet : PlayerBullet
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private bool FlipFlag;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject TargetObj = null;
        if (FlipFlag == true)
        {
            TargetObj = player1;
        }
        else
        {
            TargetObj = player2;
        }

        //à⁄ìÆ
        transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * BulletSpeed * Time.deltaTime);

        //ãóó£ÇéZèo
        float Length = Vector3.Distance(this.gameObject.transform.position, TargetObj.transform.position);
        if (Length <= BulletSpeed * Time.deltaTime)
        {
            FlipFlag = !FlipFlag;
        }

    }
}
