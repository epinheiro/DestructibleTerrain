using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPlayerController : MonoBehaviour
{
    [Range(0,10)] public float gravityForce = 1;

    public GameObject ScenarioReference;

    Transform nextPosition;

    Vector3 GravityUpdate(){
        Vector3 position = this.transform.position;
        return new Vector3(position.x, position.y - (gravityForce * Time.deltaTime), position.z);
    }

    bool isMovimentValid(Vector3 moveTo){
        Vector3 position = this.transform.position;

        Debug.Log((Vector2)position + string.Format(" to ({0}, {1})", moveTo.x, moveTo.y));

        bool condition = ScenarioReference.GetComponent<ShaderScenarioController>().isPositionFree(position);

        return condition;
    }

    void move(Vector3 moveTo){
        this.transform.position = moveTo;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveTo = GravityUpdate();

        if(isMovimentValid(moveTo)){
            move(moveTo);
        }
        
    }
}
