using System;
using System.Collections.Generic;
using System.Text;

using TriadCompiler.Parser.Common.Declaration.Polus;

namespace TriadCompiler.Parser.Common.Header
    {
    /// <summary>
    /// ?????? ?????????? ???????
    /// </summary>
    internal class Interface : CommonParser
        {
        /// <summary>
        /// ?????????
        /// </summary>
        /// <syntax>SingleInterface {SingleInterface}</syntax>
        /// <param name="endKeys">????????? ?????????? ???????? ????????</param>
        public static void Parse( EndKeyList endKeys )
            {
            do
                SingleInterface( endKeys.UniteWith( Key.LeftPar ) );
            while ( currKey == Key.LeftPar );
            }


        /// <summary>
        /// ????????? ?????????
        /// </summary>
        /// <syntax> ( PolusDeclaration {; PolusDeclaration} )</syntax>
        /// <param name="endKeys">????????? ?????????? ???????? ????????</param>
        private static void SingleInterface( EndKeyList endKeys )
            {
            if ( currKey == Key.LeftPar )
                {
                GetNextKey();

                //====by jum===
                List<IPolusType> poluses;
                //=======
                poluses = PolusDeclaration.Parse( endKeys.UniteWith( Key.Semicolon, Key.RightPar ), false );
                while ( currKey == Key.Semicolon )
                    {
                    GetNextKey();
                    poluses.AddRange(PolusDeclaration.Parse( endKeys.UniteWith( Key.Semicolon, Key.RightPar ), false ));
                    }
                //====by jum===
                if (Fabric.Instance.Parser is RoutineParser)
                {
                    (Fabric.Instance.Parser.DesignTypeInfo as RoutineInfo).Poluses = poluses;
                }
                //====
                Accept( Key.RightPar );
                }

            if ( !endKeys.Contains( currKey ) )
                {
                Fabric.Instance.ErrReg.Register( Err.Parser.WrongEndSymbol.SingleHeader, endKeys.GetLastKeys() );
                SkipTo( endKeys );
                }
            }
        }
    }
