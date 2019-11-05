using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    private bool isRunning = false;
    public GameObject player;
    public GameObject playerSprite;

    public bool Create(GameObject g, GameObject sprite)
    {
        return Create(g.transform.position, sprite.transform.eulerAngles, sprite);
    }

    public bool Create(Vector3 pos, Vector3 rot, GameObject sprite)
    {
        if (!isRunning)
        {
            GetComponent<SpriteRenderer>().sprite = sprite.GetComponent<SpriteRenderer>().sprite;
            GetComponent<SpriteRenderer>().flipX = sprite.GetComponent<SpriteRenderer>().flipX;
            transform.position = pos;
            transform.eulerAngles = rot;
            Color c = GetComponent<Renderer>().material.color;
            c.a = 1f;
            GetComponent<Renderer>().material.color = c;
            StartCoroutine("Fade");
            return true;
        }
        return false;
    }

    public void Start()
    {
        Color c = GetComponent<Renderer>().material.color;
        c.a = 0f;
        GetComponent<Renderer>().material.color = c;
    }

    private IEnumerator Fade()
    {
        isRunning = true;
        for (;;) {
            Color c = GetComponent<Renderer>().material.color;
            c.a -= 0.1f;
            if (c.a <= 0f){
                c.a = 0f;
                GetComponent<Renderer>().material.color = c;
                break;
            }
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
        isRunning = false;
    }
}
