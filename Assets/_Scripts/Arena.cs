using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public float initScale = 10f;
    public float resizeMultiplier = 0.9f;
    public GameObject UP, DOWN, LEFT, RIGHT;

    private void Start() {
        transform.localScale = Vector2.one * initScale;
    }

    private void Update() {
        if (transform.localScale.x <= 1) {
            Destroy(gameObject);
        }
    }

    private void Shrink() {
        transform.localScale *= resizeMultiplier;
    }

    private void Expand() {
        transform.localScale /= resizeMultiplier;
    }
}