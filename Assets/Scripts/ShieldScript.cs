using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    private Color shieldBlue = new Color(90f/255f, 220f/255f, 255f/255f);
    private Color shieldPink = new Color(230f/255f, 90f/255f, 255f/255f);
    private Color lerpColor;
    private float alphaLevel = 1.3f; //alpha is above max alpha level to give some buffer room before beginning vanishing
    private float shieldLifeTime = 3f;
    private SpriteRenderer shieldRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lerpColor = shieldBlue;
        shieldRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lerpColor = Color.Lerp(shieldBlue, shieldPink, Mathf.PingPong(Time.time, 1));
        alphaLevel -= 1.3f * Time.deltaTime / shieldLifeTime;
        lerpColor.a = alphaLevel;
        shieldRenderer.color = lerpColor;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.tag == "Enemy")
        {
            otherCollider.gameObject.SendMessage("TakeDamage", 100f);
        }
    }

    public void SetLifetime(float lifeTime)
    {
        shieldLifeTime = lifeTime;
    }
}
