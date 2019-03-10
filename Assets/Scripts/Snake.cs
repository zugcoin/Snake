using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    public Transform prefabBody;

    int typeMvt = 0;
	// Use this for initialization
	void Start () {
		
	}

    void InitBody() { }

    void InitTrail() { }

    void CreateBody()
    {

    }

	// Update is called once per frame
	void Update () {
        //turn left
        if (Input.GetKeyDown(KeyCode.Q)) {
            typeMvt = -1;
        }

        //Turn right
        if (Input.GetKeyDown(KeyCode.W)) {
            typeMvt = 1;
        }
        Movement();
    }

    void Movement()
    {
        //Snake movement
        transform.position = transform.position + transform.forward * 0.1f;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Food"))
        {

        }
        Debug.Log("Collider:" + other.name + " " + transform.name);
    }

}
