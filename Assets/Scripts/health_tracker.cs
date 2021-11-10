using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_tracker : MonoBehaviour
{

    public platformer Player;
    public Texture2D fullHeart;
    public Texture2D halfHeart;
    public Texture2D emptyHeart;
    int lastHealth;


    // Start is called before the first frame update
    void Start()
    {
        lastHealth = Player.getHealth();
        ProduceHearts();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHealth != Player.getHealth())
        {
            ProduceHearts();
            lastHealth = Player.getHealth();
        }
    }

    // Control the spawning of player hearts
    void ProduceHearts()
    {

        foreach(Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        int numTotalHearts = Player.getCurrentMaxHealth() / 2;
        int numFullHearts = (int)Math.Floor((double)(Player.getHealth() / 2));
        int numHalfHearts = Player.getHealth() % 2;
        int numEmptyHearts = numTotalHearts - (numFullHearts + numHalfHearts);
        int heartIndex = 0;

        for (int i = 0; i < numFullHearts; ++i)
        {
            GameObject temp = new GameObject("FullHeart" + i, typeof(RawImage), typeof(RectTransform));
            temp.transform.SetParent(GameObject.Find("HealthInfo").transform);
            temp.GetComponent<RawImage>().texture = fullHeart;
            temp.GetComponent<RectTransform>().position = temp.transform.parent.GetComponent<RectTransform>().position + new Vector3(-236 + (50 * heartIndex), temp.transform.position.y, temp.transform.position.z);
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            heartIndex++;
        }

        for (int i = 0; i < numHalfHearts; ++i)
        {
            GameObject temp = new GameObject("HalfHeart" + i, typeof(RawImage), typeof(RectTransform));
            temp.transform.SetParent(GameObject.Find("HealthInfo").transform);
            temp.GetComponent<RawImage>().texture = halfHeart;
            temp.GetComponent<RectTransform>().position = temp.transform.parent.GetComponent<RectTransform>().position + new Vector3(-236 + (50 * heartIndex), temp.transform.position.y, temp.transform.position.z);
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            heartIndex++;
        }

        for (int i = 0; i < numEmptyHearts; ++i)
        {
            GameObject temp = new GameObject("EmptyHeart" + i, typeof(RawImage), typeof(RectTransform));
            temp.transform.SetParent(GameObject.Find("HealthInfo").transform);
            temp.GetComponent<RawImage>().texture = emptyHeart;
            temp.GetComponent<RectTransform>().position = temp.transform.parent.GetComponent<RectTransform>().position + new Vector3(-236 + (50 * heartIndex), temp.transform.position.y, temp.transform.position.z);
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            heartIndex++;
        }

    }
}
