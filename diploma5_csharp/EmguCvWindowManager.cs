using System;
using Emgu.CV;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV.Structure;

namespace diploma5_csharp
{
    public static class EmguCvWindowManager
    {
        private static List<string> _windows = new List<string>();

        private static void Add(string window)
        {
            _windows.Add(window);
        }
        private static void Remove(string window)
        {
            _windows.Remove(window);
        }
        private static string GetRandomName()
        {
            string name = (new Random()).Next().ToString();
            return name;
        }

        public static string Display(IInputArray image, string name)
        {
            Add(name);
            CvInvoke.Imshow(name, image); //Show the image
            return name;
        }

        public static string Display(IInputArray image)
        {
            string name = GetRandomName();
            return Display(image,name);
        }


        public static string Close(string name)
        {
            CvInvoke.DestroyWindow(name);
            Remove(name);
            return name;
        }

        public static void CloseAll()
        {
            _windows.Select(w => { CvInvoke.DestroyWindow(w);
                                     return true;
            });
            foreach (var _window in _windows)
            {
                CvInvoke.DestroyWindow(_window);
            }
            _windows.Clear();
        }
    }
}