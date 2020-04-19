using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBackgroundRepeater : MonoBehaviour
{
    [SerializeField] private Transform bg1 = null;
    [SerializeField] private Transform bg2 = null;
    [SerializeField] private Vector2 centerPoint = Vector2.zero;

    private Vector3 mainbgPos = Vector3.zero;
    private Vector3 secondbgPos = Vector3.zero;
    private Transform currentLastBg = null;
    private Vector2 movePoint = Vector2.zero;

    private void Awake()
    {
        movePoint = bg2.position;
    }

    private void Update()
    {
        if (Vector2.Distance(bg2.position, centerPoint) < 0.5f)
        {
            bg1.position = movePoint;
        }

        if (Vector2.Distance(bg1.position, centerPoint) < 0.5f)
        {
            bg2.position = movePoint;
        }
    }
}
