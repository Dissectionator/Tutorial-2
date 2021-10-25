using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public GameObject winText;
     public GameObject livesText;
    public Text win;
    public Text lives;
    private int scoreValue = 0;
    private int livesValue = 3;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;
    private SpriteRenderer renderer_;
    Animator anim;
    void Start()
    {
        renderer_ = GetComponent<SpriteRenderer>();
        if (renderer_ == null){
        Debug.LogError("Player Sprite is missing a renderer");
        }
        anim = GetComponent <Animator>();
        musicSource.clip = musicClipOne;
        musicSource.loop = true;
        musicSource.Play();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKey("escape")){
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.D)){
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D)){
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A)){
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A)){
            anim.SetInteger("State", 0);
        }
    }   

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        
        if (livesValue > 0 && scoreValue < 8){
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        }
        if (Input.GetAxisRaw("Horizontal") > 0){
            renderer_.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0){
            renderer_.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetCountText();
            Destroy(collision.collider.gameObject);
        }
        
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
        }
    }

    void SetCountText()
    {
        if(livesValue > 0 && scoreValue <= 8){
            score.text = "Score: " + scoreValue.ToString();
        }

        if (scoreValue == 4 && livesValue > 0){
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
            transform.position = new Vector2(13, 22);
        }

        if (scoreValue == 8 && livesValue > 0){
            winText.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
        }
    }

    void SetLivesText()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0)
        {
            speed = 0;
            transform.position = new Vector2(3, 2);
            score.text = "";
            lives.text = "";
            win.text = "You Lose";
            winText.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W) && livesValue > 0 && scoreValue < 8)
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors. You can also create a public variable for it and then edit it in the inspector.
                anim.SetTrigger("Jump");         
            }
            anim.SetBool("Ground", true);
        }
    }
}

