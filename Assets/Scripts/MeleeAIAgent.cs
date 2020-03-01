using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAIAgent : NavAgent
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }


    public override void MoveAgent(Vector3 heading)
    {
        transform.Translate(heading * Time.deltaTime, Space.World);
        Vector3 curPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        desiredHeading = TARGET_SPEED * (pathPoints[0] - curPos).normalized;
        Debug.DrawLine(curPos, curPos + desiredHeading, Color.magenta);

    }
}
