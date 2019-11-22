using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectableScript : MonoBehaviour
{
    [SerializeField]
    Blackboard blackboard;
    [SerializeField]
    GameObject UncollectedParticle;
    [SerializeField]
    GameObject CollectedParticle;
    [SerializeField]
    float bobMax = 0.5f;
    float bobPerc = 0;
    int bobDir = 1;

    Vector3 origin;

    float velocity = 0;
    [SerializeField]
    float acceleration = 1;

    bool Collected = false;

    // Start is called before the first frame update
    void Start()
    {
        blackboard = GameObject.Find("GameManager").GetComponent<Blackboard>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CollectedParticle.GetComponent<ParticleSystem>().isPlaying)
        {
            velocity += (acceleration * Time.deltaTime) * bobDir;
            bobPerc += velocity * Time.deltaTime;
            if ((bobDir == 1 && bobPerc >= 0.5f) || (bobDir == -1 && bobPerc <= 0.5f))
            {
                bobDir *= -1;
            }
            transform.position = Vector3.Lerp(origin, origin + Vector3.up * bobMax, bobPerc);
        }

        if(Collected && !CollectedParticle.GetComponent<ParticleSystem>().isPlaying)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CollectedParticle.SetActive(true);
            UncollectedParticle.SetActive(false);
            Collected = true;
            blackboard.AddPlayerScore(1, transform.position);
            GetComponent<Collider>().enabled = false;
            transform.SetParent(other.transform);
            transform.localPosition = Vector3.zero + Vector3.up;
        }
    }
}
