using UnityEngine;

public class B_CustomerCheckValidTarget : MonoBehaviour
{
    public GameObject targetBillardPos;
    B_Customer_Move bcust_move;
    Manager manager;

    public bool targetAvailable = true;
    public float tmr = 0f;
    void Start()
    {
        bcust_move = GetComponent<B_Customer_Move>();
        manager = GameObject.Find("game_manager").GetComponent<Manager>();
    }

    void Update()
    {
        if (bcust_move.isEnteredStore)
        {
            CheckingTargetAvailable();
        }
    }

    void CheckingTargetAvailable()
    {
        if (!bcust_move.OnBillard && bcust_move.visitAmount != 0)
        {
            targetBillardPos = bcust_move.targetNOW;
            if (!manager.availableBillardPos.Contains(targetBillardPos))
            {
                targetAvailable = false; bcust_move.targetChoosen = false;
            }
        }
    }
}
