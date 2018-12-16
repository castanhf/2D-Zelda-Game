using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

    Animator anim;
    public float speed;
    int dir;
    float dirTimer = 0.7f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        dir = Random.Range(0, 4);
	}
	
	// Update is called once per frame
	void Update () {
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0) {
            dirTimer = 0.7f;
            dir = Random.Range(0, 4);
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
	}

    void Attack() {
        if (!canAttack) {
            return;
        }
        canAttack = false;
        if (dir == 0) {         // up
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (dir == 1) {    // left
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        if (dir == 2) {         // down
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        if (dir == 3) {         // right
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }

    void Movement() {
        if (dir == 0) { // Moving up
            transform.Translate(0, speed * Time.deltaTime, 0); // moving down with a certain speed
            anim.SetInteger("dir", dir);
            //spriteRenderer.sprite = facingDown;
        }
        else if (dir == 1) { // Moving left
            transform.Translate(-speed * Time.deltaTime, 0, 0); // moving left with a certain speed
            anim.SetInteger("dir", dir);
            //spriteRenderer.sprite = facingLeft;
        }
        else if (dir == 2) { // Moving down
            transform.Translate(0, -speed * Time.deltaTime, 0); // moving down with a certain speed
            //spriteRenderer.sprite = facingUp;
            anim.SetInteger("dir", dir);
        }
        else if (dir == 3) { // Moving right
            transform.Translate(speed * Time.deltaTime, 0, 0); // moving right with a certain speed
            //spriteRenderer.sprite = facingRight;
            anim.SetInteger("dir", dir);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Sword") {
            health--;
            if (health <= 0) {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            col.gameObject.GetComponent<Sword>().createParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            health--; // Dragon loses one point of health
            if (!col.gameObject.GetComponent<Player>().iniFrame)
            {
                col.gameObject.GetComponent<Player>().currentHealth--; // player loses one point of health
                col.gameObject.GetComponent<Player>().iniFrame = true; // 
            }
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (col.gameObject.tag == "Wall")
        {
            dir = Random.Range(0, 3);
        }
    }
}
