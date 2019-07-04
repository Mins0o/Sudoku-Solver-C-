using System;
using System.IO;

namespace SUDOKU{

	class Solver{
	
		static void Main(){
			//초기조건
			int x; int y; int val;
			bool[,,,,] Puzzle=new bool[3,3,3,3,10];//[ x구역 , y구역 , x , y , 메모(첫번째는 숫자가 정해져있는지, 나머지 9개는 각 숫자별 가능 여부)]
			int repcnt = 10;
			
			for( byte i = 0 ; i < 81 ; i++ ){
				for( byte j = 1 ; j < 10 ; j++ ){
				
					Puzzle[ ( i % 9 ) / 3 , i / 27 , i % 3 , ( i / 9 ) % 3 , j ] = true;//모든 숫자가 가능할 때,
					
				}
			
				Puzzle[ ( i % 9 ) / 3 , i / 27 , i % 3 , ( i / 9 ) % 3 , 0 ] = false;//값이 정해져 있지 않다.
			
			}
		
			bool contin = true;
		
			//입력받기
			while( contin ){
			
				Console.WriteLine( "s(solve) ; i(input) ; p(print) ; c(terminate)" );
				Console.Write( "Select menu : " );
				string key = Console. ReadLine();
				//bool solvable = true;
				switch( key ){
					
					case "s":
						for( byte i = 0 ; i < repcnt ; i++ ){
							MarkHiddenDef( Puzzle );
							MarkOnly( Puzzle );
						}
						Print( Puzzle , true );
						
					break;
					
					case "i":
						while( true ){
							
							Console.WriteLine( "번호를 입력할 칸의 좌표를 받습니다." ); Console.Write( "좌측 상단을 1(세로),1(가로)로 볼 때, 입력 칸의" );
							
							Console.WriteLine( "x : " );
							x = Console.Read() - 48;
							Console.Read(); Console.Read();
							
							if( x > 9 || x < 1 ){ break; }//계속하지 않는다.
							
							Console.WriteLine( "y :" );
							y = Console. Read() - 48;
							Console.Read(); Console.Read();
							
							if( y > 9 || y < 1 ){ break; }//계속하지 않는다.
							
							Console.Write( "해당 칸의 값은?" );
							val = Console.Read() - 48;
							Console.Read(); Console.Read();
							
							if( val < 0 || val > 9 ){ Console.WriteLine( "불가능한 값입니다" ); break; }
							
							WriteNum( Puzzle , x - 1 , y - 1 , val );
							Print( Puzzle ,false );
						
						}
						
					break;
						
					case "p":
						
						Print( Puzzle , true );
						
					break;
					
					case "c":
					
						contin = false;
							
					break;
					
				}
				
			}
			
		
			
			return;
		}		
			
						
						
		static void WriteNum( bool [,,,,] Puzzle , int x , int y , int val ){
				//좌표 x,y와 값 val을 받아서 해당 칸에 값을 기록하는 함수
				
			byte numNow = ReadNum( Puzzle , x , y );

			if( numNow != 0 ){//이미 0의 값이 아니라면 값을 초기화해줍니다.
			
				Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , 0 ] = false;
				for( byte index = 0 ; index < 9 ; index++ ){
					if( !Puzzle[ x / 3 , index / 3 , x % 3 , index % 3 , 0 ] ){ Puzzle[ x / 3 , index / 3 , x % 3 , index % 3 , numNow ] = true; }//열
					if( !Puzzle[ index / 3 , y / 3 , index % 3 , y % 3 , 0 ] ){ Puzzle[ index / 3 , y / 3 , index % 3 , y % 3 , numNow ] = true; }//행
					if( !Puzzle[ x / 3 , y / 3 , index % 3 , index % 3 , 0 ] ){ Puzzle[ x / 3 , y / 3 , index % 3 , index % 3 , numNow ] = true; }//구역
					Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , index + 1 ] = true;
				}
			}
			
			
			if( val != 0 ){//그 의외의 유효한 값은
			
				for( byte index = 0 ; index < 9 ; index++ ){ //그 칸이 속한 행/열/구역 에 대해 칸마다 그 수가 불가하다는 것을 표시하고
				
					if( !Puzzle[ x / 3 , index / 3 , x % 3 , index % 3 , 0 ] ){ Puzzle[ x / 3 , index / 3 , x % 3 , index % 3 , val ] = false; }//열
					if( !Puzzle[ index / 3 , y / 3 , index % 3 , y % 3 , 0 ] ){ Puzzle[ index / 3 , y / 3 , index % 3 , y % 3 , val ] = false; }//행
					if( !Puzzle[ x / 3 , y / 3 , index % 3 , index % 3 , 0 ] ){ Puzzle[ x / 3 , y / 3 , index % 3 , index % 3 , val ] = false; }//구역
						
				}
				
				for( byte i = 1 ; i < 10 ; i++ ){
					if( i == val ){ Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , i ] = true; }
					else { Puzzle [ x / 3 , y / 3 , x % 3 , y % 3 , i ] = false; }
					
				}
				
				Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , 0 ] = true;
				
			}
			
			return;
		}
						
				

		static byte ReadNum( bool [,,,,] Puzzle , int x , int y ){//x와 y 좌표를 받아서 그에 해당하는 숫자를 반환합니다. 만약 확정수가 없으면 0을 반환합니다.
		
			if( Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , 0 ] ){//확실한 숫자 하나가 있어야만 ReadNum을 실행합니다.
			
				for( byte i = 1 ; i < 10 ; i++ ){
					
					if( Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , i ] ){ return i; }
					//확정수가 반환됩니다.
					
				}
				
			}
			
			return 0;
		}
		
		
		
		static void Print( bool [,,,,] Puzzle , bool sv ){
			string title = "";
			if( sv == true ) {
				Console.Write( "What is the title of the save file?" );
				title = Console.ReadLine();
				if(File.Exists( title + ".txt" ) ){ File.WriteAllText( title + ".txt" , "" ); }
			}
			
			
			for( byte i = 0 ; i < 81 ; i++ ){//모든 칸에 대해 ReadNum을 양식에 맞게 실행합니다.
			
				if( i % 9 == 0 ){ Console.Write( "\n" ); if( sv == true ){File.AppendAllText( title + ".txt" , Environment.NewLine );}}
				else if( i % 3 == 0 ){ Console.Write( "  |" ); if( sv == true ){File.AppendAllText( title + ".txt" , "|" );}}
				if( i % 27 == 0 ){ Console. WriteLine( "---------------------------------" ); if( sv == true ){File.AppendAllText( title + ".txt" , "------------" + Environment.NewLine );}}
				Console.Write( "{0,3}", ReadNum( Puzzle , i % 9 , i / 9 ) );
				if( sv == true ){File.AppendAllText( title + ".txt" , ReadNum( Puzzle , i % 9 , i / 9 ).ToString() );}
				
			}

			Console.WriteLine( "\n---------------------------------" );
			
			return;
		}
		
		static bool MarkHiddenDef( bool [,,,,] Puzzle ){//어떤 칸에 가능한 수가 단 한 개 있는데 확정수로 표시되어있지 않은 경우 확정수로 전환하는 함수
			bool ret = false;
			for( byte i = 0 ; i < 81 ; i++ ){
				
				int x = i % 9;
				int y = i / 9;
				
				if( !Puzzle[ x / 3 , y / 3 , x % 3 , y % 3 , 0 ] ){
					
					byte memory = 0;
					byte truesum = 0 ;//가능한 숫자가 한개만 있는지 확인하는데 사용되는 변수
					
					for( byte scan = 1 ; scan < 10 ; scan++ ){//가능한 숫자의 개수를 세는데,
					
						if( Puzzle [ x / 3 , y / 3 , x % 3 , y % 3 , scan ] ){ truesum++ ; memory = scan; }//여기서 숫자를 셈니다.(emory로는 어느 자리가 그 한 개의 수였는지 기억합니다.
						if( truesum == 2 ){ break; }//2개를 넘어가면 바로 다음으로 넘어갑니다.
						
					}
					
					//모두 세고 난 후,
					if( truesum == 1 ){ WriteNum( Puzzle , x ,y , memory ); ret = true; }//그 수를 기록합니다.
					
				}
				
			}
			
			return ret;
		}
		
		static bool MarkOnly( bool [,,,,] Puzzle ){
			bool ret = false;
			for( byte num = 1 ; num < 10 ; num++){
				
				for( byte index = 0 ; index < 9 ; index++ ){
					
					byte aTruesum = 0; byte aMemory = 0;
					byte rTruesum = 0; byte rMemory = 0;
					byte cTruesum = 0; byte cMemory = 0;
						
					for( byte rpt = 0 ; rpt < 9 ; rpt++ ){
						
						if( Puzzle[ index % 3 , index / 3 , rpt % 3 , rpt / 3 , num ] && aTruesum < 2 ){ aTruesum++ ; aMemory = rpt; }
						if( Puzzle[ rpt / 3 , index / 3 , rpt % 3 , index % 3 , num ] && rTruesum < 2 ){ rTruesum++ ; rMemory = rpt; }
						if( Puzzle[ index / 3 , rpt / 3 , index % 3 , rpt % 3 , num ] && cTruesum < 2 ){ cTruesum++ ; cMemory = rpt; }
						
					}
					
					if( aTruesum == 1 ){ WriteNum( Puzzle , ( index % 3 ) * 3 + aMemory % 3 , ( index / 3 ) * 3 +aMemory / 3 , num ); ret = true; }
					if( rTruesum == 1 ){ WriteNum( Puzzle , rMemory , index , num ); ret = true; }
					if( cTruesum == 1 ){ WriteNum( Puzzle , index , cMemory , num ); ret = true; }
					
				}
				
			}
			
			return ret;
		}
		
	}
	
}
						
						
						
						
						
						