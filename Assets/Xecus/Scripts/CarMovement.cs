using UnityEngine;

public class CarMovement : MonoBehaviour
{
    float moveSpeed = 20f;
    float xTimer = 0f;

    void Update()
    {
        xTimer += Time.deltaTime;
        if (xTimer > 20f)
        {
            Destroy(this.gameObject, 1f);
            xTimer = 0f;
        }
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
