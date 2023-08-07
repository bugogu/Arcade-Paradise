using UnityEngine;

public class H_CheckValidTarget : MonoBehaviour
{
    public GameObject targetHockeyPos;
    H_CustomerMove hcust_move;
    Manager manager;

    public bool targetAvailable = true;
    void Start()
    {
        hcust_move = GetComponent<H_CustomerMove>();
        manager = GameObject.Find("game_manager").GetComponent<Manager>();
    }

    void Update()
    {
        CheckValid();
    }

    void CheckValid()
    {
        if (!hcust_move.OnHockey && hcust_move.goingToVisit != 0)
        {
            targetHockeyPos = hcust_move.TARGET;
            if (!manager.availableHockeyPos.Contains(targetHockeyPos))
            {
                targetAvailable = false; hcust_move.targetChoosen = false;
            }
        }
    }
}
