using System.Collections.Generic;

public class SimbolTable
{
    public List<Simbol> Simbols = new List<Simbol>();

    public SimbolTable()
    {

    }

    public int SearchPosition(string identifier)
    {
        for(int i = 0; i < Simbols.Count; i++)
        {
            if(identifier == Simbols[i].Identifier)
                return i;
        }

        return -1;
    }
    
}