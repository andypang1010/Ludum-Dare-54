using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveToMouse : MonoBehaviour
{
    public GameObject coolDown;
    private Vector3 target;

    void Update()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;

        transform.position = target;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        coolDown.transform.position = screenPos;
    }
}
