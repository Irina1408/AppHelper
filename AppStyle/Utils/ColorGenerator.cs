using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppStyle.Utils
{
    public static class ColorGenerator
    {
        public static Brush[] GenerateBrashes(int number)
        {
            var brushesList = new List<Brush>();
            var rand = new Random();
            var existing = new List<ColorHelp>();

            for (int i = 0; i < number; i++)
            {
                // generate new values
                byte a = 255;
                byte r = (byte)rand.Next(0, 255);
                byte g = (byte)rand.Next(0, 255);
                byte b = (byte)rand.Next(0, 255);

                // check on existing new value
                while(existing.Any(item => item.A > a - 5 && item.A < a + 5
                    && item.R > r - 5 && item.R < r + 5
                    && item.G > g - 5 && item.G < g + 5
                    && item.B > b - 5 && item.B < b + 5))
                {
                    r = (byte)rand.Next(0, 255);
                    g = (byte)rand.Next(0, 255);
                    b = (byte)rand.Next(0, 255);
                }

                // add new agrb value to list of existing
                existing.Add(new ColorHelp(a, r, g, b));
                // add new brush
                brushesList.Add(new SolidColorBrush(new Color(){A = a, R = r, G = g, B = b}));
            }

            // cleanup
            existing.Clear();
            existing = null;
            rand = null;

            return brushesList.ToArray();
        }

        private class ColorHelp
        {
            public ColorHelp(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public byte A { get; private set; }
            public byte R { get; private set; }
            public byte G { get; private set; }
            public byte B { get; private set; }
        }
    }
}
