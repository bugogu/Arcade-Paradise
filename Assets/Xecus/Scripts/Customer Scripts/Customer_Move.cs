using UnityEngine;
using UnityEngine.AI;

public class Customer_Move : MonoBehaviour
{   // Public  Variables
    public Animator cust_anim;
    public float moveSpeed, turnSpeed;
    public bool targetChoosen = false, playingOver = false, enteredTheStore = false, OnArcade = false;


    // Private Variables
    [SerializeField] float timer = 0f, pTimer = 0f;
    [SerializeField] int playing_time, goingToVisitAmount;
    bool path1_bool;
    GameObject[] ignoreArcades, ignoreCustomers;

    Manager game_manager;
    CheckValidTarget checkValidTarget;
    CustomerSpawner cust_spawner;

    NavMeshAgent navMeshAgent;

    public GameObject currentTarget, exitTarget, arcadeQuitPos, tutucu;

    private void Awake()
    {
        ignoreCustomers = GameObject.FindGameObjectsWithTag("Customer");
        for (int i = 0; i < ignoreCustomers.Length; i++)
        {
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), ignoreCustomers[i].GetComponent<CapsuleCollider>(), true);
        }
    }
    void Start()
    {
        // Ignore Collisions 
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitWall").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("Player").GetComponent<CapsuleCollider>(), true);


        // Variable Initialize
        moveSpeed = 7f; turnSpeed = 5f;
        enteredTheStore = false;

        // Object and Script Initialization
        cust_spawner = GameObject.Find("CustomerSpawner").GetComponent<CustomerSpawner>();
        game_manager = GameObject.Find("game_manager").GetComponent<Manager>();
        exitTarget = GameObject.Find("ExitPoint");

        navMeshAgent = GetComponent<NavMeshAgent>();

        checkValidTarget = GetComponent<CheckValidTarget>();
        cust_anim = GetComponent<Animator>();


        if (game_manager.availableGameBlocks.Count < 3)
        {
            goingToVisitAmount = 1;
        }
        else
        {
            goingToVisitAmount = Random.Range(1, 3);
        }
    }

    void Update()
    {
        if (enteredTheStore) // Dükkana Giriþ Yapýldýysa
        {
            if (game_manager.customerFreeToMove && targetChoosen == false)// En az bir Arcade açýldýysa VE Customer hedef seçmediyse
            {

                if (goingToVisitAmount != 0)
                {
                    FindTarget();
                }
            }

            else if (game_manager.customerFreeToMove && targetChoosen == true && !OnArcade)// En az bir arcade açýldýysa ve Customer hedef seçtiyse
            {
                if (!playingOver && goingToVisitAmount != 0)
                {
                    cust_anim.SetBool("isFreeToGo", true);
                    FindTarget();
                }
            }

            if (OnArcade)// Arcade ile oynuyorsa
            {
                transform.rotation = tutucu.transform.GetChild(2).rotation;
                pTimer += Time.deltaTime;
                if (pTimer > playing_time)
                {
                    game_manager.availableGameBlocks.Add(tutucu.gameObject);
                    cust_anim.SetBool("isPlayingArcade", false);
                    playingOver = true;
                    targetChoosen = false;
                    pTimer = 0f;
                    OnArcade = false;

                }
            }

            if (playingOver == true)// O anki arcade ile oynama bittiyse
            {
                navMeshAgent.destination = arcadeQuitPos.transform.position;

                if ((int)(transform.position.magnitude) == (int)((arcadeQuitPos.transform.position).magnitude))
                {
                    goingToVisitAmount = 0;

                    navMeshAgent.destination = GameObject.Find("ExitPoint").transform.position;
                    playingOver = false;
                }

            }

            if (goingToVisitAmount == 0) // Time To Leave
            {
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), false);
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), false);

                ignoreArcades = GameObject.FindGameObjectsWithTag("game_point_trigger");
                for (int i = 0; i < ignoreArcades.Length; i++)
                {
                    Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), ignoreArcades[i].GetComponent<BoxCollider>(), true);
                }

                cust_anim.SetBool("isFreeToGo", true);
            }
        }

        else // Dükkana Giriþ Yapýlmadýysa
        {
            cust_anim.SetBool("isFreeToGo", true);
            if (!path1_bool)
            {
                MoveCustomer(GameObject.Find("path1"));
            }
            else if (path1_bool)
            {
                MoveCustomer(GameObject.Find("enterance_path"));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "game_point_trigger")// Customer oyun noktasýna giriþ yaptý
        {
            int arcadePOS = (int)other.transform.parent.GetChild(2).transform.position.magnitude;
            int trgt = (int)(navMeshAgent.destination.magnitude);

            if (arcadePOS == trgt)
            {
                tutucu = other.transform.parent.gameObject;
                game_manager.availableGameBlocks.Remove(tutucu);
                playingOver = false;
                OnArcade = true;
                playing_time = Random.Range(10, 15);
                transform.position = other.transform.parent.GetChild(2).transform.position; /*currentTarget.transform.position;*/
                transform.rotation = other.transform.parent.GetChild(2).transform.rotation; /*currentTarget.transform.rotation;*/
                arcadeQuitPos = other.transform.parent.GetChild(3).gameObject;
                cust_anim.SetBool("isPlayingArcade", true);
            }

        }

        // EXIT PATHS
        else if (other.gameObject.name == "ExitPoint")
        {

            game_manager.CustomerInside--;
            navMeshAgent.destination = GameObject.Find("ExitCont").transform.position;
        }

        else if (other.gameObject.name == "ExitCont")
        {
            int randExitVal = Random.Range(1, 2);
            moveSpeed = 7f;
            if (randExitVal == 1)
            {
                navMeshAgent.destination = GameObject.Find("Exit1").transform.position;
            }
            else
            {
                navMeshAgent.destination = GameObject.Find("Exit2").transform.position;
            }
        }

        else if (other.gameObject.name == "Exit1" || other.gameObject.name == "Exit2")
        {
            cust_spawner.customerVAL--;
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }

        // ENTRY PATHS
        else if (other.gameObject.name == "path1")
        {
            path1_bool = true;
        }

        else if (other.gameObject.name == "enterance_path")// customer dükkana girdi
        {

            game_manager.CustomerInside++;
            enteredTheStore = true;
            moveSpeed = 6f;
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("enterance_path").GetComponent<BoxCollider>(), true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "game_point_trigger")
        {
            int arcadePOS = (int)other.transform.parent.GetChild(2).transform.position.magnitude;
            int trgt = (int)(navMeshAgent.destination.magnitude + 4);

            Debug.Log(other.transform.parent.GetChild(2).transform.position);
            Debug.Log(navMeshAgent.destination);

            if (arcadePOS == trgt)
            {
                other.gameObject.GetComponent<GamePointScript>().occupied = false;
            }
        }
    }

    void FindTarget()
    {
        if (!targetChoosen)
        {
            if (goingToVisitAmount == 0)// Customerýn gitmek istediði arcade kalmadýysa
            {
                navMeshAgent.destination = GameObject.Find("ExitPoint").transform.position;
                targetChoosen = true;
            }

            else
            {
                if (game_manager.availableGameBlocks.Count != 0)// Uygun arcade varsa
                {
                    int randVal = Random.Range(0, game_manager.availableGameBlocks.Count);
                    navMeshAgent.destination = game_manager.availableGameBlocks[randVal].transform.GetChild(2).position;

                    currentTarget = game_manager.availableGameBlocks[randVal].transform.GetChild(2).gameObject;

                    targetChoosen = true;
                }

                else
                {
                    if (goingToVisitAmount > game_manager.availableGameBlocks.Count)
                    {
                        goingToVisitAmount--;
                    }

                    else if (game_manager.availableGameBlocks.Count == 0)
                    {
                        goingToVisitAmount = 0;
                    }
                }
            }
        }
    }

    void MoveCustomer(GameObject TARGET)
    {
        if (transform.position != TARGET.transform.position)
        {
            Quaternion xxx = Quaternion.LookRotation(TARGET.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, xxx, turnSpeed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, TARGET.transform.position, moveSpeed * Time.deltaTime);
    }
}
