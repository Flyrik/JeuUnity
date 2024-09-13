using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; // Santé maximale
    public int currentHealth;   // Santé actuelle

    public Image healthBarFill; // Référence à l'image de la barre de vie (couleur qui diminue)

    public int damageFromMonster = 10; // Dégâts infligés par un monstre

    private Animator animator;

    void Start()
    {
        // La vie actuelle est égale à la vie maximale au début
        currentHealth = maxHealth;

        // Récupération de l'animator (si nécessaire)
        animator = GetComponent<Animator>();

        // Mise à jour de la barre de vie
        UpdateHealthBar();

        // Affiche la santé initiale dans la console pour debug
        Debug.Log(gameObject.name + " Initial Health: " + currentHealth);
    }

    // Fonction pour infliger des dégâts au personnage
    public void TakeDamage(int damage)
    {
        // Soustraire les dégâts à la santé actuelle
        currentHealth -= damage;
        // Clamp pour s'assurer que la santé ne va pas en dessous de 0 ou au-delà de la vie max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Mise à jour de la barre de vie
        UpdateHealthBar();

        // Affiche la santé après les dégâts dans la console pour debug
        Debug.Log(gameObject.name + " Health after damage: " + currentHealth);

        // Si la santé tombe à 0, le personnage meurt
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Fonction pour mettre à jour la barre de vie
    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Met à jour le `fillAmount` de l'image de la barre de vie
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log("Health bar updated. Fill amount: " + healthBarFill.fillAmount);
        }
        else
        {
            Debug.LogError("Health Bar Fill is not assigned!");
        }
    }

    // Fonction appelée quand le personnage meurt
    void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        if (animator != null)
        {
            // Déclenche l'animation de mort
            animator.SetBool("IsDead", true);

            // Appelle une méthode pour détruire l'objet après un délai
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            // Si aucun Animator n'est attaché, détruit immédiatement
            Destroy(gameObject);
        }
    }

    // Coroutine pour détruire l'objet après un délai
    IEnumerator DestroyAfterAnimation()
    {
        // Attends la durée de l'animation de mort avant de détruire l'objet
        // Ajuste la durée en fonction de la longueur de ton animation de mort
        yield return new WaitForSeconds(2.0f); // 2.0f est un exemple, ajuste en fonction de la durée de l'animation

        Destroy(gameObject);
    }


    // Méthode de détection des collisions avec le monstre
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si le personnage entre en collision avec un monstre
        if (collision.gameObject.CompareTag("Mechant"))
        {
            TakeDamage(damageFromMonster);
            Debug.Log("Personnage touché par un monstre. Dégâts infligés : " + damageFromMonster);
        }
    }

    // Méthode de test pour infliger des dégâts manuellement (en appuyant sur la touche Espace)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
}
