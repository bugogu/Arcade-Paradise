using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArcadeBlockScript : MonoBehaviour
{
    // Private Variables

    float _fill_lerp_val;
    int randGen, generated = 0;
    public bool unlocking, enough_cash, unlocked;
    Manager game_manager;

    //Public Variables
    public int remaining, arcade_price, money_put;
    public GameObject arcadeSpawnPoint;
    public List<GameObject> arcades;
    [SerializeField] Player_Collider p_col;
    [SerializeField] Image arcade_fill_img;

    [SerializeField] Transform puff;
    // DOTweenAnimation dotAnim;


    private void Start()
    {
        _fill_lerp_val = 0.02f;
        remaining = arcade_price = 500;
        p_col = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Collider>();
        game_manager = GameObject.Find("game_manager").GetComponent<Manager>();

        puff = transform.parent.GetChild(6);
        // dotAnim = puff.GetComponent<DOTweenAnimation>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")// Oyuncu ARCADE triggera giri� yapt�
        {
            if (p_col.money >= arcade_price)
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
        if (other.gameObject.tag == "Player")// Oyuncu arcade trigger i�inde duruyor
        {
            if (unlocking && !unlocked)
            {
                arcade_fill_img.fillAmount = Mathf.MoveTowards(arcade_fill_img.fillAmount, 1f, _fill_lerp_val);
                PuffScaleUp();
                if (arcade_fill_img.fillAmount == 1f && generated < 1)
                {
                    randGen = Random.Range(0, arcades.Count);
                    arcades[randGen].SetActive(true);
                    p_col.money -= arcade_price;
                    transform.parent.GetChild(1).gameObject.SetActive(false);
                    transform.parent.GetChild(6).gameObject.SetActive(false);
                    transform.parent.GetChild(4).gameObject.SetActive(true);

                    game_manager.availableGameBlocks.Add(transform.parent.gameObject);

                    game_manager.arcadeNAMES.Add(transform.parent.name);

                    FindObjectOfType<AudioManager>().Play("ArcadeUnlock");
                    game_manager.customerFreeToMove = true;
                    generated++;
                    unlocked = true;
                }
            }
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


    private void Update()
    {
        if (!unlocking)
        {
            arcade_fill_img.fillAmount = Mathf.MoveTowards(arcade_fill_img.fillAmount, 0f, _fill_lerp_val);
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
