using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroScript : MonoBehaviour
{

    //Globals
    public GameObject astro;
    public Sprite[] astroSprites;
    public AudioClip explode;

    private float hitPoints = 1f;
    private GameObject spawner;
    private AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Spawner");
        Sprite randomSprite = astroSprites[Random.Range(0, astroSprites.Length)];
        gameObject.GetComponent<SpriteRenderer>().sprite = randomSprite;
        audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.down * Time.deltaTime * movementSpeed);
        if (transform.position.y < -10 || Mathf.Abs(transform.position.x) > 4)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            otherCollider.gameObject.SendMessage("TakeDamage", 1f);
            Die(false);
        }
    }

    public void TakeDamage(float amount)
    {
        if((hitPoints -= amount) <= 0)
        {
            //if massive damage was taken, don't play sound effect on death and don't spawn minis
            if (hitPoints < -5)
            {
                Die(true);
            }
            else
            {
                Die(false);
            }
        }
    }

    private void Die(bool massiveDamage)
    {
        //play destruction sound effect
        if (!massiveDamage) { audioPlayer.PlayOneShot(explode, .5f); }
        GameObject.Find("GameplayHandler").SendMessage("IncreaseScore", 100);
        if(transform.localScale.x > 1.2 && !massiveDamage)
        {
            for(int i = 0; i < 3; i++)
            {
                spawner.SendMessage("SpawnMiniAstro", transform.position, SendMessageOptions.DontRequireReceiver);
            }
        }
        spawner.SendMessage("SpawnAstroDebris", transform.position, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
