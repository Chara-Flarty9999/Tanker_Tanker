using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Explode_DamageHit : MonoBehaviour
{
    Collider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        Invoke("AutoColliderDisable", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Debug.Log("“G‚É“–‚½‚Á‚½‚Å");
                HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();
                if (enemyHealth.CurrentHP >= 0)
                {
                    enemyHealth.TakeDamage(-5);
                }
                break;
            case "Player":
                HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
                playerHealth.TakeDamage(-1);
                break;
            default:
                Debug.Log("‘½•ª’n–Ê‚Æ‚©");
                break;
        }
    }

    void AutoColliderDisable()
    {
        _collider.enabled = false;
    }
}
