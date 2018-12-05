using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed;         // player movement
    Animator anim;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    public GameObject sword;
    public float thrustPower;   // will control speed of the sword
    public bool canMove;
    public bool canAttack;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        getHealth();
        canMove = true;
        canAttack = true;
	}

    void getHealth() {
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= currentHealth - 1; i++) {
            hearts[i].gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            currentHealth--;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentHealth++;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        getHealth();
    }

    void Attack() {

        if(!canAttack) {
            return;
        }
        canMove = false;
        canAttack = false;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation);
        newSword.GetComponent<Sword>().special = true; // This accesses Sword script
        if(currentHealth == maxHealth) {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 500;
        }
        #region //SwordRotation
        int swordDir = anim.GetInteger("dir");
        anim.SetInteger("attackDir", swordDir);
        if(swordDir == 0) {
            newSword.transform.Rotate(0, 0, 0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if(swordDir == 1) {
            newSword.transform.Rotate(0, 0, 180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        else if(swordDir == 2) {
            newSword.transform.Rotate(0, 0, 90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        else if(swordDir == 3) {
            newSword.transform.Rotate(0, 0, -90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
        #endregion
    }

    void Movement() {
        if(!canMove) {
            return;
        }
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 0);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 1);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 2);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 3);
            anim.speed = 1;
        }
        else {
            anim.speed = 0;
        }
    }
}
