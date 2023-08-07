using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    Manager game_manager;

    [SerializeField] List<GameObject> Customers;

    public float timer = 0f;
    public int SpawnerWaitTime, customerVAL = 0, toSpawnVAL, randC_val;
    public GameObject CustomerToSpawn;
    void Start()
    {
        game_manager = GameObject.Find("game_manager").GetComponent<Manager>();

    }

    private void Update()
    {
        if (game_manager.availableGameBlocks.Count > 0)
        {
            if (customerVAL < game_manager.availableGameBlocks.Count)
            {
                timer += Time.deltaTime;
                if (timer > SpawnerWaitTime)
                {
                    randC_val = Random.Range(0, Customers.Count);
                    GameObject spawnedCustomer = Instantiate(Customers[randC_val], transform.position, transform.rotation);
                    customerVAL++;
                    SpawnerWaitTime = Random.Range(2, 5);
                    timer = 0f;
                }
            }

        }
    }
}
