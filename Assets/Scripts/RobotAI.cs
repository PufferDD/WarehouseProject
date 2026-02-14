using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;

    public float detectionRange = 10f;
    public float maxChaseTime = 15f;
    public float searchDuration = 6f;

    NavMeshAgent agent;

    enum State { Patrol, Chase, Search, Return }
    State currentState;

    int patrolIndex = 0;
    float chaseTimer = 0f;
    float searchTimer = 0f;

    Vector3 originalPosition;
    Vector3 lastKnownPlayerPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        currentState = State.Patrol;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                }

                if (distance < detectionRange)
                {
                    currentState = State.Chase;
                }

                break;

            case State.Chase:

                chaseTimer += Time.deltaTime;
                lastKnownPlayerPos = player.position;
                agent.SetDestination(player.position);

                if (distance > detectionRange)
                {
                    currentState = State.Search;
                    searchTimer = 0f;
                    agent.SetDestination(lastKnownPlayerPos);
                }

                if (chaseTimer >= maxChaseTime)
                {
                    chaseTimer = 0f;
                    currentState = State.Search;
                    searchTimer = 0f;
                    agent.SetDestination(lastKnownPlayerPos);
                }

                break;

            case State.Search:

                searchTimer += Time.deltaTime;

                if (searchTimer >= searchDuration)
                {
                    currentState = State.Return;
                    agent.SetDestination(originalPosition);
                }

                break;

            case State.Return:

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.Patrol;
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                }

                break;
        }
    }

    public void HearNoise(Vector3 noisePos)
    {
        if (currentState != State.Chase)
        {
            currentState = State.Search;
            searchTimer = 0f;
            agent.SetDestination(noisePos);
        }
    }
}

