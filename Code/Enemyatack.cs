using UnityEngine;

public class AlienAttackHitbox : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 0.2f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Tự hủy sau khi gây sát thương
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage); // Giả sử có hàm này
            }
        }
    }
}
