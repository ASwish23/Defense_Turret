using UnityEngine;
using System.Collections; // Required for the "Wait" command

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int currentHealth = 1;

    [Header("Flash Settings")]
    public Color flashColor = Color.red;   // The color it flashes when hit
    public float flashDuration = 0.1f;     // How long the flash lasts

    private SpriteRenderer spriteRend;
    private Color originalColor;

    void Start()
    {
        // 1. Grab the SpriteRenderer component so we can change the color
        spriteRend = GetComponent<SpriteRenderer>();

        // 2. Remember what the enemy looked like at the start
        if (spriteRend != null)
        {
            originalColor = spriteRend.color;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth = currentHealth - damageAmount;

        // 3. Trigger the flash effect
        if (spriteRend != null && currentHealth > 0)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // This special function allows us to "Wait" for a fraction of a second
    IEnumerator FlashEffect()
    {
        // Change color to Red
        spriteRend.color = flashColor;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(flashDuration);

        // Change color back to Normal
        spriteRend.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}