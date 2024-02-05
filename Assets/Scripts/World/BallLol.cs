using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLol : MonoBehaviour
{
   
    void Update() {
      transform.Translate(200f * Time.deltaTime, 0, 0);
    }
}
