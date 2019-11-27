﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UpdateEstimationView_1 : MonoBehaviour
{
    public GameObject CONST;

    //Text conteners
    public GameObject customer;
    public GameObject projectName;
    public GameObject totalAfterDiscount;
    public GameObject totalBeforeDiscount;
    public GameObject discount;

    //actualise button
    public GameObject actualiser;

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST");

        string totalBeforeDiscountSt = CONST.GetComponent<CONST>().estimationPrice;
        string discountSt = CONST.GetComponent<CONST>().estimationDiscount;
        int discountInt = Int32.Parse(discountSt);
        int totalBeforeDiscountInt = Int32.Parse(totalBeforeDiscountSt);

        int totalAfterDiscountInt = totalBeforeDiscountInt;
        if (discountInt != 0)
        {
            totalAfterDiscountInt = totalBeforeDiscountInt / discountInt;
        }

        totalBeforeDiscount.GetComponent<UnityEngine.UI.Text>().text = totalBeforeDiscountSt;
        discount.GetComponent<UnityEngine.UI.Text>().text = discountSt;
        totalAfterDiscount.GetComponent<UnityEngine.UI.Text>().text = totalAfterDiscountInt.ToString();

        customer.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().customerName;
        projectName.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().projectName;

    }

    //Get back  button function
    public void BackPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       //Send the previous scene (ProjectSheet)
    }

    //Get back  button function
    public void NextPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Send the next scene (estimationView_2)
    }

}
