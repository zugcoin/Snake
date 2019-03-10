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

        //Player wants to rotate the Snake
        if (typeMvt != 0)
        {

            //Only rotate every unit
            if (System.Math.Round(transform.position.z, 2) % 1 == 0 && System.Math.Round(transform.position.x, 2) % 1 == 0)
            {
                if (typeMvt != 0)
                {
                    switch (typeMvt)
                    {
                        case -1:
                            transform.RotateAround(transform.position, Vector3.up, -90);
                            break;
                        case 1:
                            transform.RotateAround(transform.position, Vector3.up, 90);
                            break;
                    }
                    typeMvt = 0;
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Food"))
        {

        }
        Debug.Log("Collider:" + other.name + " " + transform.name);
    }

}
