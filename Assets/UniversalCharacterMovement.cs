using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update\

    [SerializeField] float Weight;
    [SerializeField] float MoveSpeed; 
    [SerializeField] float GroundFrictionWhileMoving;
    [SerializeField] float GroundFrictionWhenStopped;
    [SerializeField] float JumpStrength;
    [SerializeField] float AirFallSpeed; 
    [SerializeField] float AirFriction;


    [SerializeField] GameObject GroundChecker;
    bool isOnGround;


    float JumpCooldown = 0f;

    int timesjumped = 0;


    void Start()
    {
        
    }


    private void Update()
    {
        isOnGround = GroundChecker.GetComponent<UniversalGroundChecker>().onGround;

        Jump();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HorizontalMovement();
        FakeDrag();
        //another one

        if (GetComponent<Rigidbody>().velocity.y < 3f && isOnGround == false)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * AirFallSpeed * 1.50f, ForceMode.Force);
        }
    }


    void FakeDrag()
    {

        //fake friction generator
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.05f * (GroundFrictionWhileMoving / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.15f * (GroundFrictionWhenStopped / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }

        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y + ((0.25f * (AirFriction / 100)) * -GetComponent<Rigidbody>().velocity.y), GetComponent<Rigidbody>().velocity.z);


    }

    void HorizontalMovement()
    {
        if (isOnGround == true)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxisRaw("Horizontal"), 0,0) * MoveSpeed , ForceMode.Force);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxisRaw("Horizontal"), 0,0) * MoveSpeed, ForceMode.Force);
        }


    }


    void Jump()
    {

        //jump


        if (JumpCooldown >= 0f)
        {
            JumpCooldown -= 13f * Time.deltaTime;
        }




        if (isOnGround == true && Input.GetButton("Jump") && JumpCooldown < 0)
        {
            JumpCooldown = 10f;

            GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, JumpStrength);

            timesjumped += 1;
            print(timesjumped);

        }


        //jump cancel

        if (Input.GetButton("Jump") != true && GetComponent<Rigidbody>().velocity.y > 0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * GetComponent<Rigidbody>().velocity.y , ForceMode.Force);
        }





    }





}
