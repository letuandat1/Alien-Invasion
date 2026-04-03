using UnityEngine;
using System.Collections.Generic;
using System;

public class PowerUpUIManager : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    [System.Serializable]
    public class PowerUpOption
    {
        public string label;
        public GameObject buttonPrefab;
        public Action<PowerUp> effect;
        public Rarity rarity;

        public PowerUpOption(string label, GameObject prefab, Action<PowerUp> effect, Rarity rarity)
        {
            this.label = label;
            this.buttonPrefab = prefab;
            this.effect = effect;
            this.rarity = rarity;
        }
    }

    public List<GameObject> buttonPrefabs;
    public Transform buttonParent;
    public Canvas canvas;
    public PowerUp playerPowerUp;

    public int numberOfButtonsToShow = 3;
    private List<GameObject> spawnedButtons = new();

    private List<PowerUpOption> allOptions;
    private List<PowerUpOption> commonOptions = new();
    private List<PowerUpOption> rareOptions = new();
    private List<PowerUpOption> epicOptions = new();
    private List<PowerUpOption> legendaryOptions = new();

    private void Start()
    {
        InitOptions();
        ShowRandomPowerUps();
    }

    private void InitOptions()
    {
        allOptions = new List<PowerUpOption>();
        commonOptions.Clear();
        rareOptions.Clear();
        epicOptions.Clear();
        legendaryOptions.Clear();

        // Example options — add more as needed
        AddOption("Max Health +5", p => p.IncreaseMaxHealth(5f), Rarity.Common);
        AddOption("Max Armor +10", p => p.IncreaseMaxArmor(10f), Rarity.Rare);
        AddOption("Mana Recovery +1", p => p.IncreaseManaRecovery(1f), Rarity.Epic);
        AddOption("Sniper Damage +20", p => p.IncreaseSniperDamage(20f), Rarity.Legendary);
    }

    public enum Rarity { Common, Rare, Epic, Legendary }

    private void AddOption(string label, Action<PowerUp> effect, Rarity rarity = Rarity.Common)
    {
        GameObject prefab = buttonPrefabs[0];
        var option = new PowerUpOption(label, prefab, effect, rarity);
        allOptions.Add(option);

        switch (rarity)
        {
            case Rarity.Common: commonOptions.Add(option); break;
            case Rarity.Rare: rareOptions.Add(option); break;
            case Rarity.Epic: epicOptions.Add(option); break;
            case Rarity.Legendary: legendaryOptions.Add(option); break;
        }
    }

    public void ShowRandomPowerUps()
    {
        canvas.gameObject.SetActive(true);
        ClearButtons();
        Time.timeScale = 0f;

        var selectedOptions = new List<PowerUpOption>();
        var usedLabels = new HashSet<string>();

        while (selectedOptions.Count < numberOfButtonsToShow)
        {
            var option = GetRandomOptionByTier();
            if (!usedLabels.Contains(option.label))
            {
                selectedOptions.Add(option);
                usedLabels.Add(option.label);
            }
        }

        for (int i = 0; i < selectedOptions.Count; i++)
        {
            var opt = selectedOptions[i];
            GameObject btnObj = Instantiate(opt.buttonPrefab, buttonParent);

            // Manual positioning
            Vector2 targetPos = Vector2.zero;
            if (i == 0) targetPos = new Vector2(-60000f, 0f); // Left
            if (i == 1) targetPos = new Vector2(0f, 0f);    // Center
            if (i == 2) targetPos = new Vector2(60000f, 0f);  // Right
            btnObj.GetComponent<RectTransform>().anchoredPosition = targetPos;

            // Optional: color the button by rarity
            var image = btnObj.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
                image.color = GetColorByRarity(opt.rarity);

            // Initialize with big size and font
            var btnScript = btnObj.GetComponent<PowerUpButton>();
            btnScript.Initialize(
                playerPowerUp,
                canvas,
                opt.label,
                opt.effect,
                healthBar,
                new Vector2(40000f, 40000f), // Button size
                5000 // Font size
            );

            spawnedButtons.Add(btnObj);
        }
    }

    private void ClearButtons()
    {
        foreach (var b in spawnedButtons)
        {
            Destroy(b);
        }
        spawnedButtons.Clear();
    }

    private PowerUpOption GetRandomOptionByTier()
    {
        PowerUpOption option = null;
        while (option == null)
        {
            float roll = UnityEngine.Random.value;
            if (roll < 0.60f)
                option = GetRandomFrom(commonOptions);      // 60%
            else if (roll < 0.85f)
                option = GetRandomFrom(rareOptions);        // 25%
            else if (roll < 0.95f)
                option = GetRandomFrom(epicOptions);        // 10%
            else
                option = GetRandomFrom(legendaryOptions);   // 5%
        }
        return option;
    }

    private PowerUpOption GetRandomFrom(List<PowerUpOption> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    private Color GetColorByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white;
            case Rarity.Rare: return new Color(0.2f, 0.6f, 1f);
            case Rarity.Epic: return new Color(0.8f, 0.2f, 1f);
            case Rarity.Legendary: return new Color(1f, 0.6f, 0f);
            default: return Color.gray;
        }
    }
}
