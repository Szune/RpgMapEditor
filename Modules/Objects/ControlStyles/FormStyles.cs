using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RpgMapEditor.Modules.Objects.ControlStyles
{
    public static class FormStyles
    {
        private static Dictionary<string, FormStyle> _styles = new Dictionary<string, FormStyle>();

        public static FormStyle Default { get { return _styles["Default"]; } }
        public static FormStyle Pink { get { return _styles["Pink"]; } }
        public static FormStyle Blue { get { return _styles["Blue"]; } }
        public static FormStyle Light { get { return _styles["Light"]; } }
        public static FormStyle Stone { get { return _styles["Stone"]; } }

        internal static void Init()
        {
            int amountOfStyles = 3;
            int _nId = 0;

            for (int i = 0; i < amountOfStyles; i++)
            {
                FormStyle temporary = new FormStyle();
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
                    default:
                        temporary.Name = "None";
                        break;
                }
                int row = 0;
                temporary.TopLeftCorner = new Rectangle(191 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.TopBorder = new Rectangle(208 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.TopRightCorner = new Rectangle(225 + (i * 54), 0 + (17 * row), 16, 16);

                row++;
                temporary.LeftBorder = new Rectangle(191 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.Middle = new Rectangle(208 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.RightBorder = new Rectangle(225 + (i * 54), 0 + (17 * row), 16, 16);

                row++;
                temporary.BottomLeftCorner = new Rectangle(191 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.BottomBorder = new Rectangle(208 + (i * 54), 0 + (17 * row), 16, 16);
                temporary.BottomRightCorner = new Rectangle(225 + (i * 54), 0 + (17 * row), 16, 16);

                _styles[temporary.Name] = temporary;

                /*          case 3:
                                Temporary.Name = "Light";
                                break;
                            case 4:
                                Temporary.Name = "Stone";
                                break;
                */
            }

            amountOfStyles = 2;
            for (int i = 0; i < amountOfStyles; i++)
            {
                FormStyle temporary = new FormStyle();
                _nId++;
                temporary.Id = _nId;
                switch (i)
                {
                    case 0:
                        temporary.Name = "Light";
                        break;
                    case 1:
                        temporary.Name = "Stone";
                        break;
                    default:
                        temporary.Name = "None";
                        break;
                }
                int row = 0;
                temporary.TopLeftCorner = new Rectangle(191 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.TopBorder = new Rectangle(208 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.TopRightCorner = new Rectangle(225 + (i * 54), 55 + (17 * row), 16, 16);

                row++;
                temporary.LeftBorder = new Rectangle(191 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.Middle = new Rectangle(208 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.RightBorder = new Rectangle(225 + (i * 54), 55 + (17 * row), 16, 16);

                row++;
                temporary.BottomLeftCorner = new Rectangle(191 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.BottomBorder = new Rectangle(208 + (i * 54), 55 + (17 * row), 16, 16);
                temporary.BottomRightCorner = new Rectangle(225 + (i * 54), 55 + (17 * row), 16, 16);

                _styles[temporary.Name] = temporary;
            }
        }

        public static ButtonStyle GetButtonStyle(FormStyle style)
        {
            if (style.Name == "Default")
            {
                return ButtonStyles.Default;
            }
            else if (style.Name == "Pink")
            {
                return ButtonStyles.Pink;
            }
            else if (style.Name == "Blue")
            {
                return ButtonStyles.Blue;
            }
            else if (style.Name == "Light")
            {
                return ButtonStyles.Light;
            }
            else if (style.Name == "Stone")
            {
                return ButtonStyles.Stone;
            }
            else
            {
                return new ButtonStyle();
            }
        }
    }
}
