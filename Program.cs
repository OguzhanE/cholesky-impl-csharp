using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void writeMatris(ref double?[][] M){
            for (int i = 0; i < M.Length; i++)
            {
                for (int j = 0; j < M[i].Length; j++)
                {
                    Console.Write(M[i][j]+", ");
                }
                System.Console.WriteLine();
            }
        }
        public static void Main(string[] args)
        {

            
            // // ->TEST<-
            //  int n = 3;
            //  double?[][] A = new double?[3][]{
            //      new double?[]{2, 1, -3},
            //      new double?[]{-1, 2, 2},
            //      new double?[]{3, 1, -3}
            //  };
            //  double?[] B = new double?[3]{4,6,6};
            //  double?[][] U = new double?[3][]{
            //      new double?[]{null,null,null},
            //      new double?[]{null,null,null},
            //      new double?[]{null,null,null}
            //  };
            //  double?[][] L = new double?[3][]{
            //      new double?[]{null,null,null},
            //      new double?[]{null,null,null},
            //      new double?[]{null,null,null}
            //  };
            //  
            
            System.Console.WriteLine("Kare matris boyutunu giriniz:");
            int n = int.Parse(Console.ReadLine());
            double?[][] A = genRandomMatris(n);
            double?[] B = genRandom(n);
            double?[][] U = genRandomMatris(n, true);
            double?[][] L = genRandomMatris(n, true);     
            
            
            System.Console.WriteLine("--A--");
            writeMatris(ref A);
            
            System.Console.WriteLine("--B--");
            for(int p = 0;p<3;p++){
                System.Console.WriteLine(B[p]);
            }
            
            initUpperTriangle(ref U, n);
            initLowerTriangle(ref L, n);
            build_L_and_U_Matris(ref A, ref U, ref L, n, 0);
            
            System.Console.WriteLine("--L--");
            writeMatris(ref L);
            
            System.Console.WriteLine("--U--");
            writeMatris(ref U);
            
            System.Console.WriteLine("--Z--");
            var Z = buildZ(L, B, 3);
            for(int p = 0;p<3;p++){
                System.Console.WriteLine(Z[p]);
            }
            
            var X = buildX(U, Z, 3);
            System.Console.WriteLine("===========Sonuc X===========");
            for(int p = 0;p<3;p++){
                System.Console.WriteLine(X[p]);
            }
            
            Console.ReadLine();
        }
        public static double?[] buildX(double?[][] U, double?[] Z, int n)
        {
            double?[] X = new double?[n];
            fillNull(ref X, n);
            for (int k = n-1; k >= 0; k--)
            {
                X[k] = findX(U[k], X, Z[k], k, n);
            }
            return X;
        }
        public static double?[] buildZ(double?[][] L, double?[] B, int n){
            double?[] Z = new double?[n];
            fillNull(ref Z, n);
            for (int k = 0; k < n; k++)
            {
                Z[k] = findX(L[k], Z, B[k], k, n);
            }
            return Z;
        }
        public static void fillNull(ref double?[] M, int n){
            for (int i = 0; i < n; i++)
            {
                M[i]=null;
            }
        }
        private static Random rnd = new Random();
        public static double?[] genRandom(int n, bool fillNull=false){
            double?[] ret = new double?[n];          
            for (int i = 0; i < n; i++)
            {
                if(fillNull) ret[i]=null;
                else ret[i]= rnd.Next(-10, 100);
            }
            return ret;
        }
        public static double?[][] genRandomMatris(int n, bool fillNull=false){
            double?[][] ret = new double?[n][];
            for (int i = 0; i < n; i++)
            {
                ret[i]=genRandom(n, fillNull);
            }
            return ret;
        }
        
        public static void build_L_and_U_Matris(ref double?[][] A, ref double?[][] U, ref double?[][] L, int n, int toFill=0){
            for (int k = toFill; k < n; k++)
            {
                U[toFill][k] = findX(L[toFill], buildColumn(U,k,n), A[toFill][k], toFill, n);
            }
            var columnForL = buildColumn(U, toFill, n);
            for (int k = toFill+1; k < n; k++)
            {
                L[k][toFill] = findX(L[k], columnForL, A[k][toFill], toFill, n);
            }
            toFill++;
            if(toFill < n)build_L_and_U_Matris(ref A, ref U, ref L, n, toFill);
        }
        public static double?[] buildColumn(double?[][] M, int toBuildColumnIndex, int n){
            double?[] ret = new double?[n];
            for (int i = 0; i < n; i++)
            {
                ret[i]=M[i][toBuildColumnIndex];
            }
            return ret;
        }
        public static double? findX(double?[] row, double?[] column, double? res, int xIndex, int n){
            double? mult = multiplyRowAndColumn(row, column, n, xIndex);
            double? div = row[xIndex] != null? row[xIndex]:column[xIndex];
            var ret = res - mult;
            if(div==0)return ret;
            return ret / div;
        }
        public static double? multiplyRowAndColumn(double?[] row, double?[] column, int n, int? exc=null){
            double? ret = 0;
            int k = 0;
            for (; k < n; k++)
            {
                if(k==exc)continue;
                double? res = null;
                if(row[k] == 0 || column[k] == 0){
                    res = 0;
                }
                else{
                    res = row[k] * column[k];
                }
                ret += res;
            }
            if(k==0) throw new Exception("invalid matris size [n]");
            return ret;
        }
        

        public static void fillZeroUntilTop(ref double?[][] M, int i, int j){
            for (;i >= 0; i--)
            {
                M[i][j] = 0;
            }
        }
        public static void initLowerTriangle(ref double?[][] M, int n){
            int i = n - 2;
            int j = n - 1;
            int fillZeroCount = n - 1;
            for (int k = 0; k < fillZeroCount; k++)
            {
                fillZeroUntilTop(ref M, i, j);
                i--;
                j--;
            }
            for (int p = 0; p < n; p++)
            {
                M[p][p] = 1;
            }
        }
        
        public static void fillZeroUntilBottom(ref double?[][] M, int n, int i, int j){
            for (;i < n; i++)
            {
                M[i][j]=0;
            }
        }
        public static void initUpperTriangle(ref double?[][] M, int n){
            int i = 1;
            int j = 0;
            int fillZeroCount = n-1;
            for (int k = 0; k < fillZeroCount; k++)
            {
                fillZeroUntilBottom(ref M, n, i, j);
                i++;
                j++;
            }
        }

    }
}
