using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHandlerScript : MonoBehaviour
{

    //Globals
    private int lives = 1;
    private int score = 0;
    private int highestBlastroLevel = 0;
    private int bombDeployTally = 0;
    private double flightStartTime;
    private int highScore;

    public GameObject playerShip;
    private GameObject canvas;
    private UnityEngine.UI.Text scoreText;
    public GameObject spawner;
    private ParticleSystem.MainModule starParticleSystem;
    public GameObject uiBombSprite;
    public GameObject uiLifeSprite;

    //UI containers set manually in editor
    public GameObject gameplayUIContainer;
    public GameObject gameOverMenuContainer;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        scoreText = GameObject.Find("ScoreText").GetComponent<UnityEngine.UI.Text>();
        starParticleSystem = GameObject.Find("starBGEffect").GetComponent<ParticleSystem>().main;
        UpdateLifeTally();
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        //begin coroutine to blink "warning" text to ready player for gameplay
        StartCoroutine("GameplayCountdown");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        //adjust background star speed if ship is going forward or back
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            starParticleSystem.simulationSpeed = 1.5f;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            starParticleSystem.simulationSpeed = .7f;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)
            || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            starParticleSystem.simulationSpeed = 1f;
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void ExtraLife()
    {
        lives = (lives < 3) ? lives + 1 : lives;
        UpdateLifeTally();
    }

    public void LoseLife()
    {
        lives--;
        UpdateLifeTally();
        //todo: if lives = 0, end game
        if (lives > 0)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //play game over music
        GameObject.Find("AudioPlayer").SendMessage("PlayGameOverMusic");
        //update high score from file
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            highScore = score;
        }
        //hide gameplay ui
        gameplayUIContainer.SetActive(false);
        //show gameover ui
        gameOverMenuContainer.SetActive(true);
        //add stats to gameover ui elements
        GameObject.Find("FinalScoreTotal").GetComponent<UnityEngine.UI.Text>().text = "" + score;
        GameObject.Find("HighScoreTotal").GetComponent<UnityEngine.UI.Text>().text = "" + highScore;
        GameObject.Find("BlastroLevelTotal").GetComponent<UnityEngine.UI.Text>().text = "" + highestBlastroLevel;
        GameObject.Find("BombsDeployedTotal").GetComponent<UnityEngine.UI.Text>().text = "" + bombDeployTally;
        GameObject.Find("FlightTimeTotal").GetComponent<UnityEngine.UI.Text>().text = "" + System.Math.Round(Time.time - flightStartTime, 2) + "s";
        //disable spawner to cease gameplay
        spawner.SetActive(false);
    }

    //update display for number of bombs
    public void UpdateBombTally(int newBombCount)
    {
        GameObject[] uiBombs = GameObject.FindGameObjectsWithTag("UIBombSprite");
        foreach (GameObject uiElement in uiBombs)
        {
            Destroy(uiElement);
        }
        for (int i = 0; i < newBombCount; i++)
        {
            GameObject newUiElement = GameObject.Instantiate(uiBombSprite, canvas.transform, false);
            newUiElement.transform.localPosition = new Vector2(100f + (50f * i), -380f);
        }
    }

    public void UpdateLifeTally()
    {
        GameObject[] uiLives = GameObject.FindGameObjectsWithTag("UILifeSprite");
        foreach (GameObject uiElement in uiLives)
        {
            Destroy(uiElement);
        }
        for (int i = 0; i < lives; i++)
        {
            GameObject newUiElement = GameObject.Instantiate(uiLifeSprite, canvas.transform, false);
            newUiElement.transform.localPosition = new Vector2(-100f - (50f * i), -380f);
        }
    }

    public void UpdateHighestBlastroLevel(int blastroLevel)
    {
        highestBlastroLevel = highestBlastroLevel < blastroLevel ? blastroLevel : highestBlastroLevel;
    }

    public void UpdateBombDeployTally()
    {
        bombDeployTally++;
    }

    IEnumerator Respawn()
    {
        spawner.SendMessage("ReduceDifficultyModifier");
        yield return new WaitForSeconds(1f);
        Instantiate(playerShip, new Vector3(0, -3.5f, 0), Quaternion.identity);
    }

    IEnumerator GameplayCountdown()
    {
        //get hotseat text
        UnityEngine.UI.Text hotseatText = GameObject.Find("WarningHotseatText").GetComponent<UnityEngine.UI.Text>();
        //modulate text opacity back and forth
        hotseatText.CrossFadeAlpha(0f, .7f, false);
        yield return new WaitForSeconds(.7f);
        for (int i = 3; i > 0; i--)
        {
            hotseatText.text = "Warning!\nApproaching astrobelt\n" + i;
            hotseatText.CrossFadeAlpha(1f, .7f, false);
            yield return new WaitForSeconds(.7f);
            hotseatText.CrossFadeAlpha(0f, .7f, false);
            yield return new WaitForSeconds(.7f);    
        }
        //activate spawner to begin gameplay
        spawner.SetActive(true);
        flightStartTime = Time.time;
    }
}
