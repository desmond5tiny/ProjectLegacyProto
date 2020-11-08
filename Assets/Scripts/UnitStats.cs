using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitStats
{
    //Base stats
    private string dna;
    private string[] parentsId = new string[2];
    public string unitName;
    public float maxHealth;     //
    public float currentHealth;
    public int currentLevel;    //
    public float experience;
    public float expToNextLevel;
    public float age;
    public string gender;        //

    //Personalty

    public enum Attribute { Dexterity, Defense, Endurance, Strength, Intelligence, Social }

    //Attributes
    public int attDexterity { private set; get; }  // Speed
    public int attDefense { private set; get; }    // Resistance
    public int attEndurance { private set; get; }  // Grit
    public int attIntelligence { private set; get; }//
    public int attSocial { private set; get; }      // charisma
    public int attStrength { private set; get; }   //

    //Skills
    Dictionary<string, int> skills = new Dictionary<string, int>();
    /*  public float skillBuild;
        public float skillGather;
        public float skillDiscover; */
    
    //Unique traits
    Dictionary<string, int> traits = new Dictionary<string, int>();

    public UnitStats(string _dna)
    {
        dna = _dna;
        SetStats();
        SetAttributes();
        SetSkills();
        SetTraits();
        SetStartLevel();
    }

    private void SetStats()
    {
        unitName = "Hank";//assign random name from names file
        string statGene = dna.Substring(3, 5);
        maxHealth = int.Parse(statGene.Substring(0, 3));
        if (statGene.ElementAt(4).ToString() == "F") { gender = "Female"; }
        if (statGene.ElementAt(4).ToString() == "M") { gender = "Male"; }
    }

    private void SetAttributes()
    {
        string attGene = dna.Substring(8, 12);
        //Debug.Log("attribute gene: " + attGene);
    }

    private void SetSkills()
    {
        int skillGeneLength = 1 + int.Parse(dna.ElementAt(20).ToString()) * 3;
        string skillGene = dna.Substring(20, skillGeneLength);
        Debug.Log("skills gene: " + skillGene);

        for (int i = 0; i < int.Parse(skillGene.ElementAt(0).ToString()); i++)
        {
            skills.Add(skillGene.Substring(1 + (i * 3), 2), int.Parse(skillGene.ElementAt(3 + (i * 3)).ToString()));
        }
    }

    private void SetTraits()
    {
        int traitLoc = 21 + int.Parse(dna.ElementAt(20).ToString()) * 3;
        string traitGene = dna.Substring(traitLoc, 1 + int.Parse(dna.ElementAt(traitLoc).ToString()) * 3);
        //Debug.Log("trait gene: " + traitGene);

        for (int i = 0; i < int.Parse(traitGene.ElementAt(0).ToString()); i++)
        {
            traits.Add(traitGene.Substring(1 + (i * 3),2), int.Parse(traitGene.ElementAt(3 + (i * 3)).ToString()));
        }
    }

    public void SetStartLevel() => IncreaseLevel(int.Parse(dna.ElementAt(6).ToString()));

    public void AddXP(float _xpAmount)
    {
        experience += _xpAmount;
        while (experience >= expToNextLevel)
        {
            experience = experience - expToNextLevel;
            expToNextLevel = expToNextLevel * 1.1f;
            IncreaseLevel(1);
        }
    }
    public void IncreaseLevel( int _levels)
    {
        for (int i = 0; i < _levels; i++)
        {
            currentLevel++;
            //increase max health
            //increase an attribute
        }
    }

    public void IncreaseAttribute(Attribute _attribute)
    {
        if (_attribute == Attribute.Dexterity) { attDexterity++; }
        if (_attribute == Attribute.Defense) { attDefense++; }
        if (_attribute == Attribute.Endurance) { attEndurance++; }
        if (_attribute == Attribute.Intelligence) { attIntelligence++; }
        if (_attribute == Attribute.Social) { attSocial++; }
        if (_attribute == Attribute.Strength) { attStrength++; }
    }

    public void IncreaseSkill(string _skill, int _amount)
    {
        if (skills.ContainsKey(_skill)) { skills[_skill] += _amount; }
        else { skills.Add(_skill, _amount); }
    }

    public void AddTrait(string _trait, int _amount)
    {
        if (traits.ContainsKey(_trait)) { skills[_trait] += _amount; }
        else { traits.Add(_trait, _amount); }
    }
}
