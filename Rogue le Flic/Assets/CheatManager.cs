using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public GameObject Weapon1;
    public GameObject Weapon2;
    public GameObject Weapon3;

    public GameObject Module1;
    public GameObject Module2;
    public GameObject Module3;
    public GameObject Module4;
    public GameObject Module5;
    
    
    void Update()
    {
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.M))
        {
            
        }

        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.C))
        {
            CoinManager.Instance.AddCoin(100);
        }

        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.K))
        {
            
        }

        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.L))
        {
            
        }
        
        
        //ARMES
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.N))
        {
            Instantiate(Weapon1, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(Weapon2, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(Weapon3, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        
        //MODULES
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(Module1, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(Module2, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(Module3, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            Instantiate(Module4, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
        
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            Instantiate(Module5, ManagerChara.Instance.transform.position, Quaternion.identity);
        }
    }
}
