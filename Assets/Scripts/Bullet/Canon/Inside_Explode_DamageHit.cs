using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inside_Explode_DamageHit : MonoBehaviour
{
    Collider _collider;
    public GameObject exploded_Object;
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

        if (collision.gameObject.tag == "Enemy")
        {
            HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();
            if (enemyHealth.CurrentHP >= 0)
            {
                enemyHealth.TakeDamage(-25);
            }

        }
        if (collision.gameObject.tag == "Player")
        {
            HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
            playerHealth.TakeDamage(-2);
        }
    }
    void AutoColliderDisable()
    {
        _collider.enabled = false;
    }
}
