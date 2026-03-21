using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float aimSpeed;
    public float aimRange;
    private Transform player;

    public GameObject bullet;

    public AudioClip shootClip;

    public AudioSource shootSound;

    [SerializeField] private Transform turret;


    private float shootTimer;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;    
    }

    // Update is called once per frame
    void Update()
    {
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) < aimRange && Vector2.Distance(transform.position, player.position) > 3f)
            {
                Debug.Log("Player in Range");

                // Calculate the direction from the turret to the player
                Vector3 directionToPlayer = player.position - turret.position;

                // Calculate the angle in degrees from the direction to the player
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                // Apply the rotation to the turret
                turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.Euler(0f, 0f, angle + 90f), aimSpeed * Time.deltaTime);

                float angleDifference = Mathf.Abs(Mathf.DeltaAngle(turret.eulerAngles.z, angle + 90f));

                if (angleDifference <= 10f && shootTimer <= 0f)
                {   
                    shootTimer = fireRate;
                    Shoot();
                }
            }
        }
        
    }
    public void Shoot()
    {
        Instantiate(bullet, transform.position, turret.rotation);
        shootSound.Play();
    }
}
