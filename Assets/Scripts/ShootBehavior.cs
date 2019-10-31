using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehavior : MonoBehaviour {
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown ("space")) {
            ShootBullet ();
        }
    }

    void ShootBullet () {
        GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D> ().AddForce (transform.right * 10.0f, ForceMode2D.Impulse);
    }
}