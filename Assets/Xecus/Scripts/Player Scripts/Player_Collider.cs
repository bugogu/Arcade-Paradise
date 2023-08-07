using UnityEngine;

public class Player_Collider : MonoBehaviour
{
    public int coin, money;
    public bool emptying;
    float timer = 0f, coinWaitTime;

    [SerializeField] CollectHandler collectHandler;

    [SerializeField] GameObject small_coin;
    [SerializeField] Transform coinDest;
    private void Start()
    {
        coinWaitTime = .2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "trade_zone")
        {
            emptying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "trade_zone")
        {
            emptying = false;
        }
    }
}
