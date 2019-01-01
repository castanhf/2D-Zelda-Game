using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{

    Animator anim;
    public Transform rewardPosition;
    public GameObject potion;
    public float speed;
    public int dir;
    float dirTimer = 1f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;
    float specialTimer = 0.5f;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        shouldChange = false;
    }

    // Update is called once per frame
    void Update() { 
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            SpecialAttack();
            SpecialAttack();
            specialTimer = 0.5f;
        }
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 1f;
            switch (dir) {
                case 1: dir = 0;
                    break;
                case 2: dir = 1;
                    break;
                case 3: dir = 2;
                    break;
                case 0: dir = 3;
                    break;
                default: dir = 1;
                    break;
            }
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
    }

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;
        if (dir == 0)
        {         // up
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (dir == 1)
        {    // left
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        if (dir == 2)
        {         // down
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        if (dir == 3)
        {         // right
            GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }

    void Movement()
    {
        if (dir == 0)
        { // Moving up
            transform.Translate(0, speed * Time.deltaTime, 0); // moving down with a certain speed
            anim.SetInteger("dir", dir);
            //spriteRenderer.sprite = facingDown;
        }
        else if (dir == 1)
        { // Moving left
            transform.Translate(-speed * Time.deltaTime, 0, 0); // moving left with a certain speed
            anim.SetInteger("dir", dir);
            //spriteRenderer.sprite = facingLeft;
        }
        else if (dir == 2)
        { // Moving down
            transform.Translate(0, -speed * Time.deltaTime, 0); // moving down with a certain speed
            //spriteRenderer.sprite = facingUp;
            anim.SetInteger("dir", dir);
        }
        else if (dir == 3)
        { // Moving right
            transform.Translate(speed * Time.deltaTime, 0, 0); // moving right with a certain speed
            //spriteRenderer.sprite = facingRight;
            anim.SetInteger("dir", dir);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            col.gameObject.GetComponent<Sword>().createParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Instantiate(potion, rewardPosition.position, potion.transform.rotation);
                Destroy(gameObject);
            }   
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //health--; // Dragon loses one point of health
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
            if (shouldChange)
                return;

            if (dir == 0)
            {
                dir = 2;
            }
            else if (dir == 1)
            {
                dir = 3;
            }
            else if (dir == 3)
            {
                dir = 1;
            }
            else if (dir == 2)
            {
                dir = 0;
            }
            shouldChange = true;
        }
    }

    void SpecialAttack() {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        int RandomDir = Random.Range(0, 4);
        switch (RandomDir) {
            case 0: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);    // right
                break;
            case 1: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);       // up
                break;
            case 2: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * - thrustPower);  // left
                break;
            case 3: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * - thrustPower);       // up
                break;
        }
    }

}
