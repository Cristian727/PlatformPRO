using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba1 : MonoBehaviour
{
    [SerializeField] float speed = 2f; 
    [SerializeField] float xBound = -5f; 
    [SerializeField] float xLimit = 5f; 

    private Renderer rend;
    private float startX;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startX = xBound;
        transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
    }
   
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        float t = Mathf.InverseLerp(startX, xLimit, transform.position.x);
        rend.material.color = Color.Lerp(Color.red, Color.blue, t);

        
        if (transform.position.x >= xLimit)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}
