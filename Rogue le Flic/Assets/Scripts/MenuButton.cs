using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public AudioSource m_MyAudioSource;

    public GameObject objetdelamusique;
    

    
    private void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        animator.SetBool("In", false);
        objetdelamusique = GameObject.FindGameObjectWithTag("AudioMusique");

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

    public void Fade()
    {
        objetdelamusique.GetComponent<AudioSource>().DOFade(0f, 1.5f);
    }
    
    
    
}
