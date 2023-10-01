using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    public float fadeTime = 1;
    private SpriteRenderer sr;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 1 - ((Time.time - startTime) / fadeTime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (Time.time > startTime + fadeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
