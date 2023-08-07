using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BillardBlockScripts : MonoBehaviour
{
    //Private Variables
    Manager game_manager;
    float _fill_lerpVAL;
    bool unlocking = false;
    int generated = 0;

    //Public Variables
    public int remain, billard_price, money_put;

    [SerializeField] GameObject Billard_Obj;
    [SerializeField] Player_Collider p_coll;
    [SerializeField] Image billard_fill_img;

    [SerializeField] Transform puff;
    // DOTweenAnimation dotAnim;
    void Start()
    {
        _fill_lerpVAL = .02f;
        remain = billard_price = 1500;
        p_coll = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Collider>();
        game_manager = GameObject.Find("game_manager").GetComponent<Manager>();

        puff = transform.parent.GetChild(4);
        // dotAnim = puff.GetComponent<DOTweenAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (p_coll.money >= billard_price) // Enough money
            {
                unlocking = true;
            }

            else
            {
                unlocking = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (unlocking)
            {
                billard_fill_img.fillAmount = Mathf.MoveTowards(billard_fill_img.fillAmount, 1f, _fill_lerpVAL);
                PuffScaleUp();
                if (billard_fill_img.fillAmount == 1f && generated < 1)
                {
                    Billard_Obj.SetActive(true);
                    p_coll.money -= billard_price;
                    transform.parent.GetChild(1).gameObject.SetActive(false);
                    transform.parent.GetChild(4).gameObject.SetActive(false);
                    game_manager.availableBillardPos.Add(Billard_Obj.transform.GetChild(0).GetChild(0).gameObject);
                    game_manager.availableBillardPos.Add(Billard_Obj.transform.GetChild(1).GetChild(0).gameObject);

                    game_manager.BillardNAMES.Add(transform.parent.name);

                    FindObjectOfType<AudioManager>().Play("ArcadeUnlock");

                    game_manager.BcustomerFreeToMove = true;
                    generated++;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            unlocking = false;
            PuffScaleDown();
        }
    }

    private void Update()
    {
        if (!unlocking)
        {
            billard_fill_img.fillAmount = Mathf.MoveTowards(billard_fill_img.fillAmount, 0f, _fill_lerpVAL);
        }
    }

    void PuffScaleUp()
    {
        // dotAnim.DOPlay();
    }

    void PuffScaleDown()
    {
        // dotAnim.DORewind();
    }
}
