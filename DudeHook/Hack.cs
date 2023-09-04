using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace DudeHook
{
    class Hack:MonoBehaviour
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        GameObject Player;
        People[] PeopleVar;
        Camera cam;
        bool Tracers = false;
        bool NameESP = false;
        bool bMenuOpen = true;
        public static string WPName = "";


        IEnumerator CollectPlayer()
        {
            for(; ; )
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                yield return new WaitForSeconds(1f);
            }
        }
        IEnumerator CollectPeople()
        {
            for(; ; )
            {
                PeopleVar = FindObjectsOfType<People>();
                yield return new WaitForSeconds(0.1f);
            }
        }
        IEnumerator MenuOpen()
        {
            for(; ; )
            {
                if (Input.GetKey(KeyCode.Insert))
                {
                    
                    
                    bMenuOpen = !bMenuOpen;
                    Thread.Sleep(250);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        public void Start()
        {
            StartCoroutine(CollectPeople());
            StartCoroutine(CollectPlayer());
            StartCoroutine(MenuOpen());
            Waypoints.SetupDir();
            AllocConsole();
        }
        static int pageid = 0;
        private void RenderGUI(int wid)
        {
            GUI.backgroundColor = Color.red;
            GUI.contentColor = Color.green;
            GUI.color = Color.white;
            if (GUI.Button(new Rect(0,600/2-100,100,75),"NPC"))
            {
                pageid = 0;
            }    
            if (GUI.Button(new Rect(0, 600/2-100+75, 100, 75), "Player"))
            {
                pageid = 1;
            }
            if (GUI.Button(new Rect(0, 600/2-100+75+75, 100, 75), "Waypoints"))
            {
                pageid = 2;
            }

            if(pageid == 0) 
            {
                if (GUI.Button(new Rect(110, 15, 150, 25), "Kill everyone"))
                {
                    foreach (var p in PeopleVar)
                    {
                        p.HitMe(20000, false, false, p.gameObject);
                    }
                }
                if (GUI.Button(new Rect(110, 40, 150, 25), "Snaplines: " + Tracers))
                {
                    Tracers = !Tracers;
                }
                if (GUI.Button(new Rect(110, 65, 150, 25), "Name ESP: " + NameESP))
                {
                    NameESP = !NameESP;
                }
                // function doesn't exist in DS1?
                /*if (GUI.Button(new Rect(110, 90, 150, 25), "Spawn Cop"))
                {
                    var Spawner = FindObjectOfType<PeopleSpawner>();
                    Spawner.maxPeople++;
                    Spawner.SpawnPeople(3, 0f);
                }
                if (GUI.Button(new Rect(110, 115, 150, 25), "Spawn NPC"))
                {
                    var Spawner = FindObjectOfType<PeopleSpawner>();
                    Spawner.maxPeople++;
                    Spawner.SpawnPeople(1, 0f);
                }*/
                
            }
            if(pageid == 1)
            {
                
                if (GUI.Button(new Rect(110, 15, 150, 25), "Add 100 hp"))
                {
                    FindObjectOfType<PlayerParams>().playerHealth += 100;
                }
                if (GUI.Button(new Rect(110, 40, 150, 25), "Add 1000 hp"))
                {
                    FindObjectOfType<PlayerParams>().playerHealth += 1000;
                }
                if (GUI.Button(new Rect(110, 65, 150, 25), "Add 100 money"))
                {
                    FindObjectOfType<PlayerParams>().GetMoney(100);
                }
                if (GUI.Button(new Rect(110, 90, 150, 25), "Add 1000 money"))
                {
                    FindObjectOfType<PlayerParams>().GetMoney(1000);
                }
                if (GUI.Button(new Rect(110, 115, 150, 25), "TP me to random npc"))
                {
                    foreach (var p in PeopleVar)
                    {
                        Player.transform.position = p.transform.position;
                    }
                }
                if (GUI.Button(new Rect(110, 140, 150, 25), "Teleport npc to player"))
                {
                    //fixed lel
                    foreach (var p in PeopleVar)
                    {
                         p.transform.position = Player.transform.position;
                    }
                }
                if (GUI.Button(new Rect(110, 165, 150, 25), "Everyone Runs"))
                {
                    foreach (var p in PeopleVar)
                    {
                        p.SetRunToMe();
                    }
                }
            }
            if(pageid == 2)
            {
                GUI.Label(new Rect(110, 15, 150, 25), Player.transform.position.ToString());
                
                WPName = GUI.TextField(new Rect(110, 15, 150, 25), WPName);
                if(GUI.Button(new Rect(110,65,150,25),"Create waypoint here"))
                {
                    Waypoints.CreateWaypoint(Player.transform.position, WPName);
                }
                int y = 15;
                for (int i = 0; i <= Waypoints.GetFileLength(); i++)
                {
                    
                    try
                    {
                        if (GUI.Button(new Rect(450, y, 150, 25), Waypoints.ParseWaypointName(i)))
                        {
                            Player.transform.position = Waypoints.Parse(i);
                        }
                        y += 25;
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText("C:\\DudeHook\\Debug.log", ex.Message);
                    }
                }
            }
        }
        public void OnGUI()
        {
            if(bMenuOpen)
            {

                GUI.backgroundColor = Color.cyan;
                Rect Window = GUI.Window(1337, new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600), RenderGUI, "DudeHook");
            }
            foreach (var p in PeopleVar)
            {
                Vector3 w2s = cam.WorldToScreenPoint(p.transform.position);


                if (Tracers)
                {
                    Render.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(w2s.x, Screen.height - w2s.y), Color.magenta, 5f);
                }
                if (NameESP)
                {
                    GUI.Label(new Rect((int)w2s.x - 5, Screen.height - w2s.y - 14, 40, 40), "NPC");
                }
            }

        }
        
        public void Update()
        {
            
            cam = Camera.main;
        }

        public void Awake()
        {
            
            
        }
    }
}
