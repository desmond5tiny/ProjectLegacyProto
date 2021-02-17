using Boo.Lang;
using System.Linq;
using UnityEngine;

public class DnaGenerator
{
    // "a121231m0203040303052Bu1Ga22Ma1Iv2" // standard format

    private static List<string> startSkills = new List<string>(new string[] { "Bu", "Ga", "Dc", "Cb", "Ls" });
    private static List<string> startTraits = new List<string>(new string[] { "Bl", "Pr", "Vr", "Lb" });
    private static string letters = "abcdefghijklmnopqrstuvwxyz";

    public static string CreateDna() // create with no origin dna
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

    public static string CreateDna(string _dnaA, string _dnaB) //create from parent dna
    {
        string dna = GenID();

        int avgStat = Mathf.RoundToInt((int.Parse(_dnaA.Substring(3, 3)) + int.Parse(_dnaB.Substring(3, 3))) / 2);
        string addString = (avgStat + Random.Range(-10,11)).ToString();
        if (addString.Length < 3) { addString = addString.Insert(0, "0"); }
        dna += addString;
        dna += Mathf.FloorToInt(Random.Range(0.2f, 1.5f)).ToString();
        addString = "mf";
        dna += addString.ElementAt(Random.Range(0, 2));

        dna += GenAttributes(Random.Range(getParentAttPoints(_dnaA.Substring(8, 12)), getParentAttPoints(_dnaB.Substring(8, 12)))+Random.Range(-3,3));
        avgStat = Mathf.RoundToInt(int.Parse(_dnaA.ElementAt(20).ToString()) + int.Parse(_dnaB.ElementAt(20).ToString())/2 + Mathf.Ceil(Random.Range(-1.2f,1.2f)));
        //addString = Mathf.RoundToInt(_dnaA.Substring(20))
        dna += GenVarGene(Random.Range(1, 4), startSkills);

        return dna;
    }

    private static int getParentAttPoints(string _AttGenome)
    {
        int atts = 0;
        for (int i = 0; i < 6; i++)
        {
            atts += int.Parse(_AttGenome.Substring(i * 2, 2));
        }
        return atts;
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
