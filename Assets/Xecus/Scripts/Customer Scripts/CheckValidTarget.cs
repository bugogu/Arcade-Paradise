using UnityEngine;

public class CheckValidTarget : MonoBehaviour
{

    public GameObject targetARCADE;
    Customer_Move customer_move;
    Manager mngr;
    int chose_waitTime;

    public bool targetAvailable = true;
    public float tmr = 0f;
    void Start()
    {
        chose_waitTime = 2;
        customer_move = GetComponent<Customer_Move>();
        mngr = GameObject.Find("game_manager").GetComponent<Manager>();
    }

    void Update()
    {
        if (customer_move.enteredTheStore)
        {
            CheckingTargetAvailable();
        }
    }

    void CheckingTargetAvailable()
    {
        if (!customer_move.OnArcade)
        {
            targetARCADE = customer_move.currentTarget.transform.parent.gameObject;
            if (!mngr.availableGameBlocks.Contains(targetARCADE))
            {
                targetAvailable = false; customer_move.targetChoosen = false;

                tmr += Time.deltaTime;
                if (tmr > chose_waitTime)
                {
                    customer_move.moveSpeed = 4f;
                    tmr = 0f;
                }
            }
        }

    }
}
