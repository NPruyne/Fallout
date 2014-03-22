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

        //
        // GET: /Game/Details/5

        public ActionResult Details(int id)
        {
            var Chars = from m in db.Characters
                        where m.CharacterID == id
                        select m;

            return View(Chars.FirstOrDefault());
        }

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

        //
        // GET: /Game/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Game/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Character character)
        {
            try
            {
                // TODO: Add update logic here
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Game/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Game/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Character character)
        {
            try
            {
                db.Characters.Remove(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
            if (sheet.Character.HeadArmor != null) sheet.HeadSlot = sheet.Character.HeadArmor;
            if (sheet.Character.BodyArmor != null) sheet.BodySlot = sheet.Character.BodyArmor;
            return View(sheet);
        }

        // POST: /Game/SetHeadItem

        
        public ActionResult SetHeadItem(int CharacterID, int HeadSlot )
        {
            Character character = db.Characters.Find(CharacterID);

            character.HeadArmor = (Armor)character.Gear.First(m => m.EquipmentID == HeadSlot);
            
            db.SaveChanges();

            return RedirectToAction("Character", new { id = CharacterID });
        }


        public ActionResult SetBodyItem(int CharacterID, int BodySlot)
        {
            Character character = db.Characters.Find(CharacterID);

            character.BodyArmor = (Armor)character.Gear.First(m => m.EquipmentID == BodySlot);

            db.SaveChanges();

            return RedirectToAction("Character", new { id = CharacterID });
        }

        // GET: /Game/SetRightHand

        public ActionResult SetRightHand(int EquipmentID, Character character)
        { 
            Character thecharacter = db.Characters.Find(character);
            //get equipment
            Equipment item = thecharacter.Gear.First(g => g.EquipmentID == EquipmentID);
            thecharacter.RightHand = (Weapon)item;
            db.SaveChanges();

            return RedirectToAction("Character", new { id = thecharacter.CharacterID });
            
        }


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
            addskill.CharacterID = id;
            addskill.Name = "Small Guns";
            addskill.Description = "The ability to use small guns and rifles";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Big Guns
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Big Guns";
            addskill.Description = "The ability to use big guns";
            character.Skills.Add(addskill);
            //Energy Weapons
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Energy Weapons";
            addskill.Description = "The ability to use energy weapons";
            character.Skills.Add(addskill);
            //Unarmed
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Unarmed";
            addskill.Description = "The ability to fight unarmed";
            character.Skills.Add(addskill);
            //Melee
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Melee";
            addskill.Description = "The ability to use melee weapons";
            character.Skills.Add(addskill);
            //Throwing
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Throwing";
            addskill.Description = "The ability to use thrown weapons";
            character.Skills.Add(addskill);
            //First Aid
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "First Aid";
            addskill.Description = "The ability to heal minor wounds";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Doctor
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Doctor";
            addskill.Description = "The ability to heal major wounds";
            addskill.Tagged = true;
            character.Skills.Add(addskill);
            //Sneak
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Sneak";
            addskill.Description = "The ability to move silent and not be seen";
            character.Skills.Add(addskill);
            //Lockpick
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Lockpick";
            addskill.Description = "The ability to pick doors and chests";
            character.Skills.Add(addskill);
            //Steal
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Steal";
            addskill.Description = "The ability to steal things unnoticed";
            character.Skills.Add(addskill);
            //Traps
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Traps";
            addskill.Description = "The ability to lay traps";
            character.Skills.Add(addskill);
            //Science
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Science";
            addskill.Description = "The ability to invent new things";
            character.Skills.Add(addskill);
            //Repair
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Repair";
            addskill.Description = "The ability to repair guns and robots";
            character.Skills.Add(addskill);
            //Pilot
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Pilot";
            addskill.Description = "The ability to drive cars and planes";
            character.Skills.Add(addskill);
            //Speech
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Speech";
            addskill.Description = "The ability to talk to people well";
            character.Skills.Add(addskill);
            //Barter
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Barter";
            addskill.Description = "The ability to negotiate trades and sales";
            character.Skills.Add(addskill);
            //Gambling
            addskill = new Skill();
            addskill.CharacterID = id;
            addskill.Name = "Gambling";
            addskill.Description = "The ability to bend lady luck to you favor";
            character.Skills.Add(addskill);
            //Outdoorsman
            addskill = new Skill();
            addskill.CharacterID = id;
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
            armor.Name = "Leather Armor";
            armor.Description = "A shirt made of leather and padded for extra protection.";
            armor.Value = 700;
            armor.Weight = 8;
            armor.SlotType = ArmorSlot.Body;
            armor.ArmorClass = 15;
            armor.ResistNormal.Resistance = 2;
            armor.ResistNormal.Threshold = 25;
            armor.ResistLaser.Resistance = 0;
            armor.ResistLaser.Threshold = 20;
            armor.ResistFire.Resistance = 0;
            armor.ResistFire.Threshold = 20;
            armor.ResistPlasma.Resistance = 0;
            armor.ResistPlasma.Threshold = 10;
            armor.ResistExplosion.Resistance = 0;
            armor.ResistExplosion.Threshold = 20;

            character.Gear.Add(armor);

            armor = new Armor();
            armor.Name = "Metal Armor";
            armor.Description = "A jacket of armor made from pieces of scrap metal welded together.";
            armor.Value = 1100;
            armor.Weight = 35;
            armor.SlotType = ArmorSlot.Body;
            armor.ArmorClass = 10;
            armor.ResistNormal.Resistance = 4;
            armor.ResistNormal.Threshold = 30;
            armor.ResistLaser.Resistance = 6;
            armor.ResistLaser.Threshold = 75;
            armor.ResistFire.Resistance = 4;
            armor.ResistFire.Threshold = 10;
            armor.ResistPlasma.Resistance = 4;
            armor.ResistPlasma.Threshold = 20;
            armor.ResistExplosion.Resistance = 4;
            armor.ResistExplosion.Threshold = 25;
            Modifier mod = new Modifier();
            SkillModifier skillmod = new SkillModifier();
            skillmod.Skill = "Sneak";
            skillmod.Value = -25;
            mod.Skill.Add(skillmod);
            armor.Mod = mod;

            character.Gear.Add(armor);

            armor = new Armor();
            armor.Name = "Leather Cap";
            armor.Description = "A simple cap, made from tanned Brahmin hide.";
            armor.Value = 90;
            armor.Weight = 0;
            armor.SlotType = ArmorSlot.Head;
            armor.ArmorClass = 3;
            armor.ResistNormal.Resistance = 0;
            armor.ResistNormal.Threshold = 0;
            armor.ResistLaser.Resistance = 0;
            armor.ResistLaser.Threshold = 0;
            armor.ResistFire.Resistance = 0;
            armor.ResistFire.Threshold = 0;
            armor.ResistPlasma.Resistance = 0;
            armor.ResistPlasma.Threshold = 0;
            armor.ResistExplosion.Resistance = 0;
            armor.ResistExplosion.Threshold = 0;

            character.Gear.Add(armor);

            armor = new Armor();
            armor.Name = "Combat Helmet";
            armor.Description = "Part of a suit of combat armor, this helmet is made of Kevlar and reinforced plastics.";
            armor.Value = 500;
            armor.Weight = 0;
            armor.SlotType = ArmorSlot.Head;
            armor.ArmorClass = 9;
            armor.ResistNormal.Resistance = 0;
            armor.ResistNormal.Threshold = 0;
            armor.ResistLaser.Resistance = 0;
            armor.ResistLaser.Threshold = 0;
            armor.ResistFire.Resistance = 0;
            armor.ResistFire.Threshold = 0;
            armor.ResistPlasma.Resistance = 0;
            armor.ResistPlasma.Threshold = 0;
            armor.ResistExplosion.Resistance = 0;
            armor.ResistExplosion.Threshold = 0;

            character.Gear.Add(armor);

            armor = new Armor();
            armor.Name = "No Helmet";
            armor.Description = "May your hair keep you warm";
            armor.Value = 0;
            armor.Weight = 0;
            armor.SlotType = ArmorSlot.Head;
            armor.ArmorClass = 0;
            armor.ResistNormal.Resistance = 0;
            armor.ResistNormal.Threshold = 0;
            armor.ResistLaser.Resistance = 0;
            armor.ResistLaser.Threshold = 0;
            armor.ResistFire.Resistance = 0;
            armor.ResistFire.Threshold = 0;
            armor.ResistPlasma.Resistance = 0;
            armor.ResistPlasma.Threshold = 0;
            armor.ResistExplosion.Resistance = 0;
            armor.ResistExplosion.Threshold = 0;

            character.Gear.Add(armor);

            armor = new Armor();
            armor.Name = "No Armor";
            armor.Description = "Those are some snazzy clothes you have on.";
            armor.Value = 0;
            armor.Weight = 0;
            armor.SlotType = ArmorSlot.Body;
            armor.ArmorClass = 0;
            armor.ResistNormal.Resistance = 0;
            armor.ResistNormal.Threshold = 0;
            armor.ResistLaser.Resistance = 0;
            armor.ResistLaser.Threshold = 0;
            armor.ResistFire.Resistance = 0;
            armor.ResistFire.Threshold = 0;
            armor.ResistPlasma.Resistance = 0;
            armor.ResistPlasma.Threshold = 0;
            armor.ResistExplosion.Resistance = 0;
            armor.ResistExplosion.Threshold = 0;

            character.Gear.Add(armor);

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
            cyber.StaticMods = new Modifier();
            cyber.StaticMods.ResistRadiation = 80;
            cyber.StaticMods.Intelligence = 2;
            cyber.StaticMods.ResistGas = 70;
            cyber.StaticMods.Strength = 1;
            cyber.StaticMods.Endurance = 1;
            cyber.StaticMods.Agility = -1;
            cyber.StaticMods.ArmorClass = 5;
            cyber.StaticMods.ResistNormal.Resistance = 20;
            cyber.StaticMods.ResistNormal.Threshold = 3;
            cyber.StaticMods.ResistLaser.Resistance = 10;
            cyber.StaticMods.ResistLaser.Threshold = 1;
            cyber.StaticMods.ResistFire.Resistance = 10;
            cyber.StaticMods.ResistFire.Threshold = 1;
            cyber.StaticMods.ResistPlasma.Resistance = 5;
            cyber.StaticMods.ResistExplosive.Resistance = 25;
            cyber.StaticMods.ResistExplosive.Threshold = 5;


            cyber.StaticMods.Skill = new List<SkillModifier>();
            SkillModifier skill = new SkillModifier();
            skill.Skill = "Small Guns";
            skill.Value = 10;
            cyber.StaticMods.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Big Guns";
            skill.Value = 10;
            cyber.StaticMods.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Energy Weapons";
            skill.Value = 10;
            cyber.StaticMods.Skill.Add(skill);
            skill = new SkillModifier();
            skill.Skill = "Throwing";
            skill.Value = 10;
            cyber.StaticMods.Skill.Add(skill);

            Character character = db.Characters.Find(id);
            character.Cybernetics = cyber;
            db.SaveChanges();

            return RedirectToAction("Character", new { id = id });
        }

    }
}
