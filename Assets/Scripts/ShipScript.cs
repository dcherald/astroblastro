using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    //Globals
    float movementSpeed = 3.0f;
    float mobileSpeedModifier = 1.25f; //ship moves faster on mobile to feel more comfortable with touch
    public GameObject bullet;
    public AudioClip bombSound;
    public AudioClip shipDeathSound;
    public GameObject shipShield;
    private float shotFrequency = .2f;
    private float shotTimer = 0f;
    private float hitPoints = 0f;
    private GameObject gameplayHandler;
    private ParticleSystem bombActivationEffect;
    private GameObject thrusters;
    private ParticleSystem thrustersEffect;
    private AudioSource audioPlayer;
    private float lastTouchTime;
    private bool invincible = false;

    //ship and shot upgrades
    private int blastros = 0;
    private int bombs = 0;
    private float shotFrequencyUpgrade = 1f;
    private float movementSpeedUpgrade = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameplayHandler = GameObject.Find("GameplayHandler");
        audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioSource>();
        bombActivationEffect = GameObject.Find("Bomb Halo").GetComponent<ParticleSystem>();
        thrusters = GameObject.Find("Thrusters");
        thrustersEffect = thrusters.GetComponent<ParticleSystem>();
        //play particle effect for boosters and stop the bomb effect from playing
        gameObject.GetComponent<ParticleSystem>().Play();
        bombActivationEffect.Stop();
        thrustersEffect.Stop();
        StartCoroutine(ShieldUp());
    }

    // Update is called once per frame
    void Update()
    {
        //keyboard movement keys WASD, arrowkeys
        //move up
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && transform.position.y < 4f)
        {
            transform.Translate(Vector2.up * Time.deltaTime * movementSpeed * movementSpeedUpgrade);
        }
        //move down
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.position.y > -4f)
        {
            transform.Translate(Vector2.down * Time.deltaTime * movementSpeed * movementSpeedUpgrade);
        }
        //move left
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > -2.75f)
        {
            transform.Translate(Vector2.left * Time.deltaTime * movementSpeed * movementSpeedUpgrade);
            thrusters.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            if (!thrustersEffect.isPlaying) { thrustersEffect.Play(); }
        }
        //move right
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < 2.75f)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * movementSpeedUpgrade);
            thrusters.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            if (!thrustersEffect.isPlaying) { thrustersEffect.Play(); }
        }
        else if(Input.touchCount == 0) //if moving neither left nor right, turn off thrusters
        {
            if (thrustersEffect.isPlaying) { thrustersEffect.Stop(); }
        }
        //mobile movement
        if (Input.touchCount > 0)
        {
            Vector2 currentShipPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 destinationShipPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            //is moving left
            if(currentShipPos.x > destinationShipPos.x)
            {
                thrusters.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                if (!thrustersEffect.isPlaying) { thrustersEffect.Play(); }
            }
            //is moving right
            else if(currentShipPos.x < destinationShipPos.x)
            {
                thrusters.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                if (!thrustersEffect.isPlaying) { thrustersEffect.Play(); }
            }
            //moving perfectly up or down (unlikely on mobile, but still worth implementing)
            else
            {
                if (thrustersEffect.isPlaying) { thrustersEffect.Stop(); }
            }
            //move ship towards touch location
            transform.Translate((destinationShipPos - currentShipPos) * Time.deltaTime * movementSpeed * mobileSpeedModifier * movementSpeedUpgrade);
        }
        //attack code
        shotTimer += Time.deltaTime;
        if (shotTimer >= shotFrequency / shotFrequencyUpgrade)
        {
            if (Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
            {
                Fire();
                shotTimer -= shotTimer;
            }
        }
        //bomb attack code
        if (bombs > 0 && ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Keypad0))
            || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && Time.time < lastTouchTime + .5f)))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                enemy.SendMessage("TakeDamage", 100f);
            }
            bombs--;
            audioPlayer.PlayOneShot(bombSound, .5f);
            bombActivationEffect.Play();
            gameplayHandler.SendMessage("UpdateBombTally", bombs);
            gameplayHandler.SendMessage("UpdateBombDeployTally");
        }
        //log time of last touch for mobile - this is used to judge double taps
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            lastTouchTime = Time.time;
        }

    }

    // Fire bullets
    void Fire()
    {
        //give bullets coords relative to ship and instantiate
        Vector3 bulletCoords = transform.position;
        bulletCoords.y += .7f;
        GameObject mainBullet = Instantiate(bullet, bulletCoords, Quaternion.identity);
        if (blastros > 0){
            bulletCoords = transform.position;
            bulletCoords.y += .15f;
            bulletCoords.x -= .3f;
            GameObject leftBullet = Instantiate(bullet, bulletCoords, Quaternion.identity);
            if (blastros > 1)
            {
                bulletCoords.x += .6f;
                GameObject rightBullet = Instantiate(bullet, bulletCoords, Quaternion.identity);
                //increase movement speed for powerups beyond 2. Commented out in favor of frequency instead of speed
                /*if(blastros > 2)
                {
                    mainBullet.SendMessage("EnhanceSpeed", blastros);
                    leftBullet.SendMessage("EnhanceSpeed", blastros);
                    rightBullet.SendMessage("EnhanceSpeed", blastros);
                }*/
            }
        }
        
    }

    public void UpgradeBullets()
    {
        blastros++;
        if(blastros > 2)
        {
            shotFrequencyUpgrade++;
        }
    }

    public void UpgradeMovementSpeed()
    {
        movementSpeedUpgrade = movementSpeedUpgrade < 2.5f ? movementSpeedUpgrade + .5f : movementSpeedUpgrade;
    }

    //acquire bomb
    public void AcquireBomb()
    {
        bombs = (bombs < 3) ? bombs + 1 : bombs;
        gameplayHandler.SendMessage("UpdateBombTally", bombs);
    }

    //take damage and potentially destroy ship
    public void TakeDamage(float amount)
    {
        if (!invincible && (hitPoints -= amount) <= 0) //short circuit the hitpoint deduction is invincible is true
        {
            //todo: add sound effect and animation
            gameplayHandler.SendMessage("UpdateBombTally", 0, SendMessageOptions.RequireReceiver);
            GameObject.Find("Spawner").SendMessage("SpawnShipDebris", transform.position, SendMessageOptions.DontRequireReceiver);
            audioPlayer.PlayOneShot(shipDeathSound, 1f);
            Destroy(gameObject);
            gameplayHandler.SendMessage("UpdateHighestBlastroLevel", blastros, SendMessageOptions.RequireReceiver);
            gameplayHandler.SendMessage("LoseLife");
        }
    }

    public void ExtraLife()
    {
        gameplayHandler.SendMessage("ExtraLife");
    }

    private IEnumerator ShieldUp()
    {
        invincible = true;
        shipShield.SetActive(true);
        float shieldLifeTime = 3f;
        shipShield.SendMessage("SetLifetime", shieldLifeTime);
        yield return new WaitForSeconds(shieldLifeTime);
        shipShield.SetActive(false);
        invincible = false;
    }
}
