using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
    [SerializeField] public float length = 10.0f;
    [SerializeField] public float angle = 10.0f;
    [SerializeField] private GameObject branch;



    List<TransformInfo> transformStack;

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

        lst_Rules.Add(new Rule("A", "ABC"));
        lst_Rules.Add(new Rule("B", "A"));
        lst_Rules.Add(new Rule("F", "FF"));
        lst_Rules.Add(new Rule("X", "[FF[+XF-F+FX]--F+F-FX]"));

        transformStack = new List<TransformInfo>();
        branches = 0;

        SetSentence(axiom);
        //length *= 0.3f;
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
    }

    private void SetSentence(string newSentence)
    {
        sentence = newSentence;
        Debug.Log(sentence);
        StartCoroutine("turtle");
    }

    IEnumerator turtle()
    {
        foreach (char current in sentence)
        {
            /*Debug.Log("Lecture de : "+current);
            yield return new WaitForSeconds(1f);*/
            switch (current)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    GameObject treeSegment = Instantiate(branch);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    branches++;
                    //yield return new WaitForSeconds(0f);
                    break;

                case 'X':

                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;

                case '[':
                    transformStack.Add(new TransformInfo(transform.position, transform.rotation));
                    break;

                case ']':
                    TransformInfo ti = transformStack[transformStack.Count-1];
                    //Debug.Log(transformStack.Count);
                    transform.position = ti.pos;
                    transform.rotation = ti.rot;
                    transformStack.Remove(transformStack[transformStack.Count - 1]);
                    //Debug.Log(transformStack.Count);
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;
                default:
                    throw new InvalidOperationException("Invalid l-tree operation");
                    break;
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
}