using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public static RoadController Instance;
    [SerializeField] private GameObject road;

    public bool isDisable;
    private void Awake()
    {
        Instance = this;
        isDisable = true;
    }

    private void Start()
    {
        road = gameObject;
    }

    void FixedUpdate()
    {
        if(isDisable) return;
        var transformPosition = road.transform.position;
        road.transform.position = new Vector3(transformPosition.x, transformPosition.y,
            transformPosition.z -= 0.1f);
    }
}
