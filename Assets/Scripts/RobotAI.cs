using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotAI : MonoBehaviour
{
    [Header("References")]
    public Transform[] patrolPoints;

    [Tooltip("Leave empty to auto-find by tag 'Player'.")]
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float eyeHeight = 1.6f;
    public LayerMask visionMask = ~0;

    [Header("Timers")]
    public float maxChaseTime = 15f;
    public float searchDuration = 6f;

    [Header("Movement Tuning")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 4.0f;

    [Header("Aggro")]
    [Tooltip("If true, robot can chase only when GameManager says aggressive (alert threshold).")]
    public bool lockChaseUntilAggro = true;

    private NavMeshAgent agent;

    private enum State { Patrol, Chase, Search, Return }
    private State currentState;

    private int patrolIndex = 0;
    private float chaseTimer = 0f;
    private float searchTimer = 0f;

    private Vector3 originalPosition;
    private Vector3 lastKnownTargetPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        originalPosition = transform.position;

        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }

        // vähentää tönimistä
        agent.stoppingDistance = 1.5f;
        agent.autoBraking = true;

        currentState = State.Patrol;
        agent.speed = patrolSpeed;

        if (patrolPoints != null && patrolPoints.Length > 0 && patrolPoints[0] != null)
            agent.SetDestination(patrolPoints[0].position);
    }

    private void Update()
    {
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
            else return;
        }

        bool canSeePlayer = CanSeePlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ✅ Aggro-lukko
        bool aggressive = true;
        if (lockChaseUntilAggro)
        {
            aggressive = (GameManager.I != null) && GameManager.I.IsAggressive;
        }

        switch (currentState)
        {
            case State.Patrol:
            {
                agent.speed = patrolSpeed;

                if (patrolPoints != null && patrolPoints.Length > 0)
                {
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                        var next = patrolPoints[patrolIndex];
                        if (next != null) agent.SetDestination(next.position);
                    }
                }

                // ✅ Chase vain jos aggressive
                if (aggressive && (canSeePlayer || distanceToPlayer < detectionRange * 0.75f))
                {
                    chaseTimer = 0f;
                    lastKnownTargetPos = player.position;
                    currentState = State.Chase;
                }

                break;
            }

            case State.Chase:
            {
                agent.speed = chaseSpeed;

                // ✅ jos aggressio poistuu (alert laski tms), lopeta jahtaaminen
                if (lockChaseUntilAggro && !aggressive)
                {
                    currentState = State.Return;
                    agent.speed = patrolSpeed;
                    agent.SetDestination(originalPosition);
                    break;
                }

                chaseTimer += Time.deltaTime;

                if (canSeePlayer)
                    lastKnownTargetPos = player.position;

                agent.SetDestination(lastKnownTargetPos);

                if (!canSeePlayer && distanceToPlayer > detectionRange)
                {
                    currentState = State.Search;
                    searchTimer = 0f;
                    agent.SetDestination(lastKnownTargetPos);
                    break;
                }

                if (chaseTimer >= maxChaseTime)
                {
                    chaseTimer = 0f;
                    currentState = State.Search;
                    searchTimer = 0f;
                    agent.SetDestination(lastKnownTargetPos);
                }

                break;
            }

            case State.Search:
            {
                agent.speed = patrolSpeed;

                // ✅ vain jos aggressive saa palata chaseen
                if (aggressive && canSeePlayer)
                {
                    chaseTimer = 0f;
                    lastKnownTargetPos = player.position;
                    currentState = State.Chase;
                    break;
                }

                searchTimer += Time.deltaTime;

                if (searchTimer >= searchDuration)
                {
                    currentState = State.Return;
                    agent.SetDestination(originalPosition);
                }

                break;
            }

            case State.Return:
            {
                agent.speed = patrolSpeed;

                if (aggressive && canSeePlayer)
                {
                    chaseTimer = 0f;
                    lastKnownTargetPos = player.position;
                    currentState = State.Chase;
                    break;
                }

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.Patrol;
                    if (patrolPoints != null && patrolPoints.Length > 0 && patrolPoints[patrolIndex] != null)
                        agent.SetDestination(patrolPoints[patrolIndex].position);
                }

                break;
            }
        }
    }

    public void HearNoise(Vector3 noisePos, float investigateTimeBonus = 0f)
    {
        // ✅ jos ei ole aggressive, saa silti tutkia ääniä (mutta ei "Chase")
        if (currentState == State.Chase) return;

        lastKnownTargetPos = noisePos;
        currentState = State.Search;
        searchTimer = 0f;

        if (investigateTimeBonus > 0f)
            searchTimer = Mathf.Max(0f, searchTimer - investigateTimeBonus);

        agent.SetDestination(noisePos);
    }

    public void ForceSearch()
    {
        currentState = State.Search;
        searchTimer = 0f;
    }

    public void ForceChase()
    {
        // ✅ älä pakota chasea jos ei aggressive
        if (lockChaseUntilAggro && (GameManager.I == null || !GameManager.I.IsAggressive))
            return;

        chaseTimer = 0f;
        currentState = State.Chase;
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 targetPos = player.position + Vector3.up * 1.0f;
        Vector3 dir = targetPos - eyePos;

        if (dir.magnitude > detectionRange) return false;

        if (Physics.Raycast(eyePos, dir.normalized, out RaycastHit hit, detectionRange, visionMask, QueryTriggerInteraction.Ignore))
            return hit.collider.CompareTag("Player");

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * eyeHeight, 0.1f);
    }
#endif
}



