using greys;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks.Dataflow;

namespace ManagedColorPlayground
{
    using static NativeMethods;

    class Program
    {


        public static async Task<int> Main(string[] args)
        {

            var viewOption = new Option<int>(
                new String[] { "-p", "--port" },
                "The port");
            var rootCommand = new RootCommand("description ...")
            {
                viewOption,
            };
            rootCommand.SetHandler<int>(OnHandle, viewOption);
            var commandLineBuilder = new CommandLineBuilder(rootCommand).UseDefaults();
            var parser = commandLineBuilder.Build();


            /*float[] identity = BuiltinMatrices.Sepia.Cast<float>().ToArray();

            var magEffectInvert = new MAGCOLOREFFECT
            {
                transform = identity
            };

            MagInitialize();
            MagSetFullscreenColorEffect(ref magEffectInvert);*/
            Console.ReadLine();
            /*MagUninitialize();*/

            return await parser.InvokeAsync(args).ConfigureAwait(false);
        }


        private static void OnHandle(int port)
        {

            Console.WriteLine("Welcome to Greys...");
            Console.WriteLine($"Handling parameter {port}");
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