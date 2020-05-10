using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    
    public float minimum = -1.0F;
    public float maximum = 1.0F;

    // starting value for the Lerp
    static float t = 0.0f;

    void Update()
    {
        // animate the position of the game object...
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(minimum, maximum, t), transform.position.z);

        // .. and increase the t interpolater
        t += 0.4f * Time.deltaTime;

      
        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}

