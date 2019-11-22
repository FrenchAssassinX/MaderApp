using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevisPayment : MonoBehaviour
{

    public Dropdown stateDevis;
    public Dropdown stateAdvancement;
    public Image barStateAdvancement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StateDevis()
    {
        //Poster all devis state
        List<string> dropdownStateDevis = new List<string>() {"Brouillon", "En attente", "Accepté", "Refusé", "En commande", "Transfert en facturation" /*temporaire*/};
        stateDevis.AddOptions(dropdownStateDevis);
    }

    void StateAdvancement()
    {
        //Poster all devis advancement
        List<string> dropdownStateAdvancement = new List<string>() {/*step*/};
        stateAdvancement.AddOptions(dropdownStateAdvancement);
    }

    void BarStateAdvancement()
    {

    }




}
