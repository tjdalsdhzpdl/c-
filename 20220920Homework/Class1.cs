using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Runtime.Intrinsics;


//미로는 배열로 만들어 놓는다.
//bfs를 이용하여 구현한다.
//시작 위치와 끝 위치는 임의로 정해놓고, 상하좌우로만 이동할 수 있다.
//결과값을 구하면 큐에 부모 노드를 저장하여 역추적을 해야함.
namespace Simulation
{
    class Program
    {
        private static int N = 8;
        private static int[,] maze = new int[8, 8]{
            { 0,0,0,0,0,0,0,1 },
            { 0,1,1,0,1,1,0,1 },
            { 0,0,0,1,0,0,0,1 },
            { 0,1,0,0,1,1,0,0 },
            { 0,1,1,1,0,0,1,1 },
            { 0,1,0,0,0,1,0,1 },
            { 0,0,0,1,0,0,0,1 },
            { 0,1,1,1,0,1,0,0 }
            }; //미로 생성

        private static KeyValuePair<int,int>[,] mazeback = new KeyValuePair<int, int>[8, 8];
        private static int[] dx = new int[4] { -1, 1, 0, 0 };
        private static int[] dy = new int[4] { 0, 0, -1, 1 };
        private static int[,] visited = new int[8, 8];
        private static int goal_x = 7;
        private static int goal_y = 7;

        public static void bfs(int cur_x, int cur_y)
        {
            List<KeyValuePair<KeyValuePair<int,int>, int>> pa1 = new List<KeyValuePair<KeyValuePair<int,int>, int>>(); //부모 노드 연결 리스트
       
            visited[cur_x, cur_y] = 1; //현재 정점 방문 표시
            Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>(); //큐 선언
            q.Enqueue(new KeyValuePair<int, int>(cur_x, cur_y)); //시작 정점을 큐에 저장함
            while (q.Count > 0) //큐가 다 빌 때 까지(탐색이 끝나는 시점)
            {
                var v = q.Dequeue(); //큐에 저장된 정점 선택
                for (int i = 0; i < 4; i++)
                {
                    int next_x = v.Key + dx[i];
                    int next_y = v.Value + dy[i]; //동서남북으로 길 탐색

                    if (next_x < 0 || next_x >= 8 || next_y < 0 || next_y >= 8) //미로 벗어날 시 continue
                        continue;

                    if (maze[next_x, next_y] == 0 && visited[next_x, next_y] == 0) //0이거나(탐색 가능한 길), 방문한 적 없을 시
                    {

                        var par2 = new KeyValuePair<int, int>(v.Key, v.Value); 
                        visited[next_x, next_y] = visited[v.Key, v.Value] + 1; //방문 표시
                        mazeback[next_x, next_y] = par2;
                        q.Enqueue(new KeyValuePair<int, int>(next_x, next_y)); // 다음 정점을 큐에 저장
                    }
                }
            }
            Console.WriteLine();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(visited[i, j]); //방문 순서 출력
                    Console.Write("  ");
                }
                Console.WriteLine();
            }

            q.Enqueue(mazeback[goal_x, goal_y]); //부모의 정보가 담겨 있는 배열 중, 도착지점의 부모를 queue에 집어넣는다.
            maze[goal_x, goal_y] = 8; //도착지점을 표시해놓는다.

            while (true) //경로 역추적
            {
                var rr = q.Dequeue(); //도착지점의 부모의 값을 dequeue해준다.
                maze[rr.Key, rr.Value] = 8; //maze에 경로 표시
                if (rr.Key == cur_x && rr.Value == cur_y) //만약 시작지점에 돌아오면 break해준다.
                    break;
                q.Enqueue(mazeback[rr.Key, rr.Value]); //다음 부모의 값을 enqueue한다.
            }
     
           
        }
    
        public static void printMaze()
        {
            Console.WriteLine("미로 표시");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                   
                    Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
            /*
            
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(mazeback[i, j]);
                }
                Console.WriteLine();
            }
            */
        }


        public static void Main(string[] args)
        {
            Console.WriteLine("이동할 수 있는 공간 : 0, 벽: 1");
            printMaze();
            bfs(0, 0);
            Console.WriteLine();
            printMaze();

        }

    }
}
