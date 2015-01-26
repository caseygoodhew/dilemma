using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Win32;

namespace Dilemma.Common
{
    public static class ServerName
    {
        private static readonly string[] _names = { 
            "Arthur",
            "Bertha",
            "Cristobal",
            "Dolly",
            "Edouard",
            "Fay",
            "Gustav",
            "Hanna",
            "Ike",
            "Josephine",
            "Kyle",
            "Laura",
            "Marco",
            "Nana",
            "Omar",
            "Paloma",
            "Rene",
            "Sally",
            "Teddy",
            "Vicky",
            "Wilfred",
        };

        public static string GetNext(IEnumerable<string> exclude)
        {
            return _names.First(x => !exclude.Contains(x));
        }

        public static string Get()
        {
            var value = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Dilemma", "ServerName", string.Empty);
            return (value ?? string.Empty).ToString();
        }

        public static void Set(string name)
        {
            if (!String.IsNullOrEmpty(Get()))
            {
                throw new InvalidOperationException("Cannot change the server's name");
            }

            var software = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (!software.GetSubKeyNames().Contains("Dilemma"))
            {
                software.CreateSubKey("Dilemma");
            }

            var dilemma = software.OpenSubKey("Dilemma", true);
            dilemma.SetValue("ServerName", name);
        }
    }
}
