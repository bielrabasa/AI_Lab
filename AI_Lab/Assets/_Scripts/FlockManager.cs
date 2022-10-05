using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] GameObject fishPrefab;
    
    public GameObject[] allFish;
    [SerializeField] int numFish = 50;

    //Flock settings
    public Vector3 swimLimits = new Vector3(20, 20, 20);
    public bool bounded = false;
    public bool randomise = false;
    public bool followLider = false;
    public GameObject lider;

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


    //Freq
    float freq = 0.0f;
    [Range(0.0f, 2.0f)]
    [Header("\n")]
    public float calculationFreq = 0.3f;

    int d = 10; //Generation distance

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; ++i)
        {

            Vector3 pos = this.transform.position + 
                new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d)); // random position

            Vector3 randomize = new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d)); // random vector direction
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos,
                                Quaternion.LookRotation(randomize));

            allFish[i].GetComponent<Flock>().myManager = this;
        }
    }

    private void Update()
    {
        freq += Time.deltaTime;
        if(freq > calculationFreq)
        {
            Debug.Log("Calculating...");
            freq = 0.0f;
            foreach (GameObject go in allFish)
            {
                go.GetComponent<Flock>().CalculateDirection();
            }
        }
    }
}
