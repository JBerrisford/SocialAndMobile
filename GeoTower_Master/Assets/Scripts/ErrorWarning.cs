using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorWarning : MonoBehaviour
{
    public Text errorMessage;
    private Coroutine co;

    public void Init()
    {
        errorMessage.enabled = false;
    }

    public void StartError(string msg)
    {
        errorMessage.text = msg;

        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(Display(0.05f));
    }

    private IEnumerator Display(float delay)
    {
        float t = 1.0f;

        errorMessage.color = Color.red;
        errorMessage.enabled = true;

        yield return new WaitForSeconds(1.0f);

        while (t > 0.0f)
        {
            errorMessage.color = new Color(errorMessage.color.r, errorMessage.color.g, errorMessage.color.b, t);
            yield return new WaitForSeconds(delay);
            t -= delay;
        }

        errorMessage.enabled = false;
        yield return null;
    }
}
