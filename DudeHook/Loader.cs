using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DudeHook
{
    public class Loader
    {
        public static void Load()
        {
            Loader.AssemblyToLoad = new UnityEngine.GameObject();
            Loader.AssemblyToLoad.AddComponent<DudeHook.Hack>();
            UnityEngine.GameObject.DontDestroyOnLoad(Loader.AssemblyToLoad);
        }
        private static GameObject AssemblyToLoad;
    }
}
