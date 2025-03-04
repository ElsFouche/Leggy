using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public bool goingLeft;
    public Vector3 leftPoint;
    public Vector3 rightPoint;

    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        logic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void logic()
    {
        if (goingLeft) StartCoroutine(LerpValue(transform.position, leftPoint));
        else StartCoroutine(LerpValue(transform.position, rightPoint));
    }

    public IEnumerator LerpValue(Vector3 pointA, Vector3 pointB)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.position = Vector3.Lerp(pointA, pointB, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = pointB;
        goingLeft = !goingLeft;
        logic();
    }
}
