using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//2023_05_20_re_check
namespace FILE_RECOVERY_CSHARP
{
    class Polynomial32BitDouble:Operater32BitDouble
    {
        const uint ERROR_QUOTIENT = 0xFFFFFFFF;
        const uint ERROR_REMAINDER = 0xFFFFFFFF;
        //多項式の足し算.checked2021_06_14
        static protected void PlusPolynomial32BitDouble(uint aHighPolynomial1, uint aLowPolynomial1, uint aHighPolynomial2, uint aLowPolynomial2, ref uint aHighPlusPolnomial, ref uint aLowPlusPolnomial)
        {
            Operater32BitDouble.EXOR32BitDouble(aHighPolynomial1, aLowPolynomial1, aHighPolynomial2, aLowPolynomial2, ref aHighPlusPolnomial, ref aLowPlusPolnomial);
        }

        //多項式の足し算を上書きで行う。checked2021_06_14
        static protected void PlusPolynomial32BitDoubleSelfOverwrite(uint aHighPolynomial, uint aLowPolynomial, ref uint aOverwriteHighPolynomial, ref uint aOverwriteLowPolynomial)
        {
            Operater32BitDouble.EXOR32BitDoubleSelfOverwrite(aHighPolynomial, aLowPolynomial, ref aOverwriteHighPolynomial, ref  aOverwriteLowPolynomial);

        }

        //多項式の掛け算を行い、結果を参照渡しの変数に入れる.checked2021_06_14
        static public void MultiplyPolynomial32Bit(uint aPolynomial1, uint aPolynomial2, ref uint aHighMultiply, ref uint aLowMultiply)//protected
        {
            uint shift_polynomial1_high = 0;
            uint shift_polynomial1_low = 0;

            aHighMultiply = 0;
            aLowMultiply = 0;

            for (uint bitpos = 0; bitpos <= UINT_BIT_LENGTH - 1; bitpos++)//0<=bitpos<=31で繰り返し
            {
                if (IsBitOneBitpos(aPolynomial2, bitpos))//aPolynomial2の第bitposビットが1
                {
                    //aPolynomial1をbitposビット左シフトした結果を、上書きで(aHighMultiply ,aLowMultiply)に足す

                    //aPolynomial1をbitposビット左シフトした結果を(shift_polynomial1_high, shift_polynomial1_low)に入れる
                    Operater32BitDouble.LeftBitShift32BitDouble(0, aPolynomial1, bitpos, ref shift_polynomial1_high, ref shift_polynomial1_low);

                    //(shift_polynomial1_high, shift_polynomial1_low)を、上書きで上書きで(aHighMultiply ,aLowMultiply)に足す
                    PlusPolynomial32BitDoubleSelfOverwrite(shift_polynomial1_high, shift_polynomial1_low, ref aHighMultiply, ref aLowMultiply);
                }
            }
        }

        //32ビットの値を二つ連結して、64ビット同士の多項式の割り算を行う。商(64ビット)と余り(64ビット)は、上下32ビットに分離して参照渡しの変数に入れるchecked2021_07_07
        static protected void DividePolynomial32BitDouble(uint aHighDivided, uint aLowDivided, uint aHighDivisor, uint aLowDivisor, ref uint aRefHighQuotient, ref uint aRefLowQuotient, ref uint aRefHighRemainder, ref uint aRefLowRemainder)//protected
        {
            //割る多項式がゼロなら、割り算自体が成立しないので商も余りもエラーにして終了
            if ((aHighDivisor == 0) && (aLowDivisor == 0))
            {
                //商
                aRefHighQuotient = ERROR_QUOTIENT;
                aRefLowQuotient = ERROR_QUOTIENT;
                //余り
                aRefHighRemainder = ERROR_REMAINDER;
                aRefLowRemainder = ERROR_REMAINDER;
                return;//checked

            }


            //ここに来たら割る多項式は非ゼロ

            //商はゼロにしておく
            aRefHighQuotient = 0;
            aRefLowQuotient = 0;

            //割られる多項式がゼロの時、余りをゼロにして終了
            if ((aHighDivided == 0) && (aLowDivided == 0))
            {
                //商はゼロのまま
                //余りはゼロ              
                aRefHighRemainder = 0;
                aRefLowRemainder = 0;
                return;//checked
            }

            //ここに来たら両方の多項式が非ゼロ(従って、両者のdegreeを正しく取得できる)
            uint degree_divided = GetHighestBitposDouble(aHighDivided, aLowDivided);
            uint degree_divisor = GetHighestBitposDouble(aHighDivisor, aLowDivisor);

            //割られる多項式のdegree<割る多項式のdegreeの場合、商はゼロで、余りは割られる多項式と同じにして終了
            if (degree_divided < degree_divisor)
            {
                //商はゼロのまま
                //余りは割られる多項式と同じ
                aRefHighRemainder = aHighDivided;
                aRefLowRemainder = aLowDivided;
                return;//checked

            }


            //ここに来る⇔両方の多項式が非ゼロ∧割られる多項式のdegree>=割る多項式のdegree

            //割られる多項式をコピー。これらがやがて余りになる
            uint high_divided_copy = aHighDivided;
            uint low_divided_copy = aLowDivided;

            uint initial_left_shift = degree_divided - degree_divisor;//degreeの差==最初に割る多項式を左シフトするシフト幅

        
        

            //商がゼロの状態から、ビット1を高いビットから立てていく。最終的には本当の商の多項式になる
            for (uint left_shift = initial_left_shift; left_shift >= 0; left_shift--)
            {
                //(high_divided_copy, low_divided_copy)の第(left_shift+degree_divisor)ビットをブール値で取得
                bool bitflag_divided_copy = IsBitOneBitposDouble(high_divided_copy, low_divided_copy, left_shift + degree_divisor);
                if (bitflag_divided_copy) //(high_divided_copy, low_divided_copy)の第(left_shift+degree_divisor)ビットが1
                {
                    //(high_shift_divisor,low_shift_divisor)を(aHighDivisor, aLowDivisor)<<left_shiftにする
                    uint high_shift_divisor=0;
                    uint low_shift_divisor=0;
                    LeftBitShift32BitDouble(aHighDivisor, aLowDivisor, left_shift, ref high_shift_divisor, ref low_shift_divisor);

                    //(high_divided_copy,low_divided_copy)EXOR(high_shift_divisor,low_shift_divisor)→(high_divided_copy,low_divided_copy)上書き
                    //これにより、(high_divided_copy,low_divided_copy)の第(left_shift+degree_divisor)ビットが0になる
                    PlusPolynomial32BitDoubleSelfOverwrite(high_shift_divisor, low_shift_divisor, ref high_divided_copy, ref low_divided_copy);

                    //(aHighQutotient, aLowQuotient)の第left_shiftビットを1にする
                    SetBitDouble(ref aRefHighQuotient, ref aRefLowQuotient, left_shift, true);

                   

                   
                }

                if (left_shift == 0)
                {
                    break;
                }
            }
            //forループを抜けた時の(high_divided_copy,low_divided_copy)が余りになる
            aRefHighRemainder = high_divided_copy;
            aRefLowRemainder = low_divided_copy;
            return;
        }
    }
}
