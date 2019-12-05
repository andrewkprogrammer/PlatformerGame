using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinScript : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float speed = 0.5f;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    float expansionMax = 1.5f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float expansionMin = 0.5f;

    float curScale = 1;
    float scalePerc = 0;
    int lerpDir = 1;

    // Start is called before the first frame update
    void Start()
    {
        expansionMin *= transform.localScale.x;
        expansionMax *= transform.localScale.x;
        curScale = expansionMin;
        Vector3 temp = transform.localScale;
        transform.localScale = new Vector3(temp.x * curScale, temp.y * curScale, temp.z * curScale);
    }

    // Update is called once per frame
    void Update()
    {
        scalePerc += Time.deltaTime * speed * lerpDir;
        curScale = Mathf.Lerp(expansionMin, expansionMax, scalePerc);
        transform.localScale = new Vector3(curScale, curScale, curScale);

        if ((curScale == expansionMax && lerpDir == 1) || (curScale == expansionMin && lerpDir == -1))
            lerpDir *= -1;
    }
}
