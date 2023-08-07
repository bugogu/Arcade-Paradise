using UnityEngine;

public class GamePointScript : MonoBehaviour
{
    [SerializeField] GameObject CoinHeader;
    public Transform coinSpawnPoint;
    public int CoinInArcade = 0;
    public bool occupied = false;
    public float timer = 0f, CoinWaitTime = 3f;
    void Start()
    {
        CoinWaitTime = 2.5f;
    }

    void Update()
    {
        if (CoinInArcade >= 1)
        {
            CoinHeader.SetActive(true);
        }

        else
        {
            CoinHeader.SetActive(false);
        }

        if (occupied)
        {
            timer += Time.deltaTime;
            if (timer > CoinWaitTime)
            {
                CoinInArcade += 1;
                if (CoinInArcade >= 10)
                {
                    CoinInArcade = 10;
                }
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            if (other.gameObject.GetComponent<Customer_Move>().currentTarget == this.transform.parent.GetChild(2).gameObject)
            {
                occupied = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            occupied = false;
        }
    }
}
