using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //private CharacterController controller;
    public GameObject bulletPrefab;
    Transform cannonHolder;

    void Start () {
        //controller = GetComponent<CharacterController> ();
        cannonHolder = transform.Find ("CannonHolder");
    }

    void Update () {
        if (Input.GetKey (KeyCode.E))
            RotateCannon (-5);
        else if (Input.GetKey (KeyCode.Q))
            RotateCannon (5);

    }

    void FixedUpdate () {
        // Vector3 move = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
        // float speed = 10.0f;
        // Debug.Log (move * Time.deltaTime * speed);
        // controller.Move (move * Time.deltaTime * speed);
    }
    void RotateCannon (float rotation) {
        cannonHolder.Rotate (new Vector3 (0, 0, rotation));
    }
}