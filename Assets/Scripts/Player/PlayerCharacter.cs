using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    [Header("Joystick")]
    public bl_Joystick Joystick;
    public JoystickInputHandler joystickInputHandler;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float attackCooldown = 0.5f;
    public float delayBeforeShoot = 0.3f;

    [Header("Auto Aim Settings")]
    public float aimRange = 5f;

    private bool canAttack = true;
    private string currentAnimState = "";

    private void Update()
    {
        Tick();
    }

    public override void Tick()
    {
        Vector3 input = new Vector3(Joystick.Horizontal, 0f, Joystick.Vertical);
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
            else
            {
                Debug.Log("No bot in range.");
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
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(-90f, 0f, 0f); // Điều chỉnh nếu model bị lệch
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
}
