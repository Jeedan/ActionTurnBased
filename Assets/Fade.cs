using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    IEnumerator fader;
    public Image image;
    public float duration = 1.0f;
    public bool complete = false;
    public bool inProgress = false;

    public Color currentColor;
    //private Color initialColor;

    // Use this for initialization
    void Start()
    {
        //initialColor = image.color; // for reseting purposes
        currentColor = image.color;
        fader = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (complete)
        {
            fader = null;
            complete = false;
        }
    }

    public void InitFadeOut()
    {
        if (inProgress || fader != null)
        {

            Debug.Log("FadeOut in progress, please wait");
            return;
        }
        else
        {
            fader = FadeToClear();
            inProgress = true;
            StartCoroutine(fader);
        }

    }

    public void InitFadeIn()
    {
        if (inProgress || fader != null)
        {

            Debug.Log("FadeIn in progress, please wait");
            return;
        }
        else
        {
            fader = FadeToBlack();
            inProgress = true;
            StartCoroutine(fader);
        }

    }
    public void FadeToggle()
    {
        if (inProgress || fader != null)
        {

            Debug.Log("FadeIn in progress, please wait");
            return;
        }
        else
        {
            inProgress = true;
            if (image.color.a < 0.5f)
            {
                fader = FadeTo(currentColor, Color.black, duration);//FadeToBlack();
                StartCoroutine(fader);
            }
            else
            {
                fader = FadeTo(currentColor, Color.clear, duration);//FadeToClear();
                StartCoroutine(fader);
            }
        }
    }


    IEnumerator FadeTo(Color from, Color to, float _duration)
    {
        float t = 0;
        while (t < duration)
        {
            image.color = Color.Lerp(from, to, t / _duration);
            t += Time.deltaTime;
            yield return null;
        }

        complete = true;
        inProgress = false;
        currentColor = image.color;
        Debug.Log("FINISHED ROUTINE FadeTo() " + image.color.a);
    }

    IEnumerator FadeToClear()
    {
        float t = 0;
        Color transparent = Color.clear;
        if (image.color.a < 0.5f)
        {
            Debug.Log("already transparent");
            complete = true;
            inProgress = false;
            t = duration;
            yield return null;
        }

        while (t < duration)
        {
            image.color = Color.Lerp(currentColor, transparent, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("FINISHED ROUTINE FadeOut()" + image.color.a);
        complete = true;
        inProgress = false;
    }

    IEnumerator FadeToBlack()
    {
        float t = 0;
        Color transparent = Color.clear;
        if (image.color.a > 0.5f)
        {
            Debug.Log("already black");
            complete = true;
            inProgress = false;
            t = duration;
            yield return null;
        }

        while (t < duration)
        {
            image.color = Color.Lerp(transparent, currentColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("FINISHED ROUTINE FadeIn()" + image.color.a);
        complete = true;
        inProgress = false;
        currentColor = image.color;
    }


    // completely stops the coroutine
    public void Stop()
    {
        if(fader != null)
        {
            StopCoroutine(fader);
            complete = true;
            inProgress = false;
            Debug.Log("routine stopped");
        }
    }
}
