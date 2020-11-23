public class Rule
{
    private string str_InR;
    private string str_OutR;

    public Rule(string i, string o)
    {
        str_InR = i;
        str_OutR = o;
    }

    public string GetIn()
    {
        return str_InR;
    }

    public string GetOut()
    {
        return str_OutR;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}