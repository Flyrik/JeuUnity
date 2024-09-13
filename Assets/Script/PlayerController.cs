using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int damageToEnemy = 20; // D�g�ts inflig�s � l'ennemi
    public Transform enemy; // R�f�rence � l'ennemi

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mechant"))
        {
            MechantScript enemyScript = other.GetComponent<MechantScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageToEnemy);
                Debug.Log("L'ennemi a pris " + damageToEnemy + " de d�g�ts !");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (enemy != null)
            {
                MechantScript enemyScript = enemy.GetComponent<MechantScript>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damageToEnemy);
                    Debug.Log("L'ennemi a pris " + damageToEnemy + " de d�g�ts !");
                }
            }
        }
    }
}
