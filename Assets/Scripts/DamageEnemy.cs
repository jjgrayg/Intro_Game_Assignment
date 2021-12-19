using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{

    public int damage;
    public float speed;

    private int direction_ = -1;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(direction_);
        if (direction_ == 0)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
        else if (direction_ == 1)
        {
            transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<basic_enemy>().Damage(damage);
        }
        else if (collision.tag == "Player") return;
        Destroy(this.transform.gameObject);
    }

    public void SetDirection(int d)
    {
        direction_ = d;
    }
}
