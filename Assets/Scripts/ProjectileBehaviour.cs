﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
    [SerializeField] private GameObject powerUp = null;
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerEnter2D (Collider2D col) {
        if (col.gameObject.tag == "NANGE") {
            Instantiate (powerUp, col.transform.position, Quaternion.identity);
            Destroy (col.gameObject);
            Destroy (this.gameObject);
        }
    }
}