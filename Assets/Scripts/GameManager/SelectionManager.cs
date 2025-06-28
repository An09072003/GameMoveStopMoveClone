using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    [Header("Preview References")]
    public Renderer playerRenderer;
    public Transform hatSlot;
    public Transform weaponSlot;

    [Header("Hat Prefabs (Skin)")]
    public GameObject[] hatPrefabs;
    private int currentHatIndex = 0;
    private GameObject currentHat;

    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs;
    private int currentWeaponIndex = 0;
    private GameObject currentWeapon;

    [Header("Player Colors")]
    public Color[] playerColors;
    private int currentColorIndex = 0;

    void Start()
    {
        UpdateAll();
    }

    // ================= COLOR =================
    public void NextPlayerColor()
    {
        currentColorIndex = (currentColorIndex + 1) % playerColors.Length;
        UpdateColor();
    }

    public void PrevPlayerColor()
    {
        currentColorIndex = (currentColorIndex - 1 + playerColors.Length) % playerColors.Length;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (playerRenderer != null)
            playerRenderer.sharedMaterial.color = playerColors[currentColorIndex];
    }

    // ================= HAT =================
    public void NextHat()
    {
        currentHatIndex = (currentHatIndex + 1) % hatPrefabs.Length;
        UpdateHat();
    }

    public void PrevHat()
    {
        currentHatIndex = (currentHatIndex - 1 + hatPrefabs.Length) % hatPrefabs.Length;
        UpdateHat();
    }

    void UpdateHat()
    {
        if (currentHat != null)
            Destroy(currentHat);

        if (hatPrefabs.Length > 0)
        {
            GameObject hat = Instantiate(hatPrefabs[currentHatIndex], hatSlot);
            hat.transform.localPosition = Vector3.zero;
            hat.transform.localRotation = Quaternion.identity;
            hat.transform.localScale = Vector3.one; // <--- DÒNG NÀY GIÚP GIỮ TỈ LỆ
            currentHat = hat;
        }
    }

    // ================= WEAPON =================
    public void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponPrefabs.Length;
        UpdateWeapon();
    }

    public void PrevWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex - 1 + weaponPrefabs.Length) % weaponPrefabs.Length;
        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        if (weaponPrefabs.Length > 0)
        {
            GameObject weapon = Instantiate(weaponPrefabs[currentWeaponIndex], weaponSlot);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = Vector3.one; // <--- VÔ CÙNG QUAN TRỌNG
            currentWeapon = weapon;
        }
    }

    // ================= APPLY ALL =================
    void UpdateAll()
    {
        UpdateHat();
        UpdateWeapon();
        UpdateColor();
    }

    // ================= SAVE & PLAY =================
    public void SaveAndPlay()
    {
        PlayerPrefs.SetString("PlayerColor", ColorUtility.ToHtmlStringRGB(playerColors[currentColorIndex]));
        PlayerPrefs.SetInt("HatIndex", currentHatIndex);
        PlayerPrefs.SetInt("WeaponIndex", currentWeaponIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
