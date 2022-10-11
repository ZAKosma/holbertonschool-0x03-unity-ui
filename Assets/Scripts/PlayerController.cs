using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public int health = 5;
    
    public float speed;

    public float teleporterDelay = 3;
    private bool isTeleported;
    
    public GameObject[] teleporters;

    private int score = 0;
    
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        health = 5;
        score = 0;

        isTeleported = false;
    }

    private void Update()
    {
        if (health == 0)
        {
            Debug.Log("Game over!");

            SceneManager.LoadScene(0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //Move up negative in x
            playerMovement.z += speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerMovement.z -= speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerMovement.x -= speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerMovement.x += speed;
        }

        rb.AddForce(playerMovement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            score++;
            Debug.Log("Score: " + score);
            GameObject.Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            health--;
            Debug.Log("Health: " + health);
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }
        else if (other.gameObject.CompareTag("Teleporter"))
        {
            if (!isTeleported)
            {
                if (teleporters[0] == other.gameObject)
                {
                    transform.position = teleporters[1].transform.position;
                }
                else
                {
                    transform.position = teleporters[0].transform.position;
                }

                //isTeleported = true;
                StartCoroutine("TeleporterCooldown");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Teleporter"))
        {
            isTeleported = false;
        }
    }

    IEnumerator TeleporterCooldown()
    {
        isTeleported = true;
        yield return new WaitForSeconds(teleporterDelay);
        isTeleported = false;
    }
}
