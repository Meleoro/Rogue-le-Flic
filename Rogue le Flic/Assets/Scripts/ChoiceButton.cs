using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void BoolTrue()
    {
        anim.SetBool("isSelected", true);
    }

    public void BoolFalse()
    {
        anim.SetBool("isSelected", false);
    }
}
