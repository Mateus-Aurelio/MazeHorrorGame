using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    private List<Transform> walkPoints = new List<Transform>();
    [SerializeField] private List<Transform> walkPointsTop = new List<Transform>();
    [SerializeField] private List<Transform> walkPointsBot = new List<Transform>();
    [SerializeField] private List<Transform> walkPointsSide = new List<Transform>();
    private int pointsIndex;

    private enum MonsterStates { Walking, Chasing };
    private MonsterStates state = MonsterStates.Walking;

    private Transform player;
    private NavMeshAgent agent;
    private Animator anim;

    private float timeSinceSeenPlayer;
    [SerializeField] private float maxBlindChaseTime;
    //private float seesPlayerWalkingTime = 0;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;

    //[SerializeField] private float fieldOfView;
    //private float realFieldOfView;
    //[SerializeField] private Transform enemyEyes;

    //[SerializeField] private float roarWaitTime;
    //private float timeSinceRoar = 100;
    private float timeSinceGrowl;
    [SerializeField] private float lowTensionTime;
    [SerializeField] private float highTensionTime;
    private float stuckTimer;
    private float chaseTime;

    [SerializeField] private AudioSource roar;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource growl;
    //[SerializeField] private AudioSource suspenseMusic;
    //[SerializeField] private AudioSource chaseMusic;

    //[SerializeField] private Collider killPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        //realFieldOfView = fieldOfView;
        InitWalkPoints();
        pointsIndex = 0;
    }

    private void InitWalkPoints()
    {
        walkPoints = new List<Transform>(walkPointsTop.Count + walkPointsBot.Count + walkPointsSide.Count);
        walkPointsBot.ForEach(point => walkPoints.Add(point));
        walkPointsTop.ForEach(point => walkPoints.Add(point));
        walkPointsSide.ForEach(point => walkPoints.Add(point));
    }

    void Update()
    {
        timeSinceSeenPlayer += Time.deltaTime;
        //timeSinceRoar += Time.deltaTime;
        timeSinceGrowl += Time.deltaTime;
        switch (state)
        {
            case MonsterStates.Walking:
                WalkingUpdate();
                break;
            case MonsterStates.Chasing:
                ChasingUpdate();
                break;
        }
        /*if (agent.)
        {
            stuckTimer += Time.deltaTime;
        }*/
    }

    private void WalkingUpdate()
    {
        //ChaseMusic(false);
        chaseTime = 0;
        //realFieldOfView = fieldOfView;
        agent.speed = walkSpeed;
        if (Vector3.Distance(transform.position, agent.destination) <= 0.5f)
        {
            if (timeSinceSeenPlayer < lowTensionTime)
            {
                NextPointRandom();
            }
            else
            {
                //NextPointNearPlayer();
                NextPointSameSectionPlayer();
            }
        }
        /*if (SeesPlayer())
        {
            seesPlayerWalkingTime += Time.deltaTime;
            if (seesPlayerWalkingTime > 0.15f)
            {
                state = MonsterStates.Chasing;
                if (timeSinceRoar > roarWaitTime)
                {
                    timeSinceRoar = 0;
                    anim.SetInteger("State", 2);
                    Invoke("ChaseSpeed", 4.33f);
                    agent.speed = 0;
                }
                else
                {
                    anim.SetInteger("State", 1);
                    ChaseSpeed();
                }
            }
        }
        else
        {
            seesPlayerWalkingTime = 0;
        }*/
    }

    private void ChaseSpeed()
    {
        agent.speed = chaseSpeed;
        timeSinceSeenPlayer = 0;
    }

    public void Roar()
    {
        roar.Play();
    }

    public void Step()
    {
        footstep.Play();
    }

    private void ChasingUpdate()
    {
        chaseTime += Time.deltaTime;
        //ChaseMusic(true);
        //realFieldOfView = 120f;
        /*if (SeesPlayer())
        {
            timeSinceSeenPlayer = 0;
            agent.SetDestination(player.position);
            Debug.Log("Chasing player!");
        }
        else */
        //if (timeSinceSeenPlayer > maxBlindChaseTime)
        if (chaseTime > maxBlindChaseTime)
        {
            state = MonsterStates.Walking;
            anim.SetInteger("State", 0);
            if (timeSinceSeenPlayer > highTensionTime)
            {
                //NextPointRandom();
                NextPointDifferentSectionPlayer();
            }
            else
            {
                //NextPointNearPlayer();
                NextPointSameSectionPlayer();
            }
        }
        else if (Vector3.Distance(transform.position, agent.destination) < 0.5f)
        {
            //NextPointNearPlayer();
            NextPointSameSectionPlayer();
            maxBlindChaseTime += 5;
        }
    }

    private void NextPointRandom()
    {
        pointsIndex = Random.Range(0, walkPoints.Count);
        agent.SetDestination(walkPoints[pointsIndex].position);
        //Debug.Log("NextPointRandom() : " + walkPoints[pointsIndex].gameObject.name);
    }

    private void NextPointSameSectionPlayer()
    {
        switch (PlayerMove.section)
        {
            case "Top":
                pointsIndex = Random.Range(0, walkPointsTop.Count);
                agent.SetDestination(walkPointsTop[pointsIndex].position);
                break;
            case "Side":
                pointsIndex = Random.Range(0, walkPointsSide.Count);
                agent.SetDestination(walkPointsSide[pointsIndex].position);
                break;
            default:
                pointsIndex = Random.Range(0, walkPointsBot.Count);
                agent.SetDestination(walkPointsBot[pointsIndex].position);
                break;
        }
    }

    private void NextPointDifferentSectionPlayer()
    {
        switch (PlayerMove.section)
        {
            case "Top":
                pointsIndex = Random.Range(0, walkPointsBot.Count);
                agent.SetDestination(walkPointsBot[pointsIndex].position);
                break;
            case "Side":
                pointsIndex = Random.Range(0, walkPointsBot.Count);
                agent.SetDestination(walkPointsBot[pointsIndex].position);
                break;
            default:
                if (Random.Range(0, 4) == 0)
                {
                    pointsIndex = Random.Range(0, walkPointsSide.Count);
                    agent.SetDestination(walkPointsSide[pointsIndex].position);
                }
                else
                {
                    pointsIndex = Random.Range(0, walkPointsTop.Count);
                    agent.SetDestination(walkPointsTop[pointsIndex].position);
                }
                break;
        }
    }

    private void NextPointNearPlayer()
    {
        Transform newPoint = walkPoints[0];
        foreach (Transform t in walkPoints)
        {
            if (Vector3.Distance(player.position, t.position) < Vector3.Distance(player.position, newPoint.position)
                && Vector3.Distance(t.position, walkPoints[pointsIndex].position) > 0.1f)
            {
                newPoint = t;
            }
        }
        pointsIndex = walkPoints.IndexOf(newPoint);
        agent.SetDestination(walkPoints[pointsIndex].position);
        //Debug.Log("NextPointNearPlayer() : " + walkPoints[pointsIndex].gameObject.name);
    }

    /*private bool SeesPlayer()
    {
        return false;
        return PlayerWithinAngle() && PlayerUnobstructedInVision();
    }

    private bool PlayerWithinAngle()
    {
        float angleObjects = Vector3.Angle(player.transform.position - enemyEyes.position, enemyEyes.forward);
        return (angleObjects > -1 * fieldOfView && angleObjects <= fieldOfView);
    }

    private bool PlayerUnobstructedInVision()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemyEyes.position, (player.transform.position - enemyEyes.position), out hit, 75))
        {
            if (hit.transform == player.transform)
            {
                return true;
            }
        }
        return false;
    }*/

    public void HeardNoise(Vector3 location)
    {
        timeSinceSeenPlayer = 0;
        state = MonsterStates.Chasing;
        agent.SetDestination(location); 
        anim.SetInteger("State", 1);
        ChaseSpeed();
        //Debug.Log("Heard player!");
        if (timeSinceGrowl > 2.5f)
        {
            timeSinceGrowl = 0;
            growl.Play();
        }
    }

    /*private void ChaseMusic(bool given)
    {
        if (given)
        {
            suspenseMusic.volume = Mathf.Lerp(suspenseMusic.volume, 0.0f, 0.001f);
            chaseMusic.volume = Mathf.Lerp(chaseMusic.volume, 0.3f, 0.001f);
        }
        else
        {
            suspenseMusic.volume = Mathf.Lerp(suspenseMusic.volume, 0.1f, 0.001f);
            chaseMusic.volume = Mathf.Lerp(chaseMusic.volume, 0.0f, 0.001f);
        }
    }*/

    public void KilledPlayer()
    {
        anim.SetInteger("State", 3);
        agent.speed = 0;
        agent.acceleration = 0;
        agent.SetDestination(transform.position);
        agent.enabled = false;
        roar.volume = 0;

        anim.SetInteger("State", 3);
        this.enabled = false;
    }
}
