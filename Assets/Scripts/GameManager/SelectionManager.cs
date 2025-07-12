using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    [Header("Preview References")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer pantsRenderer;
    [SerializeField] private Transform hatSlot;
    [SerializeField] private Transform weaponSlot;

    [Header("Hat Prefabs (Skin)")]
    [SerializeField] private GameObject[] hatPrefabs;
    private int currentHatIndex = 0;
    private GameObject currentHat;

    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject[] weaponPrefabs;
    private int currentWeaponIndex = 0;
    private GameObject currentWeapon;

    [Header("Body Color Palette")]
    [SerializeField] private ColorPalette colorPalette;
    private int currentColorIndex = 0;

    [Header("Pants Materials")]
    [SerializeField] private Material[] pantsMaterials;
    private int currentPantsMaterialIndex = 0;

    void Start()
    {
        // Load saved selections
        string colorHex = PlayerPrefs.GetString("PlayerColor", "FFFFFF");
        if (ColorUtility.TryParseHtmlString("#" + colorHex, out Color savedColor))
        {
            for (int i = 0; i < colorPalette.colors.Length; i++)
            {
                if (colorPalette.colors[i] == savedColor)
                {
                    currentColorIndex = i;
                    break;
                }
            }
        }

        currentHatIndex = PlayerPrefs.GetInt("HatIndex", 0);
        currentWeaponIndex = PlayerPrefs.GetInt("WeaponIndex", 0);
        currentPantsMaterialIndex = PlayerPrefs.GetInt("PantsMaterialIndex", 0);

        UpdateAll();
    }

    // ========== BODY COLOR ==========
    public void NextPlayerColor()
    {
        currentColorIndex = (currentColorIndex + 1) % colorPalette.colors.Length;
        UpdateColor();
    }

    public void PrevPlayerColor()
    {
        currentColorIndex = (currentColorIndex - 1 + colorPalette.colors.Length) % colorPalette.colors.Length;
        UpdateColor();
    }

    void UpdateColor()
    {
        Color selectedColor = colorPalette.colors[currentColorIndex];
        playerRenderer.material.color = selectedColor;

        string colorHex = ColorUtility.ToHtmlStringRGB(selectedColor);
        PlayerPrefs.SetString("PlayerColor", colorHex);
        PlayerPrefs.Save();
    }

    // ========== PANTS ==========
    public void NextPants()
    {
        currentPantsMaterialIndex = (currentPantsMaterialIndex + 1) % pantsMaterials.Length;
        UpdatePants();
    }

    public void PrevPants()
    {
        currentPantsMaterialIndex = (currentPantsMaterialIndex - 1 + pantsMaterials.Length) % pantsMaterials.Length;
        UpdatePants();
    }

    void UpdatePants()
    {
        pantsRenderer.material = pantsMaterials[currentPantsMaterialIndex];
        PlayerPrefs.SetInt("PantsMaterialIndex", currentPantsMaterialIndex);
        PlayerPrefs.Save();
    }

    // ========== HAT ==========
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
        if (currentHat != null) Destroy(currentHat);
        if (hatPrefabs.Length == 0) return;

        GameObject hat = Instantiate(hatPrefabs[currentHatIndex], hatSlot);
        currentHat = hat;

        PlayerPrefs.SetInt("HatIndex", currentHatIndex);
        PlayerPrefs.Save();
    }

    // ========== WEAPON ==========
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
        if (currentWeapon != null) Destroy(currentWeapon);
        if (weaponPrefabs.Length == 0) return;

        GameObject weapon = Instantiate(weaponPrefabs[currentWeaponIndex], weaponSlot);
        currentWeapon = weapon;

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = weapon.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MonoBehaviour[] scripts = weapon.GetComponents<MonoBehaviour>();
        foreach (var s in scripts) s.enabled = false;

        PlayerPrefs.SetInt("WeaponIndex", currentWeaponIndex);
        PlayerPrefs.Save();
    }

    // ========== SAVE & PLAY ==========
    void SaveSelections()
    {
        PlayerPrefs.SetInt("HatIndex", currentHatIndex);
        PlayerPrefs.SetInt("WeaponIndex", currentWeaponIndex);
        PlayerPrefs.SetInt("PantsMaterialIndex", currentPantsMaterialIndex);

        Color selectedColor = colorPalette.colors[currentColorIndex];
        string colorHex = ColorUtility.ToHtmlStringRGB(selectedColor);
        PlayerPrefs.SetString("PlayerColor", colorHex);

        PlayerPrefs.Save();
    }

    public void SaveAndPlay()
    {
        SaveSelections();
        SceneManager.LoadScene("GameScene");
    }

    void UpdateAll()
    {
        UpdateColor();
        UpdatePants();
        UpdateHat();
        UpdateWeapon();
    }
}
