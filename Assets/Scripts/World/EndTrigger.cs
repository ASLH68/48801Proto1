using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] GameObject _camPanning;
    [SerializeField] ParticleSystem[] _eruption;
    [SerializeField] GameObject _endZones;
    [SerializeField] GameObject _displayMessage;

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
        StartCoroutine(EndMessage());
    }

    private IEnumerator EndMessage()
    {
        yield return new WaitForSeconds(1.5f);
        _displayMessage.SetActive(true);
    }
}
