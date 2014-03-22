using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fallout.Models.ViewModels
{
    public class CharacterSheet
    {
        public Character Character;
        private int characterlevel;
        private int hitPoints;
        private int sequence;
        private int healrate;
        private int crit;
        private int skillGain;
        private int armorClass;
        private int carryWeight;
        private int meleeDamage;
        private ArmorResistance resistNormal;
        private ArmorResistance resistLaser;
        private ArmorResistance resistFire;
        private ArmorResistance resistPlasma;
        private ArmorResistance resistExplosive;
        private int resistPoison;
        private int resistRadiation;
        private int resistGas;
        private int resistElectricity;
        public Armor HeadSlot { get; set; }
        public List<Armor> HeadOptions
        {
            get {
                List<Armor> head = new List<Armor>();
                foreach (Equipment item in Character.Gear)
                    if (item is Armor && ((Armor)item).SlotType == ArmorSlot.Head)
                        head.Add((Armor)item);
                return head;
            }
        }

        public Armor BodySlot { get; set; }
        public List<Armor> BodyOptions
        {
            get
            {
                List<Armor> body = new List<Armor>();
                foreach (Equipment item in Character.Gear)
                    if (item is Armor && ((Armor)item).SlotType == ArmorSlot.Body)
                        body.Add((Armor)item);
                return body;
            }
        }

        public Weapon RightHand { get; set; }
        public List<Weapon> WeaponOptions
        {
            get
            {
                List<Weapon> weapons = new List<Weapon>();
                foreach (Equipment item in Character.Gear)
                    if (item is Weapon)
                        weapons.Add((Weapon)item);
                return weapons;
            }
        }

        public Weapon LeftHand { get; set; }

        public CharacterSheet()
        {
            BodySlot = new Armor();
            HeadSlot = new Armor();
            RightHand = new Weapon();
            LeftHand = new Weapon();
        }

        #region Collates
        private List<Modifier> GetAllModifiers(bool Traits, bool Perks, bool Equipped, bool Cybernetics)
        {
            //Gather up all the modifier objects from the various lists
            List<Modifier> modlist = new List<Modifier>();
            if (Traits)
                foreach (Trait trait in Character.Traits)
                    if(trait.Mod != null)
                        modlist.Add(trait.Mod);
            if (Perks)
                foreach (Perk perk in Character.Perks)
                    if(perk.Mod != null)
                        modlist.Add(perk.Mod);
            if (Equipped)
            {
                if (Character.HeadArmor != null && Character.HeadArmor.Mod != null)
                    modlist.Add(Character.HeadArmor.Mod);
                if (Character.BodyArmor != null && Character.BodyArmor.Mod != null)
                    modlist.Add(Character.BodyArmor.Mod);
                if (Character.LeftHand != null && Character.LeftHand.Mod != null)
                    modlist.Add(Character.LeftHand.Mod);
                if (Character.RightHand != null && Character.RightHand.Mod != null)
                    modlist.Add(Character.RightHand.Mod);
            }
            if (Cybernetics && Character.Cybernetics != null && Character.Cybernetics.StaticMods != null)
                modlist.Add(Character.Cybernetics.StaticMods);
            return modlist;
        }

        public int CollateModifiers(ModifierType type)
        {
            return CollateModifiers(type, GetAllModifiers(true, true, true, true));
        }

        public int CollateModifiers(ModifierType type, List<Modifier> modlist)
        {
            int mod = 0;

            foreach (Modifier modifier in modlist)
            {
                if (modifier == null) continue;
                switch (type)
                {
                    case ModifierType.Strength:
                        mod += modifier.Strength;
                        break;
                    case ModifierType.Perception:
                        mod += modifier.Perception;
                        break;
                    case ModifierType.Endurance:
                        mod += modifier.Endurance;
                        break;
                    case ModifierType.Charisma:
                        mod += modifier.Charisma;
                        break;
                    case ModifierType.Intelligence:
                        mod += modifier.Intelligence;
                        break;
                    case ModifierType.Agility:
                        mod += modifier.Agility;
                        break;
                    case ModifierType.Luck:
                        mod += modifier.Luck;
                        break;
                    case ModifierType.ArmorClass:
                        mod += modifier.ArmorClass;
                        break;
                    case ModifierType.HitPoints:
                        mod += modifier.HitPoints;
                        break;
                    case ModifierType.CarryWeight:
                        mod += modifier.CarryWeight;
                        break;
                    case ModifierType.MeleeDamage:
                        mod += modifier.MeleeDamage;
                        break;
                    case ModifierType.ResistPoison:
                        mod += modifier.ResistPoison;
                        break;
                    case ModifierType.ResistRadiation:
                        mod += modifier.ResistRadiation;
                        break;
                    case ModifierType.ResistGas:
                        mod += modifier.ResistGas;
                        break;
                    case ModifierType.ResistElectricity:
                        mod += modifier.ResistElectricity;
                        break;


                }
            }

            return mod;
        }

        public ArmorResistance CollateModifiers(ArmorResistanceType type)
        {
            return CollateModifiers(type, GetAllModifiers(true, true, true, true));
        }

        public ArmorResistance CollateModifiers(ArmorResistanceType type, List<Modifier> modlist)
        {
            ArmorResistance res = new ArmorResistance();

            foreach (Modifier modifier in modlist)
            {
                switch (type)
                {
                    case ArmorResistanceType.ResistNormal:
                        res.Resistance += modifier.ResistNormal.Resistance;
                        res.Threshold += modifier.ResistNormal.Threshold;
                        break;
                    case ArmorResistanceType.ResistLaser:
                        res.Resistance += modifier.ResistLaser.Resistance;
                        res.Threshold += modifier.ResistLaser.Threshold;
                        break;
                    case ArmorResistanceType.ResistFire:
                        res.Resistance += modifier.ResistFire.Resistance;
                        res.Threshold += modifier.ResistFire.Threshold;
                        break;
                    case ArmorResistanceType.ResistPlasma:
                        res.Resistance += modifier.ResistPlasma.Resistance;
                        res.Threshold += modifier.ResistPlasma.Threshold;
                        break;
                    case ArmorResistanceType.ResistExplosive:
                        res.Resistance += modifier.ResistExplosive.Resistance;
                        res.Threshold += modifier.ResistExplosive.Threshold;
                        break;
                }
            }
            return res;
        }

        public int CollateModifiers(string SkillName)
        { 
            return CollateModifiers(SkillName, GetAllModifiers(true, true, true, true));
        }

        public int CollateModifiers(string SkillName, List<Modifier> modlist)
        {
            int mod = 0;
            
            foreach (Modifier modifier in modlist)
            {
                foreach (SkillModifier skillmod in modifier.Skill)
                {
                    if (skillmod.Skill == SkillName)
                        mod += skillmod.Value;
                }

            }
            return mod;
        }
        #endregion

        #region SPECIAL
        public int Strength
        {
            get
            {
                return CollateModifiers(ModifierType.Strength) + Character.Strength;
            }
        }
        public int Perception
        {
            get
            {
                return CollateModifiers(ModifierType.Perception) + Character.Perception;
            }
        }
        public int Endurance
        {
            get
            {
                return CollateModifiers(ModifierType.Endurance) + Character.Endurance;
            }
        }
        public int Charisma
        {
            get
            {
                return CollateModifiers(ModifierType.Charisma) + Character.Charisma;
            }
        }
        public int Intellegence
        {
            get
            {
                return CollateModifiers(ModifierType.Intelligence) + Character.Intelligence;
            }
        }
        public int Agility
        {
            get
            {
                return CollateModifiers(ModifierType.Agility) + Character.Agility;
            }
        }
        public int Luck
        {
            get
            {
                return CollateModifiers(ModifierType.Luck) + Character.Luck;
            }
        }
        public int CharacterLevel
        {
            get
            {
                if (Character.Experience < 1000)
                    return 1;
                else if (Character.Experience < 3000)
                    return 2;
                else if (Character.Experience < 6000)
                    return 3;
                else if (Character.Experience < 10000)
                    return 4;
                else if (Character.Experience < 15000)
                    return 5;
                else if (Character.Experience < 21000)
                    return 6;
                else if (Character.Experience < 28000)
                    return 7;
                else if (Character.Experience < 36000)
                    return 8;
                else if (Character.Experience < 45000)
                    return 9;
                else if (Character.Experience < 55000)
                    return 10;
                else if (Character.Experience < 66000)
                    return 11;
                else if (Character.Experience < 78000)
                    return 12;
                else if (Character.Experience < 91000)
                    return 13;
                else if (Character.Experience < 105000)
                    return 14;
                else if (Character.Experience < 120000)
                    return 15;
                else if (Character.Experience < 136000)
                    return 16;
                else if (Character.Experience < 153000)
                    return 17;
                else if (Character.Experience < 171000)
                    return 18;
                else if (Character.Experience < 190000)
                    return 19;
                else if (Character.Experience < 210000)
                    return 20;
                else
                    return 1;

            }
        }
        #endregion

        public int MaxHitPoints
        {
            get
            {
                int basehp;
                decimal levelhp, allLevelHp;
                basehp = 15 + (Character.Strength + (2 * Character.Endurance));
                levelhp = 3 + (Character.Endurance * .5m);
                allLevelHp = levelhp * CharacterLevel - levelhp;
                return basehp + (int)allLevelHp + CollateModifiers(ModifierType.HitPoints);
            }
        }

        public int SkillGain
        {
            get
            {
                return 5 + (2 * Character.Intelligence);
            }
        }

        public int CarryWeight
        {
            get
            {
                return 25 + (25 * Character.Strength) + CollateModifiers(ModifierType.CarryWeight);
            }
        }

        public int ArmorClassBody
        {
            get
            {
                int baseac, armorac, mods;
                baseac = Character.Agility;
                armorac = 0;
                if (BodySlot != null)
                    armorac += BodySlot.ArmorClass;
                mods = CollateModifiers(ModifierType.ArmorClass);
                return baseac + armorac + mods;
            }
        }

        public int ArmorClassHead
        {
            get
            {
                int baseac, armorac, mods;
                baseac = Character.Agility;
                armorac = 0;
                if (Character.HeadArmor != null)
                    armorac += Character.HeadArmor.ArmorClass;
                mods = CollateModifiers(ModifierType.ArmorClass);
                return baseac + armorac + mods;
            }
        }

        public int MeleeDamage
        {
            get
            {
                int basedmg, mods;
                if (Strength < 7)
                    basedmg = 1;
                else if (Strength == 7)
                    basedmg = 2;
                else if (Strength == 8)
                    basedmg = 3;
                else if (Strength == 9)
                    basedmg = 4;
                else if (Strength == 10)
                    basedmg = 5;
                else if (Strength == 11)
                    basedmg = 6;
                else if (Strength == 12)
                    basedmg = 7;
                else
                    basedmg = 1;

                mods = CollateModifiers(ModifierType.MeleeDamage);

                return basedmg + mods;
            }
        }

        #region Resistances
        public ArmorResistance ResistNormalBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();
                armorresist = new ArmorResistance();
                if (Character.BodyArmor != null)
                    armorresist = Character.BodyArmor.ResistNormal;

                mods = CollateModifiers(ArmorResistanceType.ResistNormal, GetAllModifiers(true, true, false, true));
                resists.Resistance = baseresist.Resistance + armorresist.Resistance + mods.Resistance;
                resists.Threshold = baseresist.Threshold + armorresist.Threshold + mods.Threshold;
                return resists;
            }
        }
        #endregion

        public int SkillTotal(string SkillName)
        {
            int total = 0;
            //Points
            foreach (Skill skill in Character.Skills)
            {
                if (skill.Name == SkillName)
                {
                    total += skill.PointsInvested;
                    if (skill.Tagged)
                        total += 20;
                }
                
            }
            //Base
            total += SkillBase(SkillName);
            //Mods
            total += CollateModifiers(SkillName);
            
            return total;
        }

        public int SkillBase(string SkillName)
        {
            //should include stats from traits and cybernetics
            List<Modifier> modlist = GetAllModifiers(true, false, false, true);
            int modStrength = Character.Strength + CollateModifiers(ModifierType.Strength, modlist);
            int modPerception = Character.Perception + CollateModifiers(ModifierType.Perception, modlist);
            int modEndurance = Character.Endurance + CollateModifiers(ModifierType.Endurance, modlist);
            int modCharisma = Character.Charisma + CollateModifiers(ModifierType.Charisma, modlist);
            int modIntelligence = Character.Intelligence + CollateModifiers(ModifierType.Intelligence, modlist);
            int modAgility = Character.Agility + CollateModifiers(ModifierType.Agility, modlist);
            int modLuck = Character.Luck + CollateModifiers(ModifierType.Luck, modlist);
            
            int basepoints = 0;
            switch (SkillName)
            {
                case "Small Guns":
                    basepoints = 5 + (4 * modAgility);
                    break;
                case "Big Guns":
                    basepoints = 0 + (2 * modAgility);
                    break;
                case "Energy Weapons":
                    basepoints = 0 + (2 * modAgility);
                    break;
                case "Unarmed":
                    basepoints = 30 + (2 * (modAgility + modStrength));
                    break;
                case "Melee":
                    basepoints = 20 + (2 * (modAgility + modStrength));
                    break;
                case "Throwing":
                    basepoints = 0 + (4 * modAgility);
                    break;
                case "First Aid":
                    basepoints = 0 + (2 * (modPerception + modEndurance));
                    break;
                case "Doctor":
                    basepoints = 5 + (modPerception + modIntelligence);
                    break;
                case "Sneak":
                    basepoints = 5 + (3 * modAgility);
                    break;
                case "Lockpick":
                    basepoints = 10 + (modPerception + modAgility);
                    break;
                case "Steal":
                    basepoints = 0 + (3 * modAgility);
                    break;
                case "Traps":
                    basepoints = 10 + (modPerception + modAgility);
                    break;
                case "Science":
                    basepoints = 5 + (4 * modAgility);
                    break;
                case "Repair":
                    basepoints = 0 + (3 * modIntelligence);
                    break;
                case "Pilot":
                    basepoints = 0 + (2 * (modAgility + modPerception));
                    break;
                case "Speech":
                    basepoints = 0 + (5 * modCharisma);
                    break;
                case "Barter":
                    basepoints = 0 + (4 * modCharisma);
                    break;
                case "Gambling":
                    basepoints = 0 + (4 * modLuck);
                    break;
                case "Outdoorsman":
                    basepoints = 0 + (2 * (modEndurance + modIntelligence));
                    break;
            }

            return basepoints;
        }
    }
}