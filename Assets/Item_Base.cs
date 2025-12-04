using UnityEngine;

public class Item_Base : MonoBehaviour
{
    [SerializeField] private bool IsGet;//æ“¾ó‘Ô
    [SerializeField] private GameObject Getter;//æ“¾Ò

    [SerializeField] private 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
