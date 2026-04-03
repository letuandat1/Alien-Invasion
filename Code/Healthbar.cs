using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public CharacterStats characterStats;

    [Header("Sliders")]
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider expSlider;
    public Slider armorSlider;
    [Header("Value Texts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI expText;

    public TextMeshProUGUI armorText;


    void Start()
    {
        if (characterStats != null)
        {
            if (healthSlider != null)
                healthSlider.maxValue = characterStats.maxHealth;

            if (manaSlider != null)
                manaSlider.maxValue = characterStats.maxMana;

            if (expSlider != null)
                expSlider.maxValue = characterStats.ExpToNextLevel;
            if (armorSlider != null)
                armorSlider.maxValue = characterStats.maxArmor;
        }
    }

    void Update()
    {
        if (characterStats == null) return;

        if (healthSlider != null)
        {
            healthSlider.value = characterStats.health;
            if (healthText != null)
                healthText.text = $"{Mathf.RoundToInt(characterStats.health)} / {Mathf.RoundToInt(characterStats.maxHealth)}";
        }

        if (manaSlider != null)
        {
            manaSlider.value = characterStats.mana;
            if (manaText != null)
                manaText.text = $"{Mathf.RoundToInt(characterStats.mana)} / {Mathf.RoundToInt(characterStats.maxMana)}";
        }

        if (expSlider != null)
        {
            expSlider.maxValue = characterStats.ExpToNextLevel;
            expSlider.value = characterStats.exp;

            if (expText != null)
                expText.text = $"{Mathf.RoundToInt(characterStats.exp)} / {Mathf.RoundToInt(characterStats.ExpToNextLevel)}";
        }
        if (armorSlider != null)
        {
            armorSlider.value = characterStats.armor;

            if (armorText != null)
                armorText.text = $"{Mathf.RoundToInt(characterStats.armor)} / {Mathf.RoundToInt(characterStats.maxArmor)}";
        }
    }
    public void RefreshAllBars()
{
    if (characterStats == null) return;

    if (healthSlider != null)
    {
        healthSlider.maxValue = characterStats.maxHealth;
        healthSlider.value = characterStats.health;
        if (healthText != null)
            healthText.text = $"{Mathf.RoundToInt(characterStats.health)} / {Mathf.RoundToInt(characterStats.maxHealth)}";
    }

    if (manaSlider != null)
    {
        manaSlider.maxValue = characterStats.maxMana;
        manaSlider.value = characterStats.mana;
        if (manaText != null)
            manaText.text = $"{Mathf.RoundToInt(characterStats.mana)} / {Mathf.RoundToInt(characterStats.maxMana)}";
    }

    if (expSlider != null)
    {
        expSlider.maxValue = characterStats.ExpToNextLevel;
        expSlider.value = characterStats.exp;
        if (expText != null)
            expText.text = $"{Mathf.RoundToInt(characterStats.exp)} / {Mathf.RoundToInt(characterStats.ExpToNextLevel)}";
    }

    if (armorSlider != null)
    {
        armorSlider.maxValue = characterStats.maxArmor;
        armorSlider.value = characterStats.armor;
        if (armorText != null)
            armorText.text = $"{Mathf.RoundToInt(characterStats.armor)} / {Mathf.RoundToInt(characterStats.maxArmor)}";
    }
}
}
