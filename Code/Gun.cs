using UnityEngine;
using System.Collections;
public class Gun : MonoBehaviour
{
    private CharacterStats playerStats;

    public static Gun Instance;
    [SerializeField] private Animator gunAnimator;
    public enum FireMode
    {
        Rifle,
        Sniper,
        
    }

    [Header("Fire Mode Settings")]
    public FireMode currentMode = FireMode.Rifle;

    [Header("Common Stats")]
    public float nextFireTime = 0f;

    [Header("Rifle Settings")]
    public float rifleFireRate = 10f;
    public float rifleDamage = 10f;
    public float rifleMaxBullet = 30f;
    public float rifleBullet;
    public GameObject rifleBulletPrefab;
    public Transform rifleFirePoint;

    [Header("Sniper Settings")]
    public float sniperFireRate = 1f;
    public float sniperDamage = 100f;
    public float sniperMaxBullet = 5f;
    public float sniperBullet;
    public GameObject sniperBulletPrefab;
    public Transform sniperFirePoint;
    [Header("Special Bullet Settings")]
    public GameObject specialBulletPrefab;
    public float specialBulletDamage = 100f;
    public float manaCostPerShot = 25f;
    public Transform specialFirePoint;
    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        rifleBullet = rifleMaxBullet;
        sniperBullet = sniperMaxBullet;
         playerStats = GetComponentInParent<CharacterStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("Gun không tìm thấy CharacterStats từ parent!");
        }
    }

    public void SwitchMode()
    {
        currentMode = (currentMode == FireMode.Rifle) ? FireMode.Sniper : FireMode.Rifle;

        if (currentMode == FireMode.Rifle)
            GunUI.Instance?.UpdateAmmoUI(FireMode.Rifle, rifleBullet, rifleMaxBullet);
        else
            GunUI.Instance?.UpdateAmmoUI(FireMode.Sniper, sniperBullet, sniperMaxBullet);
    }

    public void Shoot()
    {
        if (Time.time < nextFireTime)
        {

            return;
        }

        // Xác định hướng bắn
        float facingDirection = transform.root.localScale.x;
        Vector2 shootDirection = (facingDirection > 0) ? Vector2.right : Vector2.left;

        switch (currentMode)
        {
            case FireMode.Rifle:
                if (rifleBullet <= 0)
                {

                    return;
                }

                rifleBullet--;
                GunUI.Instance?.UpdateAmmoUI(FireMode.Rifle, rifleBullet, rifleMaxBullet);
                nextFireTime = Time.time + 1f / rifleFireRate;

                if (gunAnimator != null) gunAnimator.SetTrigger("Shot1");

                StartCoroutine(FireBulletWithDelay(rifleBulletPrefab, rifleFirePoint, shootDirection, Bullet.BulletType.Rifle, 0.1f));
                break;

            case FireMode.Sniper:
                if (sniperBullet <= 0)
                {

                    return;
                }

                sniperBullet--;
                GunUI.Instance?.UpdateAmmoUI(FireMode.Sniper, sniperBullet, sniperMaxBullet);
                nextFireTime = Time.time + 1f / sniperFireRate;

                if (gunAnimator != null) gunAnimator.SetTrigger("Shot2");

                StartCoroutine(FireBulletWithDelay(sniperBulletPrefab, sniperFirePoint, shootDirection, Bullet.BulletType.Sniper, 0.1f));
                break;
        }
    }

    public void Reload()
    {
        switch (currentMode)
        {
            case FireMode.Rifle:
                rifleBullet = rifleMaxBullet;
                GunUI.Instance?.UpdateAmmoUI(FireMode.Rifle, rifleBullet, rifleMaxBullet);
                break;
            case FireMode.Sniper:
                sniperBullet = sniperMaxBullet;
                GunUI.Instance?.UpdateAmmoUI(FireMode.Sniper, sniperBullet, sniperMaxBullet);

                break;
        }
    }
    private IEnumerator FireBulletWithDelay(GameObject prefab, Transform firePoint, Vector2 direction, Bullet.BulletType type, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (prefab && firePoint)
        {
            GameObject bulletObj = Instantiate(prefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.type = type;
                bulletScript.SetDirection(direction);

                float damage = 0f;
                switch (type)
                {
                    case Bullet.BulletType.Rifle:
                        damage = rifleDamage;
                        break;
                    case Bullet.BulletType.Sniper:
                        damage = sniperDamage;
                        break;
                    case Bullet.BulletType.Special:
                        damage = specialBulletDamage;
                        break;
                }

                bulletScript.SetDamage(damage);
            }
        }
    }
   public void ShootSpecial()
{
    Debug.Log("ShootSpecial called"); // 1
    if (playerStats == null)
    {
        Debug.LogError("PlayerStats is NULL");
        return;
    }

    if (playerStats.mana < manaCostPerShot)
    {
        Debug.Log("Không đủ mana!");
        return;
    }

    Debug.Log("Bắt đầu bắn Special");

    playerStats.mana -= manaCostPerShot;

    float facingDirection = transform.root.localScale.x;
    Vector2 shootDirection = (facingDirection > 0) ? Vector2.right : Vector2.left;

    if (gunAnimator != null) gunAnimator.SetTrigger("Shot3");

    StartCoroutine(FireBulletWithDelay(specialBulletPrefab, specialFirePoint, shootDirection, Bullet.BulletType.Special, 0.1f));
}


}
