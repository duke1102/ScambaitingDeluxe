using System;
using System.Collections.Generic;
using System.IO;
using SHDocVw;

namespace BrowserTabsMonitor
{
    public class IeClass
    {
        private readonly List<InternetExplorer> _ieWindows;

        public IeClass()
        {
            _ieWindows = new List<InternetExplorer>();
        }

        public List<InternetExplorer> GetIeInstances()
        {
            _ieWindows.Clear();
            ShellWindows shellWindows = new ShellWindows();

            foreach (InternetExplorer ie in shellWindows)
            {
                string filename = Path.GetFileNameWithoutExtension(ie.FullName)?.ToLower();
                if (filename != null && filename.Equals("iexplore"))
                {
                    _ieWindows.Add(ie);
                }
            }
            return _ieWindows;
        }

        public bool QuitInstance(int key)
        {
            InternetExplorer ie = _ieWindows[key];

            try
            {
                ie.Quit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void StartInstance(string url)
        {
            InternetExplorer ie = new InternetExplorer {Visible = true};
            _ieWindows.Add(ie);
        }
    }
}
