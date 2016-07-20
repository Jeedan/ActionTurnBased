using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public Camera mainCam;
    public Transform target;

    public float shakeIntensity;
    public float shakeX;
    public bool isShaking = false;

    public int shakes = 5;
    public float timeBetweenShakes = 0.1f;
    private int initShakes;

    private Vector3 targetOffset;
    public Vector3 offset = new Vector3(0, 2, -10);


    private Vector3 originalPosition;
    private Quaternion originalRotation;
    // Use this for initialization
    void Start()
    {
        initShakes = shakes;
        if (target)
        {
            targetOffset = target.position - mainCam.transform.position;
            targetOffset += offset;
        }

        originalPosition = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isShaking)
        {
            StartCoroutine(ShakeScreen());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            shakes = initShakes;
            isShaking = false;
        }

        Vector3 moveTarget = Vector3.zero;
        moveTarget.x = originalPosition.x + Mathf.Sin(Time.time) * 3;
        target.transform.position = new Vector3(moveTarget.x, target.position.y, target.position.z);


    }

    void LateUpdate()
    {
        if (target)
        {
            Vector3 desiredPos = (target.position + targetOffset);
            desiredPos.z += offset.z;
            desiredPos.x += offset.x;
            desiredPos.y += offset.y;
            mainCam.transform.position = desiredPos;
            //if (!isShaking)
              //  LookAtTarget();
        }
    }

    void LookAtTarget()
    {
        Quaternion lookRot = Quaternion.LookRotation(target.position - mainCam.transform.position);
        mainCam.transform.rotation = Quaternion.RotateTowards(mainCam.transform.rotation, lookRot, Time.deltaTime * 120.0f);
    }

    IEnumerator ShakeScreen()
    {
        originalRotation = mainCam.transform.rotation;
        isShaking = true;

        while (shakes > 0)
        {
            shakeX = Random.Range(-shakeIntensity, shakeIntensity);
            Vector3 move = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
            move.x = shakeX * shakeIntensity * Time.deltaTime;

            Vector3 desiredPos = (target.position + targetOffset);
            targetOffset.x = move.x;
            mainCam.transform.position = desiredPos;
            shakes--;
            yield return new WaitForSeconds(timeBetweenShakes);
        }

        yield return null;
        mainCam.transform.rotation = originalRotation;
        shakes = initShakes;
        isShaking = false;
    }
}
