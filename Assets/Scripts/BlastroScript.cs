using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastroScript : MonoBehaviour
{
    //Globals
    public float movementSpeed = 2f;
    public AudioClip powerupSound;
    public GameObject tinyCanvas; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * movementSpeed, relativeTo: Space.World);
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        transform.Rotate(0f, 0f, 2f);
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            otherCollider.gameObject.SendMessage("UpgradeBullets");
            GameObject.Find("AudioPlayer").GetComponent<AudioSource>().PlayOneShot(powerupSound, .3f);
            GameObject.Find("GameplayHandler").SendMessage("IncreaseScore", 1000);
            //spawn text indicating score increase
            GameObject newTinyCanvas = Instantiate(tinyCanvas, transform.position, Quaternion.identity);
            newTinyCanvas.transform.GetComponentInChildren<UnityEngine.UI.Text>().text = "1000";
            newTinyCanvas.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20f);
            Destroy(newTinyCanvas, 1.2f);
            Destroy(gameObject);
        }
    }

}
