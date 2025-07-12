using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSkinLoader : MonoBehaviour
{
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer pantsRenderer;
    [SerializeField] private Transform hatSlot;
    [SerializeField] private Transform weaponSlot;

    [SerializeField] private GameObject[] hatPrefabs;
    [SerializeField] private GameObject[] weaponPrefabs;

    [SerializeField] private ColorPalette colorPalette;
    [SerializeField] private Material[] pantsMaterials;

    void Start()
    {
        LoadSkin();
    }

    void LoadSkin()
    {
        // === Body Color ===
        string colorHex = PlayerPrefs.GetString("PlayerColor", "FFFFFF");
        if (ColorUtility.TryParseHtmlString("#" + colorHex, out Color loadedColor))
        {
            playerRenderer.material.color = loadedColor;
        }

        // === Pants ===
        int pantsIndex = PlayerPrefs.GetInt("PantsMaterialIndex", 0);
        if (pantsIndex >= 0 && pantsIndex < pantsMaterials.Length)
        {
            pantsRenderer.material = pantsMaterials[pantsIndex];
        }


        // === Hat ===
        int hatIndex = PlayerPrefs.GetInt("HatIndex", 0);
        if (hatIndex >= 0 && hatIndex < hatPrefabs.Length)
        {
            GameObject hat = Instantiate(hatPrefabs[hatIndex], hatSlot);
        }

        // === Weapon ===
        int weaponIndex = PlayerPrefs.GetInt("WeaponIndex", 0);


        if (weaponIndex >= 0 && weaponIndex < weaponPrefabs.Length)
        {
            GameObject weapon = Instantiate(weaponPrefabs[weaponIndex], weaponSlot);

            //weapon.transform.localPosition = Vector3.zero;
            //weapon.transform.localRotation = Quaternion.identity;
            //weapon.transform.localScale = Vector3.one;

            string sceneName = SceneManager.GetActiveScene().name;
            bool isSelectionScene = sceneName == "SelectionScene";
         

            Rigidbody rb = weapon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = isSelectionScene;
            }

            Collider col = weapon.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = !isSelectionScene;
            }

            MonoBehaviour[] scripts = weapon.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                s.enabled = !isSelectionScene;
            }
        }
    }
}
