using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logoUIMaster : MonoBehaviour
{
    [SerializeField] private GameObject mainName = null;
    [SerializeField] private GameObject[] rest = null;
    [SerializeField] private AudioClip shot, fullshot = null;
    [SerializeField] private float interval = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject r in rest)
            r.SetActive(false);
        mainName.SetActive(false);
        StartCoroutine(LogoDisplayInSequence());
    }

    IEnumerator LogoDisplayInSequence()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);
        mainName.SetActive(true);
        AudioPlayer.Instance.PlayOneShot(shot);
        foreach (GameObject r in rest)
        {
            yield return wait;
            AudioPlayer.Instance.PlayOneShot(shot);
            r.SetActive(true);
        }

        yield return wait;
        foreach (GameObject r in rest)
        {
            r.SetActive(false);
        }
        AudioPlayer.Instance.PlayOneShot(fullshot);
        for (int i = 0; i < 10; i++)
        {
            mainName.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
