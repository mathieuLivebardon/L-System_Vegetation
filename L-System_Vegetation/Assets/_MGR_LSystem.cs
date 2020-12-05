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

public class _MGR_LSystem : MonoBehaviour
{
    private static _MGR_LSystem pInstance = null;
    public static _MGR_LSystem Instance { get { return pInstance; } }

    public List<Rule> lst_Rules;

    public string axiom;
    private string sentence;
    [SerializeField] public float initialLength = 30.0f;
    [SerializeField] public float initialWidth = 5.0f;
    [SerializeField] public float angle = 10.0f;
    [SerializeField] public GameObject branch;

    private LineRenderer currentLine;

    public float lCoef;
    public float wCoef;

    public float wait;

    private List<TransformInfo> transformStack;

    public DrawLine line;

    public int branches;

    //Start before the Start()
    private void Awake()
    {
        if (pInstance == null)
            //if not, set instance to this
            pInstance = this;
        //If instance already exists and it's not this:
        else if (pInstance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        lst_Rules = new List<Rule>();

        lst_Rules.Add(new Rule("T", "FX"));
        lst_Rules.Add(new Rule("F", "F"));
        lst_Rules.Add(new Rule("X", "[+F-F[+XF-F+FX]--F+F-FX]"));

        transformStack = new List<TransformInfo>();
        branches = 0;
        currentLine = null;
        SetSentence(axiom);
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
        sentence = newSentence;
        Debug.Log(sentence);
        StartCoroutine("turtle");
    }

    private IEnumerator turtle()
    {
        float length = initialLength;
        float width = initialWidth;
        foreach (char current in sentence)
        {
            //Debug.Log(current);
            switch (current)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    if (currentLine == null)
                    {
                        GameObject treeSegment = Instantiate(branch);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                        treeSegment.GetComponent<LineRenderer>().widthMultiplier = width;
                        branches++;
                        currentLine = treeSegment.GetComponent<LineRenderer>();
                    }
                    else
                    {
                        currentLine.positionCount++;
                        currentLine.SetPosition(currentLine.positionCount - 1, transform.position);
                        //currentLine.widthMultiplier = width;
                    }
                    yield return new WaitForSeconds(wait);
                    break;

                case 'X':
                    break;

                case 'T':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * GetRandomAngle());
                    transform.Rotate(Vector3.left * GetRandomAngle());

                    break;

                case '[':
                    transformStack.Add(new TransformInfo(transform.position, transform.rotation));
                    length *= lCoef;
                    width *= wCoef;

                    break;

                case ']':
                    TransformInfo ti = transformStack[transformStack.Count - 1];
                    transform.position = ti.pos;
                    transform.rotation = ti.rot;
                    transformStack.Remove(transformStack[transformStack.Count - 1]);
                    length *= 1 / lCoef;
                    width *= 1 / wCoef;
                    currentLine = null;
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * GetRandomAngle());
                    transform.Rotate(Vector3.right * GetRandomAngle());
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
        return Random.Range(0.0f, 50.0f);
    }
}