using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;

namespace Trinita.Core.Native
{

    public class Button
    {
        private string _name;
        private Action _action;
        public Button(string name, Action action)
        {
            _name = name;
            _action = action;
        }

        public string Name { get => _name; set => _name = value; }
        public Action Action { get => _action; set => _action = value; }
    }

    public class Menu
    {      
        private int _nr ;
        private int _selected;
        private int _currentMenu;
        private int _previousMenu;
        private string _title;

        private List<object> _options;

        public Menu(string title)
        {
            _nr = 10;
            _selected = 0;
            _currentMenu = -1;
            _title = title;
            _options = new List<object>();
        }

        private Menu(string title, Button[] buttons) : this(title)
        {
            _options.AddRange(buttons);
        }

        public void Draw(float x = 0.875f, float y = 0.5f, float width = 0.15f, float height = 0.50f)
        {

            if (Input.JustPressed(1, Control.GameMenuDown)) _selected = MathT.Clamp(++_selected, 0, _options.Count - 1);
            if (Input.JustPressed(1, Control.GameMenuUp)) _selected = MathT.Clamp(--_selected, 0, _options.Count - 1);
            if (Input.JustPressed(1, Control.GameMenuCancel) && _previousMenu != _currentMenu)
            {
                _currentMenu = _previousMenu;
                if (_previousMenu == 0) _currentMenu = -1;
            }
            if (Input.JustPressed(1, Control.GameMenuAccept))
            {
                if (_options[_selected] is Button b) b.Action.Invoke();
                if (_options[_selected] is Menu)
                {
                    _previousMenu = _currentMenu;
                    _currentMenu = _selected;
                }
            }

            if (_currentMenu == -1) DrawMenu();
            else if(_options[_currentMenu] is Menu m2) m2.Draw();

            
        }
        public int CreateSubmenu(string title, Button[] buttons)
        {
            _options.Add(new Menu(title, buttons));
            return _options.Count - 1;
        }

        public void AddSubmenu(int id, string title, Button[] buttons)
        {
            if (_options[id] is Menu m) m._options.Add(new Menu(title, buttons));
        }

        public void AddButtons(int id, Button buttons)
        {
            if (_options[id] is Menu m) m._options.Add(buttons);
        }

        public static void DrawRect(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a, false, false);
        }

        public static void DrawText(string text, float x, float y, float scaleX = 0.8f, float scaleY = 0.8f, int font = 1, bool center = false, Vector3 color = default(Vector3))
        {

            long t = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            API.SetTextColor(255, 255, 255, 255);
            API.SetTextFontForCurrentCommand(font);
            API.SetTextScale(scaleX, scaleY);
            API.SetTextCentre(center);
            API.DisplayText(t, x, y);
        }

        public void DrawMenu(float x = 0.875f, float y = 0.5f, float width = 0.15f, float height = 0.50f)
        {
            DrawRect(x, y, width, height, 0, 0, 0, 255);
            DrawText(_title, x, y - (height * .5f), center: true);

            for (int i = 0; i < _options.Count; i++)
            {
                if (_options[i] is Menu m)
                {
                    if (i == _selected) DrawButton(m._title, i, r: 200, g: 0, b: 0, a: 150);
                    else DrawButton(m._title, i, r: 200, g: 0, b: 0);
                }

                if(_options[i] is Button b)
                {
                    if (i  == _selected) DrawButton(b.Name, i , r: 200, g: 0, b: 0, a: 150);
                    else DrawButton(b.Name, i , r: 200, g: 0, b: 0, a: 255);
                }

            }
        }

        public void DrawButton(string text, float multi = 1, float x = 0.875f, float y = 0.5f, float width = 0.15f, float height = 0.50f, int r = 0, int g = 0, int b = 0, int a = 255)
        {
            float gridHeight = height / _nr;
            float tempY = (y - (height * .45f)) + gridHeight * multi;

            API.DrawRect(x, tempY + gridHeight, width, gridHeight, r, g, b, a, false, false);
            DrawText(text, x - width * .5f, tempY + gridHeight - gridHeight * .125f, 0.2f, 0.2f);
        }

    }


}
