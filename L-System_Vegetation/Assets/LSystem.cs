using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct TransformInfo
{
    public Vector3 pos;
    public Quaternion rot;

    public TransformInfo(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
}

public class LSystem : MonoBehaviour
{
    public List<Rule> lst_Rules;

    [SerializeField] public string axiom = "T";
    private string sentence;

    [SerializeField] public GameObject branch;
    [SerializeField] public GameObject flower;
    [SerializeField] public float initialLength = 30.0f;
    [SerializeField] public float initialWidth = 5.0f;
    [SerializeField] public float angle = 10.0f;
    [SerializeField] public float lCoef = 0.9f;
    [SerializeField] public float wCoef = 0.8f;

    private LineRenderer currentLine;

    public float wait;

    private List<TransformInfo> transformStack;

    public int branches;

    private void Start()
    {
        lst_Rules = new List<Rule>();

        lst_Rules.Add(new Rule("T", "FX"));
        lst_Rules.Add(new Rule("F", "F"));
        lst_Rules.Add(new Rule("X", "[+F-F[+XF-F+FX]--F+F-FX]"));

        transformStack = new List<TransformInfo>();
        branches = 0;
        currentLine = null;
        SetSentence(axiom);
    }

    public void InitialiseRandom()
    {
        initialLength = Random.Range(15.0f, 30.0f);
        initialWidth = Random.Range(3.0f, 7.0f);
        angle = Random.Range(30.0f, 50.0f);

        lCoef = Random.Range(0.2f, 0.9f);
        wCoef = Random.Range(0.6f, 0.9f);
    }

    private void generate()
    {
        string nextSentence = "";

        bool found;
        for (int i = 0; i < sentence.Length; i++)
        {
            found = false;
            string current = sentence[i].ToString();
            foreach (Rule r in lst_Rules)
            {
                if (current == r.GetIn())
                {
                    nextSentence += r.GetOut();
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                nextSentence += current;
            }
        }

        SetSentence(nextSentence);
        wait *= 0.5f;
    }

    private void SetSentence(string newSentence)
    {
        Debug.Log(newSentence);
        sentence = newSentence;
        StartCoroutine("turtle");
    }

    private IEnumerator turtle()
    {
        float length = initialLength;
        float width = initialWidth;
        foreach (char current in sentence)
        {
            switch (current)
            {
                case 'F':
                    Vector3 initialPosition = transform.localPosition;
                    transform.Translate(Vector3.up * length);
                    if (currentLine == null)
                    {
                        GameObject treeSegment = Instantiate(branch, transform.parent);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.localPosition);
                        treeSegment.GetComponent<LineRenderer>().widthMultiplier = width;
                        branches++;
                        currentLine = treeSegment.GetComponent<LineRenderer>();
                    }
                    else
                    {
                        currentLine.positionCount++;
                        currentLine.SetPosition(currentLine.positionCount - 1, transform.localPosition);
                    }
                    if (wait > 0)
                    {
                        yield return new WaitForSeconds(wait);
                    }
                    break;

                case 'X':
                    break;

                case 'T':
                    break;

                case '+':
                    transform.Rotate(Vector3.forward * GetRandomAngle());
                    transform.Rotate(Vector3.right * GetRandomAngle());
                    break;

                case '[':
                    transformStack.Add(new TransformInfo(transform.position, transform.rotation));
                    length *= lCoef;
                    width *= wCoef;
                    break;

                case ']':
                    GameObject flowerInstance = Instantiate(flower, transform.parent);
                    flowerInstance.transform.localPosition = transform.localPosition;
                    flowerInstance.transform.localRotation = transform.localRotation;
                    TransformInfo ti = transformStack[transformStack.Count - 1];
                    transform.position = ti.pos;
                    transform.rotation = ti.rot;
                    transformStack.Remove(transformStack[transformStack.Count - 1]);
                    length *= 1 / lCoef;
                    width *= 1 / wCoef;
                    currentLine = null;
                    break;

                case '-':
                    transform.Rotate(Vector3.back * GetRandomAngle());
                    transform.Rotate(Vector3.left * GetRandomAngle());
                    break;

                default:
                    throw new InvalidOperationException("Invalid l-tree operation");
            }
        }
        yield return null;
    }

    //Call each frames
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            generate();
        }
    }

    private float GetRandomAngle()
    {
        return Random.Range(0.0f, angle);
    }
}