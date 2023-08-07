using UnityEngine;

public class B_GamePointScript : MonoBehaviour
{
    [SerializeField] GameObject HeaderCoin;
    public Transform b_coinSpawnPoint;
    public int CoinInBillard = 0;
    public bool B_occupied = false;
    public float timer = 0f, B_CoinWaitTime = 3f;
    void Start()
    {
        B_CoinWaitTime = 4f;
    }

    void Update()
    {
        if (CoinInBillard >= 1)
        {
            HeaderCoin.SetActive(true);
        }
        else
        {
            HeaderCoin.SetActive(false);
        }

        if (B_occupied)
        {
            timer += Time.deltaTime;
            if (timer > B_CoinWaitTime)
            {
                CoinInBillard += 1;
                if (CoinInBillard >= 10)
                {
                    CoinInBillard = 10;
                }
                timer = 0f;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            B_occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            B_occupied = false;
        }
    }
}
