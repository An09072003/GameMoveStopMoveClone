using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Giữ rotation cố định (khóa X/Y), chỉ cần nhìn hướng xuống hoặc theo hướng bạn setup sẵn
        transform.rotation = Quaternion.Euler(30f, 0f, 0f); // Góc cố định: 30 độ nghiêng xuống
    }
}
