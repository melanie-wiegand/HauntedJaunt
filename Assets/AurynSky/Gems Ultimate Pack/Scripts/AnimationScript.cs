using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    public float floatRate;
    private float baseHeight;  // Add base height variable

    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    public ParticleSystem explosionEffect;


    void Start ()
    {
        if (isFloating)
        {
            baseHeight = transform.position.y;  // Initialize base height
        }
    }
    
    void Update ()
    {
        if (isAnimated)
        {
            if (isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if (isFloating)
            {
                float newY = baseHeight + Mathf.Sin(Time.time * floatSpeed) * floatRate;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }

            if (isScaling)
            {
                scaleTimer += Time.deltaTime;
                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }
                if (scaleTimer >= scaleRate)
                {
                    scalingUp = !scalingUp;
                    scaleTimer = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check for collision with player
        {
            // Instantiate a new Particle System (if the original one must be kept intact)
            ParticleSystem tempEffect = Instantiate(explosionEffect, explosionEffect.transform.position, Quaternion.identity);
            tempEffect.Play();

            // Detach the particle system from the current GameObject
            tempEffect.transform.SetParent(null);

            // Destroy the temporary particle system after it finishes playing
            Destroy(tempEffect.gameObject, tempEffect.main.duration);

            // Now it's safe to deactivate or destroy the GameObject
            // gameObject.SetActive(false);  // Disable the GameObject
            Destroy(gameObject);  // Or Destroy the GameObject immediately
        }
    }
}

