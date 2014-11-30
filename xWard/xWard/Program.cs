using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace xWard
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
    
      
     
        
        public static Items.Item WardS, WardN, TrinketN, SightStone;
        public static Menu xMenu;
        private static readonly Obj_AI_Hero player = ObjectManager.Player;

        private static void Game_OnGameLoad(EventArgs args)
        {

            WardS = new Items.Item(2043, 600f);
            WardN = new Items.Item(2044, 600f);
            TrinketN = new Items.Item(3340, 600f);
            SightStone = new Items.Item(2049, 600f);

            xMenu = new Menu("xWard", "xWard", true);

            
            xMenu.AddSubMenu(new Menu("Ward", "Ward"));
            xMenu.SubMenu("Ward").AddItem(new MenuItem("Draww", "Draw Ward Locations").SetValue(true));
            xMenu.SubMenu("Ward").AddItem(new MenuItem("placew", "Place Ward").SetValue(new KeyBind('Z', KeyBindType.Press)));
            




           

            xMenu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;

            Game.PrintChat("xWard");
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (xMenu.Item("placew").GetValue<KeyBind>().Active)
            {
                PlaceW();
            }



        }

        static void Drawing_OnDraw(EventArgs args)
        {
            
            var circleRange = 125f;

            if (xMenu.Item("Draww").GetValue<bool>())
            {
                Utility.DrawCircle(new Vector3(2524, 10406, 54f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(1774, 10756, 52f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(5520, 6342, 51f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(5674, 7358, 51f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(7990, 4282, 53f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(8256, 2920, 51f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(4818, 10866, -71f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(6824, 10656, 55f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(6574, 12006, 56f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(9130, 8346, 53f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(9422, 7408, 52f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(12372, 4508, 51f), circleRange, Color.Blue);
                Utility.DrawCircle(new Vector3(13003, 3818, 51f), circleRange, Color.Blue);



                Utility.DrawCircle(new Vector3(2729, 10879, -71f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(2303, 10868, 53f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(5223, 6789, 50f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(5191, 7137, 50f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(8368, 4594, 51f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(8100, 3429, 51f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(4634, 11283, 49f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(6672, 11466, 53f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(6518, 10367, 53f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(9572, 8038, 57f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(9697, 7854, 51f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(12235, 4068, -68f), circleRange, Color.Green);
                Utility.DrawCircle(new Vector3(12443, 4021, -7f), circleRange, Color.Green);
            }

if (xMenu.Item("Draww").GetValue<bool>())
{
            Utility.DrawCircle(new Vector3(2524, 10406, 54f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(1774, 10756, 52f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(5520, 6342, 51f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(5674, 7358, 51f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(7990, 4282, 53f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(8256, 2920, 51f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(4818, 10866, -71f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(6824, 10656, 55f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(6574, 12006, 56f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(9130, 8346, 53f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(9422, 7408, 52f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(12372, 4508, 51f), circleRange, Color.Blue);
            Utility.DrawCircle(new Vector3(13003, 3818, 51f), circleRange, Color.Blue);



            Utility.DrawCircle(new Vector3(2729, 10879, -71f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(2303, 10868, 53f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(5223, 6789, 50f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(5191, 7137, 50f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(8368, 4594, 51f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(8100, 3429, 51f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(4634, 11283, 49f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(6672, 11466, 53f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(6518, 10367, 53f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(9572, 8038, 57f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(9697, 7854, 51f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(12235, 4068, -68f), circleRange, Color.Green);
            Utility.DrawCircle(new Vector3(12443, 4021, -7f), circleRange, Color.Green); 
}


            }

        private static void PlaceW()
        {
           if (TrinketN.IsReady())
           {
                TrinketN.Cast(new Vector3(2729, 10879, -71f));
                
                TrinketN.Cast(new Vector3(2303, 10868, 53f));
                TrinketN.Cast(new Vector3(5223, 6789, 50f));
                TrinketN.Cast(new Vector3(5191, 7137, 50f));
                TrinketN.Cast(new Vector3(8368, 4594, 51f));
                TrinketN.Cast(new Vector3(8100, 3429, 51f));
                TrinketN.Cast(new Vector3(4634, 11283, 49f));
                TrinketN.Cast(new Vector3(6672, 11466, 53f));
                TrinketN.Cast(new Vector3(6518, 10367, 53f ));
                TrinketN.Cast(new Vector3(9572, 8038, 57f ));
                TrinketN.Cast(new Vector3(9697, 7854, 51f));
                TrinketN.Cast(new Vector3(12235, 4068, -68f));
               TrinketN.Cast(new Vector3(12443, 4021, -7f));
               return;
           }

            if (SightStone.IsReady())
            {
                SightStone.Cast(new Vector3(2729, 10879, -71f));
                SightStone.Cast(new Vector3(2303, 10868, 53f));
                SightStone.Cast(new Vector3(5223, 6789, 50f));
                SightStone.Cast(new Vector3(5191, 7137, 50f));
                SightStone.Cast(new Vector3(8368, 4594, 51f));
                SightStone.Cast(new Vector3(8100, 3429, 51f));
                SightStone.Cast(new Vector3(4634, 11283, 49f));
                SightStone.Cast(new Vector3(6672, 11466, 53f));
                SightStone.Cast(new Vector3(6518, 10367, 53f ));
                SightStone.Cast(new Vector3(9572, 8038, 57f ));
                SightStone.Cast(new Vector3(9697, 7854, 51f));
                SightStone.Cast(new Vector3(12235, 4068, -68f));
                SightStone.Cast(new Vector3(12443, 4021, -7f));
                return;
            }

            if (WardS.IsReady())
            {
                WardS.Cast(new Vector3(2729, 10879, -71f));
                WardS.Cast(new Vector3(2303, 10868, 53f));
                WardS.Cast(new Vector3(5223, 6789, 50f));
                WardS.Cast(new Vector3(5191, 7137, 50f));
                WardS.Cast(new Vector3(8368, 4594, 51f));
                WardS.Cast(new Vector3(8100, 3429, 51f));
                WardS.Cast(new Vector3(4634, 11283, 49f));
                WardS.Cast(new Vector3(6672, 11466, 53f));
                WardS.Cast(new Vector3(6518, 10367, 53f ));
                WardS.Cast(new Vector3(9572, 8038, 57f ));
                WardS.Cast(new Vector3(9697, 7854, 51f));
                WardS.Cast(new Vector3(12235, 4068, -68f));
                WardS.Cast(new Vector3(12443, 4021, -7f));
                return;
            }
            if (WardN.IsReady())
            {
                WardN.Cast(new Vector3(2729, 10879, -71f));
                WardN.Cast(new Vector3(2303, 10868, 53f));
                WardN.Cast(new Vector3(5223, 6789, 50f));
                WardN.Cast(new Vector3(5191, 7137, 50f));
                WardN.Cast(new Vector3(8368, 4594, 51f));
                WardN.Cast(new Vector3(8100, 3429, 51f));
                WardN.Cast(new Vector3(4634, 11283, 49f));
                WardN.Cast(new Vector3(6672, 11466, 53f));
                WardN.Cast(new Vector3(6518, 10367, 53f ));
                WardN.Cast(new Vector3(9572, 8038, 57f));
                WardN.Cast(new Vector3(9697, 7854, 51f));
                WardN.Cast(new Vector3(12235, 4068, -68f));
                WardN.Cast(new Vector3(12443, 4021, -7f));
                return;
            }




        }






        }

    }

   

