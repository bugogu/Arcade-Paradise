using System.Collections.Generic;
using UnityEngine;

public class BCustomerSpawner_Script : MonoBehaviour
{
    Manager g_manager;

    [SerializeField] List<GameObject> Bcustomers;

    public float timer = 0f;

    public int SpawnerWaitTime, BcustomerVAL = 0, toSpawnVal, randBcust_val;
    public GameObject BCustomerToSpawn;
    void Start()
    {
        SpawnerWaitTime = 6;
        g_manager = GameObject.Find("game_manager").GetComponent<Manager>();
    }

    void Update()
    {
        if (g_manager.BcustomerFreeToMove)
        {
            if (BcustomerVAL < g_manager.availableBillardPos.Count)
            {
                timer += Time.deltaTime;
                if (timer > SpawnerWaitTime)
                {
                    randBcust_val = Random.Range(0, Bcustomers.Count);
                    GameObject bcust_spawned = Instantiate(Bcustomers[randBcust_val], transform.position, transform.rotation);
                    BcustomerVAL++;
                    SpawnerWaitTime = Random.Range(5, 8);
                    timer = 0f;
                }
            }
        }
    }
}
