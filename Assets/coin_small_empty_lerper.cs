using UnityEngine;

public class coin_small_empty_lerper : MonoBehaviour
{
    Transform TransformPOS;
    float Lerp_Modifier;
    void Start()
    {
        TransformPOS = GameObject.Find("Kasa").transform;
        Lerp_Modifier = 20f;
    }

    void Update()
    {
        if (transform.position != TransformPOS.position)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(TransformPOS.position.x, TransformPOS.position.y + 2.2f, TransformPOS.position.z), Lerp_Modifier * Time.deltaTime);
        }

        if ((TransformPOS.position - transform.position).magnitude < 2.21)
        {
            Destroy(gameObject);
        }
    }
}
