using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class CollectHandler : MonoBehaviour
{

    [SerializeField] GameObject coinSmall;
    [SerializeField] GameObject em_coinSmall;
    [SerializeField] GameObject StackCoin;

    [SerializeField] Transform stackPoint;

    [SerializeField] TextMeshProUGUI moneyTXT;
    [SerializeField] GameObject moneyIMG;
    [SerializeField] RectTransform canvasSpawnPOINT;
    [SerializeField] Canvas GameCanvas;

    public List<GameObject> Coins;

    GamePointScript g_point;
    H_GamePoint h_point;
    B_GamePointScript b_point, b1_point;

    [SerializeField] Transform puff;
    // DOTweenAnimation dotAnim;

    [SerializeField] Player_Collider p_coll;

    Vector2 screenVEC;

    public float pTimer = 0f, tt_Timer = 0f;
    public float timer = 0f, Htimer = 0f, Btimer = 0f, B1_timer = 0f;
    float smallCoinSpawnWaitTime;
    public float adder;
    public int counter = 0, ct;
    void Start()
    {
        puff = GameObject.Find("CashierFrame").transform;
        // dotAnim = puff.GetComponent<DOTweenAnimation>();

        adder = stackPoint.position.y;
        smallCoinSpawnWaitTime = .1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "game_point_trigger":
                g_point = other.GetComponent<GamePointScript>();

                break;

            case "hockey_trigger":
                h_point = other.transform.parent.GetChild(3).GetComponent<H_GamePoint>();

                break;

            case "billard_trigger":
                b_point = other.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<B_GamePointScript>();
                b1_point = other.transform.parent.GetChild(2).GetChild(1).GetChild(0).GetComponent<B_GamePointScript>();

                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "game_point_trigger":
                if (g_point.CoinInArcade > 0)
                {
                    timer += Time.deltaTime;
                    if (timer > smallCoinSpawnWaitTime)
                    {
                        GameObject sscoc = Instantiate(coinSmall, other.transform.parent.GetChild(22).transform.position, other.transform.parent.GetChild(22).transform.rotation);
                        GameObject scoc = Instantiate(StackCoin, stackPoint.position, stackPoint.rotation);
                        scoc.transform.SetParent(gameObject.transform);
                        Coins.Add(scoc);
                        counter++;
                        p_coll.coin++;
                        adder = stackPoint.position.y + .1f;
                        stackPoint.position = new Vector3(stackPoint.position.x, adder, stackPoint.position.z);

                        FindObjectOfType<AudioManager>().Play("CoinReceived");
                        g_point.CoinInArcade--;
                        timer = 0f;
                    }
                }
                else
                {
                    tt_Timer += Time.deltaTime;
                    if (tt_Timer > .15f)
                    {
                        FindObjectOfType<AudioManager>().Stop("CoinReceived");
                        tt_Timer = 0f;
                    }
                }

                break;

            case "hockey_trigger":
                if (h_point.CoinInHockey > 0)
                {
                    Htimer += Time.deltaTime;
                    if (Htimer > smallCoinSpawnWaitTime)
                    {
                        GameObject sscoc = Instantiate(coinSmall, other.transform.parent.GetChild(11).position, other.transform.parent.GetChild(11).rotation);
                        GameObject scoc = Instantiate(StackCoin, stackPoint.position, stackPoint.rotation);
                        scoc.transform.SetParent(gameObject.transform);
                        Coins.Add(scoc);
                        counter++;
                        p_coll.coin++;
                        adder = stackPoint.position.y + .1f;
                        stackPoint.position = new Vector3(stackPoint.position.x, adder, stackPoint.position.z);

                        FindObjectOfType<AudioManager>().Play("CoinReceived");
                        h_point.CoinInHockey--;
                        Htimer = 0f;
                    }
                }
                else
                {
                    tt_Timer += Time.deltaTime;
                    if (tt_Timer > .15f)
                    {
                        FindObjectOfType<AudioManager>().Stop("CoinReceived");
                        tt_Timer = 0f;
                    }
                }

                break;

            case "billard_trigger":
                if (b_point.CoinInBillard > 0)
                {
                    Btimer += Time.deltaTime;
                    if (Btimer > smallCoinSpawnWaitTime)
                    {
                        GameObject sscoc = Instantiate(coinSmall, other.transform.parent.GetChild(6).position, other.transform.parent.GetChild(6).rotation);
                        GameObject scoc = Instantiate(StackCoin, stackPoint.position, stackPoint.rotation);
                        scoc.transform.SetParent(gameObject.transform);
                        Coins.Add(scoc);
                        counter++;
                        p_coll.coin++;
                        adder = stackPoint.position.y + .1f;
                        stackPoint.position = new Vector3(stackPoint.position.x, adder, stackPoint.position.z);

                        FindObjectOfType<AudioManager>().Play("CoinReceived");
                        b_point.CoinInBillard--;
                        Btimer = 0f;
                    }
                }

                else
                {
                    tt_Timer += Time.deltaTime;
                    if (tt_Timer > .15f)
                    {
                        FindObjectOfType<AudioManager>().Stop("CoinReceived");
                        tt_Timer = 0f;
                    }
                }

                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "game_point_trigger")
        {
            FindObjectOfType<AudioManager>().Stop("CoinReceived");
        }
    }

    void Update()
    {
        moneyTXT.text = p_coll.money.ToString();
        if (p_coll.emptying && Coins.Count > 0)// Coinleri bo�alt
        {
            pTimer += Time.deltaTime;
            DOTUP();
            if (pTimer > .1f)
            {
                ct = Coins.Count;
                GameObject scoc = Instantiate(em_coinSmall, transform.position, Quaternion.Euler(0, 90, 0));
                GameObject ttt = Coins[ct - 1];
                Destroy(ttt);

                Coins.RemoveAt(ct - 1);
                moneyTXT.text = p_coll.money.ToString();
                p_coll.coin--; p_coll.money += DesignManager.Instance.coinConvertedValue; adder -= .1f;

                FindObjectOfType<AudioManager>().Play("CoinGiven");

                if (Coins.Count == 0)
                {
                    FindObjectOfType<AudioManager>().Stop("CoinGiven");
                }

                stackPoint.position = new Vector3(stackPoint.position.x, adder, stackPoint.position.z);// stack pointi yukar� ta��

                screenVEC = new Vector2((500), (1000));

                GameObject eas = Instantiate(moneyIMG, screenVEC, canvasSpawnPOINT.rotation);
                eas.transform.SetParent(GameCanvas.transform);
                //Destroy(ea�, 1f);


                pTimer = 0f;
            }
        }
        else
        {
            DOTDOWN();
        }

    }

    void DOTUP()
    {
        // dotAnim.DOPlay();
    }

    void DOTDOWN()
    {
        // dotAnim.DORewind();
    }

}
