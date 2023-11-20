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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isOpen = !isOpen;
        }
        Vector3 tgt;
        Vector3 cur;
        if (isOpen)
        {
            tgt = new(closedRot, 0, 0);
            cur = new(openRot, 0, 0);
            //for (int i = 0; i < lights.Length; i++) lights[i].enabled = true;
        }
        else
        {
            tgt = new(openRot, 0, 0);
            cur = new(closedRot, 0, 0);
            //for (int i = 0; i < lights.Length; i++) lights[i].enabled = false;
        }
        for (float i = 0; i <= 100; i++)
        {
            i /= 100;
            transform.localEulerAngles = Vector3.Lerp(cur, tgt, i);
        }
    }
}
