using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    [Tooltip("Sets the transition speed between colors. The higher the number, the faster the color change.")]
    [SerializeField] [Range(0f, 2f)] float lerpTime = 0;

    [Tooltip("Sets the colors to transition between. The higher the number, the more color options.")]
    [SerializeField] Color[] colors = default;

    // A variable that represents the current color of the gameobject
    SpriteRenderer colorMeshRenderer;

    // A variable to track what color the gameobject is on out of the total color choices
    int colorIndex = 0;

    // A variable that times when to switch to the next color
    float t = 0f;

    // Variable that shows the max count of color options
    int len;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variable to the gameobject's current color
        colorMeshRenderer = GetComponent<SpriteRenderer>();

        // Sets the amount of colors available to the variable
        len = colors.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //Changes the gameobject's color from its current one to the next one in the index by the speed set
        colorMeshRenderer.material.color = Color.Lerp(colorMeshRenderer.material.color, colors[colorIndex], lerpTime * Time.deltaTime);

        // Runs the timer
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        // When the timer maxes out, it resets the timer, moves to the next color, then sets the color back to zero if it has reached the last one, or keeps moving up
        if (t > .9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex >= len) ? 0 : colorIndex;
        }
    }
}
