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
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();
                Debug.Log($"残りHPは{enemyHealth.CurrentHP}やで: ダメージ判定君");
                if (enemyHealth.CurrentHP >= 0)
                {
                    enemyHealth.TakeDamage(-25);
                }
                break;
            case "Player":
                HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
                playerHealth.TakeDamage(-2);
                break;
            default:
                Debug.Log("多分地面とか");
                break;
        }
    }
    void AutoColliderDisable()
    {
        _collider.enabled = false;
    }
}
