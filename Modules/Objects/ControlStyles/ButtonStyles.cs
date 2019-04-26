using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RpgMapEditor.Modules.Objects.ControlStyles
{
    public static class ButtonStyles
    {
        private static Dictionary<string, ButtonStyle> _styles = new Dictionary<string, ButtonStyle>();

        public static ButtonStyle Default { get { return _styles["Default"]; } }
        public static ButtonStyle Pink { get { return _styles["Pink"]; } }
        public static ButtonStyle Blue { get { return _styles["Blue"]; } }
        public static ButtonStyle Light { get { return _styles["Light"]; } }
        public static ButtonStyle Stone { get { return _styles["Stone"]; } }
        public static ButtonStyle DefaultSmooth { get { return _styles["DefaultSmooth"]; } }
        public static ButtonStyle PinkSmooth { get { return _styles["PinkSmooth"]; } }
        public static ButtonStyle BlueSmooth { get { return _styles["BlueSmooth"]; } }
        public static ButtonStyle LightSmooth { get { return _styles["LightSmooth"]; } }
        public static ButtonStyle StoneSmooth { get { return _styles["StoneSmooth"]; } }

        internal static void Init()
        {
            int _nId = 0;
            int amountOfStyles = 5;
            for (int i = 0; i < amountOfStyles; i++)
            {
                ButtonStyle temporary = new ButtonStyle();
                _nId++;
                temporary.Id = _nId;
                switch (i)
                {
                    case 0:
                        temporary.Name = "Default";
                        break;
                    case 1:
                        temporary.Name = "Pink";
                        break;
                    case 2:
                        temporary.Name = "Blue";
                        break;
                    case 3:
                        temporary.Name = "Light";
                        break;
                    case 4:
                        temporary.Name = "Stone";
                        break;
                    default:
                        temporary.Name = "None";
                        break;
                }
                temporary.LeftBorder = new Rectangle(7, 1 + (i * 26), 16, 21);
                temporary.Middle = new Rectangle(24, 1 + (i * 26), 16, 21);
                temporary.RightBorder = new Rectangle(41, 1 + (i * 26), 16, 21);
                _styles[temporary.Name] = temporary;
            }

            for (int i = 0; i < amountOfStyles; i++)
            {
                ButtonStyle temporary = new ButtonStyle();
                _nId++;
                temporary.Id = _nId;
                switch (i)
                {
                    case 0:
                        temporary.Name = "DefaultSmooth";
                        break;
                    case 1:
                        temporary.Name = "PinkSmooth";
                        break;
                    case 2:
                        temporary.Name = "BlueSmooth";
                        break;
                    case 3:
                        temporary.Name = "LightSmooth";
                        break;
                    case 4:
                        temporary.Name = "StoneSmooth";
                        break;
                    default:
                        temporary.Name = "NoneSmooth";
                        break;
                }
                temporary.LeftBorder = new Rectangle(61, 266 + (i * 26), 16, 17);
                temporary.Middle = new Rectangle(78, 266 + (i * 26), 16, 17);
                temporary.RightBorder = new Rectangle(95, 266 + (i * 26), 16, 17);
                _styles[temporary.Name] = temporary;
            }
            //ButtonStyle _Default = new ButtonStyle();
            //_Default.ID = 1;
            //_Default.Name = "Default";
            //_Default.LeftBorder = new Rectangle(7, 1, 16, 21);
            //_Default.Middle = new Rectangle(24, 1, 16, 21);
            //_Default.RightBorder = new Rectangle(41, 1, 16, 21);
            //_Styles["default"] = _Default;

            //ButtonStyle _Pink = new ButtonStyle();
            //_Pink.ID = 2;
            //_Pink.Name = "Pink";
            //_Pink.LeftBorder = new Rectangle(7, 27, 16, 21);
            //_Pink.Middle = new Rectangle(24, 27, 16, 21);
            //_Pink.RightBorder = new Rectangle(41, 27, 16, 21);
            //_Styles["pink"] = _Pink;
        }
    }
}
