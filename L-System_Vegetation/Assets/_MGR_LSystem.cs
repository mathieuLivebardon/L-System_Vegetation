using System.Collections.Generic;
using UnityEngine;

public class _MGR_LSystem : MonoBehaviour
{
    private static _MGR_LSystem pInstance = null;
    public static _MGR_LSystem Instance { get { return pInstance; } }

    public List<Rule> lst_Rules;

    public string axiom;
    string sentence;
    public int len;

    public LineRenderer lineRenderer;

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


        sentence = axiom;
        lst_Rules = new List<Rule>();

   

        lst_Rules.Add(new Rule("A", "ABC"));
        lst_Rules.Add(new Rule("B", "A"));
        lst_Rules.Add(new Rule("F", "FF+[+F-F-F]-[-F+F+F]"));

        Debug.Log(sentence);
    }

    private void generate()
    {
        string nextSentence = "";

        bool found = false;
        for (int i = 0; i < sentence.Length; i++)
        {
            string current = sentence[i].ToString();
            foreach(Rule r in lst_Rules)
            {
                if (current == r.GetIn())
                { 
                    nextSentence += r.GetOut();
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                nextSentence += current;
            }
        }



        sentence = nextSentence;
        Debug.Log(sentence);
    }



    void turtle()
    {
        foreach(char current in sentence)
        {
            if(current == 'F')
            {

            }
        }

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