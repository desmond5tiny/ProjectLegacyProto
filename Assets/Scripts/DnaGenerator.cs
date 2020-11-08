using System.Linq;
using UnityEngine;

public class DnaGenerator
{
    public static string CreateRandomDna()
    {
        string letters = "abcdefghijklmnopqrstuvwxyz";

        //base stats
        string dna = letters.ElementAt(Random.Range(0, 26)).ToString();
        dna = dna + Random.Range(0, 10) + Random.Range(0, 10); //set unique id

        string healthGene = Random.Range(050, 121).ToString();
        if(healthGene.Length<3) { healthGene = healthGene.Insert(0, "0"); }
        dna += healthGene; // set health stat
        dna += Mathf.FloorToInt(Random.Range(0, 1.2f)).ToString(); // set base level
        string genders = "mf";
        dna += genders.ElementAt(Random.Range(0, 2));

        //attributes
        int totalAttPoints = Random.Range(18, 24);
        int[] attributes = new int[6];

        for (int i = 0; i < totalAttPoints; i++) { attributes[Random.Range(0, 6)]++; }
        string att = "";
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i] < 10) { att += "0"; }
            att += attributes[i].ToString();
        }
        dna += att;

        //skills

       return dna;
    }
}
