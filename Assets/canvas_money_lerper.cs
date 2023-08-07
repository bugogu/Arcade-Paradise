using UnityEngine;
using DG.Tweening;

public class canvas_money_lerper : MonoBehaviour
{

    float lerp_modidier, scaleLerpModifier;
    [SerializeField] RectTransform destination;

    GameObject counterScaleObj;

    Vector3 scaleScale;

    void Start()
    {

        scaleScale = new Vector3(.1f, .1f, .05f);
        scaleLerpModifier = 0.2f;
        lerp_modidier = 0.1f;
        destination = GameObject.FindGameObjectWithTag("counterTAG").GetComponent<RectTransform>();
        counterScaleObj = GameObject.FindGameObjectWithTag("counterTAG");
    }

    void Update()
    {
        if (transform.position != destination.position)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, lerp_modidier);
        }

        if ((destination.position - transform.position).magnitude < 100f)
        {
            Destroy(transform.gameObject);
            CounterScaleUp();
            CounterScaleDown();
        }
    }

    void CounterScaleUp()
    {
        destination.DOScale(new Vector3(2f, 2f, 1f), scaleLerpModifier).SetEase(Ease.Linear);
    }

    void CounterScaleDown()
    {
        destination.DOScale(new Vector3(1f, 1f, 1f), scaleLerpModifier).SetEase(Ease.Linear);
    }
}
