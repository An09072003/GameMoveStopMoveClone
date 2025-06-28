  using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BotCharacter : Character
{
    [Header("Bot Settings")]
    public string[] enemyTags = { "Player", "Bot" };
    public float detectRange = 5f;
    public float moveRadius = 10f;
    public float minStopTime = 1f;
    public float maxStopTime = 3f;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public float attackCooldown = 1.5f;
    public float delayBeforeShoot = 0.3f;

    private NavMeshAgent agent;
    private GameObject target;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isWaiting = false;
    private string currentAnimState = "";

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        if (isDead) return;

        Tick();

        if (agent.velocity.magnitude > 0.1f)
            ChangeAnimState("IsRun");
        else
            ChangeAnimState("IsIdle");
    }

    public override void Tick()
    {
        DetectTarget();

        if (target != null)
        {
            agent.ResetPath(); // đứng lại

            // Quay mặt về phía mục tiêu
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.forward = new Vector3(dir.x, 0, dir.z);

            if (!isAttacking)
                StartCoroutine(AttackRoutine());
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
                StartCoroutine(WaitAndMoveAgain());
        }
    }

    void DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange);
        target = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            foreach (var tag in enemyTags)
            {
                if (hit.CompareTag(tag))
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = hit.gameObject;
                    }
                }
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        Attack(); // animation
        yield return new WaitForSeconds(delayBeforeShoot);
        SpawnBullet();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    protected override void Attack()
    {
        base.Attack();
        if (animator != null)
            animator.SetTrigger("IsAttack");
    }

    void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(-90f, 0f, 0f); // giống player
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = firePoint.forward * bulletSpeed;

        // Gắn thông tin chủ nhân để tránh tự bắn chính mình
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.owner = gameObject;
        }
    }

    IEnumerator WaitAndMoveAgain()
    {
        isWaiting = true;
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        MoveToRandomPoint();
        isWaiting = false;
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void ChangeAnimState(string newState)
    {
        if (currentAnimState == newState) return;

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", false);
        animator.SetBool(newState, true);

        currentAnimState = newState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.collider.CompareTag("Weapon"))
        {
            Die();
            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} đã bị tiêu diệt!");

        if (animator != null)
            animator.SetTrigger("IsDead");

        Destroy(gameObject, 1f);
    }
}
