using UnityEngine;

public class H_GamePoint : MonoBehaviour
{
    [SerializeField] GameObject HeaderCoin;
    public Transform h_coinspawn_point;
    public int CoinInHockey = 0;
    public bool H_occupied = false;
    public float timer = 0f, H_coinWaitTime = 2.5f;
    void Start()
    {
        H_coinWaitTime = 4f;
    }

    void Update()
    {
        if (CoinInHockey >= 1)
        {
            HeaderCoin.SetActive(true);
        }
        else
        {
            HeaderCoin.SetActive(false);
        }

        if (H_occupied)
        {
            timer += Time.deltaTime;
            if (timer > H_coinWaitTime)
            {
                CoinInHockey += 1;
                if (CoinInHockey >= 10)
                {
                    CoinInHockey = 10;
                }
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            H_occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            H_occupied = false;
        }
    }
}
