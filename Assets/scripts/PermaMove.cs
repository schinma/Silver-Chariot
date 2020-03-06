using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermaMove : MonoBehaviour
{
    //gameManager's global speed
    public float _speed = 2f;

    private Vector3 _moveHorizontal;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Update speed depending on number of allies
        _speed = 2 + Mathf.Log(GameManager.identity.stat.rescued);
        _moveHorizontal = transform.right * _speed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.Translate(_moveHorizontal);
    }
}
