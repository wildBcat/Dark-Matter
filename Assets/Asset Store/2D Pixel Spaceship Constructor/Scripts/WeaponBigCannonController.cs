using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBigCannonController : WeaponController
{

    Animator animator;
    bool readyToFire = true;

    // Start is called before the first frame update
    void Start()
    {
        BaseStart();
        animator = GetComponent<Animator>();
    }

    override public void MakeOneShot()
    {
        if (readyToFire)
        {
            readyToFire = false;
            animator.SetTrigger("fire");
            OneShot();
        }
    }

    override public void StartRepeateFire()
    {
        if (!repeatFire)
        {
            repeatFire = true;
            MakeOneShot();
        }
    }

    override public void StopRepeatFire()
    {
        repeatFire = false;
    }

    /// <summary>
    /// Function will be called from AnimationClip
    /// </summary>
    public void SetReadyToFire()
    {
        readyToFire = true;
        if (repeatFire)
            MakeOneShot();
    }
}
