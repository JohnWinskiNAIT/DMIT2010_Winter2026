using UnityEngine;

public class AIMovement : MonoBehaviour
{
    float movementSpeed, forwardDist, sideDist;

    RaycastHit forwardHit, rightHit, leftHit;

    bool rightDetect, leftDetect;

    [SerializeField] GameObject hitLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 3.0f;
        forwardDist = 1.0f;
        sideDist = 3.0f;
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
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.forward, out forwardHit, forwardDist))
        {
            hitLocation.transform.position = forwardHit.point;

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
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, transform.right, out rightHit, sideDist))
        {
            rightDetect = true;
        }
        if (Physics.CapsuleCast(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 1.5f, 0), 0.5f, -transform.right, out leftHit, sideDist))
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
        //Debug.Log(other.transform.name);
        //if (other.transform.tag == "Wall")
        //{
        //    transform.Rotate(Vector3.up, 90);
        //}
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
