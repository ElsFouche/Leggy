using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeShatter : MonoBehaviour
{

    public GameObject GumballParent;
    public List<Rigidbody> GumballStorage = new List<Rigidbody>();

    public GameObject NormalDome;
    public GameObject ShatteredDome;

    public GameObject Hammer;

    // Start is called before the first frame update
    void Awake()
    {
        for (int gumball = 0; gumball < GumballParent.transform.childCount; gumball++)
        {
            GumballStorage.Add(GumballParent.transform.GetChild(gumball).GetComponent<Rigidbody>());
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.transform.parent.GetComponent<BasketData>().objectRigidbody == Hammer.GetComponent<Rigidbody>() ||
            other.GetComponent<BasketData>().objectRigidbody == Hammer.GetComponent<Rigidbody>()) 
        {
            for (int gumball = 0; gumball < GumballStorage.Count; gumball++)
            {
                GumballStorage[gumball].GetComponent<Rigidbody>().isKinematic = false;
            }

            ShatteredDome.gameObject.SetActive(true);

            for (int pieces = 0; pieces < ShatteredDome.transform.childCount; pieces++)
            {
                ShatteredDome.transform.GetChild(pieces).GetComponent<Rigidbody>().isKinematic = false;
            }
            NormalDome.SetActive(false);
        }
    }
}
