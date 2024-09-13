using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechantScript : MonoBehaviour
{
    public Transform player;            // Référence au joueur
    public float vitesse = 5f;          // Vitesse de l'ennemi
    public float distanceAttaque = 1.5f; // Distance à laquelle l'ennemi attaque
    public int maxHealth = 100;         // Santé maximale de l'ennemi
    public int currentHealth;           // Santé actuelle de l'ennemi
    public int attackDamage = 20;       // Dégâts infligés par l'ennemi
    public Image BackGround;            // Référence à l'image de fond de la barre de vie
    public Image Image2;               // Référence à l'image de la barre de vie (couleur qui diminue)
    public TextMeshProUGUI gameOverText;
    // Référence au texte "Game Over"

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Assure-toi que la barre de vie est correctement initialisée
        UpdateHealthBar();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // Assurez-vous que le texte est caché au début
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            // L'ennemi est mort, n'exécute pas la logique de déplacement
            animator.SetBool("IsDead", true);
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > distanceAttaque)
        {
            animator.SetBool("IsAttacking", false);

            if (direction.magnitude > 0.1f)
            {
                Quaternion nouvelleRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, nouvelleRotation, 0.15f);

                transform.Translate(Vector3.forward * vitesse * Time.deltaTime);

                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Met à jour la barre de vie
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " a été tué.");
        animator.SetBool("IsDead", true);

        // Affiche le texte "Game Over"
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        // Optionnel : Détruire l'objet après un délai pour permettre à l'animation de jouer
        Destroy(gameObject, 2.0f);
    }

    void UpdateHealthBar()
    {
        if (Image2 != null)
        {
            // Calcule le nouveau fillAmount en fonction de la santé actuelle
            float fillAmount = (float)currentHealth / maxHealth;
            Image2.fillAmount = fillAmount;

            // Debug pour vérifier la mise à jour
            Debug.Log("Barre de vie mise à jour. Santé actuelle : " + currentHealth + ", Fill Amount : " + fillAmount);
        }
        else
        {
            Debug.LogError("Image2 (Barre de vie) non assignée !");
        }
    }
}
