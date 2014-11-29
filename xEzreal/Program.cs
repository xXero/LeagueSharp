﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace Ezreal
{
    class Program
    {
        public static string ChampName = "Ezreal";
        public static Orbwalking.Orbwalker Orbwalker;
        private static readonly Obj_AI_Hero player = ObjectManager.Player;
        public static Spell Q, W, R;
        public static SpellSlot IgniteSlot;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        public static Menu xMenu;


        private static void Game_OnGameLoad(EventArgs args)
        {
            if (player.BaseSkinName != ChampName) return;

            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 900);

            R = new Spell(SpellSlot.R, float.MaxValue);

            Q.SetSkillshot(0.5f, 80f, 1200, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.5f, 80f, 1200, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000, false, SkillshotType.SkillshotLine);

            IgniteSlot = player.GetSpellSlot("SummonerDot");

         
            xMenu = new Menu("x" + ChampName, ChampName, true);
          
            xMenu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(xMenu.SubMenu("Orbwalker"));
          
            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            xMenu.AddSubMenu(ts);
           
            xMenu.AddSubMenu(new Menu("Combo", "Combo"));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            xMenu.AddSubMenu(new Menu("Harass", "Harass"));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("hQ", "Use Q?").SetValue(true));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("hW", "Use W?").SetValue(true));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("Harassmana", "Mana to use").SetValue(new Slider(30)));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("HarassActive", "Harass").SetValue(new KeyBind('C', KeyBindType.Press)));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("HarassToogle", "Harass").SetValue(new KeyBind('T', KeyBindType.Toggle, false)));

            xMenu.AddSubMenu(new Menu("Killsteal", "Killsteal"));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillQ", "Steal with Q?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillW", "Steal with W?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillR", "Steal with R?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillI", "Steal with Ignite?").SetValue(true));

            xMenu.AddSubMenu(new Menu("Drawing", "Drawing"));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawQ", "Draw Q?").SetValue(true));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawW", "Draw W?").SetValue(true));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawAA", "Draw Range?").SetValue(true));
            


            
            xMenu.AddItem(new MenuItem("Packet", "Packet Casting").SetValue(true));
           
            xMenu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate; 

            Game.PrintChat("x" + ChampName);
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (xMenu.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }

            if (xMenu.Item("HarassActive").GetValue<KeyBind>().Active || xMenu.Item("HarassToggle").GetValue<KeyBind>().Active)
            {
                Harass();
            }

            KillSteal();
        }


        private static float GetIgniteDamage(Obj_AI_Hero enemy)
        {
            if (IgniteSlot == SpellSlot.Unknown || player.SummonerSpellbook.CanUseSpell(IgniteSlot) != SpellState.Ready) return 0f;
            return (float)player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (xMenu.Item("DrawQ").GetValue<bool>() == true)
            {
                Utility.DrawCircle(player.Position, Q.Range, Color.Red);
            }

            if (xMenu.Item("DrawW").GetValue<bool>() == true)
            {
                Utility.DrawCircle(player.Position, W.Range, Color.Orange);
            }
            if (xMenu.Item("DrawAA").GetValue<bool>() == true)
            {
                Utility.DrawCircle(player.Position, player.AttackRange, Color.Blue);
            }
            




        }

        public static void KillSteal()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("KillQ").GetValue<bool>() == true && ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
            {
                Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());
            }

            if (target.IsValidTarget(W.Range) && W.IsReady() && xMenu.Item("KillW").GetValue<bool>() == true && ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) > target.Health)
            {
                W.Cast(target, xMenu.Item("Packet").GetValue<bool>());
            }

            if (target.IsValidTarget(3000) && R.IsReady() && xMenu.Item("KillR").GetValue<bool>() == true && ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health)
            {
                R.Cast(target, xMenu.Item("Packet").GetValue<bool>());
            }


            if (xMenu.Item("KillI").GetValue<bool>() == true)
            {
                if (IgniteSlot != SpellSlot.Unknown &&
                    player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                {
                    if (target.Health + 20 <= GetIgniteDamage(target))
                    {
                        player.SummonerSpellbook.CastSpell(IgniteSlot, target);
                    }
                }

            }




        }



        public static void Harass()
        {
            
            if (player.Mana / player.MaxMana * 100 < xMenu.SubMenu("Harass").Item("harassmana").GetValue<Slider>().Value)
                return;

            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null)
                return;
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("hQ").GetValue<bool>() == true)
                {
                    Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                }

                if (target.IsValidTarget(W.Range) && W.IsReady() && xMenu.Item("hW").GetValue<bool>() == true)
                {
                    W.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                }


            }






        }


        public static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("useQ").GetValue<bool>() == true)
            {
                Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());



            }

            if (target.IsValidTarget(W.Range) && W.IsReady() && xMenu.Item("useW").GetValue<bool>() == true)
            {
                W.Cast(target, xMenu.Item("Packet").GetValue<bool>());
            }



            if (target.IsValidTarget(R.Range) && R.IsReady() && ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health && xMenu.Item("useR").GetValue<bool>() == true)
            {
                R.CastOnUnit(target, xMenu.Item("Packet").GetValue<bool>());
            }








        }
    }
}
