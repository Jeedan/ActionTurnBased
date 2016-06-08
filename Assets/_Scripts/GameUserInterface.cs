using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUserInterface : MonoBehaviour
{
    public GameObject titleScreenGO;
    public GameObject mainMenuGO;
    public GameObject overWorldGO;
    public GameObject battleGUIGO;

    public Image pressStartImage;
    public float duration = 0.2f;
    public float visibleDuration = 0.5f;

    // Use this for initialization
    void Start()
    {
        mainMenuGO.SetActive(false);
        overWorldGO.SetActive(false);
        battleGUIGO.SetActive(false);

        StartCoroutine(fadeTextInOut());
    }

    IEnumerator fadeTextInOut()
    {
        Color initial = pressStartImage.color;
        bool keepBlinking = true;
        while (keepBlinking)
        {

            //float t = 0;

            Color to = Color.clear;
            StartCoroutine(FadeTo(to));

            yield return new WaitForSeconds(visibleDuration);

            //pressStart.color = Color.clear;
            to = initial;

            StartCoroutine(FadeTo(to));

            yield return new WaitForSeconds(visibleDuration);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                keepBlinking = false;

                yield return null;
            }
        }
    }

    IEnumerator FadeTo(Color to)
    {
        float t = 0;
        Color from = pressStartImage.color;
        // Color to = Color.clear;
        while (t < duration)
        {
            pressStartImage.color = Color.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
