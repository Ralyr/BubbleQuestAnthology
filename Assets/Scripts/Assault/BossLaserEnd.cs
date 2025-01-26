using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserEnd : MonoBehaviour
{
    //The moving end of the eye laser trail
    [SerializeField] Transform endPosTop;
    [SerializeField] LineRenderer lineRenderer;
    BossHead head;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 target;

    bool isMoving = false;
    float minDist = 0.1f;

    float maxDistDelta = 0.1f;

    private void Start()
    {
        startPos = transform.position;
        endPos = endPosTop.position;
        target = startPos;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void SetHead(BossHead h) { head = h; }

    public void StartMove()
    {
        isMoving = true;
        lineRenderer.SetPosition(0, head.GetLaserStart());
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;

        if (target == endPos)
            target = startPos;
        else
            target = endPos;
    }

    public Vector3 GetPosition() { return transform.position; }

    private void Update()
    {
        if (isMoving)
        {
            //check if we're close, then stop
            if (Vector3.Distance(transform.position, target) <= minDist)
            {
                //done moving
                head.LaserMoveDone();
                isMoving = false;
                lineRenderer.enabled = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, maxDistDelta);
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }
}
