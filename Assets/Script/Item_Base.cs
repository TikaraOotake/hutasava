using UnityEngine;

public class Item_Base : MonoBehaviour
{
    [SerializeField] protected bool IsGet;//æ“¾ó‘Ô
    [SerializeField] protected GameObject Getter;//æ“¾Ò

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //æ“¾ó‘Ô
        if (Getter != null)
        {
            //
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            //ƒvƒŒƒCƒ„[‚È‚ç‰ñûó‘Ô‚É
            Getter = other.gameObject;
        }
    }
}
