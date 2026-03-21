using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    public GameObject hitEffect;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject me;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = -transform.up * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D()
    {
        Destroy(me);
    }

}
