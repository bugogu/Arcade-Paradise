using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class IceHockeyBlockScript : MonoBehaviour
{
    //Private Variables
    float fill_lerp_val, fillToVal;
    int randNUM, generated = 0;
    bool unlocking = false;
    Manager manager;

    //Public Variables
    public int remaining, hockey_price, money_put;

    public List<GameObject> hockeys;
    [SerializeField] Player_Collider p_col;
    [SerializeField] Image hockey_fill_img;

    [SerializeField] Transform puff;
    // DOTweenAnimation dotAnim;
    void Start()
    {
        fill_lerp_val = 0.02f;
        remaining = hockey_price = 3000;
        p_col = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Collider>();
        manager = GameObject.Find("game_manager").GetComponent<Manager>();

        puff = transform.parent.GetChild(2);
        // dotAnim = puff.GetComponent<DOTweenAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (p_col.money >= hockey_price)
                {
                    unlocking = true;
                }
                else
                {
                    unlocking = false;
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (unlocking)
                {
                    hockey_fill_img.fillAmount = Mathf.MoveTowards(hockey_fill_img.fillAmount, 1f, fill_lerp_val);
                    PuffScaleUp();
                    if (hockey_fill_img.fillAmount == 1f && generated < 1)
                    {
                        randNUM = Random.Range(0, hockeys.Count);
                        hockeys[randNUM].SetActive(true);
                        p_col.money -= hockey_price;
                        transform.parent.GetChild(1).gameObject.SetActive(false);
                        transform.parent.GetChild(2).gameObject.SetActive(false);

                        manager.availableHockeyPos.Add(transform.parent.GetChild(3).gameObject);
                        manager.availableHockeyPos.Add(transform.parent.GetChild(4).gameObject);

                        manager.HockeyNAMES.Add(transform.parent.name);

                        FindObjectOfType<AudioManager>().Play("ArcadeUnlock");

                        manager.HcustomerFreeToMove = true;

                        generated++;
                    }
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                unlocking = false;
                PuffScaleDown();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (!unlocking)
        {
            hockey_fill_img.fillAmount = Mathf.MoveTowards(hockey_fill_img.fillAmount, 0f, fill_lerp_val);
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
