using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private void Start()
    {
        animator.SetBool("In", false);
    }

    public void OnPointerEnter()
    {
        animator.SetBool("In", true);
    }
    
    public void OnPointerExit()
    {
        animator.SetBool("In", false);
    }
}
