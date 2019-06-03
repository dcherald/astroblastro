using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //spawnables
    public GameObject astro;
    public GameObject blastro;
    public GameObject extraLife;
    public GameObject bomb;
    public GameObject astroDebris;
    public GameObject shipDebris;

    //spawnables timers
    private float astroFrequency = .3f;
    private float astroTimer = 0f;
    private float blastroFrequency = 10f;
    private float blastroTimer = 0f;
    private float extraLifeFrequency = 30f;
    private float extraLifeTimer = 15f; //timer starts high to get first bonus life out early
    private float bombFrequency = 20f;
    private float bombTimer = 0f;
    //private float movementUpgradeFrequency = 15f;
    //private float movementUpgradeTimer = 0f;
    private float difficultyIncreaseModifier = 0f;

    private UnityEngine.UI.Image difficultyBar;

    // Start is called before the first frame update
    void Start()
    {
        difficultyBar = GameObject.Find("DifficultyBar").GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        astroTimer += Time.deltaTime;
        blastroTimer += Time.deltaTime;
        extraLifeTimer += Time.deltaTime;
        bombTimer += Time.deltaTime;

        //difficulty modifier increases over time, resets on death (reset method below, called from gameplay handler)
        if (difficultyIncreaseModifier < 1.5)
        {
            difficultyIncreaseModifier += .0005f;
        }
        else
        {
            difficultyIncreaseModifier += .0001f;
        }
        //fill difficulty bar graphic to show increase
        difficultyBar.fillAmount = difficultyIncreaseModifier / 2.5f;
        //print(difficultyIncreaseModifier);

        //spawn astros and apply force, with increasing frequency and force from difficulty modifier
        if (astroTimer * (1 + difficultyIncreaseModifier) > astroFrequency)
        {
            astroTimer = 0f;
            float randomX = Random.Range(-3f, 3f);
            float randomSize = Random.Range(.75f, 1.5f);
            Vector3 spawnLocation = new Vector3(randomX, 6, 0);
            GameObject newAstro = GameObject.Instantiate(astro, spawnLocation, Quaternion.identity);
            newAstro.transform.localScale = new Vector3(randomSize, randomSize, 1);
            float movementForce = Random.Range(75f, 150f) * (1 + (difficultyIncreaseModifier/2));
            newAstro.GetComponent<Rigidbody2D>().AddForce(Vector2.down * movementForce);
        }
        //spawn blastros (weapon upgrades)
        if(blastroTimer > blastroFrequency)
        {
            blastroTimer = 0f;
            float randomX = Random.Range(-3f, 3f);
            Vector3 spawnLocation = new Vector3(randomX, 6, -1);
            GameObject newBlastro = GameObject.Instantiate(blastro, spawnLocation, Quaternion.identity);
        }
        //spawn bombs
        if(bombTimer > bombFrequency)
        {
            bombTimer = 0f;
            float randomX = Random.Range(-3f, 3f);
            Vector3 spawnLocation = new Vector3(randomX, 6, -1);
            GameObject newBomb = GameObject.Instantiate(bomb, spawnLocation, Quaternion.identity);
        }
        //spawn extra lives
        if(extraLifeTimer > extraLifeFrequency)
        {
            extraLifeTimer = 0f;
            float randomX = Random.Range(-3f, 3f);
            Vector3 spawnLocation = new Vector3(randomX, 6, -1);
            GameObject newLife = GameObject.Instantiate(extraLife, spawnLocation, Quaternion.identity);
        }
        //spawn movement speed upgrades
        //if(movementUpgradeTimer > movementUpgradeFrequency)
        //{

        //}
        
    }

    public void SpawnMiniAstro(Vector3 spawnPosition)
    {
        float randomSize = Random.Range(.2f, .4f);
        GameObject newAstro = GameObject.Instantiate(astro, spawnPosition, Quaternion.identity);
        newAstro.transform.localScale = new Vector3(randomSize, randomSize, 1);
        float movementForce = Random.Range(75f, 150f);
        newAstro.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * movementForce);
    }

    public void SpawnAstroDebris(Vector3 spawnPosition)
    {
        for(int i=0; i < 5; i++)
        {
            float randomSize = Random.Range(.2f, .5f);
            GameObject newDebris = GameObject.Instantiate(astroDebris, spawnPosition, Quaternion.identity);
            newDebris.transform.localScale = new Vector3(randomSize, randomSize, 1);
            newDebris.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100);
            Destroy(newDebris, 1f);
        }
    }

    public void ReduceDifficultyModifier()
    {
        difficultyIncreaseModifier /= 2f;
    }

    public void SpawnShipDebris(Vector3 spawnPosition)
    {
        for (int i = 0; i < 7; i++)
        {
            float randomSize = Random.Range(.5f, 1f);
            GameObject newDebris = GameObject.Instantiate(shipDebris, spawnPosition, new Quaternion(0f,0f,Random.Range(0f, 1f),1f));
            newDebris.transform.localScale = new Vector3(randomSize, randomSize, 1);
            newDebris.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100);
            Destroy(newDebris, 1.3f);
        }
    }
}
