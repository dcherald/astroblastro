using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAstroScript : MonoBehaviour
{

    //Globals
    public Sprite[] astroSprites;

    // Start is called before the first frame update
    void Start()
    {
        Sprite randomSprite = astroSprites[Random.Range(0, astroSprites.Length - 1)];
        gameObject.GetComponent<SpriteRenderer>().sprite = randomSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
