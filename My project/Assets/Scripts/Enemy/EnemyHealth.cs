using Unity.Netcode;
using UnityEngine;
using System;

public class EnemyHealth : NetworkBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(
        100f, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server
    );

    [Header("Visual Feedback")]
    [SerializeField] private GameObject deathEffect; 
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageFlashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;
    private bool isFlashing = false;

    public event Action OnDeath;

    void Start()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }


        currentHealth.OnValueChanged += OnHealthChanged;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
    }


    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        Debug.Log($"Enemy health changed: {oldHealth} -> {newHealth}");
        

        if (newHealth < oldHealth && !isFlashing)
        {
            StartCoroutine(FlashDamage());
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;
        Debug.Log($"Enemy took {damage} damage! Health: {currentHealth.Value}/{maxHealth}");

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }


    public void TakeDamage(float damage)
    {
        if (!IsServer)
        {
            Debug.LogWarning("TakeDamage called on client! Use TakeDamageServerRpc instead.");
            return;
        }

        currentHealth.Value -= damage;
        Debug.Log($"Enemy took {damage} damage! Health: {currentHealth.Value}/{maxHealth}");

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!IsServer) return;

        Debug.Log("Enemy died!");
        OnDeath?.Invoke();
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (NetworkObject != null)
        {
            NetworkObject.Despawn();
        }
        
        Destroy(gameObject);
    }


    private System.Collections.IEnumerator FlashDamage()
    {
        if (spriteRenderer == null) yield break;

        isFlashing = true;
        spriteRenderer.color = damageFlashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }


    public float GetCurrentHealth()
    {
        return currentHealth.Value;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return currentHealth.Value / maxHealth;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        currentHealth.OnValueChanged -= OnHealthChanged;
    }
}