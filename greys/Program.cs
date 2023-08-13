using greys;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks.Dataflow;

namespace ManagedColorPlayground
{
    using static NativeMethods;

    class Program
    {


        static void Main(string[] args)
        {
            float[] identity = BuiltinMatrices.Sepia.Cast<float>().ToArray();

            var magEffectInvert = new MAGCOLOREFFECT
            {
                transform = identity
            };

            MagInitialize();
            MagSetFullscreenColorEffect(ref magEffectInvert);
            Console.ReadLine();
            MagUninitialize();
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