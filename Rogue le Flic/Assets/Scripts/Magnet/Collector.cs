using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (!collision.CompareTag("Ennemy"))
      {
         var exp = collision.GetComponent<EXP>();
         if (exp != null)
         {
            exp.Collect();
            ExpManager.pointCount += 1;
            ExpBar.Instance.currentXp += 1;
         }
      
         ICollectible collectible = collision.GetComponent<ICollectible>();
      }
   }
}
