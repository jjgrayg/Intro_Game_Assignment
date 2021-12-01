using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic_enemy : MonoBehaviour
{

    public float knockbackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Checking if player");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Applying force");
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;
            Debug.Log(direction);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackSpeed);
        }
    }

    void Patrol()
    {

    }
}
