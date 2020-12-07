using UnityEngine;

public class ColorRandom : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}