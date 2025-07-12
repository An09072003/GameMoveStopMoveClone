using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    protected bool isMoving = false;

    [Header("Animation")]
    protected Animator animator;


    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Tick được lớp con gọi thủ công trong Update
    public abstract void Tick();

    protected void Move(Vector3 direction)
    {
        if (!isMoving)
            isMoving = true;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    protected void Stop()
    {
        if (!isMoving) return;

        isMoving = false;
    }

    protected virtual void Attack()
    {
    }


}
