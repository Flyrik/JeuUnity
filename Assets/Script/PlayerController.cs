using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int damageToEnemy = 20; // Dégâts infligés à l'ennemi
    public Transform enemy; // Référence à l'ennemi

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mechant"))
        {
            MechantScript enemyScript = other.GetComponent<MechantScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageToEnemy);
                Debug.Log("L'ennemi a pris " + damageToEnemy + " de dégâts !");
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
                    Debug.Log("L'ennemi a pris " + damageToEnemy + " de dégâts !");
                }
            }
        }
    }
}
