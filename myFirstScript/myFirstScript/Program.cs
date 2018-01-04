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
    partial class Program : MyGridProgram
    {
        IMyTextPanel debugLCD;
        List<IMyTerminalBlock> _blockList;
        List<IMyCameraBlock> _cameraList;

        public Program()

        {
            int cmerasCount = 0,
                textPanelsCount = 0;

            _blockList = new List<IMyTerminalBlock>();
            _cameraList = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocks(_blockList);
            
            foreach (var currentBlock in _blockList)
            {
                if (currentBlock is IMyCameraBlock)
                {
                    currentBlock.CustomName = $"{base.Me.CubeGrid.CustomName}::Camera 000{0.ToString()}";
                    (currentBlock as IMyCameraBlock).EnableRaycast = true;
                    _cameraList.Add(currentBlock as IMyCameraBlock);

                    cmerasCount++;
                    continue;
                }

                if (currentBlock is IMyTextPanel)
                {
                    if (currentBlock.CustomName == "DebugLCD")
                    {   
                        debugLCD = currentBlock as IMyTextPanel;
                        debugLCD.CustomName = $"{Me.CubeGrid.CustomName}::{debugLCD.CustomName}";
                        debugLCD.ShowPublicTextOnScreen();
                        continue;
                    }

                    (currentBlock as IMyTextPanel).ShowPublicTextOnScreen();
                    (currentBlock as IMyTextPanel).CustomName = $"{Me.CubeGrid.CustomName}::TextPanel 000{textPanelsCount}";

                    textPanelsCount++;
                    continue;
                }
            }

        }

        public void Save()
        {

        }

        public void Main(string argument)
        {
            if (debugLCD != null)
            {
                debugLCD.WritePublicText("ehuuuu");
                debugLCD.ShowPublicTextOnScreen();
            } else
            {
                Echo("DebugLCD not found :(");
            }
        }
    }
}