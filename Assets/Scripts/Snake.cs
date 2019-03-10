using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenMvt
{
    public Vector3 nextPos;
    public int nextMvt;
}

public class BodyParts
{
    public Transform trans;
    public List<ChildrenMvt> listMvt = new List<ChildrenMvt>();
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
        CreateBody();
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
        Vector3 bodyPos = listParts[listParts.Count - 1].trans.position - listParts[listParts.Count - 1].trans.forward;
        Transform trans = Instantiate(prefabBody, bodyPos, listParts[listParts.Count - 1].trans.rotation);
        BodyParts bodyPart = new BodyParts();
        trans.name = "body" + (listParts.Count);
        bodyPart.trans = trans;
        listParts.Add(bodyPart);
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

    void RotateChildrens(int _typeMvt)
    {
        //For each body and taill, add next rotation and position
        for (int i = 0; i < listParts.Count; i++)
        {
            ChildrenMvt childMvt = new ChildrenMvt();
            childMvt.nextMvt = _typeMvt;
            childMvt.nextPos = transform.position;
            listParts[i].listMvt.Add(childMvt);
        }
    }


    float distance;

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
                            transform.RotateAround(transform.position, Vector3.up, -90); RotateChildrens(typeMvt);
                            break;
                        case 1:
                            transform.RotateAround(transform.position, Vector3.up, 90); RotateChildrens(typeMvt);
                            break;
                    }
                    typeMvt = 0;
                }
            }
        }


        for (int i = listParts.Count-1; i > -1; i--)
        {
            //Detect if tail or body need to be rotated
            if (listParts[i].listMvt.Count > 0)
            {
                distance = Vector3.Distance(listParts[i].trans.position, listParts[i].listMvt[0].nextPos);
                //If the body or trail reach the target position
                if (distance <= 0.01f)
                {
                    switch (listParts[i].listMvt[0].nextMvt)
                    {
                        case -1: listParts[i].trans.RotateAround(listParts[i].trans.position, Vector3.up, -90); break;
                        case 1: listParts[i].trans.RotateAround(listParts[i].trans.position, Vector3.up, 90); break;
                    }
                    listParts[i].listMvt.RemoveAt(0);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Food"))
        {
            Debug.Log("Food");
        }
        else
        {
            //Debug.Log("Collider:" + other.name + " " + transform.name);
        }
    }

}
