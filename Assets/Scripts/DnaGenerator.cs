using Boo.Lang;
using System.Linq;
using UnityEngine;

public class DnaGenerator
{
    private static List<string> startSkills = new List<string>(new string[] { "Bu", "Ga", "Dc", "Cb", "Ls" });
    private static List<string> startTraits = new List<string>(new string[] { "Bl", "Pr", "Vr", "Lb" });
    private static string letters = "abcdefghijklmnopqrstuvwxyz";

    public static string CreateDna()
    {
        //base stats
        string dna = GenID(); //set unique id

        string addString = Random.Range(050, 121).ToString();
        if(addString.Length<3) { addString = addString.Insert(0, "0"); }
        dna += addString; // set health stat
        dna += Mathf.FloorToInt(Random.Range(0, 1.2f)).ToString(); // set base level
        addString = "mf";
        dna += addString.ElementAt(Random.Range(0, 2)); // set gender

        dna += GenAttributes(Random.Range(18, 24)); //attributes
        dna += GenVarGene(Random.Range(1, 4), startSkills); //skills
        dna += GenVarGene(Random.Range(0, 3), startTraits); //traits

       return dna;
    }

    public static string CreateDna(string _parentA, string _parentB)
    {
        string dna = GenID();


        return dna;
    }

    private static string GenID()
    {
        string id = letters.ElementAt(Random.Range(0, 26)).ToString();
        id = id + Random.Range(0, 10) + Random.Range(0, 10);
        return id;
    }

    private static string GenAttributes(int _points)
    {
        string attGenome = "";
        int[] attributes = new int[6];

        for (int i = 0; i < _points; i++) { attributes[Random.Range(0, 6)]++; }

        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i] < 10) { attGenome += "0"; }
            attGenome += attributes[i].ToString();
        }

        return attGenome;
    }

    private static string GenVarGene(int _amount, List<string> _possibleSkills)
    {
        string genome = _amount.ToString();
        List<string> addedSoFar = new List<string>();

        for (int i = 0; i < _amount; i++)
        {
            string nextskill = _possibleSkills[Random.Range(0, _possibleSkills.Count)];
            if (addedSoFar.Contains(nextskill)) { i--; }
            else
            {
                genome += nextskill + Random.Range(1, 6);
                addedSoFar.Add(nextskill);
            }
        }

        return genome;
    }
}
