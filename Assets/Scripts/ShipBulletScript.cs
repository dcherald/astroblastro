using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBulletScript : MonoBehaviour
{
    //Globals
    private float bulletSpeed = 5f;
    private float damageAmount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * bulletSpeed);
        if(transform.position.y > 5)
        {
            Destroy(gameObject);
        }
    }

    public void EnhanceSpeed(int upgradeCount)
    {
        bulletSpeed = bulletSpeed <= 20 ?  bulletSpeed + upgradeCount : bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Enemy"))
        {
            otherCollider.gameObject.SendMessage("TakeDamage", damageAmount);
            Destroy(gameObject);
        }
    }
}
