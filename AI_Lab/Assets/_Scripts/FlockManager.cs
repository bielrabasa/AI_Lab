using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] GameObject beePrefab;
    
    public GameObject[] allBees;
    [SerializeField] int numBees = 50;
    
    [SerializeField] GameObject lider;
    [HideInInspector] public Vector3 liderPos;
    float targetChangeTime = 30.0f;
    float changeTarget = 0.0f;

    //Fish settings
    [Header("\nFish Settings")]

    [Range(0.0f, 5.0f)]
    public float minSpeed = 3;
    [Range(0.0f, 5.0f)]
    public float maxSpeed = 5;
    [Range(0.0f, 10.0f)]
    public float neighbourDistance = 4;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed = 1;

    //AI
    [Header("\nAI Settings")]
    [Range(0.0f, 3.0f)]
    public float followLiderForce = 1.0f;
    [Range(0.0f, 3.0f)]
    public float cohesionForce = 1.0f;
    [Range(0.0f, 3.0f)]
    public float alignForce = 1.0f;
    [Range(0.0f, 3.0f)]
    public float separationForce = 1.0f;

    //Freq
    float freq = 1.0f;
    [Range(0.0f, 2.0f)]
    [Header("\n")]
    public float calculationFreq = 0.3f;

    int d = 3; //Generation distance

    // Start is called before the first frame update
    void Start()
    {
        liderPos = lider.transform.position;
        
        allBees = new GameObject[numBees];
        for (int i = 0; i < numBees; ++i)
        {

            Vector3 pos = this.transform.position + 
                new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d)); // random position

            Vector3 initDir = new Vector3(-Random.Range(0.5f, 1.0f), -Random.Range(0.5f, 1.0f), -Random.Range(0.5f, 1.0f));
            
            //Create elements inside this object (keep hierarchy clean)
            allBees[i] = (GameObject)Instantiate(beePrefab, pos, Quaternion.LookRotation(initDir), transform);

            allBees[i].GetComponent<Flock>().myManager = this;
        }
    }

    private void Update()
    {
        freq += Time.deltaTime;
        changeTarget += Time.deltaTime;

        //Calculate direction only every "calculationFreq" seconds
        if(freq > calculationFreq)
        {
            freq = 0.0f;
            foreach (GameObject go in allBees)
            {
                go.GetComponent<Flock>().CalculateDirection();
            }
        }

        //Change target between 2 hives
        if(changeTarget > targetChangeTime)
        {
            changeTarget = 0.0f;

            if (liderPos != transform.position) liderPos = transform.position;
            else liderPos = lider.transform.position;
        }
    }
}
