using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction = 0;
    float timer = 1.5f; // two seconds
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingRight;
    public Sprite facingLeft;
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = facingUp;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 1.5f;
            direction = Random.Range(0, 3); // random number from 0 to 3
        }
        Movement();
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
            transform.Translate(0, speed * Time.deltaTime, 0); // moving down with a certain speed
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
            Destroy(col.gameObject);
        }
    }
}
