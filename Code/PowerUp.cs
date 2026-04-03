using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public CharacterStats character;
    public Gun gun;// Gắn đối tượng nhân vật trong Inspector

    private void Awake()
    {
        character = GetComponent<CharacterStats>();
        gun = GetComponent<Gun>();


    }
    // ----------- Max Health -----------
    // ----------- Health, Armor, Mana -----------F
    public void IncreaseMaxHealth(float amount)
    {
        character.maxHealth += amount;
        character.health += amount;
    }

    public void IncreaseMaxArmor(float amount)
    {
        character.maxArmor += amount;
        character.armor += amount;
    }

    public void IncreaseMaxMana(float amount)
    {
        character.maxMana += amount;
        character.mana += amount;
    }

    // ----------- Move Speed -----------
    public void IncreaseMoveSpeed(float amount)
    {
        character.moveSpeed += amount;
    }

    // ----------- Recovery -----------
    public void IncreaseHealthRecovery(float amount)
    {
        character.healthRecover += amount;
    }

    public void IncreaseManaRecovery(float amount)
    {
        character.manaRecover += amount;
    }

    // ----------- RIFLE PowerUp -----------
    public void IncreaseRifleFireRate(float amount)
    {
        gun.rifleFireRate += amount;
    }


    public void IncreaseRifleMaxBullet(float amount)
    {
        gun.rifleMaxBullet += amount;
        gun.rifleBullet += amount;
    }

    // ----------- SNIPER PowerUp -----------
    public void IncreaseSniperFireRate(float amount)
    {
        gun.sniperFireRate += amount;
    }

    public void IncreaseRifleDamage(float amount)
    {
        if (gun != null)
            gun.rifleDamage += amount;
        else
            Debug.LogWarning("Gun is null in PowerUp.IncreaseRifleDamage");
    }

    public void IncreaseSniperDamage(float amount)
    {
        if (gun != null)
            gun.sniperDamage += amount;
        else
            Debug.LogWarning("Gun is null in PowerUp.IncreaseSniperDamage");
    }

    public void IncreaseSniperMaxBullet(float amount)
    {
        gun.sniperMaxBullet += amount;
        gun.sniperBullet += amount;
    }
    public void IncreaseSpecaildame(float amount)
    {
        if (gun != null)
            gun.specialBulletDamage += amount;
        else
            Debug.LogWarning("Gun is null in PowerUp.IncreaseRifleDamage");
    }
    public void manacost(float amount)
    {
        if (gun != null)
        {
            gun.manaCostPerShot -= amount;
            if (gun.manaCostPerShot < 0) // Đảm bảo manaCost không âm
                gun.manaCostPerShot = 0;
        }
        else
        {
            Debug.LogWarning("Gun is null in PowerUp.manacost");

        }
    }
}