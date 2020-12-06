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
}