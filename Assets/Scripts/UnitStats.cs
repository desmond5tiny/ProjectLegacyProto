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
    public float maxHealth;     
    public float currentHealth;
    public int currentLevel;    
    public float experience;
    public float expToNextLevel;
    public float age;
    public string gender;        

    //Personalty

    public enum Attribute { Dexterity, Defense, Endurance, Strength, Intelligence, Social }

    //Attributes
    public int attDexterity { private set; get; }  // Speed
    public int attDefense { private set; get; }    // Resistance / fortitude
    public int attEndurance { private set; get; }  // Grit 
    public int attIntelligence { private set; get; }//
    public int attSocial { private set; get; }      // charisma
    public int attStrength { private set; get; }   //

    //Skills
    Dictionary<string, int> skills = new Dictionary<string, int>();
    
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
        string statGenome = dna.Substring(3, 5);
        maxHealth = int.Parse(statGenome.Substring(0, 3));
        if (statGenome.ElementAt(4).ToString() == "F") { gender = "Female"; }
        if (statGenome.ElementAt(4).ToString() == "M") { gender = "Male"; }
    }

    private void SetAttributes()
    {
        string attGenome = dna.Substring(8, 12);
        Debug.Log("attribute gene: " + attGenome);

        attDexterity = int.Parse(attGenome.Substring(0, 2));
        attDefense = int.Parse(attGenome.Substring(2, 2));
        attEndurance = int.Parse(attGenome.Substring(4, 2));
        attIntelligence = int.Parse(attGenome.Substring(6, 2));
        attSocial = int.Parse(attGenome.Substring(8, 2));
        attStrength = int.Parse(attGenome.Substring(10, 2));
    }

    private void SetSkills()
    {
        int skillGenomeLength = 1 + int.Parse(dna.ElementAt(20).ToString()) * 3;
        string skillGenome = dna.Substring(20, skillGenomeLength);

        for (int i = 0; i < int.Parse(skillGenome.ElementAt(0).ToString()); i++)
        {
            skills.Add(skillGenome.Substring(1 + (i * 3), 2), int.Parse(skillGenome.ElementAt(3 + (i * 3)).ToString()));
        }
    }

    private void SetTraits()
    {
        int traitLoc = 21 + int.Parse(dna.ElementAt(20).ToString()) * 3;
        string traitGenome = dna.Substring(traitLoc, 1 + int.Parse(dna.ElementAt(traitLoc).ToString()) * 3);

        for (int i = 0; i < int.Parse(traitGenome.ElementAt(0).ToString()); i++)
        {
            traits.Add(traitGenome.Substring(1 + (i * 3),2), int.Parse(traitGenome.ElementAt(3 + (i * 3)).ToString()));
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

    public int GetSkillLevel(string _skill)
    {
        if (!skills.ContainsKey(_skill)) { return 0; }

        return skills[_skill];
    }

    public void AddTrait(string _trait, int _amount)
    {
        if (traits.ContainsKey(_trait)) { skills[_trait] += _amount; }
        else { traits.Add(_trait, _amount); }
    }
}
