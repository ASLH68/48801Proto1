using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LavaSign : MonoBehaviour
{
    public GameObject lavaBeans;
    public TextMeshPro signText;
    void Awake()
    {
        lavaBeans = GameObject.Find("Level/Lava/Beans");
    }

    void Update()
    {
        //Debug.Log(lavaBeans.gameObject.GetComponent<Lava>().lavaHeight.ToString());
        signText.text = "Lava height: " + lavaBeans.gameObject.GetComponent<Lava>().lavaHeight.ToString();
    }
}
