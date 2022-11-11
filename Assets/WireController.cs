using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WireController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect wireVFX;

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
    private float wireThickness = .2f;

    [SerializeField]
    private AnimationCurve wireThicknessOvertime;
    [SerializeField]
    private Vector2 maxWireDeviationWidth = new Vector2(1, 1);

    [Header("WireNoise")]
    [SerializeField] float wireMaxNoise = 2;

    [SerializeField]
    private AnimationCurve wireNoiseOverTime;
    
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

    [Header("Wire End Animation Curve")]
    [SerializeField]
    private AnimationCurve endAnimation_X;

    [SerializeField]
    private AnimationCurve endAnimation_Y;

    [SerializeField]
    private AnimationCurve endAnimation_Z;

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

        if (!wireVFX)
        {
            wireVFX = GetComponentInChildren<VisualEffect>();
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

        //updating arc points
        if (time == 0)
        {
            foreach (Transform t in arcPoints)
            {
                t.position = transform.position;
            }
        }
        else
        {
            arcPoints[3].position =
                SetArcPoint(arcPoints[3].localPosition, endAnimation_X, endAnimation_Y, endAnimation_Z);
            arcPoints[1].position = SetArcPoint(arcPoints[1].localPosition, midStartAnimation_X, midStartAnimation_Y,
                midStartAnimation_Z);
            arcPoints[2].position = SetArcPoint(arcPoints[2].localPosition, midEndAnimation_X, midEndAnimation_Y,
                midEndAnimation_Z);
        }
        
        //updating thickness
        wireVFX.SetFloat("Thickness",wireThickness*wireThicknessOvertime.Evaluate(time));
        
        wireVFX.SetFloat("NoiseRange",wireNoiseOverTime.Evaluate(time));
    }


    public void SetTarget(Transform t)
    {
        targetTransform = t;
    }

    public Vector3 SetArcPoint(Vector3 arcPoint, AnimationCurve x, AnimationCurve y, AnimationCurve z)
    {
        Vector3 evaluatedTime = new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
        arcPoint = targetDir * (targetMag * evaluatedTime.z);
        arcPoint += transform.right * (maxWireDeviationWidth.x * evaluatedTime.x);
        arcPoint += transform.up * (maxWireDeviationWidth.y * evaluatedTime.y);

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