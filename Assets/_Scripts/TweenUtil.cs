using UnityEngine;
using System.Collections;

public static class TweenUtil
{
    public static IEnumerator PingPong(Vector3 direction, float distance,float speed, Transform target)
    {
        Vector3 startPos = target.position;
        Vector3 dodgePos = target.position + (direction * distance);

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            // this interpolates from 0 to 1 and then back from 1 to 0
            float interpolate = (-percent * percent + percent) * 4;

            target.position = Vector3.Lerp(startPos, dodgePos, interpolate);

            yield return null;
        }
    }

    public static IEnumerator LerpTowardsTarget(Vector3 start, Vector3 end, float speed, float duration, Transform target)
    {
        float t = 0;
        while (t < duration)
        {
            target.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime * speed;
            yield return null;
        }
    }

}
