using UnityEngine;

public class CoinRotateMove : MonoBehaviour
{
    float upDownMoveSpeed, rotateSpeed;
    float upPos, downPos;
    void Start()
    {
        upDownMoveSpeed = 2.0f; rotateSpeed = 35.0f;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
