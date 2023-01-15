using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public AudioSource m_MyAudioSource;

    
    private void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
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

    public void PlayAudioHover()
    {
        m_MyAudioSource.Play();
    }
    
    
    
}
