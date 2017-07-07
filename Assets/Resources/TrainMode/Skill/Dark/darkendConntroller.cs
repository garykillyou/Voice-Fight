using UnityEngine;
using System.Collections;

public class darkendConntroller : MonoBehaviour {

    // Use this for initialization
    Animator playerAnimator = new Animator();

    void Start()
    {

    }

    void over()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator = (Animator)GetComponent("Animator");
        playerAnimator.Play("end");
    }
}
