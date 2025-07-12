using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;
    private Vector3 startPoint;

    void Start()
    {
        startPoint = transform.position;

        // Nếu là đạn thật (có owner), thì tự hủy sau 3s
        if (owner != null)
        {
            Destroy(gameObject, 2f);
        }

        // Bỏ qua va chạm với các đạn khác
        Collider myCollider = GetComponent<Collider>();
        Bullet[] allBullets = FindObjectsOfType<Bullet>();

        foreach (Bullet otherBullet in allBullets)
        {
            if (otherBullet == this) continue;

            Collider otherCollider = otherBullet.GetComponent<Collider>();
            if (otherCollider != null)
            {
                Physics.IgnoreCollision(myCollider, otherCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner) return;

        // Nếu đụng phải Bot, Player, hoặc đạn khác → huỷ nếu là đạn thật (có owner)
        if (owner != null &&
            (collision.gameObject.CompareTag("Player") ||
             collision.gameObject.CompareTag("Bot") ||
             collision.gameObject.GetComponent<Bullet>() != null))
        {
            Destroy(gameObject);
        }
    }
}
