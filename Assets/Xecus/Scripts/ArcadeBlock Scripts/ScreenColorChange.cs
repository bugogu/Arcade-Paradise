using UnityEngine;

public class ScreenColorChange : MonoBehaviour
{
    MeshRenderer mesh;
    [SerializeField] GamePointScript g_point;
    [SerializeField][Range(0f, 2f)] float lerpTime;
    [SerializeField] Color[] myColors;
    [SerializeField] Material[] materials;
    [SerializeField] Material blackMAT;

    int colorIndex = 0, len;
    float t = 0f;
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        g_point = transform.parent.parent.parent.GetChild(4).GetComponent<GamePointScript>();
        len = myColors.Length;
    }

    void Update()
    {
        if (transform.parent.gameObject.activeSelf)
        {
            if (g_point.occupied)
            {
                mesh.material = materials[colorIndex];

                t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
                if (t > .9f)
                {
                    colorIndex++;
                    colorIndex = (colorIndex >= len) ? 0 : colorIndex;
                    t = 0f;
                }
            }
        }

    }
}
