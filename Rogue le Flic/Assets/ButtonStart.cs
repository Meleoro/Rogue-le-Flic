using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonStart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private void Start()
    {
        animator.SetBool("In", false);
    }

    public void OnPointerEnter()
    {
        Debug.Log(12);
        animator.SetBool("In", true);
    }
    
    public void OnPointerExit()
    {
        animator.SetBool("In", false);
        Debug.Log(122222);
    }
}
