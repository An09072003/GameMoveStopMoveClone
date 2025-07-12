using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public GameObject BulletPrefab => bulletPrefab;
}
