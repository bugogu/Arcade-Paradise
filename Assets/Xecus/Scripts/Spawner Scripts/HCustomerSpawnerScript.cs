using System.Collections.Generic;
using UnityEngine;

public class HCustomerSpawnerScript : MonoBehaviour
{
    Manager g_manager;

    [SerializeField] List<GameObject> Hcustomers;

    public float timer = 0f;

    public int SpawnerWaitTime, HcustomerVAL = 0, Hcust_val;
    public GameObject HCustomerToSpawn;
    void Start()
    {
        SpawnerWaitTime = 5;
        g_manager = GameObject.Find("game_manager").GetComponent<Manager>();
    }

    void Update()
    {
        if (g_manager.HcustomerFreeToMove)
        {
            if (HcustomerVAL < g_manager.availableHockeyPos.Count)
            {
                timer += Time.deltaTime;
                if (timer > SpawnerWaitTime)
                {
                    Hcust_val = Random.Range(0, Hcustomers.Count);
                    GameObject hcust_spawned = Instantiate(Hcustomers[Hcust_val], transform.position, transform.rotation);
                    HcustomerVAL++;
                    SpawnerWaitTime = Random.Range(4, 8);
                    timer = 0f;
                }
            }
        }
    }
}
