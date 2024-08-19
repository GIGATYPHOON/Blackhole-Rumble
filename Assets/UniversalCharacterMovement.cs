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



        //this is for regulating the jump arc

        if (GetComponent<Rigidbody>().velocity.y < 2f && isOnGround == false)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * AirFallSpeed * 3.50f, ForceMode.Force);
        }
    }


    void FakeDrag()
    {
        //this is because we need the vertical and horizontal drags to be different depending on what a character needs

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.05f * (GroundFrictionWhileMoving / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.25f * (GroundFrictionWhenStopped / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }

        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y + ((0.25f * (AirFriction / 100)) * -GetComponent<Rigidbody>().velocity.y), GetComponent<Rigidbody>().velocity.z);


    }

    void HorizontalMovement()
    {
        //this takes the left and right inputs etc etc easy enough right

        GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * MoveSpeed * 1.5f, ForceMode.Force);
    }


    void Jump()
    {

        //this is for jumping, just a velocity set







        if (isOnGround == true && Input.GetButton("Jump"))
        {

            GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, JumpStrength * 1.3333f);


        }


        //when releasing jump, do a short hop

        if (Input.GetButton("Jump") != true && GetComponent<Rigidbody>().velocity.y > 0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * GetComponent<Rigidbody>().velocity.y , ForceMode.Force);
        }





    }





}
