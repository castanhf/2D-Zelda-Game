using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
