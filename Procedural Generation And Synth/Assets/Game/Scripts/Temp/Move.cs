using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1f), _speed * Time.deltaTime);
        }
    }
}
