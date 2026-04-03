using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class CharacterStats : MonoBehaviour
{
    private bool isStunned = false;
    public float health = 100f;
    public float maxHealth = 100f;
    private bool isDead = false;
    private Animator animator;
    public float armor;
    public float maxArmor;

    public float mana = 20f;
    public float maxMana = 20f;

    public float exp = 0f; // Kinh nghiệm hiện tại

    public float moveSpeed = 3f; // Tốc độ di chuyển

    public float healthRecover = 1f; // Lượng máu hồi mỗi lần
    public float manaRecover = 1f;
    public int level = 1;
    public int maxLevel = 10;   // Lượng mana hồi mỗi lần
    private float baseExpToNextLevel = 50f;
    public float ExpToNextLevel => baseExpToNextLevel * level;
    public PowerUpUIManager powerUpUIManager;
    private Rigidbody2D rb;
    private PlayerInput input;
    private Player movement;
    private GunManager gunManager;


    public void AddExp(float amount)
    {
        exp += amount;
        Debug.Log("Gained EXP: " + exp + " / " + ExpToNextLevel);

        while (exp >= ExpToNextLevel && level < 10) // max level = 10
        {
            LevelUp();
            exp = 0f; // Reset exp after leveling up

        }
    }     // Kéo PowerUpUIManager từ Inspector vào đây

    public void LevelUp()
    {

        if (level >= maxLevel)
            return;

        level++;
        Debug.Log("Leveled up to: " + level);

        if (powerUpUIManager != null)
        {
            powerUpUIManager.ShowRandomPowerUps();
        }
        else
        {
            Debug.LogWarning("PowerUpUIManager chưa được gán!");
        }
    }
    private void Start()
    {


        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        movement = GetComponent<Player>();
        gunManager = GetComponent<GunManager>();


    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        float remainingDamage = amount;

        if (armor > 0)
        {
            float armorDamage = Mathf.Min(armor, remainingDamage);
            armor -= armorDamage;
            remainingDamage -= armorDamage;
        }

        if (remainingDamage > 0)
        {
            health -= remainingDamage;
            health = Mathf.Max(health, 0);
        }
        if (!isDead)
        {
            StartCoroutine(StunCoroutine(0.3f)); // stun 0.5s
        }

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

   private void Die()
{
    isDead = true;

        if (animator != null)
        {
            // Reset các trigger/bool khác để tránh đè animation chết

            animator.ResetTrigger("Stun");
            animator.ResetTrigger("Shot1");
            animator.SetTrigger("Dead");
            animator.ResetTrigger("Shot3");
            animator.ResetTrigger("Shot2");
        animator.ResetTrigger("Reload");
    }

    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    if (input != null)
    {
        input.enabled = false;
        input.SwitchCurrentActionMap(null);
    }

    if (movement != null)
    {
        movement.enabled = false;
        movement.ResetInput();
    }

    if (gunManager != null)
        gunManager.enabled = false;

    StartCoroutine(HandleDeath());
}


    private IEnumerator HandleDeath()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;


    }

    public void RecoverHealth()
    {
        if (health < maxHealth)
        {
            health = Mathf.Min(health + healthRecover, maxHealth);
        }
    }

    public void RecoverMana()
    {
        if (mana < maxMana)
        {
            mana = Mathf.Min(mana + manaRecover, maxMana);
        }
    }
    // addmana
    public void AddMana(float amount)
    {
        mana += amount;
        mana = Mathf.Min(mana, maxMana); // Đảm bảo mana không vượt quá maxMana
    }
    //health
    public void AddHealth(float amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth); // Đảm bảo health không vượt quá maxHealth
    }

    private IEnumerator StunCoroutine(float duration)
    {
        if (isStunned || isDead) yield break;

        isStunned = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Stun");

        if (movement != null)
            movement.enabled = false;

        if (input != null)
            input.enabled = false;

        if (gunManager != null)
            gunManager.enabled = false;

        yield return new WaitForSeconds(duration);

        if (!isDead)
        {
            if (movement != null)
            {
                movement.enabled = true;
                movement.ResetInput();
            }

            if (input != null)
                input.enabled = true;

            if (gunManager != null)
                gunManager.enabled = true;
        }

        isStunned = false;
    }

    public bool IsStunned()
    {
        return isStunned;
    }

public bool IsDead()
{
    return isDead;
}}
