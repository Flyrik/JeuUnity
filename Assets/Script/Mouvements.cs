using UnityEngine;

public class DeplacementObjet : MonoBehaviour
{
    public float vitesse = 5f;
    public float forceSaut = 7f;
    private bool estAuSol = true;
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    public Transform mechant; // Référence au méchant

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Aucun Animator trouvé dans les enfants de Player");
        }

        if (mechant == null)
        {
            mechant = GameObject.FindGameObjectWithTag("Mechant").transform;
        }
    }

    void Update()
    {
        // Gestion des mouvements
        float deplacementHorizontal = Input.GetAxis("Horizontal");
        float deplacementVertical = Input.GetAxis("Vertical");

        // Créer un vecteur de mouvement relatif à la rotation du personnage
        Vector3 mouvement = transform.right * deplacementHorizontal + transform.forward * deplacementVertical;

        if (mouvement.magnitude > 0.1f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Appliquer le mouvement au personnage
        transform.Translate(mouvement.normalized * vitesse * Time.deltaTime, Space.World);

        // Gestion du saut
        if (Input.GetKeyDown(KeyCode.Space) && estAuSol)
        {
            rb.AddForce(Vector3.up * forceSaut, ForceMode.Impulse);
            animator.SetBool("IsJumping", true);
            estAuSol = false;
        }

        // Gestion du combat
        if (Input.GetKeyDown(KeyCode.E)) // Le joueur appuie sur la touche 'E'
        {
            animator.SetBool("IsFighting", true);

            // Vérifier si le méchant est à portée
            if (Vector3.Distance(transform.position, mechant.position) < 1.5f)
            {
                // Infliger des dégâts au méchant
                mechant.GetComponent<CharacterStats>().TakeDamage(20); // Ajuste les dégâts à 20
            }
        }
        else if (Input.GetKeyUp(KeyCode.E)) // Le joueur relâche la touche 'E'
        {
            animator.SetBool("IsFighting", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estAuSol = true;
            animator.SetBool("IsJumping", false); // Réinitialise l'état de saut lorsque le personnage atterrit
        }
    }
}