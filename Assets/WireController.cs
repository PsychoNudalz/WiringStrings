using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [Header("Animation Time")]
    [SerializeField]
    [Range(0f, 1f)]
    private float time = 0;

    private float lastTime = 0;

    [SerializeField]
    private Animator animator;

    [Header("Arc Points")]
    [SerializeField]
    private List<Transform> arcPoints;


    [Header("Wire Animation ")]
    [SerializeField]
    private Vector2 maxWireWidth = new Vector2(1, 1);

    [Header("Wire Mid Start Animation Curve")]
    [SerializeField]
    private AnimationCurve midStartAnimation_X;

    [SerializeField]
    private AnimationCurve midStartAnimation_Y;

    [SerializeField]
    private AnimationCurve midStartAnimation_Z;

    [Header("Wire Mid End Animation Curve")]
    [SerializeField]
    private AnimationCurve midEndAnimation_X;

    [SerializeField]
    private AnimationCurve midEndAnimation_Y;

    [SerializeField]
    private AnimationCurve midEndAnimation_Z;

    [Space(10f)]
    [Header("Target")]
    [SerializeField]
    private Transform targetTransform;

    private Vector3 targetDir;
    private float targetMag;
    private bool activeWire = false;


    // Start is called before the first frame update
    void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        lastTime = time;

        if (arcPoints.Count < 4)
        {
            Debug.LogError("wire has less than 4 points");
            enabled = false;
        }

        if (!targetTransform)
        {
            targetTransform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform)
        {
            UpdateWire();
        }
        if (Math.Abs(time - lastTime) > .001f)
        {
            UpdateWireAnimation();
        }

        lastTime = time;
    }
    void UpdateWire()
    {
        if (!targetTransform)
        {
            return;
        }

        targetDir = (targetTransform.position - transform.position);
        targetMag = Vector3.Magnitude(targetDir);
        targetDir = targetDir.normalized;
        transform.forward = targetDir.normalized;
    }

    void UpdateWireAnimation()
    {
        if (!targetTransform)
        {
            return;
        }

        if (time == 0)
        {
            foreach (Transform t in arcPoints)
            {
                t.position = transform.position;
            }
        }
        else
        {
            arcPoints[3].transform.position = transform.position+ targetDir*(targetMag*time);
            arcPoints[1].position=SetArcPoint(arcPoints[1].localPosition, midStartAnimation_X, midStartAnimation_Y, midStartAnimation_Z);
            arcPoints[2].position=SetArcPoint(arcPoints[2].localPosition, midEndAnimation_X, midEndAnimation_Y, midEndAnimation_Z);
        }
    }




    public void SetTarget(Transform t)
    {
        targetTransform = t;
    }

    public Vector3 SetArcPoint( Vector3 arcPoint, AnimationCurve x, AnimationCurve y, AnimationCurve z)
    {
        Vector3 evaluatedTime = new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
        arcPoint = targetDir * (targetMag * evaluatedTime.z);
        arcPoint += transform.right * (maxWireWidth.x * evaluatedTime.x);
        arcPoint += transform.up * (maxWireWidth.y * evaluatedTime.y);
        
        arcPoint = transform.position + arcPoint;
        return arcPoint;
    }

    public void SetFire(bool b)
    {
        animator.SetBool("Shoot", b);
        if (false)
        {
            
        }
    }
    
    public void ToggleFire()
    {
        activeWire = !activeWire;
        SetFire(activeWire);
    }
}