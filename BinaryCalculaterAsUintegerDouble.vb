Public Class BinaryCalculaterAsUintegerDouble
    Inherits BitOperaterForUintegerDouble

    '上下連結の大小の判定をする。checked2024_02_10
    Public Shared Function JudgeLeftAndRightAboutUintegerDouble(ByVal aLeftHigh As UInteger, ByVal aLeftLow As UInteger, ByVal aRightHigh As UInteger, ByVal aRightLow As UInteger) As UInteger

        Dim judge_high As UInteger = BinaryCalculaterUinteger.JudgeLeftAndRightUintegerValue(aLeftHigh, aRightHigh)

        If judge_high <> BinaryCalculaterUinteger.LEFT_RIGHT_EQUAL Then '上の値が異なる
            '上の大小がそのまま全体の大小になる
            Return judge_high 'checked
        End If

        'ここに来る⇔上の値が同じ⇔下の値の判定が全体の大小の判定になる

        Dim judge_low As UInteger = BinaryCalculaterUinteger.JudgeLeftAndRightUintegerValue(aLeftLow, aRightLow)
        Return judge_low

    End Function

    '上下4バイトを連結した8バイト同士の足し算をする。結果の8バイトを、上下4バイトで得る.
    '下の4バイトから上の4バイトへのキャリーは扱う。だが、上の4バイトからの、更に上のキャリーは無視.checked2024_02_08
    Public Shared Sub PlusTwoUintegerDoubleValues(ByVal aHighA As UInteger, ByVal aLowA As UInteger, ByVal aHighB As UInteger, ByVal aLowB As UInteger, ByRef aRefPlusHigh As UInteger, ByRef aRefPlusLow As UInteger)

        '下の4バイトを計算して、キャリーと、下4バイトの結果を得る。aLowAとaLowBを足して、下から上へのキャリーを得る
        Dim ref_carry_low_to_high As UInteger = 0
        BinaryCalculaterUinteger.PlusTwoUintegerValues(aLowA, aLowB, ref_carry_low_to_high, aRefPlusLow) 'checked

        '下から上へのキャリーと、aHighAの計算をして、無視するキャリー1と、足し算の結果を得る
        Dim ref_high_a_carry As UInteger = 0
        Dim ref_ignored_carry_1 As UInteger = 0
        BinaryCalculaterUinteger.PlusTwoUintegerValues(ref_carry_low_to_high, aHighA, ref_ignored_carry_1, ref_high_a_carry)

        'ref_high_a_carryと、aHighBの計算をして、無視するキャリー2と、足し算の結果を得る
        Dim ref_ignored_carry_2 As UInteger = 0
        BinaryCalculaterUinteger.PlusTwoUintegerValues(ref_high_a_carry, aHighB, ref_ignored_carry_2, aRefPlusHigh)

        Return
    End Sub

    '上下4バイトを連結した8バイト同士の足し算をする。結果は上書き.checked2024_02_09
    Public Shared Sub PlusOverWriteUintegerDoubleValue(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByRef aRefOverWriteHigh As UInteger, ByRef aRefOverWriteLow As UInteger)

        '最初に、足し算する前の値をコピーで記憶
        Dim previous_copy_high As UInteger = aRefOverWriteHigh
        Dim previous_copy_low As UInteger = aRefOverWriteLow

        PlusTwoUintegerDoubleValues(aUintegerHigh, aUintegerLow, previous_copy_high, previous_copy_low, aRefOverWriteHigh, aRefOverWriteLow)

        Return

    End Sub


    '32ビット連結での、2の補数による引き算.checked2024_02_09
    Public Shared Sub MinusTwoUintegerDoubleValuesByTwoComplement(ByVal aLeftHigh As UInteger, ByVal aLeftLow As UInteger, ByVal aRightHigh As UInteger, ByVal aRightLow As UInteger, ByRef aRefMinusHigh As UInteger, ByRef aRefMinusLow As UInteger)

        'Rightの値の、連結のNOTを求める(checked)
        Dim not_right_high As UInteger = 0
        Dim not_right_low As UInteger = 0
        BitOperaterForUintegerDouble.NOTAsUintegerDouble(aRightHigh, aRightLow, not_right_high, not_right_low)

        '(連結のLeftの値)+(連結のRightのNOTの値)の計算をする(この計算のキャリーは無視)(checked)
        Dim plus_left_not_right_high As UInteger = 0
        Dim plus_left_not_right_low As UInteger = 0
        PlusTwoUintegerDoubleValues(aLeftHigh, aLeftLow, not_right_high, not_right_low, plus_left_not_right_high, plus_left_not_right_low)

        '最後、+1をした結果がマイナスの結果(checked)
        PlusTwoUintegerDoubleValues(plus_left_not_right_high, plus_left_not_right_low, 0, 1, aRefMinusHigh, aRefMinusLow)


        Return
    End Sub

    '32ビット連結での、2の補数による引き算で、オーバーライト.checked2024_02_10
    Public Shared Sub MinusOverWriteTwoUintegerDoubleValuesByTwoComplement(ByRef aRefOverWriteLeftHigh As UInteger, ByRef aRefOverWriteLeftLow As UInteger, ByVal aRightHigh As UInteger, ByVal aRightLow As UInteger)

        '参照渡しの値をコピー
        Dim copy_left_high As UInteger = aRefOverWriteLeftHigh
        Dim copy_left_low As UInteger = aRefOverWriteLeftLow

        'コピーの値と、値渡しの引数の値で、引き算を実行。その結果は、再び参照渡しの値に入れる
        MinusTwoUintegerDoubleValuesByTwoComplement(copy_left_high, copy_left_low, aRightHigh, aRightLow, aRefOverWriteLeftHigh, aRefOverWriteLeftLow)

        Return

    End Sub

    '64ビット(32ビット上下連結)同士の、二進法による割り算。checked2024_02_10
    Public Shared Sub DivideUintegerDouble(ByVal aDividedHigh As UInteger, ByVal aDividedLow As UInteger, ByVal aDivisorHigh As UInteger, ByVal aDivisorLow As UInteger, ByRef aRefQuotientHigh As UInteger, ByRef aRefQuotientLow As UInteger, ByRef aRefRemainHigh As UInteger, ByRef aRefRemainLow As UInteger)

        '一旦、参照渡しの変数は全て0
        aRefQuotientHigh = 0
        aRefQuotientLow = 0
        aRefRemainHigh = 0
        aRefRemainLow = 0


        If (aDivisorHigh = 0) And (aDivisorLow = 0) Then '割る数が0

            Throw New Exception("divisor_is_zero") 'checked
            Return

        End If

        'ここに来る⇔割る数>=1

        If (aDivisorHigh = 0) And (aDivisorLow = 1) Then '割る数が丁度1

            '商は、割られる数になる
            aRefQuotientHigh = aDividedHigh
            aRefQuotientLow = aDividedLow
            '余りは、0
            aRefRemainHigh = 0
            aRefRemainLow = 0

            Return 'checked
        End If

        'ここに来る⇔割る数>=2

        '割られる数と割る数の大小関係の判定をしておく

        Dim judge_divided_divisor As UInteger = JudgeLeftAndRightAboutUintegerDouble(aDividedHigh, aDividedLow, aDivisorHigh, aDivisorLow)

        If judge_divided_divisor = BinaryCalculaterUinteger.RIGHT_IS_BIGGER Then '割られる数<割る数
            '商の上下は共に0
            aRefQuotientHigh = 0
            aRefQuotientLow = 0
            '余りは、割られる数
            aRefRemainHigh = aDividedHigh
            aRefRemainLow = aDividedLow

            Return 'checked
        End If

        'ここに来る⇔{割る数>=2}かつ{割られる数>=割る数}⇔{2<=割る数<=割られる数}

        If judge_divided_divisor = BinaryCalculaterUinteger.LEFT_RIGHT_EQUAL Then
            '商の上は0
            aRefQuotientHigh = 0
            '商の下は1
            aRefQuotientLow = 1

            '余りは0
            aRefRemainHigh = 0
            aRefRemainLow = 0

            Return 'checked
        End If
        'ここに来る⇔{割る数>=2}かつ{割られる数>=割る数}⇔{2<=割る数<割られる数}


        '∴計算で割り算をする

        '割られる数の、最も高いビット1の位置
        Dim highest_bitpos_divided As UInteger = GetHighestBitOnePosInDoubleUinteger(aDividedHigh, aDividedLow)

        '割る数の、最も高いビット1の位置
        Dim highest_bitpos_divisor As UInteger = GetHighestBitOnePosInDoubleUinteger(aDivisorHigh, aDivisorLow)

        '割る数の、左シフト数の最大
        Dim divisor_max_left_shift As UInteger = highest_bitpos_divided - highest_bitpos_divisor 'ここでは必ず0以上

        'これから減らしていく数の初期化。割られる数で初期化する
        Dim reduced_high As UInteger = aDividedHigh
        Dim reduced_low As UInteger = aDividedLow

        '商の上下は、0で初期化
        Dim quotient_high As UInteger = 0
        Dim quotient_low As UInteger = 0

        'divisor_max_left_shiftから0までの、デクリメントのループ
        For left_shift As Integer = divisor_max_left_shift To 0 Step -1

            '割る数を、連結の左シフトで<<left_shiftした値を得る(checked)
            Dim divisor_left_shift_high As UInteger
            Dim divisor_left_shift_low As UInteger
            LeftShiftUintegerDouble(aDivisorHigh, aDivisorLow, left_shift, divisor_left_shift_high, divisor_left_shift_low)

            '連結(reduced_high,reduced_low)の値と、連結で左シフトした値の大小比較
            Dim judge_reduced_divisor_left_shift As UInteger = JudgeLeftAndRightAboutUintegerDouble(reduced_high, reduced_low, divisor_left_shift_high, divisor_left_shift_low)


            '連結(reduced_high,reduced_low)の値>=連結で左シフトした値
            If judge_reduced_divisor_left_shift <> BinaryCalculaterUinteger.RIGHT_IS_BIGGER Then 'checked

                '連結(reduced_high,reduced_low)の値-=連結で左シフトした値(つまり、引き算の結果の値の上書き)
                MinusOverWriteTwoUintegerDoubleValuesByTwoComplement(reduced_high, reduced_low, divisor_left_shift_high, divisor_left_shift_low)

                '連結quotientの、第left_shitビットを1にする
                SetBitUintegerDouble(quotient_high, quotient_low, left_shift, True)



            End If

            If left_shift = 0 Then 'デクリメントのループが、無限ループになる事を防ぐ

                Exit For 'checked

            End If

        Next

        '参照渡しの、商の変数に、上下の結果を入れる
        aRefQuotientHigh = quotient_high
        aRefQuotientLow = quotient_low
        '参照渡しの、余りの変数に、上下の結果を入れる
        aRefRemainHigh = reduced_high
        aRefRemainLow = reduced_low

        Return

    End Sub




End Class
