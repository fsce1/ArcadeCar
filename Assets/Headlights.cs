using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headlights : MonoBehaviour
{
    public bool isOpen;
    public float speed;
    public float closedRot;
    public float openRot;
    public Light[] lights;
    public Vector3 start;
    public Vector3 tgt;
    private void Start()
    {
        start = transform.localEulerAngles;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isOpen = !isOpen;
            start = tgt;
        }
    }
    void FixedUpdate()
    {
        if (isOpen)
        {
            tgt = new(closedRot, 0, 0);
            foreach (Light l in lights) l.enabled = false;
        }
        else
        {
            tgt = new(openRot, 0, 0);
            foreach (Light l in lights) l.enabled = true;
        }

        if (transform.localEulerAngles == tgt)
        {
            transform.localEulerAngles += Move(start, tgt, 1);
        }
    }
    private Vector3 Move(Vector3 start, Vector3 tgt, float secs)
    {
        Vector3 diff = tgt - start;
        Vector3 move = diff * (secs / 50);
        return move;
    }


}
