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
        Invoke("AutoColliderDisable", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("“G‚É“–‚½‚Á‚½‚Å");
            HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();
            enemyHealth.TakeDamage(-5);
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("‰´‚É“–‚½‚Á‚½‚Å");
            HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
            playerHealth.TakeDamage(-5);
        }

    }

    void AutoColliderDisable()
    {
        _collider.enabled = false;
    }
}
