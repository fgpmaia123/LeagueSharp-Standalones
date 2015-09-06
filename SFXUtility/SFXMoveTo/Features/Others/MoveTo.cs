#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 MoveTo.cs is part of SFXMoveTo.

 SFXMoveTo is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXMoveTo is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXMoveTo. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SFXMoveTo.Classes;
using SFXMoveTo.Library.Logger;

#endregion

namespace SFXMoveTo.Features.Others
{
    internal class MoveTo : Child<App>
    {
        private float _lastCheck = Environment.TickCount;

        public MoveTo(App parent) : base(parent)
        {
            OnLoad();
        }

        public override string Name
        {
            get { return "Move To"; }
        }

        protected override void OnEnable()
        {
            Game.OnUpdate += OnGameUpdate;

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= OnGameUpdate;

            base.OnDisable();
        }

        private void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (!Menu.Item(Name + "Hotkey").GetValue<KeyBind>().Active)
                {
                    return;
                }
                if (_lastCheck + new Random().Next(125, 500) > Environment.TickCount)
                {
                    return;
                }
                _lastCheck = Environment.TickCount;

                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
            catch (Exception ex)
            {
                Global.Logger.AddItem(new LogItem(ex));
            }
        }

        protected override sealed void OnLoad()
        {
            try
            {
                Menu = new Menu(Name, Name);
                Menu.AddItem(new MenuItem(Name + "Hotkey", "Hotkey").SetValue(new KeyBind('G', KeyBindType.Press)));

                Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));

                Parent.Menu.AddSubMenu(Menu);
            }
            catch (Exception ex)
            {
                Global.Logger.AddItem(new LogItem(ex));
            }
        }
    }
}