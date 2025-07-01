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

    [Header("Color Palette")]
    public ColorPalette colorPalette;
    private int currentColorIndex = 0;

    void Start()
    {
        // Load saved color from PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerColor") && colorPalette != null && colorPalette.colors.Length > 0)
        {
            string colorHex = PlayerPrefs.GetString("PlayerColor");
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
        }

        UpdateAll();
    }

    // ================= COLOR =================
    public void NextPlayerColor()
    {
        if (colorPalette == null)
        {
            Debug.LogWarning("ColorPalette is null!");
            return;
        }

        if (colorPalette.colors.Length == 0)
        {
            Debug.LogWarning("ColorPalette has no colors!");
            return;
        }

        currentColorIndex = (currentColorIndex + 1) % colorPalette.colors.Length;
        Debug.Log("Switched to color index: " + currentColorIndex);
        UpdateColor();
    }


    public void PrevPlayerColor()
    {
        if (colorPalette == null || colorPalette.colors.Length == 0) return;
        currentColorIndex = (currentColorIndex - 1 + colorPalette.colors.Length) % colorPalette.colors.Length;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (playerRenderer != null && colorPalette != null && colorPalette.colors.Length > 0)
        {
            playerRenderer.material.color = colorPalette.colors[currentColorIndex];

        }
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
            GameObject selectedHat = hatPrefabs[currentHatIndex];

            if (selectedHat != null)
            {
                GameObject hat = Instantiate(selectedHat, hatSlot);
                hat.transform.localPosition = Vector3.zero;
                hat.transform.localRotation = Quaternion.identity;
                hat.transform.localScale = Vector3.one;
                currentHat = hat;
            }
            else
            {
                currentHat = null;
            }
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
            weapon.transform.localScale = Vector3.one;
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
        if (colorPalette != null && colorPalette.colors.Length > 0)
        {
            PlayerPrefs.SetString("PlayerColor", ColorUtility.ToHtmlStringRGB(colorPalette.colors[currentColorIndex]));
        }

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
