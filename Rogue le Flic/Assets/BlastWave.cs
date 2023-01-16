using System;
using UnityEngine;
using System.Collections;

public class BlastWave : MonoBehaviour
{
    public int pointsCount;
    public float maxRadius;
    public float speed;
    public float startWidth;

    private LineRenderer lineRenderer;

   private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount + 1;
    }

   private void Start()
   {
       StartCoroutine(Blast());
   }

   public IEnumerator Blast()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
        
        lineRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 2) * -1) - 1;
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(Blast());*/
    }
}
