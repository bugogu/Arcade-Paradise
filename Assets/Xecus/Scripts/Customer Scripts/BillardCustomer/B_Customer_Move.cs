using UnityEngine;
using UnityEngine.AI;

public class B_Customer_Move : MonoBehaviour
{
    public Animator bcust_anim;
    BCustomerSpawner_Script Bcust_spawner;
    Manager manager;
    NavMeshAgent NM_agent;

    public float timer, pTimer, xTimer, moveSpeed, turnSpeed;
    public bool isEnteredStore, path_bool, targetChoosen, playing_over;
    public int playing_time, visitAmount;

    public bool OnBillard;

    public GameObject targetNOW, exitTarget, tutucu;
    void Start()
    {   // Ignore Collisions
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitWall").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("Player").GetComponent<CapsuleCollider>(), true);

        bcust_anim = GetComponent<Animator>();
        NM_agent = GetComponent<NavMeshAgent>();

        moveSpeed = 7f; turnSpeed = 5f; timer = 0f; pTimer = 0f; xTimer = 0f;
        isEnteredStore = false; path_bool = false;
        visitAmount = 1;
        manager = GameObject.Find("game_manager").GetComponent<Manager>();
        Bcust_spawner = GameObject.Find("BCustomer_Spawner").GetComponent<BCustomerSpawner_Script>();
    }

    void Update()
    {
        if (isEnteredStore) // Dükkana giriþ yapýldýysa
        {
            if (manager.BcustomerFreeToMove && !targetChoosen)
            {
                FindTarget();
            }

            else if (manager.BcustomerFreeToMove && targetChoosen)
            {
                if (!OnBillard)
                {
                    bcust_anim.SetBool("B_FreeToGo", true);
                }
            }

            if (OnBillard)
            {
                pTimer += Time.deltaTime;
                if (pTimer > playing_time)
                {
                    manager.availableBillardPos.Add(tutucu.gameObject);
                    bcust_anim.SetBool("B_FreeToGo", false);
                    playing_over = true;
                    targetChoosen = false;
                    pTimer = 0f;
                    OnBillard = false;
                }
            }

            if (playing_over == true)
            {
                NM_agent.destination = GameObject.Find("ExitPoint").transform.position;
                xTimer += Time.deltaTime;
                if (xTimer > 1f)
                {
                    visitAmount = 0;
                    playing_over = false;
                    xTimer = 0f;
                }
            }

            if (visitAmount == 0)
            {
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), false);
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), false);

                bcust_anim.SetBool("B_FreeToGo", true);
            }
        }

        else // Dükkana giriþ yapmadýysa
        {
            bcust_anim.SetBool("B_FreeToGo", true);
            if (!path_bool)
            {
                MoveBCustomer(GameObject.Find("path1"));
            }
            else if (path_bool)
            {
                MoveBCustomer(GameObject.Find("enterance_path"));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Customer")
        {
            Debug.Log("Bcust Collided");
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), collision.gameObject.GetComponent<CapsuleCollider>());
        }
    }

    private void OnTriggerEnter(Collider other)// Bilardo noktasýna giriþ yaptý
    {
        if (other.gameObject.tag == "billard_point_trigger")
        {
            if ((int)other.transform.position.magnitude == (int)(NM_agent.destination.magnitude))
            {
                tutucu = other.transform.gameObject;
                manager.availableBillardPos.Remove(tutucu);
                playing_over = false;
                OnBillard = true;

                playing_time = Random.Range(12, 18);
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;
                bcust_anim.SetBool("B_FreeToGo", false);
            }
        }

        // EXIT PATHS
        else if (other.gameObject.name == "ExitPoint")
        {
            manager.BCustomerInside--;
            NM_agent.destination = GameObject.Find("ExitCont").transform.position;
        }

        else if (other.gameObject.name == "ExitCont")
        {
            int randExitVal = Random.Range(1, 2);
            moveSpeed = 7f;
            if (randExitVal == 1)
            {
                NM_agent.destination = GameObject.Find("Exit1").transform.position;
            }
            else
            {
                NM_agent.destination = GameObject.Find("Exit2").transform.position;
            }
        }

        else if (other.gameObject.name == "Exit1" || other.gameObject.name == "Exit2")
        {
            Bcust_spawner.BcustomerVAL--;
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }


        // ENTERANCE PATHS
        else if (other.gameObject.name == "path1")
        {
            path_bool = true;
        }

        else if (other.gameObject.name == "enterance_path")// customer dükkana girdi
        {
            manager.BCustomerInside++;
            isEnteredStore = true;
            moveSpeed = 4f;
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("enterance_path").GetComponent<BoxCollider>(), true);
        }
    }

    void MoveBCustomer(GameObject TARGET)
    {
        if (transform.position != TARGET.transform.position)
        {
            Quaternion xxx = Quaternion.LookRotation(TARGET.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, xxx, turnSpeed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, TARGET.transform.position, moveSpeed * Time.deltaTime);
    }

    public void FindTarget()
    {
        if (!targetChoosen)
        {
            if (visitAmount == 0)// Customerýn gitmek istediði billardo kalmadýysa
            {
                NM_agent.destination = GameObject.Find("ExitPoint").transform.position;
                targetChoosen = true;
            }

            else
            {
                if (manager.availableBillardPos.Count != 0)// Uygun billardo varsa
                {
                    int randVal = Random.Range(0, manager.availableBillardPos.Count);
                    NM_agent.destination = manager.availableBillardPos[randVal].transform.position;

                    targetNOW = manager.availableBillardPos[randVal].transform.gameObject;

                    targetChoosen = true;
                }

                else
                {
                    visitAmount = 0;
                }
            }
        }
    }
}
