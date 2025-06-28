using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour
{
    public Renderer playerRenderer;
    public Transform hatSlot;
    public Transform weaponSlot;

    public GameObject[] hatPrefabs;
    public GameObject[] weaponPrefabs;

    void Start()
    {
        LoadSkin();
    }

    void LoadSkin()
    {
        // Load màu
        string colorHex = PlayerPrefs.GetString("PlayerColor", "FFFFFF");
        if (ColorUtility.TryParseHtmlString("#" + colorHex, out Color loadedColor))
        {
            playerRenderer.material.color = loadedColor;
        }

        // Load mũ
        int hatIndex = PlayerPrefs.GetInt("HatIndex", 0);
        if (hatIndex >= 0 && hatIndex < hatPrefabs.Length)
        {
            GameObject hat = Instantiate(hatPrefabs[hatIndex], hatSlot);
            hat.transform.localPosition = Vector3.zero;
            hat.transform.localRotation = Quaternion.identity;
            hat.transform.localScale = Vector3.one;
        }

        // Load vũ khí
        int weaponIndex = PlayerPrefs.GetInt("WeaponIndex", 0);
        if (weaponIndex >= 0 && weaponIndex < weaponPrefabs.Length)
        {
            GameObject weapon = Instantiate(weaponPrefabs[weaponIndex], weaponSlot);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = Vector3.one;
        }
    }
}
