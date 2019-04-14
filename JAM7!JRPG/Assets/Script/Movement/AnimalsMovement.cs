using UnityEngine;

public class AnimalsMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animalAnimator;
    private SpriteRenderer spriteRenderer;
    private float stateTimer;

    private bool moving;
    private bool headingLeft;
    void Start()
    {
        animalAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateTimer = 0.0f;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animalAnimator == null) return;
        if (spriteRenderer == null) return;

        if (stateTimer <= 0.0f)
        {
            moving = !moving;
            if (moving)
            {
                animalAnimator.StopPlayback();
                headingLeft = !headingLeft;
            }
            else
            {
                animalAnimator.StartPlayback();
            }
            spriteRenderer.flipX = !headingLeft;
            stateTimer = Random.Range(1.0f, 2.0f);
        }

        stateTimer -= Time.deltaTime;
        if (moving)
        {
            transform.Translate((headingLeft ? Vector3.left : Vector3.right) * 2.0f * Time.deltaTime);
        }
    }
}
