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
            IMyTextPanel debugLCD = GridTerminalSystem.GetBlockWithName( "DebugLCD" ) as IMyTextPanel;
            debugLCD.ShowPublicTextOnScreen();
            GridInitializer gridInitializer = new GridInitializer( this, debugLCD );

            gridInitializer.init();


        }

        public void Save()
        {

        }

        public void Main(string argument)
        {

        }

        public class BlocksNumerator
        {
            byte _numberLenght = 4;
            public uint _currentNum
            {
                private set;
                get;
            }

            public BlocksNumerator numberLenght(byte value)
            {
                _numberLenght = value;
                return this;
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
                for (uint i = 0; i < Math.Abs( _numberLenght - num.ToString().Length ); i++)
                {
                    number += "0";
                }
                number += num.ToString();

                return number;
            }
        }

        public static class BlocksNameficator
        {
            public static bool namefy(IMyTerminalBlock block, BlocksNumerator numerator,/*Debug*/ Program parent)
            {
                string typeName = block.GetType().ToString();
                int indexOfBlockNameStart = typeName.IndexOf( "My" ) + 2;


                /*Debug*/
                parent.Echo( $"Debug: BlocksNameficator::namefy: tipeName: {typeName}" );
                /*Debug*/
                parent.Echo( $"Debug: BlocksNameficator::namefy: indexOfBlockNameStart: {indexOfBlockNameStart}" );


                string numeratedBlockkName = typeName.Substring( indexOfBlockNameStart ) + numerator.blockNumber();
                block.CustomName = numeratedBlockkName;
                /*Debug*/
                parent.Echo( $"Debug: BlocksNameficator::namefy: tipeName: {numeratedBlockkName}" );

                ///*Debug*/
                //parent.Echo( $"Debug: BlocksNameficator::namefy: tipeName: {nuberatedBlockkName}" );
                return true;
            }
        }

        /// <summary>
        ///     TypesIndexes [ Type blockType, byte indexOfType ]
        /// </summary>
        public class TypesIndexes : Dictionary<Type, byte>
        {
        }

        /// <summary>
        ///     BlocksCollection
        ///     contanes some Blocks and Quantity of objects of this type
        /// </summary>
        public class BlocksCollection : Dictionary<IMyTerminalBlock, uint>
        {
        }

        /// <summary>
        ///     BlockTypesDictionary [ Type blockType, uint countBlocksOfthisType ]
        ///     contanes All Bloks of grid, type of every block and quontity of blocks of this Type or Interface
        /// </summary>
        public class BlockTypesDictionary : Dictionary<Type, uint>
        {
        }

        public class GridInitializer
        {
            Program _parentProgram;
            IMyTextPanel _debugLCD;
            public BlockTypesDictionary _blocksDictionary
            {
                get;
                private set;
            }

            public TypesIndexes _typesIdexes
            {
                get;
                private set;
            }

            public GridInitializer(Program parentProgram, IMyTextPanel debugLCD)
            {
                _parentProgram = parentProgram;
                _debugLCD = debugLCD;
                _blocksDictionary = new BlockTypesDictionary();
                _typesIdexes = new TypesIndexes();

                /*Debug*/
                _parentProgram.Echo( "Debug: GridInitializer::GridInitializer(this)  " );
            }
            public void init()
            {
                /*Debug*/
                _parentProgram.Echo( "Debug: GridInitializer::init()  " );
                string debugStr;


                List<IMyTerminalBlock> blocksList = new List<IMyTerminalBlock>();
                _parentProgram.GridTerminalSystem.GetBlocks( blocksList );

                BlocksNumerator[] countOfType = new BlocksNumerator[ 32 ];

                _typesIdexes = new TypesIndexes();

                byte curTypeIndex = 0;

                Random random = new Random();



                foreach (var currentBlock in blocksList)
                {
                    /*Debug*/
                    _parentProgram.Echo( "Debug: GridInitializer::init() foreach " );
                    /**
                    if (currentBlock is IMyCameraBlock)
                    {
                        BlocksNameficator.namefy<IMyCameraBlock>( (currentBlock as IMyCameraBlock), countOfType[0]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyCameraBlock).GetType()] = countOfType[0]._currentNum;
                    }
                    else if (currentBlock is IMyTextPanel)
                    {
                        BlocksNameficator.namefy<IMyTextPanel>(currentBlock as IMyTextPanel, countOfType[1]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyTextPanel).GetType()] = countOfType[1]._currentNum;
                    }
                    else if (currentBlock is IMyButtonPanel)
                    {
                        BlocksNameficator.namefy<IMyButtonPanel>((currentBlock as IMyButtonPanel), countOfType[2]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyButtonPanel).GetType()] = countOfType[2]._currentNum;
                    }
                    else if (currentBlock is IMyControlPanel)
                    {
                        BlocksNameficator.namefy<IMyControlPanel>((currentBlock as IMyControlPanel), countOfType[3]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyControlPanel).GetType()] = countOfType[3]._currentNum;
                    }
                    else if (currentBlock is IMySolarPanel)
                    {
                        BlocksNameficator.namefy<IMySolarPanel>((currentBlock as IMySolarPanel), countOfType[4]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMySolarPanel).GetType()] = countOfType[4]._currentNum;
                    }
                    else if (currentBlock is IMySoundBlock)
                    {
                        BlocksNameficator.namefy<IMySoundBlock>((currentBlock as IMySoundBlock), countOfType[5]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMySoundBlock).GetType()] = countOfType[5]._currentNum;
                    }
                    else if (currentBlock is IMyTimerBlock)
                    {
                        BlocksNameficator.namefy<IMyTimerBlock>((currentBlock as IMyTimerBlock), countOfType[6]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyTimerBlock).GetType()] = countOfType[6]._currentNum;
                    }
                    else if (currentBlock is IMyShipMergeBlock)
                    {
                        BlocksNameficator.namefy<IMyShipMergeBlock>((currentBlock as IMyShipMergeBlock), countOfType[7]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyShipMergeBlock).GetType()] = countOfType[7]._currentNum;
                    }
                    else if (currentBlock is IMyArtificialMassBlock)
                    {
                        BlocksNameficator.namefy<IMyArtificialMassBlock>((currentBlock as IMyArtificialMassBlock), countOfType[8]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyArtificialMassBlock).GetType()] = countOfType[8]._currentNum;
                    }
                    else if (currentBlock is IMyVirtualMass)
                    {
                        BlocksNameficator.namefy<IMyVirtualMass>((currentBlock as IMyVirtualMass), countOfType[9]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyVirtualMass).GetType()] = countOfType[9]._currentNum;
                    }
                    else if (currentBlock is IMyGravityGenerator)
                    {
                        BlocksNameficator.namefy<IMyGravityGenerator>((currentBlock as IMyGravityGenerator), countOfType[10]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyGravityGenerator).GetType()] = countOfType[10]._currentNum;
                    }
                    else if (currentBlock is IMyGravityGeneratorSphere)
                    {
                        BlocksNameficator.namefy<IMyGravityGeneratorSphere>((currentBlock as IMyGravityGeneratorSphere), countOfType[11]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyGravityGeneratorSphere).GetType()] = countOfType[11]._currentNum;
                    }
                    else if (currentBlock is IMyInteriorLight)
                    {
                        BlocksNameficator.namefy<IMyInteriorLight>( (currentBlock as IMyInteriorLight), countOfType[12]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyInteriorLight).GetType()] = countOfType[12]._currentNum;
                    }
                    else if (currentBlock is IMyLandingGear)
                    {
                        BlocksNameficator.namefy<IMyLandingGear>((currentBlock as IMyLandingGear), countOfType[13]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyLandingGear).GetType()] = countOfType[13]._currentNum;
                    }
                    else if (currentBlock is IMyLargeGatlingTurret)
                    {
                        BlocksNameficator.namefy<IMyLargeGatlingTurret>((currentBlock as IMyLargeGatlingTurret), countOfType[14]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyLargeGatlingTurret).GetType()] = countOfType[14]._currentNum;
                    }
                    else if (currentBlock is IMyLargeInteriorTurret)
                    {
                        BlocksNameficator.namefy<IMyLargeInteriorTurret>((currentBlock as IMyLargeInteriorTurret), countOfType[15]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyLargeInteriorTurret).GetType()] = countOfType[15]._currentNum;
                    }
                    else if (currentBlock is IMyLargeMissileTurret)
                    {
                        BlocksNameficator.namefy<IMyLargeMissileTurret>((currentBlock as IMyLargeMissileTurret), countOfType[16]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyLargeMissileTurret).GetType()] = countOfType[16]._currentNum;
                    }
                    else if (currentBlock is IMyMedicalRoom)
                    {
                        BlocksNameficator.namefy<IMyMedicalRoom>((currentBlock as IMyMedicalRoom), countOfType[17]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyMedicalRoom).GetType()] = countOfType[17]._currentNum;
                    }
                    else if (currentBlock is IMyOxygenFarm)
                    {
                        BlocksNameficator.namefy<IMyOxygenFarm>((currentBlock as IMyOxygenFarm), countOfType[18]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyOxygenFarm).GetType()] = countOfType[18]._currentNum;
                    }
                    else if (currentBlock is IMyParachute)
                    {
                        BlocksNameficator.namefy<IMyParachute>((currentBlock as IMyParachute), countOfType[19]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyParachute).GetType()] = countOfType[19]._currentNum;
                    }
                    else if (currentBlock is IMySpaceBall)
                    {
                        BlocksNameficator.namefy<IMySpaceBall>((currentBlock as IMySpaceBall), countOfType[20]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMySpaceBall).GetType()] = countOfType[20]._currentNum;
                    }
                    else if (currentBlock is IMyVirtualMass)
                    {
                        BlocksNameficator.namefy<IMyVirtualMass>((currentBlock as IMyVirtualMass), countOfType[21]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyVirtualMass).GetType()] = countOfType[21]._currentNum;
                    }
                    else if (currentBlock is IMyAirVent)
                    {
                        BlocksNameficator.namefy<IMyAirVent>((currentBlock as IMyAirVent), countOfType[22]);
                        TypesCollection camsCollection = new TypesCollection();

                        camsCollection[(currentBlock as IMyAirVent).GetType()] = countOfType[22]._currentNum;
                    }*/
                    if (!_blocksDictionary.ContainsKey( currentBlock.GetType() ))
                    {
                        /*Debug*/
                        debugStr = $"Debug: GridInitializer::init foreach _blockDic ! conteins TKey: \n\t {currentBlock.GetType()} ";
                        _parentProgram.Echo( debugStr );
                        _debugLCD.WritePublicText( debugStr );
                        
                        _blocksDictionary.Add( currentBlock.GetType(), 1 );

                        /*Debug*/
                        debugStr = $"Debug: GridInitializer::init foreach \\(_blockDic ! conteins TKey): \n\t {currentBlock.GetType()} ";
                        _parentProgram.Echo( debugStr );
                        _debugLCD.WritePublicText( debugStr );
                    } else
                    {
                        _blocksDictionary[ currentBlock.GetType() ]++;
                    }


                    if (!_typesIdexes.ContainsKey( currentBlock.GetType() ))
                    {
                        _typesIdexes.Add( currentBlock.GetType(), (byte)curTypeIndex );
                        countOfType[ curTypeIndex ] = new BlocksNumerator( 5 );
                        curTypeIndex++;
                    }

                    BlocksNameficator.namefy( currentBlock as IMyTerminalBlock,
                                                 (countOfType[ _typesIdexes[ currentBlock.GetType() ] ]), /*Debug*/ _parentProgram );
                }
                /*Debug*/
                _parentProgram.Echo( "Debug: GridInitializer::init() ////////// " );
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}