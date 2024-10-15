using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterCamera : MonoBehaviour
{
    // Start is called before the first frame update
    bool lockedin = false;



    [SerializeField]
    private GameObject playerchar;

    private Vector3 targetpos;

    Vector3 smeg = Vector3.zero;

    float everincreasingnumber = 0;

    float everincreasingnumber2 = 0;

    Vector3 lockedposition;

    Vector3 lamlockedposition;

    Vector3 lockedinscroll = new Vector3(0, 0, 0f);

    float lockedinscrollspeedhorizontal = 0f;
    float lockedinscrollspeedvertical = 0f;

    float speed = 8f;

    float lockscreenalpha = 0f;



    bool okaysure = false;

    float lastRealTime = 0f;
    float deltaRealTime = 0f;

    void Start()
    {
        okaysure = false;

    }

    // Update is called once per frame
    void Update()
    {
        deltaRealTime = Time.realtimeSinceStartup - lastRealTime;



        lastRealTime = Time.realtimeSinceStartup;

       playercharCameraCursorShit();


        if (Time.timeScale == 0)
        {

        }
        else
        {

        }


        if (lockedin == true && Time.timeScale == 0)
        {

            if (Input.mousePosition.y >= Screen.height * 0.85)
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, 9f, speed * deltaRealTime);
            }
            else if (Input.mousePosition.y <= Screen.height * 0.15)
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, -9f, speed * deltaRealTime);
            }
            else
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, 0f, speed * deltaRealTime);
            }

            if (Input.mousePosition.x >= Screen.width * 0.85)
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, 9f, speed * deltaRealTime);
            }
            else if (Input.mousePosition.x <= Screen.width * 0.15)
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, -9f, speed * deltaRealTime);
            }
            else
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, 0f, speed * deltaRealTime);
            }



            lockedinscroll = lockedinscroll + (Vector3.up * deltaRealTime * lockedinscrollspeedvertical);

            lockedinscroll = lockedinscroll + (Vector3.right * deltaRealTime * lockedinscrollspeedhorizontal);


        }



    }


    private void FixedUpdate()
    {



        if (lockedin == true && Time.timeScale > 0)
        {

            if (Input.mousePosition.y >= Screen.height * 0.85)
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, 9f, speed * Time.deltaTime);
            }
            else if (Input.mousePosition.y <= Screen.height * 0.15)
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, -9f, speed * Time.deltaTime);
            }
            else
            {
                lockedinscrollspeedvertical = Mathf.Lerp(lockedinscrollspeedvertical, 0f, speed * Time.deltaTime);
            }

            if (Input.mousePosition.x >= Screen.width * 0.85)
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, 9f, speed * Time.deltaTime);
            }
            else if (Input.mousePosition.x <= Screen.width * 0.15)
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, -9f, speed * Time.deltaTime);
            }
            else
            {
                lockedinscrollspeedhorizontal = Mathf.Lerp(lockedinscrollspeedhorizontal, 0f, speed * Time.deltaTime);
            }



            lockedinscroll = lockedinscroll + (Vector3.up * Time.deltaTime * lockedinscrollspeedvertical);

            lockedinscroll = lockedinscroll + (Vector3.right * Time.deltaTime * lockedinscrollspeedhorizontal);


        }


    }





   void playercharCameraCursorShit()
    {
        //if (Input.mouseScrollDelta.y > 0 || (Input.GetButtonDown("Pause") && okaysure == false))
        //{
        //    if (lockedin == true)
        //    {

        //        lockedin = false;



        //    }

        //    //print(okaysure);

        //}
        //else if (Input.mouseScrollDelta.y < 0 || Time.timeScale == 0)
        //{
        //    if (lockedin == false)
        //    {

        //        lockedposition = this.transform.position;
        //        lamlockedposition = playerchar.transform.position;

        //        lockedin = true;




        //        if (Time.timeScale == 1)
        //        {
        //            okaysure = true;
        //        }
        //        else
        //        {
        //            okaysure = false;
        //        }

        //    }


        //}

        if (Input.mouseScrollDelta.y > 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }



        if (lockedin == false)
        {


            targetpos = new Vector3(playerchar.transform.position.x, playerchar.transform.position.y + 2, -13);

        //    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10, 7f * Time.deltaTime);


            Vector3 mousePosition = new Vector3(Camera.main.ScreenToViewportPoint(Input.mousePosition).x - 0.5f, Camera.main.ScreenToViewportPoint(Input.mousePosition).y - 0.5f, 0f);

            mousePosition = new Vector3(mousePosition.x * 15f, mousePosition.y * 8f, mousePosition.z);

            smeg = Vector3.Lerp(smeg, mousePosition, 12f * Time.deltaTime);

            targetpos += smeg;


            //cuck above

            everincreasingnumber += 530f * Time.deltaTime;

            if (everincreasingnumber >= 100f)
            {
                okaysure = true;
                this.transform.position = targetpos;
                everincreasingnumber = 100f;
            }
            else
            {

                float ajax;
                float bjax;

                ajax = Mathf.LerpUnclamped(this.transform.position.x, targetpos.x, everincreasingnumber * Time.deltaTime);
                bjax = Mathf.LerpUnclamped(this.transform.position.y, targetpos.y, everincreasingnumber * Time.deltaTime);

                okaysure = false;
                this.transform.position = new Vector3(ajax, bjax, this.transform.position.z);
            }



            lockedinscroll = Vector2.zero;






            if (lockscreenalpha > 0)
            {
                lockscreenalpha -= 4f * deltaRealTime;

            }



            Vector3 mousePosition2 = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);




        }


    }
}
