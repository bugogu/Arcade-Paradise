using UnityEngine;
using UnityEngine.AI;

public class H_CustomerMove : MonoBehaviour
{
    public Animator hcust_anim;
    HCustomerSpawnerScript Hcust_spawner;
    Manager manager;
    NavMeshAgent NM_agent;
    GameObject tutucu;

    public float timer, pTimer, xTimer, moveSpeed, turnSpeed;
    public bool enteredStore, path_bool, targetChoosen, playing_over;
    public int playing_time, goingToVisit;

    public bool OnHockey;

    public GameObject TARGET, exitTarget;
    void Start()
    {   //Ignore Collisions
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitWall").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), true);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("Player").GetComponent<CapsuleCollider>(), true);

        //Variable Declaration
        hcust_anim = GetComponent<Animator>();
        moveSpeed = 7f; turnSpeed = 5f; timer = 0f; pTimer = 0f; xTimer = 0f;
        enteredStore = false; path_bool = false;
        goingToVisit = 1;

        // Script Attachments
        NM_agent = GetComponent<NavMeshAgent>();
        manager = GameObject.Find("game_manager").GetComponent<Manager>();
        Hcust_spawner = GameObject.Find("HCustomer_Spawner").GetComponent<HCustomerSpawnerScript>();
    }

    void Update()
    {
        if (enteredStore)
        {
            if (manager.HcustomerFreeToMove && !targetChoosen)
            {
                FindTarget();
            }

            else if (manager.HcustomerFreeToMove && targetChoosen)
            {
                if (!OnHockey)
                {
                    //MoveHCustomer(TARGET);
                }
            }

            if (OnHockey)
            {
                pTimer += Time.deltaTime;
                if (pTimer > playing_time)
                {
                    manager.availableHockeyPos.Add(tutucu.gameObject);
                    hcust_anim.SetBool("isPlayingHockey", false);
                    playing_over = true;
                    targetChoosen = false;
                    pTimer = 0f;
                    OnHockey = false;
                }
            }

            if (playing_over == true)
            {
                NM_agent.destination = GameObject.Find("ExitPoint").transform.position;
                xTimer += Time.deltaTime;
                if (xTimer > 1f)
                {
                    goingToVisit = 0;
                    playing_over = false;
                    xTimer = 0f;
                }
            }

            if (goingToVisit == 0)
            {
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitPoint").GetComponent<BoxCollider>(), false);
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("ExitCont").GetComponent<BoxCollider>(), false);

                hcust_anim.SetBool("isPlayingHockey", false);
            }
        }

        else
        {
            if (!path_bool)
            {
                MoveHCustomer(GameObject.Find("path1"));
            }
            else if (path_bool)
            {
                MoveHCustomer(GameObject.Find("enterance_path"));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Customer")
        {
            Debug.Log("Hcust Collided");
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), collision.gameObject.GetComponent<CapsuleCollider>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "hockey_point_trigger")
        {
            tutucu = other.transform.gameObject;
            manager.availableHockeyPos.Remove(tutucu);
            playing_over = false;
            OnHockey = true;

            playing_time = Random.Range(8, 12);
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
            hcust_anim.SetBool("isPlayingHockey", true);
        }

        //Exit Paths
        else if (other.gameObject.name == "ExitPoint")
        {
            manager.HCustomerInside--;
            NM_agent.destination = GameObject.Find("ExitCont").transform.position;
        }

        else if (other.gameObject.name == "ExitCont")
        {
            int randExit = Random.Range(1, 2);
            moveSpeed = 7f;

            if (randExit == 1)
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
            Hcust_spawner.HcustomerVAL--;
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }

        //Enterance Paths
        else if (other.gameObject.name == "path1")
        {
            path_bool = true;
        }
        else if (other.gameObject.name == "enterance_path")
        {
            manager.HCustomerInside++;
            enteredStore = true;
            moveSpeed = 5f;
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("enterance_path").GetComponent<BoxCollider>(), true);

        }
    }

    void MoveHCustomer(GameObject _target)
    {
        if (transform.position != _target.transform.position)
        {
            Quaternion xxx = Quaternion.LookRotation(_target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, xxx, turnSpeed * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, moveSpeed * Time.deltaTime);
    }

    void FindTarget()
    {
        if (!targetChoosen)
        {
            if (goingToVisit == 0)
            {
                NM_agent.destination = GameObject.Find("ExitPoint").transform.position;
                targetChoosen = true;
            }

            else
            {
                if (manager.availableHockeyPos.Count != 0)
                {
                    int randVAL = Random.Range(0, manager.availableHockeyPos.Count);
                    NM_agent.destination = manager.availableHockeyPos[randVAL].transform.position;

                    TARGET = manager.availableHockeyPos[randVAL].transform.gameObject;
                    targetChoosen = true;
                }

                else
                {
                    goingToVisit = 0;
                }
            }
        }
    }
}
