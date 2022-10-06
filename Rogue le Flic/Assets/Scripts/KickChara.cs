using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class KickChara : MonoBehaviour
{
    public static KickChara Instance;

    public GameObject kick;
    
    public float kickDuration;
    public float kickStrenght;

    public float propulsionChara;


    public void Awake()
    {
        Instance = this;

        kick.SetActive(false);
    }
    


    public IEnumerator Kick()
    {
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ManagerChara.Instance.transform.position;
        
        kick.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg, Vector3.forward);
        
        kick.SetActive(true);
        
        ManagerChara.Instance.noControl = true;
        ManagerChara.Instance.rb.AddForce(new Vector2(mousePos.x - charaPos.x, mousePos.y - charaPos.y).normalized * propulsionChara, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(kickDuration);
        
        kick.SetActive(false);
        ManagerChara.Instance.noControl = false;
    }
}
