using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fallout.Models;
using System.Data.Entity;
using Fallout.Models.ViewModels;

namespace Fallout.Controllers
{
    public class GameController : Controller
    {
        GameContext db = new GameContext();

        //
        // GET: /Game/

        public ActionResult Index()
        {
            var Chars = from m in db.Characters
                        select m;
            return View(Chars.ToList());
        }

        #region Create Character
        //
        // GET: /Game/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Game/Create

        [HttpPost]
        public ActionResult Create(Character character)
        {
            try
            {
                // TODO: Add insert logic here

                db.Characters.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion


        //
        // GET: /Game/Delete/5

        public ActionResult Delete(int id)
        {
            Character player = db.Characters.Find(id);
            player.Traits.Clear();
            player.Skills.Clear();
            player.Perks.Clear();
            player.Gear.Clear();

            db.Characters.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        //
        // GET: /Game/Inventory/2
        public ActionResult Inventory(long id)
        {
            Character thecharacter = db.Characters.Find(id);
            return View(thecharacter);
        }

        [HttpPost]
        public ActionResult RemoveInventory(long CharacterID, int EquipmentID)
        {
            Character thecharacter = db.Characters.Find(CharacterID);
            thecharacter.Gear.Remove(thecharacter.Gear.First(e => e.EquipmentID == EquipmentID));
            if (thecharacter.HeadArmorID == EquipmentID)
                thecharacter.HeadArmorID = null;
            if (thecharacter.BodyArmorID == EquipmentID)
                thecharacter.BodyArmorID = null;
            if (thecharacter.LeftHandID == EquipmentID)
                thecharacter.LeftHandID = null;
            if (thecharacter.RightHandID == EquipmentID)
                thecharacter.RightHandID = null;
            db.SaveChanges();
            return RedirectToAction("Inventory", new { id = CharacterID });

        }

        public ActionResult AddInventory()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddInventory(long id, Equipment equipment)
        {
            Character player = db.Characters.Find(id);
            player.Gear.Add(equipment);
            db.SaveChanges();
            return RedirectToAction("Inventory", new { id = id });
        }
        //
        // GET: /Game/Character

        public ActionResult Character(int id)
        {
            var Chars = from m in db.Characters
                        where m.CharacterID == id
                        select m;

            Fallout.Models.ViewModels.CharacterSheet sheet = new Models.ViewModels.CharacterSheet();
            sheet.Character = Chars.FirstOrDefault();
            if (sheet.Character.HeadArmorID != null)
            {
                sheet.HeadSlot = (Armor)sheet.Character.Gear.First(e => e.EquipmentID == sheet.Character.HeadArmorID);
            }
            if (sheet.Character.BodyArmorID != null)
            {
                sheet.BodySlotID = sheet.Character.BodyArmorID;
                sheet.BodySlot = (Armor)sheet.Character.Gear.First(e => e.EquipmentID == sheet.Character.BodyArmorID);
            }
            if (sheet.Character.LeftHandID != null)
            {
                sheet.LeftHandID = sheet.Character.LeftHandID;
                sheet.LeftHand = (Weapon)sheet.Character.Gear.First(e => e.EquipmentID == sheet.Character.LeftHandID);
            }
            if (sheet.Character.RightHandID != null)
            {
                sheet.RightHandID = sheet.Character.RightHandID;
                sheet.RightHand = (Weapon)sheet.Character.Gear.First(e => e.EquipmentID == sheet.Character.RightHandID);
            }
            return View(sheet);
        }

        #region SetEquippedItems
        
        public ActionResult EquipItem(int CharacterID, int? BodySlotID, int? HeadSlotID, int? LeftHandID, int? RightHandID, EquipmentSlot EquipmentType )
        {
            Character character = db.Characters.Find(CharacterID);
            switch (EquipmentType)
            {
                case EquipmentSlot.Head:
                    character.HeadArmorID = HeadSlotID;
                    break;
                case EquipmentSlot.Body:
                    character.BodyArmorID = BodySlotID;
                    break;
                case EquipmentSlot.LeftHand:
                    character.LeftHandID = LeftHandID;
                    break;
                case EquipmentSlot.RightHand:
                    character.RightHandID = RightHandID;
                    break;
            }
            db.SaveChanges();

            return RedirectToAction("Character", new { id = CharacterID });
        }

        #endregion


        #region Seeds
        //
        // GET: /Game/SetSkills/2

        
        public ActionResult SetSkills(int id)
        {
            Character character = db.Characters.Find(id);
            List<Skill> skilllist = new List<Skill>();

            List<Skill> oldskills = new List<Skill>();
            foreach (Skill killskill in character.Skills)
            {
                oldskills.Add(killskill);
            }
            foreach(Skill theskill in oldskills)
            {
                db.Entry(theskill).State = EntityState.Deleted;
            }
            db.SaveChanges();
            Skill addskill = new Skill();
            //Small Guns
            addskill = new Skill();
            addskill.Name = "Small Guns";
            addskill.Description = "The ability to use small guns and rifles";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Big Guns
            addskill = new Skill();
            addskill.Name = "Big Guns";
            addskill.Description = "The ability to use big guns";
            character.Skills.Add(addskill);
            //Energy Weapons
            addskill = new Skill();
            addskill.Name = "Energy Weapons";
            addskill.Description = "The ability to use energy weapons";
            character.Skills.Add(addskill);
            //Unarmed
            addskill = new Skill();
            addskill.Name = "Unarmed";
            addskill.Description = "The ability to fight unarmed";
            character.Skills.Add(addskill);
            //Melee
            addskill = new Skill();
            addskill.Name = "Melee";
            addskill.Description = "The ability to use melee weapons";
            character.Skills.Add(addskill);
            //Throwing
            addskill = new Skill();
            addskill.Name = "Throwing";
            addskill.Description = "The ability to use thrown weapons";
            character.Skills.Add(addskill);
            //First Aid
            addskill = new Skill();
            addskill.Name = "First Aid";
            addskill.Description = "The ability to heal minor wounds";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Doctor
            addskill = new Skill();
            addskill.Name = "Doctor";
            addskill.Description = "The ability to heal major wounds";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Sneak
            addskill = new Skill();
            addskill.Name = "Sneak";
            addskill.Description = "The ability to move silent and not be seen";
            character.Skills.Add(addskill);
            //Lockpick
            addskill = new Skill();
            addskill.Name = "Lockpick";
            addskill.Description = "The ability to pick doors and chests";
            character.Skills.Add(addskill);
            //Steal
            addskill = new Skill();
            addskill.Name = "Steal";
            addskill.Description = "The ability to steal things unnoticed";
            character.Skills.Add(addskill);
            //Traps
            addskill = new Skill();
            addskill.Name = "Traps";
            addskill.Description = "The ability to lay traps";
            character.Skills.Add(addskill);
            //Science
            addskill = new Skill();
            addskill.Name = "Science";
            addskill.Description = "The ability to invent new things";
            character.Skills.Add(addskill);
            //Repair
            addskill = new Skill();
            addskill.Name = "Repair";
            addskill.Description = "The ability to repair guns and robots";
            character.Skills.Add(addskill);
            //Pilot
            addskill = new Skill();
            addskill.Name = "Pilot";
            addskill.Description = "The ability to drive cars and planes";
            character.Skills.Add(addskill);
            //Speech
            addskill = new Skill();
            addskill.Name = "Speech";
            addskill.Description = "The ability to talk to people well";
            character.Skills.Add(addskill);
            //Barter
            addskill = new Skill();
            addskill.Name = "Barter";
            addskill.Description = "The ability to negotiate trades and sales";
            character.Skills.Add(addskill);
            //Gambling
            addskill = new Skill();
            addskill.Name = "Gambling";
            addskill.Description = "The ability to bend lady luck to you favor";
            character.Skills.Add(addskill);
            //Outdoorsman
            addskill = new Skill();
            addskill.Name = "Outdoorsman";
            addskill.Description = "The ability to live off the land, know dangerous spots";
            character.Skills.Add(addskill);
            db.SaveChanges();
            return RedirectToAction("Character", new { id = id });
        }

        //
        // GET: /Game/WeaponSeed

        public ActionResult WeaponSeed(int id)
        {
            Character character = db.Characters.Find(id);

            Weapon wpn = new Weapon();

            //small gun
            wpn = new Weapon();
            wpn.Name = "Colt 6520 10mm Pistol";
            wpn.Description = "An outloading pistol, each pull of the trigger will automatically reload the firearm until the magazine is empty.";
            wpn.Value = 250;
            wpn.Weight = 4;
            wpn.MinimumStrength = 3;
            wpn.Damage = 6;
            wpn.Range = 19;
            wpn.APSingle = 5;
            wpn.APTargeted = 6;
            wpn.APBurst = -1;

            character.Gear.Add(wpn);
            //big gun
            wpn = new Weapon();
            wpn.Name = "Browning M2 Minigun";
            wpn.Description = "Originally designed as a tripod-mounted weapon in the last stages of World War I, the Browning was later adapted for infantry use as the first true minigun.";
            wpn.Value = 3000;
            wpn.Weight = 40;
            wpn.MinimumStrength = 7;
            wpn.Damage = 8;
            wpn.Range = 20;
            wpn.APSingle = -1;
            wpn.APTargeted = -1;
            wpn.APBurst = 7;

            character.Gear.Add(wpn);
            //energy weapon
            wpn = new Weapon();
            wpn.Name = "Wattz 1600 Laser Pistol";
            wpn.Description = "The Laser Pistol is perhaps the simplest of the energy weapons.";
            wpn.Value = 1400;
            wpn.Weight = 7;
            wpn.MinimumStrength = 3;
            wpn.Damage = 10;
            wpn.Range = 35;
            wpn.APSingle = 5;
            wpn.APTargeted = 6;
            wpn.APBurst = -1;

            character.Gear.Add(wpn);
            //melee weapon
            wpn = new Weapon();
            wpn.Name = "Spear";
            wpn.Description = "Your basic polearm. A wooden pole with a sharpened piece of metal on the end.";
            wpn.Value = 80;
            wpn.Weight = 4;
            wpn.MinimumStrength = 4;
            wpn.Damage = 3;
            wpn.Range = 2;
            wpn.APSingle = 4;
            wpn.APTargeted = 5;
            wpn.APBurst = -1;

            character.Gear.Add(wpn);

            db.SaveChanges();
            return RedirectToAction("Character", new { id = id });
        }

        //
        // GET: /Game/ArmorSeed

        public ActionResult ArmorSeed(int id)
        {
            Character character = db.Characters.Find(id);

            Armor armor = new Armor();
            Modifier mod = new Modifier();
            ArmorResistance res = new ArmorResistance();
            armor.Name = "Leather Armor";
            armor.Description = "A shirt made of leather and padded for extra protection.";
            armor.Value = 700;
            armor.Weight = 8;
            armor.SlotType = ArmorSlot.Body;
            mod = new Modifier();
            mod.ArmorClass = 15;
            //Normal
            res = new ArmorResistance();
            res.Threshold = 2;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Normal;
            mod.ArmorResists.Add(res);
            //Laser
            res = new ArmorResistance();
            res.Threshold = 0;
            res.Resistance = 20;
            res.ResistanceType = ResistType.Laser;
            mod.ArmorResists.Add(res);
            //Fire
            res = new ArmorResistance();
            res.Threshold = 0;
            res.Resistance = 20;
            res.ResistanceType = ResistType.Fire;
            mod.ArmorResists.Add(res);
            //Plasma
            res = new ArmorResistance();
            res.Threshold = 0;
            res.Resistance = 10;
            res.ResistanceType = ResistType.Plasma;
            mod.ArmorResists.Add(res);
            //Explosion
            res = new ArmorResistance();
            res.Threshold = 0;
            res.Resistance = 20;
            res.ResistanceType = ResistType.Explosion;
            mod.ArmorResists.Add(res);
            armor.Mod = mod;
            character.Gear.Add(armor);


            armor = new Armor();
            mod = new Modifier();
            res = new ArmorResistance();
            armor.Name = "Football Padding";
            armor.Description = "Football Padding";
            armor.Value = 1000;
            armor.Weight = 15;
            armor.SlotType = ArmorSlot.Body;
            mod = new Modifier();
            mod.ArmorClass = 15;
            //Normal
            res = new ArmorResistance();
            res.Threshold = 2;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Normal;
            mod.ArmorResists.Add(res);
            //Laser
            res = new ArmorResistance();
            res.Threshold = 1;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Laser;
            mod.ArmorResists.Add(res);
            //Fire
            res = new ArmorResistance();
            res.Threshold = 2;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Fire;
            mod.ArmorResists.Add(res);
            //Plasma
            res = new ArmorResistance();
            res.Threshold = 5;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Plasma;
            mod.ArmorResists.Add(res);
            //Explosion
            res = new ArmorResistance();
            res.Threshold = 5;
            res.Resistance = 20;
            res.ResistanceType = ResistType.Explosion;
            mod.ArmorResists.Add(res);
            armor.Mod = mod;
            character.Gear.Add(armor);

            //armor = new Armor();
            //armor.Name = "Metal Armor";
            //armor.Description = "A jacket of armor made from pieces of scrap metal welded together.";
            //armor.Value = 1100;
            //armor.Weight = 35;
            //armor.SlotType = ArmorSlot.Body;
            //mod = new Modifier();
            //mod.ArmorClass = 10;
            //mod.ResistNormal.Resistance = 30;
            //mod.ResistNormal.Threshold = 4;
            //mod.ResistLaser.Resistance = 75;
            //mod.ResistLaser.Threshold = 6;
            //mod.ResistFire.Resistance = 10;
            //mod.ResistFire.Threshold = 4;
            //mod.ResistPlasma.Resistance = 20;
            //mod.ResistPlasma.Threshold = 4;
            //mod.ResistExplosion.Resistance = 25;
            //mod.ResistExplosion.Threshold = 4;
            //armor.Mod = mod;
            //SkillModifier skillmod = new SkillModifier();
            //skillmod.Skill = "Sneak";
            //skillmod.Value = -25;
            //mod.Skill.Add(skillmod);
            

            //character.Gear.Add(armor);

            //armor = new Armor();
            //armor.Name = "Leather Cap";
            //armor.Description = "A simple cap, made from tanned Brahmin hide.";
            //armor.Value = 90;
            //armor.Weight = 0;
            //armor.SlotType = ArmorSlot.Head;
            //mod = new Modifier();
            //mod.ArmorClass = 3;
            //mod.ResistNormal.Resistance = 0;
            //mod.ResistNormal.Threshold = 0;
            //mod.ResistLaser.Resistance = 0;
            //mod.ResistLaser.Threshold = 0;
            //mod.ResistFire.Resistance = 0;
            //mod.ResistFire.Threshold = 0;
            //mod.ResistPlasma.Resistance = 0;
            //mod.ResistPlasma.Threshold = 0;
            //mod.ResistExplosion.Resistance = 0;
            //mod.ResistExplosion.Threshold = 0;
            //armor.Mod = mod;
            //character.Gear.Add(armor);

            //armor = new Armor();
            //armor.Name = "Combat Helmet";
            //armor.Description = "Part of a suit of combat armor, this helmet is made of Kevlar and reinforced plastics.";
            //armor.Value = 500;
            //armor.Weight = 0;
            //armor.SlotType = ArmorSlot.Head;
            //mod = new Modifier();
            //mod.ArmorClass = 9;
            //mod.ResistNormal.Resistance = 0;
            //mod.ResistNormal.Threshold = 0;
            //mod.ResistLaser.Resistance = 0;
            //mod.ResistLaser.Threshold = 0;
            //mod.ResistFire.Resistance = 0;
            //mod.ResistFire.Threshold = 0;
            //mod.ResistPlasma.Resistance = 0;
            //mod.ResistPlasma.Threshold = 0;
            //mod.ResistExplosion.Resistance = 0;
            //mod.ResistExplosion.Threshold = 0;
            //armor.Mod = mod;
            //character.Gear.Add(armor);

            //armor = new Armor();
            //armor.Name = "No Helmet";
            //armor.Description = "May your hair keep you warm";
            //armor.Value = 0;
            //armor.Weight = 0;
            //armor.SlotType = ArmorSlot.Head;
            //mod = new Modifier();
            //mod.ArmorClass = 0;
            //mod.ResistNormal.Resistance = 0;
            //mod.ResistNormal.Threshold = 0;
            //mod.ResistLaser.Resistance = 0;
            //mod.ResistLaser.Threshold = 0;
            //mod.ResistFire.Resistance = 0;
            //mod.ResistFire.Threshold = 0;
            //mod.ResistPlasma.Resistance = 0;
            //mod.ResistPlasma.Threshold = 0;
            //mod.ResistExplosion.Resistance = 0;
            //mod.ResistExplosion.Threshold = 0;
            //armor.Mod = mod;
            //character.Gear.Add(armor);

            //armor = new Armor();
            //armor.Name = "No Armor";
            //armor.Description = "Those are some snazzy clothes you have on.";
            //armor.Value = 0;
            //armor.Weight = 0;
            //armor.SlotType = ArmorSlot.Body;
            //mod = new Modifier();
            //mod.ArmorClass = 0;
            //mod.ResistNormal.Resistance = 0;
            //mod.ResistNormal.Threshold = 0;
            //mod.ResistLaser.Resistance = 0;
            //mod.ResistLaser.Threshold = 0;
            //mod.ResistFire.Resistance = 0;
            //mod.ResistFire.Threshold = 0;
            //mod.ResistPlasma.Resistance = 0;
            //mod.ResistPlasma.Threshold = 0;
            //mod.ResistExplosion.Resistance = 0;
            //mod.ResistExplosion.Threshold = 0;
            //armor.Mod = mod;
            //character.Gear.Add(armor);
            //armor = new Armor();
            //armor.Name = "Robe";
            //armor.Description = "Those are some snazzy clothes you have on.";
            //armor.Value = 1;
            //armor.Weight = 2;
            //armor.SlotType = ArmorSlot.Body;
            //mod = new Modifier();
            //mod.ArmorClass = 5;
            //mod.ResistNormal.Resistance = 20;
            //mod.ResistNormal.Threshold = 0;
            //mod.ResistLaser.Resistance = 25;
            //mod.ResistLaser.Threshold = 0;
            //mod.ResistFire.Resistance = 10;
            //mod.ResistFire.Threshold = 0;
            //mod.ResistPlasma.Resistance = 10;
            //mod.ResistPlasma.Threshold = 0;
            //mod.ResistExplosion.Resistance = 10;
            //mod.ResistExplosion.Threshold = 0;
            //armor.Mod = mod;
            //character.Gear.Add(armor);
            db.SaveChanges();
            return RedirectToAction("Character", new { id = id });
        }

        public ActionResult TraitSeed(int id)
        {
            Trait trait = new Trait();
            trait.Name = "Good Mannered";
            trait.Description = "Good at medicy-talky, bad at shooty stabby";
            trait.Mod = new Modifier();
            SkillModifier smallguns = new SkillModifier();
            smallguns.Skill = "Small Guns";
            smallguns.Value = -10;
            trait.Mod.Skill.Add(smallguns);
            SkillModifier bigguns = new SkillModifier();
            bigguns.Skill = "Big Guns";
            bigguns.Value = -10;
            trait.Mod.Skill.Add(bigguns);
            SkillModifier energy = new SkillModifier();
            energy.Skill = "Energy Weapons";
            energy.Value = -10;
            trait.Mod.Skill.Add(energy);
            SkillModifier unarmed = new SkillModifier();
            unarmed.Skill = "Unarmed";
            unarmed.Value = -10;
            trait.Mod.Skill.Add(unarmed);
            SkillModifier melee = new SkillModifier();
            melee.Skill = "Melee";
            melee.Value = -10;
            trait.Mod.Skill.Add(melee);
            SkillModifier firstaid = new SkillModifier();
            firstaid.Skill = "First Aid";
            firstaid.Value = 20;
            trait.Mod.Skill.Add(firstaid);
            SkillModifier doctor = new SkillModifier();
            doctor.Skill = "Doctor";
            doctor.Value = 20;
            trait.Mod.Skill.Add(doctor);
            SkillModifier speech = new SkillModifier();
            speech.Skill = "Speech";
            speech.Value = 20;
            trait.Mod.Skill.Add(speech);
            SkillModifier barter = new SkillModifier();
            barter.Skill = "Barter";
            barter.Value = 20;
            trait.Mod.Skill.Add(barter);

            Character character = db.Characters.Find(id);
            character.Traits = new List<Trait>();
            character.Traits.Add(trait);
            db.SaveChanges();

            return RedirectToAction("Character", new { id = id });
        }

        public ActionResult CyberneticSeed(int id)
        {
            Cybernetics cyber = new Cybernetics();
            Modifier mod = new Modifier();
            mod.ResistRadiation = 80;
            mod.Intelligence = 2;
            mod.ResistGasDR = 70;
            mod.Strength = 1;
            mod.Endurance = 1;
            mod.Agility = -1;
            mod.ArmorClass = 5;
            ArmorResistance res = new ArmorResistance();
            //Normal
            res = new ArmorResistance();
            res.Threshold = 3;
            res.Resistance = 20;
            res.ResistanceType = ResistType.Normal;
            mod.ArmorResists.Add(res);
            //Laser
            res = new ArmorResistance();
            res.Threshold = 1;
            res.Resistance = 10;
            res.ResistanceType = ResistType.Laser;
            mod.ArmorResists.Add(res);
            //Fire
            res = new ArmorResistance();
            res.Threshold = 1;
            res.Resistance = 10;
            res.ResistanceType = ResistType.Fire;
            mod.ArmorResists.Add(res);
            //Plasma
            res = new ArmorResistance();
            res.Threshold = 0;
            res.Resistance = 5;
            res.ResistanceType = ResistType.Plasma;
            mod.ArmorResists.Add(res);
            //Explosion
            res = new ArmorResistance();
            res.Threshold = 5;
            res.Resistance = 25;
            res.ResistanceType = ResistType.Explosion;
            mod.ArmorResists.Add(res);

            SkillModifier skill = new SkillModifier();
            skill.Skill = "Small Guns";
            skill.Value = 10;
            mod.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Big Guns";
            skill.Value = 10;
            mod.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Energy Weapons";
            skill.Value = 10;
            mod.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Throwing";
            skill.Value = 10;
            mod.Skill.Add(skill);
            cyber.StaticMods = mod;
            Character character = db.Characters.Find(id);
            character.Cybernetics = cyber;
            db.SaveChanges();

            return RedirectToAction("Character", new { id = id });
        }

        public ActionResult CharacterSeed()
        {
            Character character = new Character();

            character.PlayerName = "Nicholas";
            character.CharacterName = "Samuel";
            character.Age = 32;
            character.Sex = "Male";
            character.Race = "Cyborg";
            character.Eyes = "Green";
            character.Hair = "Brown";
            character.Height = 6;
            character.Weight = 199;
            character.Description = "Crazy Healer Dude";
            character.Strength = 4;
            character.Perception = 7;
            character.Endurance = 7;
            character.Charisma = 3;
            character.Intelligence = 8;
            character.Agility = 7;
            character.Luck = 4;
            character.Experience = 1087;
            character.CurrentActionPoints = 0;
            character.CurrentHitPoints = 0;

            character.Traits = new List<Trait>();
            character.Skills = new List<Skill>();
            character.Perks = new List<Perk>();
            character.Gear = new List<Equipment>();

            db.Characters.Add(character);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        #endregion

    }
}
