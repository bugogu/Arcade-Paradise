using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Manager : MonoSing<Manager>
{
    ArcadeBlockScript arcadeBS;
    IceHockeyBlockScript hockeyBS;
    [SerializeField] CollectHandler col_handler;

    [SerializeField] Player_Collider p_coll;
    [SerializeField] GameObject stackCoin;
    [SerializeField] Transform stackPOINT;

    GameObject[] arcadeSAAS; GameObject[] billardSAAS; GameObject[] hockeySAAS;

    public List<string> arcadeNAMES;
    public List<string> BillardNAMES;
    public List<string> HockeyNAMES;

    public List<GameObject> availableGameBlocks;
    public List<GameObject> availableBillardPos;
    public List<GameObject> availableHockeyPos;



    public bool isAnyGameUnlocked, customerFreeToMove; // Default Customer
    public bool isAnyBillardUnlocked, BcustomerFreeToMove; // Billard Customer
    public bool isAnyHockeyUnlocked, HcustomerFreeToMove; //Hockey Customer

    public int CustomerInside, BCustomerInside, HCustomerInside;
    public int RandVAL;

    private void Awake()
    {
        LoadDatas();
    }

    void Start()
    {
        CustomerInside = 0; BCustomerInside = 0; HCustomerInside = 0;
        isAnyGameUnlocked = false;
        isAnyBillardUnlocked = false;
        isAnyHockeyUnlocked = false;
    }

    void Update()
    {
        if (isAnyGameUnlocked)
        {
            customerFreeToMove = true;
        }

        if (isAnyBillardUnlocked)
        {
            BcustomerFreeToMove = true;
        }

        if (isAnyHockeyUnlocked)
        {
            HcustomerFreeToMove = true;
        }
    }

    void SaveDatas()
    {
        PlayerPrefs.SetInt("PlayerMoney", p_coll.money);
        PlayerPrefs.SetInt("PlayerCoin", p_coll.coin);
        PlayerPrefs.SetFloat("StackPointY", col_handler.adder);

        for (int i = 0; i < arcadeNAMES.Count; i++)// Arcade Saving
        {
            PlayerPrefs.SetString(arcadeNAMES[i], arcadeNAMES[i]);

        }

        for (int i = 0; i < BillardNAMES.Count; i++)// Billard Saving
        {
            PlayerPrefs.SetString(BillardNAMES[i], BillardNAMES[i]);
        }

        for (int i = 0; i < HockeyNAMES.Count; i++)// Hockey Saving
        {
            PlayerPrefs.SetString(HockeyNAMES[i], HockeyNAMES[i]);
        }
    }

    void LoadDatas()
    {
        arcadeSAAS = GameObject.FindGameObjectsWithTag("Arcade_Comp");
        billardSAAS = GameObject.FindGameObjectsWithTag("Billard_Comp");
        hockeySAAS = GameObject.FindGameObjectsWithTag("Hockey_Comp");

        if (PlayerPrefs.HasKey("PlayerMoney"))
        {
            p_coll.money = PlayerPrefs.GetInt("PlayerMoney");
        }
        else
        {
            p_coll.money = DesignManager.Instance.startMoney;
        }

        if (PlayerPrefs.HasKey("PlayerCoin"))
        {
            p_coll.coin = PlayerPrefs.GetInt("PlayerCoin");
            for (int i = 0; i < p_coll.coin; i++)
            {
                GameObject scoc = Instantiate(stackCoin, stackPOINT.position, stackPOINT.rotation);
                scoc.transform.SetParent(GameObject.Find("Player").transform);
                col_handler.Coins.Add(scoc);
                float xxx = col_handler.adder = stackPOINT.position.y + .1f;
                stackPOINT.position = new Vector3(stackPOINT.position.x, xxx, stackPOINT.position.z);
            }
            p_coll.coin = PlayerPrefs.GetInt("PlayerCoin");
        }

        for (int i = 0; i < arcadeSAAS.Length; i++)
        {
            if (PlayerPrefs.HasKey(arcadeSAAS[i].name))// Önceki sessionda unlocked arcade
            {
                customerFreeToMove = true;
                GameObject thatARCADE = GameObject.Find(arcadeSAAS[i].name);
                arcadeBS = thatARCADE.transform.GetChild(0).GetComponent<ArcadeBlockScript>();
                RandVAL = Random.Range(0, arcadeBS.arcades.Count);
                thatARCADE.transform.GetChild(1).gameObject.SetActive(false);
                thatARCADE.transform.GetChild(6).gameObject.SetActive(false);
                thatARCADE.transform.GetChild(4).gameObject.SetActive(true);
                arcadeBS.arcades[RandVAL].SetActive(true);// Random arcade active et
                availableGameBlocks.Add(thatARCADE);// Available Listesine ekle
                arcadeNAMES.Add(arcadeSAAS[i].name);
                arcadeBS.unlocked = true;
            }
        }

        for (int i = 0; i < hockeySAAS.Length; i++)
        {
            if (PlayerPrefs.HasKey(hockeySAAS[i].name))// Önceki sessionda unlocked hockey
            {
                HcustomerFreeToMove = true;
                GameObject thatHockey = GameObject.Find(hockeySAAS[i].name);
                hockeyBS = thatHockey.transform.GetChild(0).GetComponent<IceHockeyBlockScript>();
                RandVAL = Random.Range(0, hockeyBS.hockeys.Count);
                thatHockey.transform.GetChild(1).gameObject.SetActive(false);
                thatHockey.transform.GetChild(2).gameObject.SetActive(false);
                hockeyBS.hockeys[RandVAL].SetActive(true);
                availableHockeyPos.Add(thatHockey.transform.GetChild(3).gameObject);
                availableHockeyPos.Add(thatHockey.transform.GetChild(4).gameObject);
                HockeyNAMES.Add(hockeySAAS[i].name);
            }
        }

        for (int i = 0; i < billardSAAS.Length; i++)
        {
            if (PlayerPrefs.HasKey(billardSAAS[i].name))// Önceki sessionda unlocked billard
            {
                BcustomerFreeToMove = true;
                GameObject thatBillard = GameObject.Find(billardSAAS[i].name);
                thatBillard.transform.GetChild(1).gameObject.SetActive(false);
                thatBillard.transform.GetChild(4).gameObject.SetActive(false);
                thatBillard.transform.GetChild(2).gameObject.SetActive(true);

                availableBillardPos.Add(thatBillard.transform.GetChild(2).GetChild(0).GetChild(0).gameObject);
                availableBillardPos.Add(thatBillard.transform.GetChild(2).GetChild(1).GetChild(0).gameObject);
                BillardNAMES.Add(billardSAAS[i].name);
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus) => SaveDatas();
}
