using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float midScreenMessage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator BuildText()
    {
        for (int i = 0; i < text.Length; i++)
        {
            textComponent.text = string.Concat(textComponent.text, text *);

            yield return new WaitForSeconds(timeLapse);
        }
    }
}
