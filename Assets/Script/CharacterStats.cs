using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; // Sant� maximale
    public int currentHealth;   // Sant� actuelle

    public Image healthBarFill; // R�f�rence � l'image de la barre de vie (couleur qui diminue)

    public int damageFromMonster = 10; // D�g�ts inflig�s par un monstre

    private Animator animator;

    void Start()
    {
        // La vie actuelle est �gale � la vie maximale au d�but
        currentHealth = maxHealth;

        // R�cup�ration de l'animator (si n�cessaire)
        animator = GetComponent<Animator>();

        // Mise � jour de la barre de vie
        UpdateHealthBar();

        // Affiche la sant� initiale dans la console pour debug
        Debug.Log(gameObject.name + " Initial Health: " + currentHealth);
    }

    // Fonction pour infliger des d�g�ts au personnage
    public void TakeDamage(int damage)
    {
        // Soustraire les d�g�ts � la sant� actuelle
        currentHealth -= damage;
        // Clamp pour s'assurer que la sant� ne va pas en dessous de 0 ou au-del� de la vie max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Mise � jour de la barre de vie
        UpdateHealthBar();

        // Affiche la sant� apr�s les d�g�ts dans la console pour debug
        Debug.Log(gameObject.name + " Health after damage: " + currentHealth);

        // Si la sant� tombe � 0, le personnage meurt
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Fonction pour mettre � jour la barre de vie
    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Met � jour le `fillAmount` de l'image de la barre de vie
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log("Health bar updated. Fill amount: " + healthBarFill.fillAmount);
        }
        else
        {
            Debug.LogError("Health Bar Fill is not assigned!");
        }
    }

    // Fonction appel�e quand le personnage meurt
    void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        if (animator != null)
        {
            // D�clenche l'animation de mort
            animator.SetBool("IsDead", true);

            // Appelle une m�thode pour d�truire l'objet apr�s un d�lai
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            // Si aucun Animator n'est attach�, d�truit imm�diatement
            Destroy(gameObject);
        }
    }

    // Coroutine pour d�truire l'objet apr�s un d�lai
    IEnumerator DestroyAfterAnimation()
    {
        // Attends la dur�e de l'animation de mort avant de d�truire l'objet
        // Ajuste la dur�e en fonction de la longueur de ton animation de mort
        yield return new WaitForSeconds(2.0f); // 2.0f est un exemple, ajuste en fonction de la dur�e de l'animation

        Destroy(gameObject);
    }


    // M�thode de d�tection des collisions avec le monstre
    private void OnCollisionEnter(Collision collision)
    {
        // V�rifie si le personnage entre en collision avec un monstre
        if (collision.gameObject.CompareTag("Mechant"))
        {
            TakeDamage(damageFromMonster);
            Debug.Log("Personnage touch� par un monstre. D�g�ts inflig�s : " + damageFromMonster);
        }
    }

    // M�thode de test pour infliger des d�g�ts manuellement (en appuyant sur la touche Espace)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
}
