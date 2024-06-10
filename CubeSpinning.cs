using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpinning : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _rotationCenter;
    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - _rotationCenter.position;
    }

    private void Update()
    {
        transform.position = _rotationCenter.position + _offset;
        transform.RotateAround(_rotationCenter.position, Vector3.up, _speed * Time.deltaTime);
        transform.Rotate(Vector3.up * _speed * Time.deltaTime);
        _offset = transform.position - _rotationCenter.position;
    }
}
