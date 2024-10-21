using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UniversalCharacterMovement : NetworkBehaviour
{
    // Start is called before the first frame update\

    [SerializeField] public float Gravity;
    [SerializeField] float MoveSpeed; 
    [SerializeField] float GroundFrictionWhileMoving;
    [SerializeField] float GroundFrictionWhenStopped;
    [SerializeField] float JumpStrength;
    [SerializeField] public float AirFallSpeed; 
    [SerializeField] float AirFriction;
    [SerializeField] float AirFrictionHorizontal;


    [SerializeField] GameObject GroundChecker;
    bool isOnGround;

    [SerializeField] string thefloor;


    public NetworkVariable<bool> CHAR0EventusNoGravity = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        
    }


    private void Update()
    {
        if (!IsOwner)
            return;

        isOnGround = GroundChecker.GetComponent<UniversalGroundChecker>().onGround;

        thefloor = GroundChecker.GetComponent<UniversalGroundChecker>().whatisfloor;



        PlatformBypass();

        Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner)
            return;

        HorizontalMovement();
        FakeDrag();



        //this is for regulating the jump arc


        if(DeniedGravity()== false)
        {
            if (GetComponent<Rigidbody>().velocity.y < 2f && isOnGround == false)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.down * AirFallSpeed * 2.50f, ForceMode.Force);
            }

            GetComponent<Rigidbody>().AddForce(Vector3.down * Gravity * 0.7f, ForceMode.Force);

        }

        JumpRelease();

    }


    void FakeDrag()
    {
        //this is because we need the vertical and horizontal drags to be different depending on what a character needs

        if (isOnGround)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.05f * (GroundFrictionWhileMoving / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.33f * (GroundFrictionWhenStopped / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + ((0.05f * (AirFrictionHorizontal / 100)) * -GetComponent<Rigidbody>().velocity.x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);


        }


        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y + ((0.12f * (AirFriction / 100)) * -GetComponent<Rigidbody>().velocity.y), GetComponent<Rigidbody>().velocity.z);


    }

    void HorizontalMovement()
    {
        //this takes the left and right inputs etc etc easy enough right

        GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * MoveSpeed * 1f, ForceMode.Force);
    }


    void Jump()
    {

        //this is for jumping, just a velocity set







        if (isOnGround == true && Input.GetButtonDown("Jump"))
        {

            GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, JumpStrength * 0.65f);


        }









    }


    void JumpRelease()
    {
        //when releasing jump, do a short hop

        if (Input.GetButton("Jump") != true && GetComponent<Rigidbody>().velocity.y > 0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * GetComponent<Rigidbody>().velocity.y * 5f, ForceMode.Force);
        }

    }




    void PlatformBypass()
    {

        if(GetComponent<Rigidbody>().velocity.y > 0.1f || Input.GetButton("Descend"))
        {

            foreach(GameObject platform in GameObject.FindGameObjectsWithTag("Platform"))
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), platform.gameObject.GetComponent<Collider>(), true);

            }

            if(thefloor == "Platform")
            {
                isOnGround = false;
            }

        }
        else
        {

            foreach (GameObject platform in GameObject.FindGameObjectsWithTag("Platform"))
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), platform.gameObject.GetComponent<Collider>(), false);
            }

        }


    }




    // status effects go under vvvvvvv OKAY GOOD


    bool DeniedGravity()
    {
        if(CHAR0EventusNoGravity.Value == true)
        {

            return true;
        }
        else
        {
            return false;
        }


    }




    public void CHAR0Eventus(bool onoff)
    {

        if(IsOwner)
        {


            GetComponent<UniversalCharacterMovement>().CHAR0EventusNoGravity.Value = onoff;
        }
    }




}
