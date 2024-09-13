using UnityEngine;

public class DeplacementObjet : MonoBehaviour
{
    public float vitesse = 5f;
    public float forceSaut = 7f;
    private bool estAuSol = true;
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    public Transform mechant; // R�f�rence au m�chant

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Aucun Animator trouv� dans les enfants de Player");
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

        // Cr�er un vecteur de mouvement relatif � la rotation du personnage
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

            // V�rifier si le m�chant est � port�e
            if (Vector3.Distance(transform.position, mechant.position) < 1.5f)
            {
                // Infliger des d�g�ts au m�chant
                mechant.GetComponent<CharacterStats>().TakeDamage(20); // Ajuste les d�g�ts � 20
            }
        }
        else if (Input.GetKeyUp(KeyCode.E)) // Le joueur rel�che la touche 'E'
        {
            animator.SetBool("IsFighting", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estAuSol = true;
            animator.SetBool("IsJumping", false); // R�initialise l'�tat de saut lorsque le personnage atterrit
        }
    }
}