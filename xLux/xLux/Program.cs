﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace xLux
{
    class Program
    {
        public static string ChampName = "Lux";
        public static Orbwalking.Orbwalker Orbwalker;
        private static readonly Obj_AI_Hero player = ObjectManager.Player;
        public static Spell Q, E, E2, R;
        private static GameObject E2TargetObject;
        public static SpellSlot IgniteSlot;
        public static Items.Item Dfg;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        public static Menu xMenu;


        private static void Game_OnGameLoad(EventArgs args)
        {
            if (player.BaseSkinName != ChampName) return;

            Q = new Spell(SpellSlot.Q, 1175);
            E = new Spell(SpellSlot.E, 1075);
            
            R = new Spell(SpellSlot.R, 3340);

            Q.SetSkillshot(0.5f, 80f, 1200, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.15f, 275f, 1300f, false, SkillshotType.SkillshotCircle);
           
            R.SetSkillshot(1.75f, 190f, 3000, false, SkillshotType.SkillshotLine);
            Dfg = new Items.Item(3128, 750f);

            IgniteSlot = player.GetSpellSlot("SummonerDot");

            
            xMenu = new Menu("x" + ChampName, ChampName, true);
          
            xMenu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(xMenu.SubMenu("Orbwalker"));
            
            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            xMenu.AddSubMenu(ts);
           
            xMenu.AddSubMenu(new Menu("Combo", "Combo"));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useE", "Use E?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("useItems", "Use Items?").SetValue(true));
            xMenu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            xMenu.AddSubMenu(new Menu("Harass", "Harass"));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("hQ", "Harass with Q?").SetValue(true));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("hE", "Harras with E").SetValue(true));
           
            xMenu.SubMenu("Harass").AddItem(new MenuItem("HarassActive", "Harass").SetValue(new KeyBind('C', KeyBindType.Press)));
            xMenu.SubMenu("Harass").AddItem(new MenuItem("HarassToggle", "Harass").SetValue(new KeyBind('T', KeyBindType.Toggle)));

            xMenu.AddSubMenu(new Menu("Laneclear", "Laneclear"));
            xMenu.SubMenu("Laneclear").AddItem(new MenuItem("laneclearW", "Clear with E?").SetValue(true));
            xMenu.SubMenu("Laneclear").AddItem(new MenuItem("laneclearnum", "Number of Minions").SetValue(new Slider(2, 1, 5)));
            xMenu.SubMenu("Laneclear").AddItem(new MenuItem("LaneclearActive", "Laneclear Active").SetValue(new KeyBind('V', KeyBindType.Press)));
            



            xMenu.AddSubMenu(new Menu("Killsteal", "Killsteal"));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillQ", "Steal with Q?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillE", "Steal with E?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillR", "Steal with R?").SetValue(true));
            xMenu.SubMenu("Killsteal").AddItem(new MenuItem("KillI", "Steal with Ignite?").SetValue(true));

            xMenu.AddSubMenu(new Menu("Drawing", "Drawing"));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawQ", "Draw Q?").SetValue(true));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawE", "Draw E?").SetValue(true));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawR", "DrawR?").SetValue(true));
            xMenu.SubMenu("Drawing").AddItem(new MenuItem("DrawAA", "Draw Range?").SetValue(true));

            xMenu.AddSubMenu(new Menu("Misc", "Misc"));
            xMenu.SubMenu("Misc").AddItem(new MenuItem("Packet", "Packet Casting").SetValue(true));
            xMenu.SubMenu("Misc").AddItem(new MenuItem("JSteal", "Jungle Steal").SetValue(true));

         
            xMenu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Game.PrintChat("x" + ChampName);
            GameObject.OnCreate += OnCreateObject;
            GameObject.OnDelete += OnDeleteObject;	
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

            if (xMenu.Item("LaneclearActive").GetValue<KeyBind>().Active)
            {
                Laneclear();
            }
            JungleSteal();
            KillSteal();
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("LuxLightstrike_tar_green"))
            {
                E2TargetObject = sender;
                return;
            }
        }

        private static void OnDeleteObject(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("LuxLightstrike_tar_green"))
            {
                E2TargetObject = null;
                return;
            }
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
                Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, Color.Red);
            }

            if (xMenu.Item("DrawE").GetValue<bool>() == true)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, E.Range, Color.Orange);
            }

            if (xMenu.Item("DrawR").GetValue<bool>() == true)
            {
                Utility.DrawCircle(player.Position, R.Range, Color.Blue, 5, 30, true);
            }

            if (xMenu.Item("DrawAA").GetValue<bool>() == true)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange, Color.Red);
            }

        }

        private static float GetDistanceSqr(Obj_AI_Hero source, Obj_AI_Base Target)
        {
            return Vector2.DistanceSquared(source.Position.To2D(), Target.ServerPosition.To2D());
        }


        private static float ComboDamage(Obj_AI_Base enemy)
        {
            double damage = 0d;

            if (Dfg.IsReady())
                damage += player.GetItemDamage(enemy, Damage.DamageItems.Dfg) / 1.2;

            if (Q.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.Q);

            if (R.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.R);

            if (Dfg.IsReady())
                damage = damage * 1.2;

            if (E.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.E);

            if (IgniteSlot != SpellSlot.Unknown && player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                damage += ObjectManager.Player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);

            if (Items.HasItem(3155, (Obj_AI_Hero)enemy))
            {
                damage = damage - 250;
            }

            if (Items.HasItem(3156, (Obj_AI_Hero)enemy))
            {
                damage = damage - 400;
            }
            return (float)damage;
        }

        private static void Laneclear()
        {
            if (xMenu.SubMenu("Laneclear").Item("laneclearW").GetValue<bool>() && E.IsReady())
            {
                var farmLocation = MinionManager.GetBestCircularFarmLocation(MinionManager.GetMinions(player.Position, E.Range).Select(minion => minion.ServerPosition.To2D()).ToList(), E.Width, E.Range);

                if (farmLocation.MinionsHit >= xMenu.SubMenu("Laneclear").Item("laneclearnum").GetValue<Slider>().Value && player.Distance(farmLocation.Position) <= E.Range)
                    E.Cast(farmLocation.Position);
                CastE2();
            }
        }

        private static void JungleSteal()
        {
            var Minions = MinionManager.GetMinions(Game.CursorPos, 1000, MinionTypes.All, MinionTeam.Neutral);
            foreach (var minion in Minions.Where(minion => minion.IsVisible && !minion.IsDead))
            {
                if ((minion.SkinName == "SRU_Blue" || minion.SkinName == "SRU_Red" || minion.SkinName == "SRU_Baron" || minion.SkinName == "SRU_Dragon") &&
                                ComboDamage(minion) > minion.Health && xMenu.Item("JSteal").GetValue<bool>()== true)
                {
                    if (Q.IsReady() && GetDistanceSqr(player, minion) <= Q.Range * Q.Range) Q.Cast(minion, xMenu.Item("Packet").GetValue<bool>());
                    if (E.IsReady() && GetDistanceSqr(player, minion) <= E.Range * E.Range)
                    {
                        E.Cast(minion, xMenu.Item("Packet").GetValue<bool>());
                        while (E2TargetObject != null)
                        {
                            E.CastOnUnit(player, xMenu.Item("Packet").GetValue<bool>());
                            break;
                        }
                    }
                    if (R.IsReady() && minion.IsValidTarget(R.Range)) R.Cast(minion, xMenu.Item("Packet").GetValue<bool>());
                }
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

            if (target.IsValidTarget(E.Range) && E.IsReady() && xMenu.Item("KillE").GetValue<bool>() == true && ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > target.Health)
            {
                E.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                CastE2();
            }

            if (target.IsValidTarget(R.Range) && R.IsReady() && xMenu.Item("KillR").GetValue<bool>() == true && ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health)
            {
                R.Cast(target, xMenu.Item("Packet").GetValue<bool>());
            }


            if (xMenu.Item("KillI").GetValue<bool>() == true)
            {
                if (IgniteSlot != SpellSlot.Unknown &&
                    player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                {
                    if (target.Health <= GetIgniteDamage(target))
                    {
                        player.SummonerSpellbook.CastSpell(IgniteSlot, target);
                    }
                }

            }




        }


        public static void Harass()
        {
            


            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null)
                return;
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("hQ").GetValue<bool>() == true)
                {
                    Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                }

                if (target.IsValidTarget(E.Range) && E.IsReady() && xMenu.Item("hE").GetValue<bool>() == true)
                {
                    E.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                    CastE2();
                }
                if (target.IsValidTarget(550) && target.HasBuff("luxilluminatingfraulein"))
                {
                    player.IssueOrder(GameObjectOrder.AttackUnit, target);
                }


            }
        }

        public static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            float dmg = ComboDamage(target);

            if (dmg > target.Health + 20)
            {
                if (Dfg.IsReady() && xMenu.Item("useItems").GetValue<bool>() == true)
                {
                    Dfg.Cast(target);
                }

                if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("useQ").GetValue<bool>() == true)
                {
                    Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                    


                }

                if (target.IsValidTarget(E.Range) && E.IsReady() && xMenu.Item("useE").GetValue<bool>() == true)
                {
                    E.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                    CastE2();
                }
                if (target.IsValidTarget(550) && target.HasBuff("luxilluminatingfraulein"))
                {
                    player.IssueOrder(GameObjectOrder.AttackUnit, target);
                }


                if (target.IsValidTarget(R.Range) && R.IsReady() && target.HasBuff("LuxLightBindingMis") && target.Health + 20 < player.GetSpellDamage(target, SpellSlot.R))
                {
                    R.CastOnUnit(target, xMenu.Item("Packet").GetValue<bool>());
                }
                
            }

            else
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady() && xMenu.Item("useQ").GetValue<bool>() == true)
                {
                    Q.Cast(target, xMenu.Item("Packet").GetValue<bool>());



                }

                if (target.IsValidTarget(E.Range) && E.IsReady() && xMenu.Item("useE").GetValue<bool>() == true)
                {
                    E.Cast(target, xMenu.Item("Packet").GetValue<bool>());
                    CastE2();
                }
            }}

            

             private static void CastE2()
		{
        	if (E2TargetObject == null) return;
        	foreach (Obj_AI_Hero current in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget() && enemy.IsEnemy &&
        	                          Vector3.Distance(E2TargetObject.Position, enemy.ServerPosition) <= E.Width+15))
        	{
				E.CastOnUnit(player);	
				return;
        	}
			if (Vector3.Distance(player.Position, E2TargetObject.Position) > 800)	E.CastOnUnit(player);
		}







        }

    }
