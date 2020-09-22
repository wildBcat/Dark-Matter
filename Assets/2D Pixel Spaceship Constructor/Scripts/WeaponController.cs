using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    const float TRACE_DELAY = 0.06f;

    public GameObject bulletPrefab;
    [Tooltip("Set to true if muzzle has animation. E")]
    public bool muzzleHasAnimation;
    [Tooltip("Lest of empty child GameObjects in the weapon where bullet will appear")]
    public Transform[] bulletStartPoses;
    public float bulletSpeed;
    [Tooltip("Delay between each bullet if repeat fire mode")]
    public float fireDelay;
    [Tooltip("Should bullets appear one after another or all at once. Use for turret.")]
    public bool fireInSequence;
    [Tooltip("Should bullet has the same rotation as weapon. Use for bullets with tale.")]
    public bool weaponRotationForBullet = false;

    [Space(20)]
    public bool tracingEnable = true;
    [Tooltip("Target object to trace")]
    public Transform traceTarget = null;
    [Tooltip("Step angle of rotation when tracing is enable")]
    public float deltaAngle;
    [Tooltip("Set true to emulate slow rotation with constant speed. Good for big heavy cannon.")]
    public bool oneStepRotation = false;
    [Tooltip("Time between each rotate by 'Delta Angle'. Works when 'One Step Rotation' is true. For low level see comments in 'BaseStart' function.")]
    public float deltaTimeStepRotation;

    float lastTimeRotated = 0.0f;
    protected bool repeatFire = false;
    int fireIndex = 0;
    Animator[] animators;

    protected void BaseStart()
    {
        /// To save processor time start coroutine to check target position instead using Update function.
        /// In case deltaTimeStepRotation lower or close to TRACE_DELAY constant move content of TraceTarget function into Update function.
        if (tracingEnable)
            StartCoroutine(TraceTarget());

        if (muzzleHasAnimation)
        {
            animators = new Animator[bulletStartPoses.Length];
            int index = 0;
            foreach (Transform oneStartPos in bulletStartPoses)
                animators[index++] = oneStartPos.parent.GetComponent<Animator>();

        }
    }

    private void Start()
    {
        BaseStart();
    }

    IEnumerator TraceTarget()
    {
        while(tracingEnable && traceTarget != null)
        {
            if (oneStepRotation)
            {
                if (Time.time - lastTimeRotated > deltaTimeStepRotation)
                {
                    if (Tools.TraceTarget(transform, traceTarget.position, deltaAngle, true))
                        lastTimeRotated = Time.time;
                }
            } else
            {
                Tools.TraceTarget(transform, traceTarget.position, deltaAngle);
            }
            yield return new WaitForSeconds(TRACE_DELAY);
        }
    }
    
    protected void OneShot(int index = 0)
    {
        if (IfIndexGood(index))
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletStartPoses[index].position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletSpeed * (-bulletStartPoses[index].up);
            if (weaponRotationForBullet)
                bullet.transform.rotation = transform.rotation;
            if (muzzleHasAnimation)
                animators[index].SetTrigger("fire");
        }
    }

    bool IfIndexGood(int index)
    {
        if (bulletStartPoses != null && index >= 0 && index < bulletStartPoses.Length)
        {
            return true;
        }
        else
        {
            Debug.LogWarning("index is out of range in bulletStartPoses");
            return false;
        }
    }

    virtual public void StartRepeateFire()
    {
        if (!repeatFire)
        {
            repeatFire = true;
            fireIndex = 0;
            StartCoroutine(RepeateFire());
        }
    }

    virtual public void StopRepeatFire()
    {
        repeatFire = false;
    }

    virtual public void MakeOneShot()
    {
        for (int index = 0; index < bulletStartPoses.Length; index++)
            OneShot(index);
    }

    private void OnDestroy()
    {
        StopCoroutine(RepeateFire());
    }

    IEnumerator RepeateFire()
    {
        while (repeatFire)
        {
            if (fireInSequence)
            {
                OneShot(fireIndex);
                if (++fireIndex >= bulletStartPoses.Length)
                    fireIndex = 0;
            }
            else
            {
                MakeOneShot();
            }
            yield return new WaitForSeconds(fireDelay);
        }
    }
}
