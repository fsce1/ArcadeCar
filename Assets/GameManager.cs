using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public CarController car;
    public TMP_Text flipText;
    public TMP_Text MphText;

    void Update()
    {
        if(!car.willDoSuspension && car.transform.up.y <0)
        {
            flipText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                car.transform.position += Vector3.up;
                car.transform.eulerAngles = Vector3.zero;
            }
        }
        else
        {
            flipText.gameObject.SetActive(false);
        }

        MphText.text = Mathf.RoundToInt(car.wheels[1].wheelVelocityLS.z * 2.23694f) + " MPH";


    }
}
