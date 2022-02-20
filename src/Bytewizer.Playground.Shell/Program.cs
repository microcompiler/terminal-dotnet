using Bytewizer.TinyCLR.Terminal;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.Playground.Terminal
{
    internal class Program
    {
        private static ShellServer? _ShellServer;

        static void Main()
        {
            var rsaKey = new RSAParameters()
            {
                Modulus = Convert.FromBase64String("xXKzcIH/rzcfv2D7VcvLdxR5S5iw2TTsP65Aa82S4+9ZIqLPTNtuzr76Mz6Cx0yDOhawHlIujtalqaaQzaUvkudCtMcVMnj37OcCYz7XDAYejalCxf/vtJo7U4mnYdCM+nAOQKNIDKLGbLtGuEAGwdi560DOJY2plhnBf1oOI+k="),
                Exponent = Convert.FromBase64String("AQAB"),
                P = Convert.FromBase64String("37kMr9YiU4cSqHqTJSjBJ/szG2O4n5xSIlPy4MZ4aAN5NALHxfsN0dq1y8NL6GTLMI5qoykvp4Bjrm2ZgU1cDQ=="),
                Q = Convert.FromBase64String("4e84rF+UsFBfQEKJc2pbACWWJjttNW0hccdQZzA3IUxRmd/Z4yEMr1L70TP0XV7dw1RDs1JyU7xnXBIbGy5ETQ=="),
                DP = Convert.FromBase64String("vP0TbI6VnL3j0xMIrkFJOj8Ho0GQOrTQ5VLJP3wpRqR4hKk8nVBBEl+RZznpK73Jr5D/ICmwqezZSAYpwILbGQ=="),
                DQ = Convert.FromBase64String("bHdzZtWwRYEgaXJIGL+7lnN1BT/MazTMNJpykEeGgBbqqgvcx/zq4RTezg26SEUuBANlSSbQukCeAoayurbYlQ=="),
                InverseQ = Convert.FromBase64String("Irq9vR7CXIVR+r09caYIxIY8BOig+HShN1bXvATERJcjTW2jUgJrUttDGNEx70/hBd7m1NWCZz5YO3RH9Bdf5w=="),
                D = Convert.FromBase64String("AqxsufxFcW9TDCAmQK4mwVdsoQjRp2jfcULmkM8fl9u40dtxTr6Csv5dz7qfKLWxHTGlDUDabCK2t/DCcZZoA3rsqwLADe4ZerDdg6xiq4MBzNprM8Y0IfNESEdFB9T0T73ONQCsMalUzEvUknC4Ed4Fya34LUHntgQtEhXpDJE=")
            };

            _ShellServer = new ShellServer(options =>
            {
                options.HostKey("ssh-rsa", rsaKey);
                options.Pipeline(app =>
                {
                    app.UseAutoMapping();
                });
            });
            _ShellServer.Start();
        }
    }
}
