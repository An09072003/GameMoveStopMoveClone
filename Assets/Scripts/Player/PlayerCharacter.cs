using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    [Header("Joystick")]
    [SerializeField] private bl_Joystick joystick;
    [SerializeField] private JoystickInputHandler joystickInputHandler;

    [Header("Fire Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float delayBeforeShoot = 0.3f;

    [Header("Auto Aim Settings")]
    [SerializeField] private float aimRange = 5f;

    private bool canAttack = true;
    private bool isDead = false;
    private string currentAnimState = "";

    private void Update()
    {
        Tick();
    }

    public override void Tick()
    {
        if (isDead) return;

        Vector3 input = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        bool isMoving = joystickInputHandler != null && joystickInputHandler.isMoving;

        if (isMoving)
        {
            Move(input.normalized);
            transform.forward = input.normalized;
            ChangeAnimState("IsRun");
        }
        else
        {
            Stop();
            ChangeAnimState("IsIdle");

            GameObject nearestBot = FindNearestBotInRange();
            if (nearestBot != null)
            {
                Vector3 dir = (nearestBot.transform.position - transform.position).normalized;
                transform.forward = new Vector3(dir.x, 0f, dir.z);

                if (canAttack)
                {
                    TriggerAttack();
                }
            }
        }
    }

    private GameObject FindNearestBotInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aimRange);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Bot")) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.gameObject;
            }
        }

        return nearest;
    }

    private void TriggerAttack()
    {
        if (!canAttack) return;

        canAttack = false;
        animator.SetTrigger("IsAttack");
        StartCoroutine(DelayedAttack());
    }

    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(delayBeforeShoot);
        SpawnBullet();

        yield return new WaitForSeconds(attackCooldown - delayBeforeShoot);
        canAttack = true;
    }

    private void SpawnBullet()
    {
        if (weaponSlot == null || firePoint == null) return;

        Weapon weapon = weaponSlot.GetComponentInChildren<Weapon>();
        if (weapon == null || weapon.BulletPrefab == null) return;

        GameObject bullet = Instantiate(weapon.BulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(-90f, 0f, 0f); // Tuỳ chỉnh hướng nếu đạn bị lệch
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = firePoint.forward * bulletSpeed;
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

        if (collision.collider.CompareTag("Bullet"))
        {
            Die();
            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("IsDead");

        Stop(); // dừng di chuyển
        this.enabled = false;

        Destroy(gameObject, 1f);
        gameObject.tag = "Untagged";
    }
}
