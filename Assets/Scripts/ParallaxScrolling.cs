using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{

    [SerializeField] private float speedX = 0.2f;
    [SerializeField] private float speedY = 0.2f;

    [SerializeField] private Transform cameraPosition;

    [SerializeField] private float startXPos, startYPos;

    // Start is called before the first frame update
    void Start()
    {
        startXPos = transform.position.x;
        startYPos = transform.position.y;
        cameraPosition = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceX = cameraPosition.position.x * speedX;
        float distanceY = cameraPosition.position.y * speedY;
        transform.position = new Vector3(startXPos + distanceX, startYPos + distanceY, transform.position.z);
    }
}
