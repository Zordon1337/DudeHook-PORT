using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace DudeHook
{
    class Waypoints
    { 
        public static int GetFileLength()
        {
            
            string data = File.ReadAllText("C:\\DudeHook\\waypoints.dh");
            int count = data.Split('|').Length - 1;


            return count;

        }
        public static Vector3 Parse(int i)
        {
            string data = File.ReadAllText("C:\\DudeHook\\waypoints.dh");
            string[] vectors = data.Split('|');
            string[] cords = vectors[i].Split(':'); // split by : and parse as float
            Vector3 result = new Vector3(float.Parse(cords[0]), float.Parse(cords[1]), float.Parse(cords[2]));
            return result;
        }
        public static string ParseWaypointName(int i)
        {
            string data = File.ReadAllText("C:\\DudeHook\\waypoints.dh");
            string[] vectors = data.Split('|');
            string[] cords = vectors[i].Split(':'); // split by : and parse as float
            // yes i was too lazy and i literally copied Parse() 
            string result = cords[3];
            return result;
        }
        public static void SetupDir()
        {
            if (!Directory.Exists("C:\\DudeHook"))
            {
                Directory.CreateDirectory("C:\\DudeHook");

            }
            if (!System.IO.File.Exists("C:\\DudeHook\\waypoints.dh"))
            {
                System.IO.File.Create("C:\\DudeHook\\waypoints.dh");

            }
        }

        public static void CreateWaypoint(Vector3 Pos, string name)
        {
            string ToWrite = $"|{Pos.x}:{Pos.y}:{Pos.z}:{name}";
            File.AppendAllText("C:\\DudeHook\\waypoints.dh",ToWrite);

        }
    }
}
