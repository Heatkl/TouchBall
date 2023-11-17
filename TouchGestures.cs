using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGestures : MonoBehaviour
{
    [SerializeField] private float minSwipeLenghtX = 100.0f;
    [SerializeField] private float maxSwipeLenghtY = 50.0f;

    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2 currentSwipe;

    private bool isRightMove = true;

    private float currentDistance = 0.0f;
    private float previousDistance = 0.0f;

    private float initialDistance = 0.0f;
    private bool isZooming = false;
    private Vector2 previousTouch1Pos;
    private Vector2 previousTouch2Pos;

    private float minChangeDistance = 0.1f;
    private float coefOppositeDirection = -0.5f;

    void Update()
    {
        DetectSwipe();
        DetectPinchToZoom();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstTouchPosition = t.position;
            }


            if (t.phase == TouchPhase.Ended)
            {
                lastTouchPosition = t.position;
                currentSwipe = new Vector2(lastTouchPosition.x - firstTouchPosition.x, lastTouchPosition.y - firstTouchPosition.y);

                if (Mathf.Abs(currentSwipe.y) <= maxSwipeLenghtY && currentSwipe.x >= minSwipeLenghtX)
                {
                    Debug.Log("Swipped right");
                }
                else if (Mathf.Abs(currentSwipe.y) <= maxSwipeLenghtY && currentSwipe.x <= -minSwipeLenghtX)
                {
                    Debug.Log("Swipped left");
                }
            }
        }
    }

    private void IsCnangeDirection(Touch t)
    {
        if (isRightMove && t.deltaPosition.x < 0)
        {
            isRightMove = false;
            firstTouchPosition = t.position;
        }
        else if (!isRightMove && t.deltaPosition.x > 0)
        {
            isRightMove = true;
            firstTouchPosition = t.position;
        }
    }


    private void DetectPinchToZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (!isZooming)
            {
                initialDistance = Vector2.Distance(t1.position, t2.position);
                previousTouch1Pos = t1.position;
                previousTouch2Pos = t2.position;
                isZooming = true;
            }
            else
            {
                float currentDist = Vector2.Distance(t1.position, t2.position);

                if ((currentDist - initialDistance) > initialDistance * minChangeDistance)
                {
                    Vector2 direction1 = t1.position - previousTouch1Pos;
                    Vector2 direction2 = t2.position - previousTouch2Pos;

                    if (Vector2.Dot(direction1.normalized, direction2.normalized) < coefOppositeDirection)
                    {

                        currentDistance = currentDist;

                        if (previousDistance != 0)
                        {
                            float deltaDistance = currentDistance - previousDistance;
                            if (deltaDistance > 0)
                                Debug.Log("Zoom");
                        }
                        previousDistance = currentDistance;

                    }

                    previousTouch1Pos = t1.position;
                    previousTouch2Pos = t2.position;

                }
            }
        }
        else
        {
            isZooming = false;
            previousDistance = 0f;
        }

    }
}

