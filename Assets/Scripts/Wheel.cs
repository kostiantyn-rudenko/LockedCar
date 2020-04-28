using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    // Start is called before the first frame update
    private float _rotationIndex = 30f;
    private Mover _carMover;
    private void Start()
    {
        _carMover = GetComponentInParent<Mover>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_carMover == null)
        {
            Debug.Log("ERROR: no parent (car) for wheel");
            return;
        }

        if (_carMover.curSpeed != 0f)
            transform.Rotate(new Vector3(0f, 0f, _rotationIndex * _carMover.curSpeed));
    }
}
