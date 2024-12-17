using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STATIC_CSHARP_PROJECT
{
    class UintValueOperater
    {
        public const uint LEFT_IS_BIGGER = 1;
        public const uint LEFT_RIGHT_EQUAL = 2;
        public const uint RIGHT_IS_BIGGER = 3;

        //左右の値の大小比較を行う。checked2024_12_03
        public static uint CompareLeftAndRightValue(uint aLeftValue, uint aRightValue)
        {
            if (aLeftValue > aRightValue)
            {
                return LEFT_IS_BIGGER;
            }

            //ここに来る⇔{aLeftValue==aRightValue}又は{aLeftValue<aRightValue}

            if (aLeftValue == aRightValue)
            {
                //ここに来る⇔{aLeftValue==aRightValue}
                return LEFT_RIGHT_EQUAL;
            }

            //ここに来る⇔{aLeftValue<aRightValue}
            return RIGHT_IS_BIGGER;
        }


        //一次元のエリアがあるかどうか、判定.checked2024_11_06
        public static bool ExistOnedimSubArea(uint aWholeLength, uint aStartIndex, uint aRangeLength)
        {
            bool good_length_flag = ((aWholeLength >= 1) & (aRangeLength >= 1));

            if (!good_length_flag)//checked
            {
                return false;
            }

            //ここに来る⇔長さは共に1
            //∴エリアがあるかどうか、判定できる

            //本当はaStartIndex + aRangeLength -1 <= aWholeLength-1の判定をしている
            bool exist_onedim_sub_area = (aStartIndex + aRangeLength <= aWholeLength);

            return exist_onedim_sub_area;
        }

        //二次元のサブエリアが存在するかどうか、判定.checked2024_11_06
        public static bool ExistTwodimSubArea(uint aRowWholeLength, uint aStartRow, uint aRowRangeLength, uint aColWholeLength, uint aStartCol, uint aColRangeLength)
        {
            //縦方向に、一次元のサブエリアが存在するかどうか、判定
            bool exist_one_dim_area_row = ExistOnedimSubArea(aRowWholeLength, aStartRow, aRowRangeLength);

            //横方向に、一次元のサブエリアが存在するかどうか、判定
            bool exist_one_dim_area_col = ExistOnedimSubArea(aColWholeLength, aStartCol, aColRangeLength);

            //二次元のサブエリアがある⇔縦方向と横方向共に、一次元のサブエリアが存在する
            bool exit_twodim_sub_area = (exist_one_dim_area_row & exist_one_dim_area_col);
            return exit_twodim_sub_area;
        }

        private static uint GetLoopMax(uint aUintValue)
        {
            double sqrt_value = Math.Sqrt(aUintValue);

            uint loop_max = (uint)Math.Floor(sqrt_value);

            return loop_max;
        }
        public static bool IsPrime(uint aUintValue)
        {
            if (aUintValue <= 1)
            {
                return false;
            }

            //ここに来る⇔aUintValue>=2

            if (aUintValue <= 3)
            {
                //ここに来る⇔aUintValueは2または3
                return true;
            }

            //ここに来る⇔aUintValue>=4
            if (aUintValue % 2 == 0)
            {
                //ここに来る⇔aUintValueは4以上の偶数
                return false;
            }

            //ここに来る⇔aUintValueは5以上の奇数
            if (aUintValue <= 7)
            {
                //ここに来る⇔aUintValueは5または7
                return true;
            }

            //ここに来る⇔aUintValueは9以上の奇数
            //∴割る数を3からスタートして、2づつ増やしてチェックすれば良い

            //ループの最大値を得る
            uint loop_max = GetLoopMax(aUintValue);


            //素数かどうか、判定
            //loop_maxが、奇数の場合に、divide_valueがloop_maxまできたとしたら、
            //loop_maxで割り切れるかどうか、判定して終わり
            //(その時にifにヒットしないでdivide_valueが2増えたら、divide_valueがloop_max+2になり、
            //ループの継続条件はfalse)
            //loop_maxが、偶数の場合、最後にloop_max-1まできたとしたら、loop_max -1で
            //割り切れるかどうか調べて、判定して終わり
            //(その時にifにヒットしないでdivide_valueが2増えたら、divide_valueがloop_max+1になり、
            //ループの継続条件はfalse)
            
            for (uint divide_value = 3; divide_value <= loop_max; divide_value += 2)
            {
                if (aUintValue % divide_value == 0)
                {
                    return false;
                }
            }

            //ここに来る⇔3<=divide_value<=loop_maxの範囲で、割り切れる値がない
            //⇔aUintValueは素数
            return true;

        }

        //スタート値から増やしていって、素数を探す
        static uint SearchPrimeFromStart(uint aStart)
        {
            for (uint i = aStart; i <= 0xFFFFFFFF; i++)
            {
                if (IsPrime(i))
                {
                    return i;           
                }
            }

            //ここに来る⇔aStart<=i<=0xFFFFFFFFで素数が見つからなかった
            //∴エラーとして0を返す
            return 0;

        }

        //左の引数が、右の引数で何回割れるかを得る
        static public uint GetDividableCount(uint aDividedValue, uint aDivisor)
        {
            if (aDividedValue <= 1 || aDivisor <= 1)
            {
                return 0;
            }

            //ここに来る⇔二つの引数は共に2以上
            //∴何回割れるか、カウントできる

            uint dividable_count = 0;

            uint now_divided_value = aDividedValue;//必ず2以上

            //もし、ここですでにnow_divided_valueがaDivisorで割り切れないなら、
            //whileループに入らないでdividable_countは0のまま

            while (now_divided_value % aDivisor == 0)
            {
                now_divided_value /= aDivisor;//ここでの割り算は必ず割り切れる
                dividable_count += 1;       
            }

            return dividable_count;

        }

        //素因数分解を実行
        public static void ExecutePrimeFactorization(uint aValue, ref uint[] aRefPrimeArray, ref uint[] aRefPrimeDividableCountArray)
        {
            aRefPrimeArray =new uint[0];
            aRefPrimeDividableCountArray =new uint[0];

            if (aValue <= 1)
            {
                return;
            }

            //ここに来る⇔aValue>=2
            //∴素因数分解ができる

            //十分大きなサイズの配列を宣言
            uint[] prime_array_enough = new uint[aValue];
            uint[] prime_dividable_count_array_enough = new uint[aValue];
            uint prime_count = 0;//今までに使用した素数の個数。ローカルの配列のインデックスの役目もする

            uint now_divided_value = aValue;
            uint now_prime = 2;
            
            while (now_divided_value >= 2)
            {
                //毎回のループの開始時、prime_countが、これまでに見つかった素因数の種類と等しい
                //now_primeで、now_divided_valueが、何回割れるか、チェック
                uint now_dividable_count = GetDividableCount(now_divided_value, now_prime);

                if (now_dividable_count >= 1)
                {
                    prime_array_enough[prime_count] = now_prime;
                    prime_dividable_count_array_enough[prime_count] = now_dividable_count;

                    //now_divided_valueを、now_primeの、now_dividable_count乗で割って更新
                    now_divided_value /= (uint)Math.Pow(now_prime, now_dividable_count);

                    prime_count += 1;

                }

                //now_prime+1からスタートして、次の素数next_primeを得る
                uint next_prime = SearchPrimeFromStart(now_prime + 1);

                if (next_prime <= 1)
                {
                    //ここに来る⇔now_prime+1～0xFFFFFFFFの範囲で素数が見当たらない
                    break;
                }

                //ここに来る⇔now_prime+1～0xFFFFFFFFの範囲の、最小の素数がnext_prime
                //∴素数を更新
                now_prime = next_prime;
            }

            //ここでのprime_countの値が、aValueの素因数の種類の数に等しい
            //つまり、ここにおいて、ローカルのenoughの配列の、先頭からprime_count個が正しい結果になっている
            

            aRefPrimeArray = GenericArrayOperater.TakeSubArray<uint>(prime_array_enough, 0, prime_count);

            aRefPrimeDividableCountArray = GenericArrayOperater.TakeSubArray<uint>(prime_dividable_count_array_enough, 0, prime_count);






        }


    }
}
