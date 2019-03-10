using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Text txtScore, txtCountdown;

    List<BodyParts> listParts = new List<BodyParts>();
    float speed = 0.1f;
    int typeMvt = 0;
    int score;
    float countdown = 3.4f;

    // Use this for initialization
    void Start()
    {
        GameObject.Find("Food").transform.position = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
        InitTrail();
        InitBody();
    }

    void InitBody()
    {
        for (int i = 0; i < 3; i++)
            CreateBody();
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
        Vector3 bodyPos = listParts[0].trans.position;
        Transform trans = Instantiate(prefabBody, bodyPos, listParts[0].trans.rotation);
        trans.rotation = listParts[0].trans.rotation;

        BodyParts bodyPart = new BodyParts();
        trans.name = "body" + (listParts.Count);
        bodyPart.trans = trans;

        if (listParts[0].listMvt.Count > 0)
        {
            ChildrenMvt childMvt = new ChildrenMvt();
            childMvt.nextMvt = listParts[0].listMvt[0].nextMvt;
            childMvt.nextPos = listParts[0].listMvt[0].nextPos;
            bodyPart.listMvt.Add(childMvt);
        }
        listParts.Add(bodyPart);
        listParts[0].trans.position = listParts[0].trans.position - listParts[0].trans.forward;


        //Vector3 
    }

    // Update is called once per frame
    void Update()
    {
        //Move only if countdown ended
        if (countdown < 0)
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
        else
        {
            countdown -= Time.deltaTime;
            //Countdown value for 3,2,1
            if (countdown > 0.5f)
                txtCountdown.text = "" + Mathf.RoundToInt(countdown);
            else
            {
                txtCountdown.text = "GO";
                StartCoroutine(RoutineCloseCountdown());
            }
        }
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


        for (int i = listParts.Count - 1; i > -1; i--)
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
                        case 2: listParts[i].trans.RotateAround(listParts[i].trans.position, Vector3.up, 180); break;
                    }
                    //Remove the first movement in the list
                    listParts[i].listMvt.RemoveAt(0);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the head collide with a Food
        if (other.name.Equals("Food"))
        {
            score += listParts.Count - 1;
            //Move the Food to a random place
            other.transform.position = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            //Increase the snake size
            CreateBody();
            //Update the score
            txtScore.text = "SCORE : " + score;
        }
        else if (other.name.Equals("border"))
        {
            transform.RotateAround(transform.position, Vector3.up, 180); RotateChildrens(2);
        }
        else if (!other.name.Equals("body1"))
        {
            Debug.Log("Collider:" + other.name + " " + transform.name);
            Restart();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator RoutineCloseCountdown()
    {
        yield return new WaitForSeconds(0.3f);
        txtCountdown.text = "";
    }

}
