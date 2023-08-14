using greys;
using Pastel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ManagedColorPlayground
{
    using static NativeMethods;

    class Program
    {

        static void Main(string[] args)
        {
            welcomeUser();

            List<string> colorFiltersStr = new List<string> {
                "Neutral",
                "Negative",
                "GrayScale",
                "NegativeGrayScale",
                "Red",
                "NegativeRed",
                "Sepia",
                "NegativeSepia",
                "HueShift180",
                "NegativeHueShift180"
              };

            float[] identity = BuiltinMatrices.Identity.Cast<float>().ToArray();
            float[] Negative = BuiltinMatrices.Negative.Cast<float>().ToArray();
            float[] GrayScale = BuiltinMatrices.GrayScale.Cast<float>().ToArray();
            float[] NegativeGrayScale = BuiltinMatrices.NegativeGrayScale.Cast<float>().ToArray();
            float[] Red = BuiltinMatrices.Red.Cast<float>().ToArray();
            float[] NegativeRed = BuiltinMatrices.NegativeRed.Cast<float>().ToArray();
            float[] Sepia = BuiltinMatrices.Sepia.Cast<float>().ToArray();
            float[] NegativeSepia = BuiltinMatrices.NegativeSepia.Cast<float>().ToArray();
            float[] HueShift180 = BuiltinMatrices.HueShift180.Cast<float>().ToArray();
            float[] NegativeHueShift180 = BuiltinMatrices.NegativeHueShift180.Cast<float>().ToArray();

            List<float[]> colorFilters = new List<float[]> {
                identity,
                Negative,
                GrayScale,
                NegativeGrayScale,
                Red,
                NegativeRed,
                Sepia,
                NegativeSepia,
                HueShift180,
                NegativeHueShift180
              };
            var magEffectInvert = new MAGCOLOREFFECT
            {
                transform = Negative
            };
            MagInitialize();

            int optionsCount = colorFiltersStr.Count;
            int selected = 0;
            bool done = false;
            while (!done)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    if (selected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    Console.WriteLine($"{i}> {colorFiltersStr[i]}");
                    Console.ResetColor();
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selected = Math.Max(0, selected - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selected = Math.Min(optionsCount - 1, selected + 1);
                        break;
                    case ConsoleKey.Enter:
                        magEffectInvert = new MAGCOLOREFFECT
                        {
                            transform = colorFilters[selected]
                        };
                        MagSetFullscreenColorEffect(ref magEffectInvert);
                        break;
                }

                if (!done)
                    Console.CursorTop = Console.CursorTop - optionsCount;
            }

            Console.WriteLine($"Selected {selected}.");

            colorFiltersStr = colorFiltersStr.ConvertAll(d => d.ToLower());
            /*Dictionary<string, string> descriptions = new Dictionary<string, string>();
            descriptions.Add(colorFilters[0], "no color transformation");
            descriptions.Add(colorFilters[1], "simple colors transformation.");
            descriptions.Add(colorFilters[2], "simple colors transformation.");
            descriptions.Add(colorFilters[3], "simple colors transformation.");
            descriptions.Add(colorFilters[4], "simple colors transformation.");
            descriptions.Add(colorFilters[5], "simple colors transformation.");
            descriptions.Add(colorFilters[6], "simple colors transformation.");
            descriptions.Add(colorFilters[7], "simple colors transformation.");
            descriptions.Add(colorFilters[8], "simple colors transformation.");
            descriptions.Add(colorFilters[9], "simple colors transformation.");*/

            /*foreach (KeyValuePair<string, string> kvp in descriptions)
            {
                Console.WriteLine("Color filter = {0}, description = {1}", kvp.Key, kvp.Value);
            }*/

            Console.ReadLine();
            MagUninitialize();

        }

        private static void welcomeUser()
        {
            // Get an array with the values of ConsoleColor enumeration members.
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            // Save the current background and foreground colors.
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            // Display all foreground colors except the one that matches the background.
            Console.WriteLine("All the foreground colors except {0}, the background color:",
                              currentBackground);
            foreach (var color in colors)
            {
                if (color == currentBackground) continue;

                Console.ForegroundColor = color;
                Console.WriteLine("   The foreground color is {0}.", color);
            }
            Console.WriteLine();
            // Restore the foreground color.
            Console.ForegroundColor = currentForeground;

            // Display each background color except the one that matches the current foreground color.
            Console.WriteLine("All the background colors except {0}, the foreground color:",
                              currentForeground);
            foreach (var color in colors)
            {
                if (color == currentForeground) continue;

                Console.BackgroundColor = color;
                Console.WriteLine("   The background color is {0}.", color);
            }

            // Restore the original console colors.
            Console.ResetColor();
            Console.WriteLine("\nOriginal colors restored...");
            Console.Write("  Red      ".Pastel(Color.Black).PastelBg("FF0000"));
            Console.Write("  Green    ".Pastel(Color.Black).PastelBg("00FF00"));
            Console.Write("  Blue     ".Pastel(Color.Black).PastelBg("0000FF"));
            Console.Write("  Yellow   ".Pastel(Color.Black).PastelBg("FFFF00"));
            Console.WriteLine("");
        }
    }

    static class NativeMethods
    {
        const string Magnification = "Magnification.dll";

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagInitialize();

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagUninitialize();

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagSetFullscreenColorEffect(ref MAGCOLOREFFECT pEffect);

        public struct MAGCOLOREFFECT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public float[] transform;
        }
    }
}