using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    [SerializeField] private List<Transform> walkPoints = new List<Transform>();
    private int pointsIndex;

    private enum MonsterStates { Walking, Chasing };
    private MonsterStates state = MonsterStates.Walking;

    private Transform player;
    private NavMeshAgent agent;
    private Animator anim;

    private float timeSinceSeenPlayer;
    [SerializeField] private float maxBlindChaseTime;
    private float seesPlayerWalkingTime = 0;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float fieldOfView;
    private float realFieldOfView;
    [SerializeField] private Transform enemyEyes;

    [SerializeField] private float roarWaitTime;
    private float timeSinceRoar = 100;
    [SerializeField] private float lowTensionTime;
    [SerializeField] private float highTensionTime = 45;
    private float chaseTime;

    [SerializeField] private AudioSource roar;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource suspenseMusic;
    [SerializeField] private AudioSource chaseMusic;

    //[SerializeField] private Collider killPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        realFieldOfView = fieldOfView;

        /*GameObject[] points = GameObject.FindGameObjectsWithTag("walkPoint");
        foreach (GameObject g in points)
        {
            walkPoints.Add(g.transform);
        }
        pointsIndex = Random.Range(0, walkPoints.Count);*/
        pointsIndex = 0;
    }

    void Update()
    {
        timeSinceSeenPlayer += Time.deltaTime;
        timeSinceRoar += Time.deltaTime;
        switch (state)
        {
            case MonsterStates.Walking:
                WalkingUpdate();
                break;
            case MonsterStates.Chasing:
                ChasingUpdate();
                break;
        }
    }

    private void WalkingUpdate()
    {
        ChaseMusic(false);
        chaseTime = 0;
        realFieldOfView = fieldOfView;
        agent.speed = walkSpeed;
        if (Vector3.Distance(transform.position, agent.destination) <= 0.5f)
        {
            if (timeSinceSeenPlayer < lowTensionTime)
            {
                NextPointRandom();
            }
            else
            {
                NextPointNearPlayer();
            }
        }
        if (SeesPlayer())
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
        }
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
        ChaseMusic(true);
        realFieldOfView = 120f;
        if (SeesPlayer())
        {
            timeSinceSeenPlayer = 0;
            agent.SetDestination(player.position);
            //Debug.Log("Chasing player!");
        }
        else if (timeSinceSeenPlayer > maxBlindChaseTime)
        {
            state = MonsterStates.Walking;
            anim.SetInteger("State", 0);
            if (chaseTime > highTensionTime)
            {
                NextPointRandom();
            }
            else
            {
                NextPointNearPlayer();
            }
        }
        else if (Vector3.Distance(transform.position, agent.destination) < 0.5f)
        {
            NextPointNearPlayer();
        }
    }

    private void NextPoint()
    {
        pointsIndex++;
        if (pointsIndex >= walkPoints.Count)
        {
            pointsIndex = 0;
        }
        agent.SetDestination(walkPoints[pointsIndex].position);
        //Debug.Log("NextPoint() : " + walkPoints[pointsIndex].gameObject.name);
    }

    private void NextPointRandom()
    {
        pointsIndex = Random.Range(0, walkPoints.Count);
        agent.SetDestination(walkPoints[pointsIndex].position);
        //Debug.Log("NextPointRandom() : " + walkPoints[pointsIndex].gameObject.name);
    }

    private void NextPointNearPlayer()
    {
        Transform newPoint = walkPoints[0];
        foreach (Transform t in walkPoints)
        {
            if (Vector3.Distance(player.position, t.position) < Vector3.Distance(player.position, newPoint.position))
            {
                newPoint = t;
            }
        }
        pointsIndex = walkPoints.IndexOf(newPoint);
        agent.SetDestination(walkPoints[pointsIndex].position);
        //Debug.Log("NextPointNearPlayer() : " + walkPoints[pointsIndex].gameObject.name);
    }

    private bool SeesPlayer()
    {
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
    }

    private void ChaseMusic(bool given)
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
    }

    public void KilledPlayer()
    {
        anim.SetInteger("State", 3);
        agent.speed = 0;
        agent.acceleration = 0;
        agent.SetDestination(transform.position);
        agent.enabled = false;
        roar.volume = 0;
        Invoke("Dead1", 0.01f);
        Invoke("Dead2", 0.05f);
    }

    private void Dead1()
    {
        anim.SetInteger("State", 3);
    }

    private void Dead2()
    {
        anim.SetInteger("State", 3);
        this.enabled = false;
    }
}
