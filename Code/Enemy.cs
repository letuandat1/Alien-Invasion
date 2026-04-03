using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float speed = 2f;
    [Header("Item Drop Settings")]
    public GameObject healthItemPrefab;
    [Range(0f, 1f)]
    public float dropChance = 1f; 
    protected virtual void MoveTowardsTarget(Transform target) { }

    protected virtual void AttackPlayer(GameObject player) { }
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} bị trúng đạn, máu còn lại: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} đã chết!");
        TryDropHealthItem();
        
        Destroy(gameObject, 0.1f);
    }
    private void TryDropHealthItem()
    {
           Debug.Log("Thử rơi vật phẩm"); 
        if (healthItemPrefab != null && Random.value <= dropChance)
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
    }
}
