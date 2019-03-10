using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyParts
{
    public Transform trans;
    public Vector3 nextPos;
    public int nextMvt;
}

public class Snake : MonoBehaviour
{
    public Transform prefabBody, prefabTail;
    List<BodyParts> listParts = new List<BodyParts>();
    float speed = 0.1f;
    int typeMvt = 0;
    // Use this for initialization
    void Start()
    {
        InitTrail();

    }

    void InitBody()
    {

    }

    void InitTrail()
    {
        Vector3 tailPos = transform.position - transform.forward * (listParts.Count + 1);
        Transform trans = Instantiate(prefabTail, tailPos, Quaternion.identity);
        BodyParts bodyPart = new BodyParts();
        trans.name = "tail";
        bodyPart.trans = trans;
        listParts.Add(bodyPart);
    }

    void CreateBody()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //turn left
        if (Input.GetKeyDown(KeyCode.Q))
        {
            typeMvt = -1;
        }

        //Turn right
        if (Input.GetKeyDown(KeyCode.W))
        {
            typeMvt = 1;
        }
        Movement();
    }

    void Movement()
    {
        //Snake movement
        transform.position = transform.position + transform.forward * speed;

        //Snake body and tail movement
        for (int i = 0; i < listParts.Count; i++)
        {
            listParts[i].trans.position = listParts[i].trans.position + listParts[i].trans.forward * speed;
        }

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
