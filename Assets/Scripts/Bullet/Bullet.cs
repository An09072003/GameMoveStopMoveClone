using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;
    private Vector3 startPoint;

    void Start()
    {
        startPoint = transform.position;
        Destroy(gameObject, 1f); // Hủy sau 1 giây (dù có bay đến đâu)
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Không làm gì nếu va chạm chính chủ
        if (collision.gameObject == owner) return;

        // Nếu va chạm với Player, Bot, hoặc Weapon thì huỷ đạn
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Bot"))
        {
            Destroy(gameObject);
        }
    }
}
