// Copyright (c) 2023 A.B
// yanna92yar@gmail.com

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
                "NegativeHueShift180",
                "NegativeHueShift180Variation1",
                "NegativeHueShift180Variation2",
                "NegativeHueShift180Variation3"
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
            float[] NegativeHueShift180Variation1 = BuiltinMatrices.NegativeHueShift180Variation1.Cast<float>().ToArray();
            float[] NegativeHueShift180Variation2 = BuiltinMatrices.NegativeHueShift180Variation2.Cast<float>().ToArray();
            float[] NegativeHueShift180Variation3 = BuiltinMatrices.NegativeHueShift180Variation3.Cast<float>().ToArray();

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
                NegativeHueShift180,
                NegativeHueShift180Variation1,
                NegativeHueShift180Variation2,
                NegativeHueShift180Variation3
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