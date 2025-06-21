using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Explode_DamageHit : MonoBehaviour
{
    Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
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
            Enemy enemydata = collision.gameObject.GetComponent<Enemy>();
            enemydata.DealDamage_Heal(-5);
        }
        if (collision.gameObject.tag == "Player")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            GameManager manager = gameManager.GetComponent<GameManager>();
            manager.GetDamage_Heal(-5);
        }

    }

    void AutoColliderDisable()
    {
        collider.enabled = false;
    }
}
