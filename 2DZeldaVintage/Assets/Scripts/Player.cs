using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed;         // player movement
    Animator anim;
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    public GameObject sword;
    public float thrustPower;   // will control speed of the sword
    public bool canMove;
    public bool canAttack;
    public bool iniFrame;
    SpriteRenderer sr;
    float iniTimer = 1f; // 1 second

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

        // Checks if the current scene index is 0, if it is, reset player prefs.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.DeleteAll();
        }

        // Sets player prefs
        if (!PlayerPrefs.HasKey("maxHealth"))
        {
            maxHealth = 2;
            currentHealth = maxHealth;
            SaveGame();
        }
        LoadGame(); // Loads the stats
        getHealth();
        canMove = true;
        canAttack = true;
        iniFrame = false;
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            Attack();
        }
        #region // Master code - get/lose health - P or L keyButtons. Also, minimum health exception handling
        if (Input.GetKeyDown(KeyCode.P)) {
            currentHealth--;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentHealth++;
        }
        if (currentHealth <= 0)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        #endregion

        // If we get hurt, we become invincible for 1 second
        if (iniFrame == true) {
            iniTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            // Character is flickering
            if (rn < 50) sr.enabled = false;
            if (rn > 50) sr.enabled = true;
            if (iniTimer <= 0) {
                iniTimer = 1f;
                iniFrame = false;
                sr.enabled = true;
            }
        }
        getHealth();
    }

    void getHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= currentHealth - 1; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

    void Attack() {

        if(!canAttack) {
            return;
        }
        canMove = false;
        canAttack = false;
        thrustPower = 250;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation);
        // This accesses Sword script but it is not necessary
        // newSword.GetComponent<Sword>().special = true;
        if (currentHealth == maxHealth) {
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

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "EnemyBullet") {
            if (!iniFrame) {
                iniFrame = true;
                currentHealth--;
                // Upgrade function will take care of this afterwards
            }
            col.gameObject.GetComponent<Bullet>().CreateParticle();
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Potion") {
            currentHealth = maxHealth;
            Destroy(col.gameObject);
            if (maxHealth >= 5) {
                return;
            }
            maxHealth++;
            currentHealth = maxHealth;
        }
    }

    public void SaveGame() {
        PlayerPrefs.SetInt("maxHealth", maxHealth);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    void LoadGame() {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");
    }

}
