using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    //Globals
    public float movementSpeed = 1f;
    public AudioClip powerupSound;
    public GameObject tinyCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * movementSpeed);
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            otherCollider.gameObject.SendMessage("AcquireBomb");
            GameObject.Find("AudioPlayer").GetComponent<AudioSource>().PlayOneShot(powerupSound, .3f);
            GameObject.Find("GameplayHandler").SendMessage("IncreaseScore", 2000);
            //spawn text indicating score increase
            GameObject newTinyCanvas = Instantiate(tinyCanvas, transform.position, Quaternion.identity);
            newTinyCanvas.transform.GetComponentInChildren<UnityEngine.UI.Text>().text = "2000";
            newTinyCanvas.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20f);
            Destroy(newTinyCanvas, 1.2f);
            Destroy(gameObject);
        }
    }
}
