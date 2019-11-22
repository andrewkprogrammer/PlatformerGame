using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedUIElement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 targetPos;

    Vector3 OriginPos;
    float perc = 0;
    [SerializeField]
    [Range(0, 10)]
    float speed = 1;

    private void Start()
    {
        OriginPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(OriginPos, targetPos, perc);

        perc += speed * Time.deltaTime;

        if (transform.position == targetPos)
            Destroy(gameObject);
    }
}
