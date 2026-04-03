using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 50f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.AddHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
