using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float damp;

    [SerializeField]
    private Transform target;
    private Vector3 _moveVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _moveVelocity, damp);
    }

    private void FixedUpdate()
    {
        Vector3 _newPos;

        _newPos.y = transform.position.y;
        _newPos.z = transform.position.z;
        _newPos.x = target.position.x;

        transform.position = _newPos;
    }
}
