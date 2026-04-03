using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GunUI : MonoBehaviour
{
    public static GunUI Instance;

    [Header("UI for Rifle")]
    public GameObject rifleUI;
    public TextMeshProUGUI rifleAmmoText;

    [Header("UI for Sniper")]
    public GameObject sniperUI;
    public TextMeshProUGUI sniperAmmoText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmoUI(Gun.FireMode mode, float currentAmmo, float maxAmmo)
    {
        if (mode == Gun.FireMode.Rifle)
        {
            rifleUI.SetActive(true);
            sniperUI.SetActive(false);

            if (rifleAmmoText != null)
                rifleAmmoText.text = $"{currentAmmo} / {maxAmmo}";
        }
        else if (mode == Gun.FireMode.Sniper)
        {
            rifleUI.SetActive(false);
            sniperUI.SetActive(true);

            if (sniperAmmoText != null)
                sniperAmmoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }
}
