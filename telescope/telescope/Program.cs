using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    class Program : MyGridProgram
    {
        /// <summary>
        /// start
        /// </summary>
        int Clock = 1;
        int Tick;
        int runMode; // 0-stop, 1-test 
                     // IMyTimerBlock Timer; 
        public IMyTextPanel TPDebug;
        Telescope telescope;


        public Program()
        {
            TPDebug = GridTerminalSystem.GetBlockWithName("TPDebug") as IMyTextPanel;
            telescope = new Telescope(this, "", 128);
        }

        public void Main(string args)
        {
            Tick++;

            if (args == "Stop")
            {
                runMode = 0;
            }

            if (args == "TakePic")
            {
                telescope.TakeNewShot(100, 128);
                runMode = 1;
            }

            if (runMode != 0)
            {
                if (Tick % Clock == 0)
                {
                    Runtime.UpdateFrequency = UpdateFrequency.Update100;
                    telescope.Update();
                }
                Runtime.UpdateFrequency = UpdateFrequency.Once;
            }


        }




        public class EGA_Monitor
        {
            const char green = '\uE001';
            const char blue = '\uE002';
            const char red = '\uE003';
            const char yellow = '\uE004';
            const char darkGrey = '\uE00F';
            private Program ParentProgram;
            private string[] matrix;
            private IMyTextPanel TP;
            private string Prefix;
            public int screenResolution;

            public EGA_Monitor(Program pProg, string pref, int scrRes)
            {
                ParentProgram = pProg;
                Prefix = pref;
                TP = ParentProgram.GridTerminalSystem.GetBlockWithName(Prefix) as IMyTextPanel;
                SetNewResolution(scrRes);
            }

            public void SetNewResolution(int newRes)
            {
                screenResolution = newRes;
                matrix = new string[screenResolution];
                TP.SetValue<float>("FontSize", (float)16 / screenResolution);
                ClearScreen();
                RefreshScreen();
            }

            public void Plot(int x, int y, char c)
            {
                if (x >= 0 && y >= 0 && x < screenResolution && y < screenResolution)
                {
                    char[] chars = matrix[y].ToCharArray();
                    chars[x] = c;
                    matrix[y] = new string(chars);
                }
            }

            public void RefreshScreen()
            {
                string output = "";
                for (int i = 0; i < screenResolution; i++)
                {
                    output += matrix[i];
                }
                TP.WritePublicText(output, false);

            }

            public void ClearScreen()
            {
                for (int y = 0; y < screenResolution; y++)
                {
                    matrix[y] = "";
                    for (int x = 0; x < screenResolution; x++)
                    {
                        matrix[y] += darkGrey;
                    }
                    matrix[y] += "\n";
                }
            }
        }

        public class RaycastGroup
        {
            Program ParentProgram;
            private string Prefix;
            private List<IMyCameraBlock> CamArray;
            private int CamIndex;
            public int CamQuantity;
            public int SearchLimit = 50;

            public RaycastGroup(Program MyProg, string pref)
            {
                ParentProgram = MyProg;
                Prefix = pref;
                CamIndex = 0;
                CamArray = new List<IMyCameraBlock>();
                List<IMyTerminalBlock> TempArray = new List<IMyTerminalBlock>();
                ParentProgram.GridTerminalSystem.SearchBlocksOfName(Prefix, TempArray);
                for (int i = 0; i < TempArray.Count; i++)
                {
                    IMyCameraBlock Cam = TempArray[i] as IMyCameraBlock;
                    if (Cam != null)
                    {
                        Cam.EnableRaycast = true;
                        CamArray.Add(Cam);
                    }

                }
                CamQuantity = CamArray.Count;
            }

            public IMyCameraBlock GetCamera(Vector3D Target)
            {
                int SearchCounter = 0;
                while (SearchCounter < SearchLimit)
                {
                    SearchCounter++;
                    CamIndex++;
                    if (CamIndex >= CamArray.Count)
                    {
                        CamIndex = 0;
                    }

                    if (CamArray[CamIndex].CanScan(Target))
                    {
                        ParentProgram.TPDebug.WritePublicText("\n Cam ready: " + CamIndex.ToString(), false);
                        return CamArray[CamIndex];
                    }
                }
                return null;
            }

            public double TotalRange()
            {
                double totalRange = 0;
                for (int i = 0; i < CamArray.Count; i++)
                {
                    totalRange += CamArray[i].AvailableScanRange;
                }
                return totalRange;

            }
        }

        public class ScanPoints
        {
            public int side;
            public int step;
            public int X;
            public int Y;
            public bool ScanComplete;

            public ScanPoints(int sideSize)
            {
                StartNewScan(sideSize);
            }

            public void Next()
            {
                step++;
                X = step % side;
                Y = step / side;
                if (Y >= side)
                    ScanComplete = true;
            }

            public void StartNewScan(int sideSize)
            {
                step = 0;
                side = sideSize;
                ScanComplete = false;
            }
        }

        public class Telescope
        {
            Program ParentProgram;
            private string Prefix;
            RaycastGroup insectEye;
            EGA_Monitor Manitu;
            ScanPoints scanPoints;
            int scanLimit = 200;
            bool IsActive;
            int ScanRes;

            public Telescope(Program MyProg, string pref, int resolution)
            {
                ParentProgram = MyProg;
                Prefix = pref;
                ScanRes = resolution;
                insectEye = new RaycastGroup(ParentProgram, "Camera");
                Manitu = new EGA_Monitor(ParentProgram, "LCD", ScanRes);
                IsActive = false;
            }

            public void TakeNewShot(double frameSize, int resolution)
            {
                ScanRes = resolution;
                scanPoints = new ScanPoints(resolution);
                Manitu.SetNewResolution(resolution);
                IsActive = true;
            }

            public void Update()
            {
                if (IsActive)
                {
                    for (int i = 0; i < scanLimit; i++)
                    {
                        if (!scanPoints.ScanComplete)
                        {
                            Vector3D scanTarget = new Vector3D(scanPoints.X, scanPoints.Y, 500);
                            IMyCameraBlock ActiveCam = insectEye.GetCamera(scanTarget);
                            if (ActiveCam != null)
                            {
                                ParentProgram.TPDebug.WritePublicText("\n Cam used: " + ActiveCam.CustomName, true);
                                ParentProgram.TPDebug.WritePublicText("\n Scan point: " + scanTarget.ToString(), true);
                                if (!ActiveCam.Raycast(scanTarget).IsEmpty())
                                {
                                    Manitu.Plot(scanPoints.X, scanPoints.Y, '\uE001');
                                }
                                scanPoints.Next();
                            } else
                            {
                                return;
                            }
                        } else
                        {
                            IsActive = false;
                            return;
                        }

                    }
                    Manitu.RefreshScreen();
                }
            }
        }

        /// stop

    }

}