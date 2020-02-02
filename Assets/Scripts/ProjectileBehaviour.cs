using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject powerUp = null;
    [SerializeField] private AudioClip oh = null;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "NANGE")
        {            
            AudioPlayer.Instance.PlayOneShot(oh);
            //Instantiate(powerUp, col.transform.position, Quaternion.identity);
            Debug.Log("ASD");
            CameraShake.instance.Shake(0.1f,0.5f);
            Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
    }
}