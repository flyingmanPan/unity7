using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ActorController : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private new Rigidbody rigidbody;

    private Vector3 vec;
    private float rotSpeed = 15f;
    private float runSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //Animation
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        vec = new Vector3(x, 0, z);
        if (x != 0 || z != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(vec);
            if (transform.rotation != rotation) transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotSpeed);
        }

        this.transform.position += vec * Time.fixedDeltaTime * runSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.CompareTag("Guardian"))
        if (collision.gameObject.name=="Guardian")
        {
            //Animation
            Debug.Log("hit");
            Publish publish = Publisher.GetInstance();
            publish.Notify(ActorState.DEATH, 0, null);
        }
    }
}
