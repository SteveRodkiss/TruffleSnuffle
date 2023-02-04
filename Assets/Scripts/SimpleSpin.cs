using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpin : MonoBehaviour
{

    public float spinSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
    }
}
