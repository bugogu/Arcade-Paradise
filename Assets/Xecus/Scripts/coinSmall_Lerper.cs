using UnityEngine;

public class coinSmall_Lerper : MonoBehaviour
{
    Transform PlayerPos;
    float Lerp_Modifier;
    void Start()
    {
        PlayerPos = GameObject.Find("Player").transform;
        Lerp_Modifier = 20f;
    }

    void Update()
    {
        if (transform.position != PlayerPos.position)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(PlayerPos.position.x, PlayerPos.position.y + 2.2f, PlayerPos.position.z), Lerp_Modifier * Time.deltaTime);
        }

        if ((PlayerPos.position - transform.position).magnitude < 2.21)
        {
            Destroy(gameObject);
        }
    }
}
