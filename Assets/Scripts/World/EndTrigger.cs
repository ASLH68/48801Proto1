using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] GameObject _camPanning;
    [SerializeField] ParticleSystem[] _eruption;
    [SerializeField] GameObject _endZones;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndTrigger"))
        {
            StartCoroutine(StartVolcano());
            _camPanning.SetActive(true);
            Destroy(_endZones);
        }
    }

    private IEnumerator StartVolcano()
    {
        yield return new WaitForSeconds(4.5f);

        foreach(ParticleSystem syst in _eruption)
        {
            syst.Play();
        }
    }
}
