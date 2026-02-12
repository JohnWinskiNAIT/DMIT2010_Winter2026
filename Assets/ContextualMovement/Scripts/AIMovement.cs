using UnityEngine;

public class AIMovement : MonoBehaviour
{
    float movementSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 3.0f;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.tag == "Wall")
        {
            transform.Rotate(Vector3.up, 90);
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
