using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Rifle,
        Sniper,
        Special
    }

    public BulletType type;

    [Header("Rifle Stats")]
    public float speedRifle = 20f;
    public float lifetimeRifle = 2f;

    [Header("Sniper Stats")]
    public float speedSniper = 40f;
    public float lifetimeSniper = 2f;
    [Header("Special Stats")]
    public float speedSpecial = 25f;
    public float lifetimeSpecial = 2f;
    private int targetsHit = 0;
    private int maxPenetration = 1;  // 👈 số mục tiêu tối đa đạn đặc biệt có thể xuyên

    
    private float speed;
    private float lifetime;
    private float damage;

    private Vector2 moveDirection = Vector2.right;

    private void Start()
    {
        // Gán tốc độ và thời gian tồn tại theo loại đạn
        switch (type)
        {
            case BulletType.Rifle:
                speed = speedRifle;
                lifetime = lifetimeRifle;
                 maxPenetration = 1;
                break;

            case BulletType.Sniper:
                speed = speedSniper;
                lifetime = lifetimeSniper;
                  maxPenetration = 3;
                break;

            case BulletType.Special:
                speed = speedSpecial;
                lifetime = lifetimeSpecial;
                 maxPenetration = 5;
                break;
        }

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Di chuyển đạn theo hướng
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                targetsHit++;

                if (targetsHit >= maxPenetration)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
