using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CHAR0 : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ObjectsToFlip;


    string CurrentStance = "Time Stance";


    [SerializeField] GameObject StanceText;


    [SerializeField] GameObject HorizonStrikesObject;

    float HorizonTimer = 0f;

    bool HorizonStriking = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizonStrikes();


        Stances();

        Facing();
    }



    void Facing()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            GetComponent<UniversalEntityProperties>().isFacingRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            GetComponent<UniversalEntityProperties>().isFacingRight = false;
        }


        if (GetComponent<UniversalEntityProperties>().isFacingRight)
        {
            ObjectsToFlip.transform.localScale = Vector3.one;
        }
        else
        {
            ObjectsToFlip.transform.localScale = new Vector3(-1, 1, 1);
        }


    }




    void Stances()
    {

        if(CurrentStance == "Time Stance")
        {
            GetComponent<Animator>().SetFloat("AnimMultiplier", 1.5f);
        }
        else
        {
            GetComponent<Animator>().SetFloat("AnimMultiplier", 1f);
        }


        if (CurrentStance == "Space Stance")
        {
            HorizonStrikesObject.transform.localScale = new Vector3(2f,1f,1f);
        }
        else
        {
            HorizonStrikesObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }


        if(Input.mouseScrollDelta.y > 0)
        {

            CurrentStance = "Space Stance";
        }
        if (Input.mouseScrollDelta.y < 0)
        {

            CurrentStance = "Time Stance";
        }

        StanceText.GetComponent<TextMeshProUGUI>().text = CurrentStance;


    }



    void HorizonStrikes()
    {
        if(Input.GetButton("Fire1"))
        {


            GetComponent<Animator>().Play("HorizonStrikes");
        }

        else
        {






        }




    }

}
