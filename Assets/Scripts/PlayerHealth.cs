using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; 
    [SerializeField] private int currentHealth;


    //Sistema de vida para el personaje que se pueda definir aqui, ej vida max, vida actual
    //Y en el de enemigo crear un enemigo volador que al entrar en contacto con el jugador explote(particulas) y quite vida al jugador 
    //EXTRA y que lo empuje

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // prueba para reducir vida 
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            TakeDamage(10);

        }

        // prueba para curarse
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(10);
        }
    }

    // Reducir la vida del jugador
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

       
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        print($"El jugador recibio {damageAmount} de daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para curar al jugador
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Asegurarse de que no supera la vida máxima
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        print($"El jugador se curó {healAmount}. Vida actual: {currentHealth}");
    }

    // Proximamente
    private void Die()
    {
        print("El jugador ha muerto.");
       
    }

    // Reiniciar la vida del jugador
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        print("La vida del jugador ha sido reiniciada.");
    }
}
