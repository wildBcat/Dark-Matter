using UnityEngine;

/// <summary>
/// Stores a range of colors and changes them based off of a preset speed
/// </summary>
public class ColorLerp : MonoBehaviour
{
    [Tooltip("Sets the transition speed between colors. The higher the number, the faster the color change.")]
    [SerializeField] [Range(0f, 2f)] private float lerpTime = 0;
    [Tooltip("Sets the colors to transition between. The higher the number, the more color options.")]
    [SerializeField] private Color[] colors = default;

    SpriteRenderer colorMeshRenderer;
    private int colorIndex = 0;
    private float t = 0f;
    private int len;

    // Start is called before the first frame update
    void Start()
    {
        colorMeshRenderer = GetComponent<SpriteRenderer>();
        len = colors.Length;
    }

    /*
     * Changes the gameobject's color from its current one to the next one in the index by the speed set
     * Runs the timer
     * When the timer maxes out, it resets the timer, moves to the next color 
     * sets the color back to zero if it has reached the last one, or keeps moving up
     */
    void Update()
    {
        colorMeshRenderer.material.color = Color.Lerp(colorMeshRenderer.material.color, colors[colorIndex], lerpTime * Time.deltaTime);        
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);        
        if (t > .9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex >= len) ? 0 : colorIndex;
        }
    }
}
