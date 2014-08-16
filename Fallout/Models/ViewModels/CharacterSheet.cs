using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fallout.Models;
using System.Web.Mvc;

namespace Fallout.Models.ViewModels
{
    public class CharacterSheet
    {
        public Character Character;
        public Armor HeadSlot { get; set; }
        public Armor BodySlot { get; set; }
        public Weapon RightHand { get; set; }
        public Weapon LeftHand { get; set; }
        
        public int? BodySlotID { get; set; }
        public int? HeadSlotID { get; set; }
        public int? LeftHandID { get; set; }
        public int? RightHandID { get; set; }
        public int? LeftHandAmmoID { get; set; }
        public int? RightHandAmmoID { get; set; }

        public CharacterSheet()
        {
            BodySlot = new Armor();
            HeadSlot = new Armor();
            RightHand = new Weapon();
            LeftHand = new Weapon();
        }

        
        #region DDL Fillers
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
        #endregion
        #region Collaters
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
                if (HeadSlot != null && HeadSlot.Mod != null)
                    modlist.Add(HeadSlot.Mod);
                if (BodySlot != null && BodySlot.Mod != null)
                    modlist.Add(BodySlot.Mod);
                //if (LeftHand != null && LeftHand.Mod != null)
                //    modlist.Add(LeftHand.Mod);
                //if (RightHand != null && RightHand.Mod != null)
                //    modlist.Add(RightHand.Mod);
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
                    case ModifierType.ResistGasDT:
                        mod += modifier.ResistGasDT;
                        break;
                    case ModifierType.ResistGasDR:
                        mod += modifier.ResistGasDR;
                        break;
                    case ModifierType.ResistBurn:
                        mod += modifier.ResistBurn;
                        break;
                    case ModifierType.ResistElectricity:
                        mod += modifier.ResistElectricity;
                        break;


                }
            }

            return mod;
        }

        public ArmorResistance CollateModifiers(ResistType type)
        {
            return CollateModifiers(type, GetAllModifiers(true, true, true, true));
        }

        public ArmorResistance CollateModifiers(ResistType type, List<Modifier> modlist)
        {
            ArmorResistance res = new ArmorResistance();

            foreach (Modifier modifier in modlist)
            {
                foreach (ArmorResistance mod in modifier.ArmorResists)
                {
                    if (mod.ResistanceType == type)
                    {
                        res.Threshold += mod.Threshold;
                        res.Resistance += mod.Resistance;
                    }  
                }


                //switch (type)
                //{
                //    case ArmorResistanceType.ResistNormal:
                //        res.Resistance += modifier.ResistNormal.Resistance;
                //        res.Threshold += modifier.ResistNormal.Threshold;
                //        break;
                //    case ArmorResistanceType.ResistLaser:
                //        res.Resistance += modifier.ResistLaser.Resistance;
                //        res.Threshold += modifier.ResistLaser.Threshold;
                //        break;
                //    case ArmorResistanceType.ResistFire:
                //        res.Resistance += modifier.ResistFire.Resistance;
                //        res.Threshold += modifier.ResistFire.Threshold;
                //        break;
                //    case ArmorResistanceType.ResistPlasma:
                //        res.Resistance += modifier.ResistPlasma.Resistance;
                //        res.Threshold += modifier.ResistPlasma.Threshold;
                //        break;
                //    case ArmorResistanceType.ResistExplosive:
                //        res.Resistance += modifier.ResistExplosion.Resistance;
                //        res.Threshold += modifier.ResistExplosion.Threshold;
                //        break;
                //}
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
                basehp = 15 + (Strength + (2 * Endurance));
                levelhp = 3 + (Endurance * .5m);
                allLevelHp = levelhp * (CharacterLevel - 1);
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
                baseac = Agility;
                armorac = 0;
                if (BodySlot != null && BodySlot.Mod != null)
                    armorac += BodySlot.Mod.ArmorClass;
                mods = CollateModifiers(ModifierType.ArmorClass, GetAllModifiers(true, true, false, true));
                return baseac + armorac + mods;
            }
        }

        public int ArmorClassHead
        {
            get
            {
                int baseac, armorac, mods;
                baseac = Agility;
                armorac = 0;
                if (BodySlot != null)
                    armorac += BodySlot.Mod.ArmorClass;
                mods = CollateModifiers(ModifierType.ArmorClass);
                return baseac + armorac + mods;
            }
        }

        public int Sequence
        {
            get { 
                int basesq = 2 * Perception;
                return basesq;
            }
        }

        public int ActionPoints
        {
            get { 
                int ap = 5;
                if (Agility < 2)
                    ap = 5;
                else if (Agility < 4)
                    ap = 6;
                else if (Agility < 6)
                    ap = 7;
                else if (Agility < 8)
                    ap = 8;
                else if (Agility < 10)
                    ap = 9;
                else if (Agility >= 10)
                    ap = 10;
                return ap;
            }
        }

        public int Critical
        {
            get
            {
                int crit = Luck;
                return crit;
            }
        }

        public int PERange
        {
            get
            {
                return (Perception * 2) - 1;
            }
        }

        public int HealingRate
        {
            get
            {
                int heal = 0;
                if (Endurance < 6)
                    heal = 1;
                else if (Endurance < 9)
                    heal = 2;
                else if (Endurance < 11)
                    heal = 3;
                else
                    heal = 4;
                return heal;
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
        public int ResistPoison
        {
            get
            {
                return CollateModifiers(ModifierType.ResistPoison);
            }
        }
        public int ResistRadiation
        {
            get
            {
                return CollateModifiers(ModifierType.ResistRadiation);
            }
        }
        public int ResistElectricity
        {
            get
            {
                return CollateModifiers(ModifierType.ResistElectricity);
            }
        }
        public int ResistGasDT
        {
            get
            {
                return CollateModifiers(ModifierType.ResistGasDT);
            }
        }
        public int ResistGasDR
        {
            get
            {
                return CollateModifiers(ModifierType.ResistGasDR);
            }
        }
        public int ResistBurn
        {
            get
            {
                return CollateModifiers(ModifierType.ResistBurn);
            }
        }
        public ArmorResistance ResistNormalBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();

                armorresist = new ArmorResistance();
                if (BodySlot != null && BodySlot.Mod != null)
                    foreach(ArmorResistance res in BodySlot.Mod.ArmorResists)
                        if (res.ResistanceType == ResistType.Normal)
                            armorresist = res;

                mods = CollateModifiers(ResistType.Normal, GetAllModifiers(true, true, false, true));
                
                resists.Resistance = baseresist.Resistance + armorresist.Resistance + mods.Resistance;
                resists.Threshold = baseresist.Threshold + armorresist.Threshold + mods.Threshold;
                return resists;
            }
        }

        public ArmorResistance ResistLaserBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();

                armorresist = new ArmorResistance();
                if (BodySlot != null && BodySlot.Mod != null)
                    foreach (ArmorResistance res in BodySlot.Mod.ArmorResists)
                        if (res.ResistanceType == ResistType.Laser)
                            armorresist = res;

                mods = CollateModifiers(ResistType.Laser, GetAllModifiers(true, true, false, true));

                resists.Resistance = baseresist.Resistance + armorresist.Resistance + mods.Resistance;
                resists.Threshold = baseresist.Threshold + armorresist.Threshold + mods.Threshold;
                return resists;
            }
        }

        public ArmorResistance ResistFireBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();

                armorresist = new ArmorResistance();
                if (BodySlot != null && BodySlot.Mod != null)
                    foreach (ArmorResistance res in BodySlot.Mod.ArmorResists)
                        if (res.ResistanceType == ResistType.Fire)
                            armorresist = res;

                mods = CollateModifiers(ResistType.Fire, GetAllModifiers(true, true, false, true));

                resists.Resistance = baseresist.Resistance + armorresist.Resistance + mods.Resistance;
                resists.Threshold = baseresist.Threshold + armorresist.Threshold + mods.Threshold;
                return resists;
            }
        }

        public ArmorResistance ResistPlasmaBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();

                armorresist = new ArmorResistance();
                if (BodySlot != null && BodySlot.Mod != null)
                    foreach (ArmorResistance res in BodySlot.Mod.ArmorResists)
                        if (res.ResistanceType == ResistType.Plasma)
                            armorresist = res;

                mods = CollateModifiers(ResistType.Plasma, GetAllModifiers(true, true, false, true));

                resists.Resistance = baseresist.Resistance + armorresist.Resistance + mods.Resistance;
                resists.Threshold = baseresist.Threshold + armorresist.Threshold + mods.Threshold;
                return resists;
            }
        }

        public ArmorResistance ResistExplosionBody
        {
            get
            {
                ArmorResistance resists = new ArmorResistance();
                ArmorResistance baseresist, armorresist, mods;
                baseresist = new ArmorResistance();

                armorresist = new ArmorResistance();
                if (BodySlot != null && BodySlot.Mod != null)
                    foreach (ArmorResistance res in BodySlot.Mod.ArmorResists)
                        if (res.ResistanceType == ResistType.Explosion)
                            armorresist = res;

                mods = CollateModifiers(ResistType.Explosion, GetAllModifiers(true, true, false, true));

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