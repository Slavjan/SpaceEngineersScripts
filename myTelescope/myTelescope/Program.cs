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


        public Program()
        {
            
        }

        public void Save()
        {

        }

        public void Main(string argument)
        {

        }

        public class BlocksNumerator
        {
            ushort _numberLenght = 4;
            public uint _currentNum
            {
                private set;
                get;
            }

            public BlocksNumerator(ushort numberLenght = 4)
            {
                _currentNum = 0;
            }

            uint newNumber()
            {
                _currentNum++;
                return _currentNum - 1;
            }

            public string blockNumber()
            {
                string number = " ";

                uint num = newNumber();
                for (uint i = 0; i < Math.Abs(_numberLenght - num.ToString().Length); i++)
                {
                    number += "0";
                }
                number += num.ToString();

                return number;
            }
        }

        public static class BlocksNameficator
        {
            public static void namefy<T>(T block, BlocksNumerator numerator)
            {
                string typeName = block.GetType().ToString();
                string newBlockName = typeName.Substring(3) + numerator.blockNumber();
            }           
        }

        /// <summary>
        ///     TypesCollection
        ///     contanes some Types and Quantity of objects of this type
        /// </summary>
        public class TypesCollection : Dictionary<Type, uint>
        {
        }

        /// <summary>
        ///     BlocksOfTypeCollection
        ///     contanes All Bloks of grid, type of every block and quontity of blocks of this Type or Interface
        /// </summary>
        public class BlocksOfTypeCollection : Dictionary<IMyTerminalBlock, TypesCollection>
        {
        }

        public class GridInitializer
        {
            Program _parentProgram;
            BlocksOfTypeCollection _blocksDictionary;

            GridInitializer(Program parentProgram)
            {
                _parentProgram = parentProgram;

                _blocksDictionary = new BlocksOfTypeCollection();
            }

            public GridInitializer()
            {
                List<IMyTerminalBlock> blocksList = new List<IMyTerminalBlock>();
                _parentProgram.GridTerminalSystem.GetBlocks(blocksList);

                BlocksNumerator[] countOfType = new BlocksNumerator[32];
                uint i = 0;
                foreach (var currentBlock in blocksList)
                {
                    if (currentBlock is IMyCameraBlock)
                    {
                        BlocksNameficator.namefy<IMyCameraBlock>( (currentBlock as IMyCameraBlock), countOfType[0]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyCameraBlock).GetType()] = countOfType[0]._currentNum;
                    }
                    if (currentBlock is IMyTextPanel)
                    {
                        BlocksNameficator.namefy<IMyTextPanel>(currentBlock as IMyTextPanel, countOfType[1]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyTextPanel).GetType()] = countOfType[1]._currentNum;
                    }
                    if (currentBlock is IMyButtonPanel)
                    {
                        BlocksNameficator.namefy<IMyButtonPanel>((currentBlock as IMyButtonPanel), countOfType[2]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyButtonPanel).GetType()] = countOfType[2]._currentNum;
                    }
                    if (currentBlock is IMyControlPanel)
                    {
                        BlocksNameficator.namefy<IMyControlPanel>((currentBlock as IMyControlPanel), countOfType[3]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyControlPanel).GetType()] = countOfType[3]._currentNum;
                    }
                    if (currentBlock is IMySolarPanel)
                    {
                        BlocksNameficator.namefy<IMySolarPanel>((currentBlock as IMySolarPanel), countOfType[4]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMySolarPanel).GetType()] = countOfType[4]._currentNum;
                    }
                    if (currentBlock is IMySoundBlock)
                    {
                        BlocksNameficator.namefy<IMySoundBlock>((currentBlock as IMySoundBlock), countOfType[5]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMySoundBlock).GetType()] = countOfType[5]._currentNum;
                    }
                    if (currentBlock is IMyTimerBlock)
                    {
                        BlocksNameficator.namefy<IMyTimerBlock>((currentBlock as IMyTimerBlock), countOfType[6]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyTimerBlock).GetType()] = countOfType[6]._currentNum;
                    }
                    if (currentBlock is IMyShipMergeBlock)
                    {
                        BlocksNameficator.namefy<IMyShipMergeBlock>((currentBlock as IMyShipMergeBlock), countOfType[0]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyShipMergeBlock).GetType()] = countOfType[0]._currentNum;
                    }
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}