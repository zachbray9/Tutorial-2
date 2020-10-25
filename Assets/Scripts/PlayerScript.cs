using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue;
    public Text winText;
    private int lives;
    public Text livesText;
    public Text loseText;
    private bool facingRight = true;
    private bool isJumping = false;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        lives = 3;
        scoreValue = 0;
        setScoreText();
        winText.text = "";
        setLivesText();
        loseText.text = "";

        musicSource.clip = musicClipOne;
        musicSource.loop = true;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));


        if(hozMovement < 0)
        {
            anim.SetInteger("State", 1);

            if(facingRight == true)
            {
                Flip();
            }

        }
        else if(hozMovement > 0)
        {
            anim.SetInteger("State", 1);
            
            if(facingRight == false)
            {
                Flip();
            }
        }
        else
        {
            anim.SetInteger("State", 0);
        }

        if(isJumping){
            anim.SetInteger("State", 2);
        }

        

        if(Input.GetKey("escape")){
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            setScoreText();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
            transform.position = new Vector3(35f, 3f, 0.0f);
            lives = 3;
            setLivesText();
            }
        }
        else if(collision.collider.tag == "Enemy")
        {
            lives -= 1;
            setLivesText();
            Destroy(collision.collider.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isJumping = false;

            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    void setScoreText()
    {
        score.text = "Score: " + scoreValue.ToString();

        if(scoreValue >= 8)
        {
            winText.text = "You win! Game created by Zach Bray";
            musicSource.clip = musicClipTwo;
            musicSource.loop = false;
            musicSource.Play();
            Destroy(this);
        }
    }

    void setLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();

        if(lives <= 0)
        {
            loseText.text = "You Lose";
            Destroy(this);
        }
    }

    void Flip()
    {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }
}
