using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandlerScript : MonoBehaviour
{

    //spawnables
    public GameObject astro;

    //spawnables timers
    private float astroFrequency = .3f;
    private float astroTimer = 0f;
    private GameObject astroTitleText;
    private Vector3 astroTitleTextOGPos;
    private GameObject blastroTitleText;
    private Vector3 blastroTitleTextOGPos;
    private bool introActive = true;

    public AudioClip menuMusicIntro;
    public ParticleSystem astroTitleTextPS;
    public ParticleSystem blastroTitleTextPS;

    // Start is called before the first frame update
    void Start()
    { 
        astroTitleText = GameObject.Find("AstroText");
        astroTitleTextOGPos = astroTitleText.transform.localPosition;
        astroTitleText.transform.localPosition = new Vector3(astroTitleTextOGPos.x + 500, astroTitleTextOGPos.y, astroTitleTextOGPos.z);
        blastroTitleText = GameObject.Find("BlastroText");
        blastroTitleTextOGPos = blastroTitleText.transform.localPosition;
        blastroTitleText.transform.localPosition = new Vector3(blastroTitleTextOGPos.x - 500, blastroTitleTextOGPos.y, blastroTitleTextOGPos.z);

    }

    // Update is called once per frame
    void Update()
    {
        astroTimer += Time.deltaTime;
        
        //move title text into place over the length of the menu music intro, then activate particle effect
        if(introActive && astroTitleText.transform.localPosition.x >= astroTitleTextOGPos.x)
        {
            Vector2 movementVector = Vector2.left * Time.deltaTime * 500 / (menuMusicIntro.length - 1f);
            //multiply by parent scale to ensure accurate translation
            astroTitleText.transform.Translate(movementVector * astroTitleText.transform.parent.localScale);
        }
        if (introActive && blastroTitleText.transform.localPosition.x <= blastroTitleTextOGPos.x)
        {
            Vector2 movementVector = Vector2.right * Time.deltaTime * 500 / (menuMusicIntro.length - 1f);
            blastroTitleText.transform.Translate(movementVector * blastroTitleText.transform.parent.localScale);
        }
        else if(introActive)
        {
            introActive = false;
            astroTitleText.transform.localPosition = astroTitleTextOGPos;
            blastroTitleText.transform.localPosition = blastroTitleTextOGPos;
            astroTitleTextPS.Play();
            blastroTitleTextPS.Play();
        }

        //spawn astros and apply force, with increasing frequency and force from difficulty modifier
        if (!introActive && astroTimer > astroFrequency)
        {
            astroTimer = 0f;
            float randomX = Random.Range(-1f, 10f);
            float randomSize = Random.Range(.75f, 1.5f);
            Vector3 spawnLocation = new Vector3(randomX, 6, 2);
            GameObject newAstro = GameObject.Instantiate(astro, spawnLocation, Quaternion.identity);
            newAstro.transform.localScale = new Vector3(randomSize, randomSize, 1);
            float movementForce = Random.Range(90f, 180f);
            newAstro.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,-1) * movementForce);
        }        
    }
}
