using UnityEngine;

public class Cam_Script : MonoBehaviour
{
    Vector3 offset, desiredPOS;
    [SerializeField] Transform follow_obj;
    void Start()
    {
        offset = new Vector3(0, -22, 18);
    }

    void LateUpdate()
    {
        desiredPOS = follow_obj.transform.position - offset;
        transform.position = desiredPOS;
    }
}
