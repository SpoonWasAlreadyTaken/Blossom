using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationRandomFrame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        var state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, 0, Random.Range(0, 1f));
    }
}
