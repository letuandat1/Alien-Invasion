using UnityEngine;

public class Alien : Enemy
{
    public float exp = 10f;
    public float detectRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.2f;
    private float lastAttackTime = 0f;
    public float damage = 10f;
    private Transform player;
    private CharacterStats playerStats;
    private Rigidbody2D rb; // Sử dụng Rigidbody2D cho vật lý 2D
    private Animator anim; // 👈 Animator
    public GameObject attackHitboxPrefab; // Prefab được gán từ Inspector
    public Transform attackSpawnPoint;
    private bool isDead = false;
    public float timeSinceStart = 0f;
public float levelUpInterval = 30f;
    public static int currentStatLevel = 0;
    void Start()
    {
      
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       timeSinceStart += Time.deltaTime;
if (timeSinceStart >= levelUpInterval)
{
    Alien.currentStatLevel++;
    timeSinceStart = 0f;
    Debug.Log("Tăng cấp độ spawn quái lên: " + Alien.currentStatLevel);
}
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > detectRange)
        {
            anim.SetBool("isWalking", false);
            rb.linearVelocity = Vector2.zero;
        }
        else if (distance > attackRange)
        {
            anim.SetBool("isWalking", true);
            MoveTowardsTarget(player);
        }
        else
        {
            anim.SetBool("isWalking", false);
            rb.linearVelocity = Vector2.zero;
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                anim.SetTrigger("isAttacking");
                AttackPlayer(player.gameObject);
                lastAttackTime = Time.time;
            }
        }
    }


    protected override void MoveTowardsTarget(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;

        // Quay mặt theo hướng
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

       
        rb.linearVelocity = direction * speed;
    }

    protected override void AttackPlayer(GameObject player)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Alien (2D) tấn công người chơi!");

            anim.SetTrigger("isAttacking");

            if (attackHitboxPrefab && attackSpawnPoint)
            {
                Vector3 spawnPos = attackSpawnPoint.position;

                Quaternion rotation = (transform.localScale.x >= 0) ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);

                GameObject hitbox = Instantiate(attackHitboxPrefab, spawnPos, rotation);

                // 👇 Truyền damage
                AlienAttackHitbox hitboxScript = hitbox.GetComponent<AlienAttackHitbox>();
                if (hitboxScript != null)
                {
                    hitboxScript.damage = damage;
                }
            }
            lastAttackTime = Time.time;
        }
    }
    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Alien died and gave exp");
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("isDead");

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

      
        base.Die();

    
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            var stats = playerObj.GetComponent<CharacterStats>();
            if (stats != null)
            {
                stats.AddExp(exp);
                stats.AddMana(5f); // hoặc chia theo tỷ lệ % exp
            }
        }

        Destroy(gameObject, 0.5f); // Cho animation chạy xong
    }
    public void ApplyStatLevel(int level)
{
    damage += level * 2f;
    exp += level * 5f;
    speed += level * 0.1f;
    health += level * 25f;

  
    maxHealth += level * 25f;

    Debug.Log("Alien được tạo ở cấp độ " + level + ": damage=" + damage + ", speed=" + speed + ", exp=" + exp + ", health=" + health);
}
}
