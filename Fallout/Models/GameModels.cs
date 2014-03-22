using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fallout.Models
{
    public class GameContext : DbContext
    {
        public GameContext() : base("GameData") { }

        public DbSet<Character> Characters { get; set; }
    }

    public class Character
    {
        //Basics
        public int CharacterID { get; set; }
        public string PlayerName { get; set; }
        //RolePlay Stuff
        public string CharacterName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Race { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        //public string LastRoll { get; set; }
        //Base Stats
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        //Secondary Stats
        public int Experience { get; set; }
        public int CurrentHitPoints { get; set; }
        public int CurrentActionPoints { get; set; }
        //Gear Slots
        public virtual Armor BodyArmor { get; set; }
        public virtual Armor HeadArmor { get; set; }
        public virtual Weapon LeftHand { get; set; }
        public virtual Weapon RightHand { get; set; }
        //gogoCybernetics
        public virtual Cybernetics Cybernetics { get; set; }
        //OmgLists
        public virtual ICollection<Trait> Traits { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Perk> Perks { get; set; }
        public virtual ICollection<Equipment> Gear { get; set; }
        
    }
    public class Modifier
    {
        public int ModifierID { get; set; }
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public int CarryWeight { get; set; }
        public int MeleeDamage { get; set; }
        public int ResistPoison { get; set; }
        public int ResistRadiation { get; set; }
        public int ResistGas { get; set; }
        public int ResistElectricity { get; set; }
        public virtual ArmorResistance ResistNormal { get; set; }
        public virtual ArmorResistance ResistLaser { get; set; }
        public virtual ArmorResistance ResistFire { get; set; }
        public virtual ArmorResistance ResistPlasma { get; set; }
        public virtual ArmorResistance ResistExplosive { get; set; }

        
        public virtual ICollection<SkillModifier> Skill { get; set; }

        public Modifier()
        {
            ResistNormal = new ArmorResistance();
            ResistLaser = new ArmorResistance();
            ResistFire = new ArmorResistance();
            ResistPlasma = new ArmorResistance();
            ResistExplosive = new ArmorResistance();
            Skill = new List<SkillModifier>();

        }

        public bool HasSkill(string Skillname)
        {
            foreach (SkillModifier sm in Skill)
                if (sm.Skill == "Skillname")
                    return true;
            return false;
        }
    }

    public class Cybernetics
    {
        public int CyberneticsID { get; set; }

        public int? ModifierID { get; set; }
        public virtual Modifier StaticMods { get; set; }
        
        public string OtherMods { get; set; }

    }

    public enum ArmorResistanceType
    { 
        ResistNormal = 1,
        ResistLaser,
        ResistFire,
        ResistPlasma,
        ResistExplosive
    }

    public enum ModifierType
    { 
        Strength = 1,
        Perception,
        Endurance,
        Charisma,
        Intelligence,
        Agility,
        Luck,
        ArmorClass,
        HitPoints,
        CarryWeight,
        MeleeDamage,
        ResistPoison,
        ResistRadiation,
        ResistGas,
        ResistElectricity      
    }

    public class SkillModifier
    {
        public int SkillModifierID { get; set; }
        public int ModifierID { get; set; }
        public string Skill { get; set; }
        public int Value { get; set; }

        public virtual Modifier Modifier { get; set; }
    }
    public class Trait
    {
        public int TraitID { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? ModifierID { get; set; }
        public virtual Modifier Mod { get; set; }

        public int CharacterID { get; set; }
        public virtual Character Character { get; set; }
    }
    public class Skill
    {
        public int SkillID { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Tagged { get; set; }
        public int PointsInvested { get; set; }
        public int BaseTotal { get; set; }

        public int CharacterID { get; set; }
        public virtual Character Character { get; set; }

        public int TotalPercent
        {
            get
            {
                return BaseTotal + PointsInvested;
            }
        }
    }
    public class Perk
    {
        public int PerkID { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? ModifierID { get; set; }
        public virtual Modifier Mod { get; set; }
        
        public int CharacterID { get; set; }
        public virtual Character Character { get; set; }
    }
    public class Equipment
    {
        public int EquipmentID { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Weight { get; set; }

        public int? ModifierID { get; set; }
        public virtual Modifier Mod { get; set; }

        public int CharacterID { get; set; }
        public virtual Character Character { get; set; }
    }
    public class Weapon : Equipment
    {
        public int MinimumStrength { get; set; }
        public decimal Damage { get; set; }
        public int Range { get; set; }
        public int APSingle { get; set; }
        public int APTargeted { get; set; }
        public int APBurst { get; set; }
        public int MagSize { get; set; }
    }
    
    public class Armor : Equipment
    {
        public ArmorSlot SlotType { get; set; }
        public int ArmorClass { get; set; }
        public virtual ArmorResistance ResistNormal { get; set; }
        public virtual ArmorResistance ResistLaser { get; set; }
        public virtual ArmorResistance ResistFire { get; set; }
        public virtual ArmorResistance ResistPlasma { get; set; }
        public virtual ArmorResistance ResistExplosion { get; set; }

        public Armor()
        {
            ResistNormal = new ArmorResistance();
            ResistLaser = new ArmorResistance();
            ResistFire = new ArmorResistance();
            ResistPlasma = new ArmorResistance();
            ResistExplosion = new ArmorResistance();
        }
    }

    public enum ArmorSlot
    { 
        Head = 1,
        Body = 2
    }
    
    public class ArmorResistance
    {
        public int ArmorResistanceID { get; set; }
        public int Threshold { get; set; }
        public int Resistance { get; set; }

        //public int ArmorID { get; set; }
        //public virtual Armor Armor { get; set; }
    }
}