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
        const string debugLCDname = "DebugLCD";
        IMyTextPanel debugLCD;
        List<IMyTerminalBlock> _blockList;
        List<IMyCameraBlock> _cameraList;

        uint tick;
        double runtime = 0;
        public Program()
        {
            tick = 0;
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            UInt64 camerasCount = 0,
                    textPanelsCount = 0;

            _blockList = new List<IMyTerminalBlock>();
            _cameraList = new List<IMyCameraBlock>();
            GridTerminalSystem.GetBlocks(_blockList);

            foreach (var currentBlock in _blockList)
            {
                if (currentBlock is IMyCameraBlock)
                {
                    currentBlock.CustomName = $"{base.Me.CubeGrid.CustomName}::Camera 000{camerasCount.ToString()}";
                    (currentBlock as IMyCameraBlock).EnableRaycast = true;
                    _cameraList.Add(currentBlock as IMyCameraBlock);

                    camerasCount++;
                    continue;
                }

                if (currentBlock is IMyTextPanel)
                {
                    if (currentBlock.CustomName.Contains(debugLCDname) )
                    {
                        debugLCD = currentBlock as IMyTextPanel;
                        debugLCD.CustomName = $"{Me.CubeGrid.CustomName}::{debugLCDname}";
                        debugLCD.ShowPublicTextOnScreen();
                        continue;
                    } else
                    {

                        (currentBlock as IMyTextPanel).ShowPublicTextOnScreen();
                        (currentBlock as IMyTextPanel).CustomName = $"{Me.CubeGrid.CustomName}::TextPanel 000{textPanelsCount.ToString()}";

                        textPanelsCount++;
                        continue;
                    }
                }
            }
            Echo($"camerasCount: {camerasCount}");
            Echo($"textPanelsCount: {textPanelsCount}");
        }

        public void Save()
        {

        }

        public void Main(string argument)
        {
            tick++;
            runtime += Runtime.LastRunTimeMs;

            if (debugLCD != null)
            {
                debugLCD.WritePublicText($"Run Time: {runtime.ToString()} \n" +
                                         $"Tick: {tick}");
                debugLCD.ShowPublicTextOnScreen();
            } else
            {
                Echo("DebugLCD not found :(");
            }


        }
    }
}


/*}catch (Exception e){

        Echo( $"Except : \n {e}");

}
*/