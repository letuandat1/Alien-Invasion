using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PowerUpButton : MonoBehaviour
{
    private PowerUp powerUpRef;
    private Canvas canvasToClose;
    [SerializeField] private HealthBar healthBar;
    public string buttonText;

    public TMP_Text labelText; // Attach your UI Text element here

    private Action<PowerUp> applyEffect; // Delegate for applying effect

    /// <summary>
    /// Applies the power-up effect and closes the canvas.
    /// </summary>
    private void ApplyPowerUp()
    {
        if (powerUpRef == null) return;

        applyEffect?.Invoke(powerUpRef);

        // Refresh bars after applying effect
        if (healthBar != null)
            healthBar.RefreshAllBars();

        // Close the canvas
        if (canvasToClose != null)
            canvasToClose.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    /// <summary>
    /// Initializes the button with data and behavior.
    /// </summary>
    /// <param name="powerUp">The power-up this button triggers</param>
    /// <param name="canvas">Canvas to close after applying</param>
    /// <param name="label">Label text to display</param>
    /// <param name="effect">Delegate to apply the effect</param>
    /// <param name="bar">Reference to health/mana/exp bar</param>
    /// <param name="buttonSize">New button size (optional)</param>
    /// <param name="textSize">Text font size (optional)</param>
    public void Initialize(PowerUp powerUp, Canvas canvas, string label, Action<PowerUp> effect, HealthBar bar, Vector2? buttonSize = null, int textSize = 24)
    {
        powerUpRef = powerUp;
        canvasToClose = canvas;
        applyEffect = effect;
        healthBar = bar;

        buttonText = label;

        // Update label text and font size
        if (labelText != null)
        {
            labelText.text = buttonText;
            labelText.fontSize = textSize;
        }

        // Update button size if provided
        if (buttonSize.HasValue)
        {
            GetComponent<RectTransform>().sizeDelta = buttonSize.Value;
        }

        // Clean and assign click event
        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(ApplyPowerUp);
    }
}
