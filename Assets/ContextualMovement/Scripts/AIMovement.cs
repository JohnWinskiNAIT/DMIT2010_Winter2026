using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed, forwardDist, sideDist;

    RaycastHit forwardHit, rightHit, leftHit;

    [SerializeField] List<GameObject> targetList = new List<GameObject>();

    LayerMask frontMask;

    bool rightDetect, leftDetect;

    [SerializeField] GameObject hitLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 3.0f;
        forwardDist = 1.0f;
        sideDist = 3.0f;
        frontMask = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        DetectForward();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.fixedDeltaTime);
    }

    void DetectForward()
    {
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.forward, out forwardHit, forwardDist, frontMask))
        {
            if (hitLocation != null)
            {
                hitLocation.transform.position = forwardHit.point;
            }
            

            rightDetect = false;
            leftDetect = false;

            DetectSides();

            // The new Vector3 was added to avoid taking on the wall x or z rotation.
            transform.LookAt(transform.position - new Vector3(forwardHit.normal.x, 0, forwardHit.normal.z));

            DetectSides();

            RotateFromObject();
        }
    }

    void DetectSides()
    {
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.right, out rightHit, sideDist, frontMask))
        {
            rightDetect = true;
        }
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, -transform.right, out leftHit, sideDist, frontMask))
        {
            leftDetect = true;
        }        
    }

    void RotateFromObject()
    {
        if (rightDetect && !leftDetect)
        {
            transform.Rotate(Vector3.up, -90);
        }
        else if (!rightDetect && leftDetect)
        {
            transform.Rotate(Vector3.up, 90);
        }
        else if (rightDetect && leftDetect)
        {
            transform.Rotate(Vector3.up, 180);
        }
        else
        {
            transform.Rotate(Vector3.up, 90);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);

        if (other.tag == "GoodObject")
        {
            targetList.Add(other.gameObject);
        }
        //if (other.transform.tag == "Wall")
        //{
        //    transform.Rotate(Vector3.up, 90);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GoodObject")
        {
            targetList.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name);

        //if (collision.transform.tag == "Wall")
        //{
        //    transform.Rotate(Vector3.up, 90);
        //}
        
    }
}
