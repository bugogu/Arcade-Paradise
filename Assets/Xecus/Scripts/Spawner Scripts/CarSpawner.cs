using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> Cars;

    float timer = 0f, CAR_wait_time;
    int randCARVAL;
    void Start()
    {
        CAR_wait_time = 10f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > CAR_wait_time)
        {
            randCARVAL = Random.Range(0, Cars.Count);
            Instantiate(Cars[randCARVAL], transform.position, transform.rotation);
            CAR_wait_time = Random.Range(10, 20);
            timer = 0f;
        }
    }
}
