using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireRotateScript : MonoBehaviour
{
    float rotateSpeed;
    void Start()
    {
        rotateSpeed = 150f;
    }

    void Update()
    {
        transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
