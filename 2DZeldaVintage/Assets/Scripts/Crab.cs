using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 1f; // two seconds
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingRight;
    public Sprite facingLeft;
    float changeTimer = .2f;
    bool shouldChange;
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // spriteRenderer.sprite = facingUp;
        direction = Random.Range(0, 4);
        shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 1.5f;
            direction = Random.Range(0, 4); // random number from 0 to 3
        }
        Movement();

        if (shouldChange) {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0) {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
	}

    void Movement() {
        if (direction == 0) {
            transform.Translate(0, -speed * Time.deltaTime, 0); // moving down with a certain speed
            spriteRenderer.sprite = facingDown;
        }
        else if (direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0); // moving left with a certain speed
            spriteRenderer.sprite = facingLeft;
        }
        else if (direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0); // moving right with a certain speed
            spriteRenderer.sprite = facingRight;
        }
        else if (direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0); // moving up with a certain speed
            spriteRenderer.sprite = facingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Sword") {
            health--;
            if(health <= 0) {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            col.GetComponent<Sword>().createParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {

            health--; // crab loses one point of health
            if (!col.gameObject.GetComponent<Player>().iniFrame) {
                col.gameObject.GetComponent<Player>().currentHealth--; // player loses one point of health
                col.gameObject.GetComponent<Player>().iniFrame = true; // 
            }
            if (health <= 0) {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        if (col.gameObject.tag == "Wall") {
            
            if (shouldChange == true) {
                return;
            }

            if (direction == 0) {
                direction = 3;
            }
            else if (direction == 1) {
                direction = 2;
            }
            else if (direction == 2) {
                direction = 1;
            }
            else if (direction == 3) {
                direction = 0;
            }
            shouldChange = true;
        }
    }
}
