Public Class BitOperaterForUintegerDouble
    Inherits BitOperaterForUinteger

    Public Const UINTEGER_DOUBLE_BIT_LENGTH As UInteger = 2 * UINTEGER_BIT_LENGTH



    '32ビット上下連結のOR.checked2023_10_11
    Public Shared Sub ORAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '上のuinteger同士のOR
        aRefHighResult = ORAsUinteger(aHighUinteger1, aHighUinteger2)

        '下のuinteger同士のOR
        aRefLowResult = ORAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub


    '32ビット上下連結のAND.checked2023_10_11
    Public Shared Sub ANDAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '上のuinteger同士のAND
        aRefHighResult = ANDAsUinteger(aHighUinteger1, aHighUinteger2)

        '下のuinteger同士のAND
        aRefLowResult = ANDAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub


    '32ビット上下連結のEXOR.checked2023_10_11
    Public Shared Sub EXORAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '上のuinteger同士のEXOR
        aRefHighResult = EXORAsUinteger(aHighUinteger1, aHighUinteger2)

        '下のuinteger同士のEXOR
        aRefLowResult = EXORAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub

    '32ビット上下連結のNOT.checked2023_10_11
    Public Shared Sub NOTAsUintegerDouble(ByVal aHighUinteger As Double, ByVal aLowUinteger As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '上のuintegerのNOT
        aRefHighResult = NOTAsUinteger(aHighUinteger)

        '下のuintegerのNOT
        aRefLowResult = NOTAsUinteger(aLowUinteger)

    End Sub


    '32ビットを連結して64ビットにして、左シフト.checked2023_10_10
    Public Shared Sub LeftShiftUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aLeftShiftNum As UInteger, ByRef aRefHighShiftResult As UInteger, ByRef aRefLowShiftResult As UInteger)

        aRefHighShiftResult = 0
        aRefLowShiftResult = 0
        If aLeftShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH Then


            Return 'checked

        End If

        'ここに来る⇔aLeftShiftNum<= UINTEGER_DOUBLE_BIT_LENGTH - 1
        '∴左シフトができる
        If aLeftShiftNum >= UINTEGER_BIT_LENGTH Then
            'ここに来る⇔UINTEGER_BIT_LENGTH<=aLeftShiftNum<= UINTEGER_DOUBLE_BIT_LENGTH - 1

            '下のシフト結果は0のまま

            '上のシフト結果は下の引数を<<(aLeftShiftNum - UINTEGER_BIT_LENGTH)した値(checked)
            aRefHighShiftResult = LeftShiftUinteger(aLowUinteger, aLeftShiftNum - UINTEGER_BIT_LENGTH)

            Return 'checked

        End If

        'ここに来る⇔UINTEGER_BIT_LENGTH-1>=aLeftShiftNum >=0

        '下の結果については、下の引数の値<<aLeftShitNum
        aRefLowShiftResult = LeftShiftUinteger(aLowUinteger, aLeftShiftNum)

        '上の結果については、次の二つの値の(uinteger同士の)OR
        '引数の上の値<<aLeftShiftNum
        Dim high_uinteger_left_shift As UInteger = LeftShiftUinteger(aHighUinteger, aLeftShiftNum)


        '引数の下の値>>(UINTEGER_BIT_LENGTH-aLeftShiftNum)
        Dim low_uinteger_right_shift As UInteger = RightShiftUinteger(aLowUinteger, UINTEGER_BIT_LENGTH - aLeftShiftNum)

        '上の結果
        aRefHighShiftResult = ORAsUinteger(high_uinteger_left_shift, low_uinteger_right_shift)

        Return
    End Sub

    '32ビットを連結して64ビットにして、右シフト.checked2023_10_10

    Public Shared Sub RightShiftUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aRightShiftNum As UInteger, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '最初、参照渡しの変数は、0で初期化
        aRefHighResult = 0
        aRefLowResult = 0


        If aRightShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH Then
            Return
        End If

        'ここに来る⇔aRightShiftNum<=UINTEGER_DOUBLE_BIT_LENGTH-1

        If aRightShiftNum >= UINTEGER_BIT_LENGTH Then 'checked
            'ここに来る⇔UINTEGER_BIT_LENGTH<=aRightShiftNum<=UINTEGER_DOUBLE_BIT_LENGTH-1

            '上の参照渡しの変数は0のまま

            '下の参照渡しの変数は、上の引数を>>(aRightShiftNum-UINTEGER_BIT_LENGTH)した値(値も正常)
            aRefLowResult = RightShiftUinteger(aHighUinteger, aRightShiftNum - UINTEGER_BIT_LENGTH)

            Return

        End If

        'ここに来る⇔aRightShiftNum<=UINTEGER_BIT_LENGTH-1

        '上の結果は、上の引数を>>aRightShiftNumした値
        aRefHighResult = RightShiftUinteger(aHighUinteger, aRightShiftNum)

        '下の結果は、次の二つの値のORである

        '上の引数を<<(UINTEGER_BIT_LENGTH-aRightShiftNum)した値
        Dim high_uinteger_left_shift As UInteger = LeftShiftUinteger(aHighUinteger, UINTEGER_BIT_LENGTH - aRightShiftNum)

        '下の引数を>>aRightShiftNumした値
        Dim low_uinteger_right_shift As UInteger = RightShiftUinteger(aLowUinteger, aRightShiftNum)

        aRefLowResult = ORAsUinteger(high_uinteger_left_shift, low_uinteger_right_shift)

        Return


    End Sub

    '64ビット(32ビット上下連結)の、符号つき左シフト.checked2024_03_01
    Public Shared Sub LeftShiftUintegerDoubleWithSignBit(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aLeftShiftNum As UInteger, ByRef aRefLeftShiftHigh As UInteger, ByRef aRefLeftShiftLow As UInteger)

        aRefLeftShiftHigh = 0
        aRefLeftShiftLow = 0

        If aLeftShiftNum = 0 Then
            aRefLeftShiftHigh = aUintegerHigh
            aRefLeftShiftLow = aUintegerLow
            Return 'checked
        End If

        'ここに来る⇔1<=aLeftShiftNum

        '最も上のビットだけ記憶
        Dim only_sign_bit_memory As UInteger = ANDAsUinteger(aUintegerHigh, &H80000000L)


        If aLeftShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH - 1 Then

            aRefLeftShiftHigh = only_sign_bit_memory
            aRefLeftShiftLow = 0

            Return 'checked

        End If

        'ここに来る⇔1<=aLeftShiftNum<=63
        '∴符号つき左シフトができる

        '通常の左シフトを行う
        Dim ref_left_shift_high As UInteger = 0
        Dim ref_left_shift_low As UInteger = 0
        LeftShiftUintegerDouble(aUintegerHigh, aUintegerLow, aLeftShiftNum, ref_left_shift_high, ref_left_shift_low)

        '最上位ビットを一旦0にする
        Dim ref_left_shift_high_sign_bit_zero As UInteger = ANDAsUinteger(ref_left_shift_high, &H7FFFFFFF)

        '下の32ビットは、下のif文にヒットしてもしなくても、通常の左シフトの結果
        aRefLeftShiftLow = ref_left_shift_low

        If only_sign_bit_memory = 0 Then '元々の、記憶しておいた最上位ビットが0
            aRefLeftShiftHigh = ref_left_shift_high_sign_bit_zero

            Return 'checked
        End If

        'ここに来る⇔{1<=aLeftShiftNum<=63}かつ{元々の、記憶しておいた最上位ビットが1}

        '上の32ビットは、最上位ビットを一旦0にした結果と、0x80000000とのOR
        aRefLeftShiftHigh = ORAsUinteger(ref_left_shift_high_sign_bit_zero, &H80000000L)

        '下の32ビットは、通常の左シフトの結果なので、そのまま

        Return

    End Sub

    '最初から作り直す。32ビットだけの、ビット1の連続のビットマスク(左シフトつき)があるので、
    'それを利用して、64ビット(32ビット上下連結)の、ビット1の連続のビットマスク(左シフトつき)を作れば良い
    ''64ビット(32ビット上下連結)の、符号つき右シフト.checked2024_03_01

    Public Shared Sub RightShiftUintegerDoubleWithSignBit(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aRightShift As UInteger, ByRef aRefRightShiftHigh As UInteger, ByRef aRefRightShiftLow As UInteger)

        aRefRightShiftHigh = 0
        aRefRightShiftLow = 0

        '最上位ビットを記憶
        Dim memory_only_sign_bit As UInteger = ANDAsUinteger(aUintegerHigh, &H80000000L)

        If aRightShift >= UINTEGER_DOUBLE_BIT_LENGTH - 1 Then
            '最上位ビットに合わせて、全てビット1または全てビット0
            Dim all_bit_one_or_zero As UInteger = IIf(memory_only_sign_bit = &H80000000L, &HFFFFFFFFL, 0)
            aRefRightShiftHigh = all_bit_one_or_zero
            aRefRightShiftLow = all_bit_one_or_zero
            Return
        End If

        'ここに来る⇔aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-2
        If aRightShift = 0 Then
            '値渡しの引数をそのまま入れて終了
            aRefRightShiftHigh = aUintegerHigh
            aRefRightShiftLow = aUintegerLow
            Return 'checked
        End If

        'ここに来る⇔1<=aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-2

        '通常の右シフトをする
        Dim standard_right_shift_high As UInteger = 0
        Dim standard_right_shift_low As UInteger = 0
        RightShiftUintegerDouble(aUintegerHigh, aUintegerLow, aRightShift, standard_right_shift_high, standard_right_shift_low)



        If memory_only_sign_bit = 0 Then
            '通常の右シフトの結果を、参照渡しの変数に入れる
            aRefRightShiftHigh = standard_right_shift_high
            aRefRightShiftLow = standard_right_shift_low
            Return 'checked
        End If

        'ここに来る⇔{1<=aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-1}
        '∴通常の右シフトの結果に対して、左端に、aRightShift個のビット1を連続して入れれば良い

        '左シフトしたビットマスクを作る
        Dim bit_mask_left_shift_high As UInteger = 0
        Dim bit_mask_left_shift_low As UInteger = 0
        SetContinuousBitOneBitMaskUintegerDoubleWithLeftShift(aRightShift, UINTEGER_DOUBLE_BIT_LENGTH - aRightShift, bit_mask_left_shift_high, bit_mask_left_shift_low)

        '通常の右シフトの結果と、左シフトしたビットマスクとのORを、最終的な答えとする
        aRefRightShiftHigh = ORAsUinteger(bit_mask_left_shift_high, standard_right_shift_high)
        aRefRightShiftLow = ORAsUinteger(bit_mask_left_shift_low, standard_right_shift_low)

        Return
    End Sub

    '連続したビット1のビットマスクを作る。checked
    Public Shared Sub SetContinuousBitOneBitMaskUintegerDouble(ByVal aContinousBitOneCount As Byte, ByRef aRefBitMaskHigh As UInteger, ByRef aRefBitMaskLow As UInteger)

        aRefBitMaskHigh = 0
        aRefBitMaskLow = 0
        If aContinousBitOneCount = 0 Then
            Return 'checked
        End If

        'ここに来る⇔1<=aContinousBitOneCount

        If aContinousBitOneCount >= UINTEGER_DOUBLE_BIT_LENGTH Then
            aRefBitMaskHigh = &HFFFFFFFFL
            aRefBitMaskLow = &HFFFFFFFFL
            Return 'checked
        End If

        'ここに来る⇔1<=aContinousBitOneCount<=UINTEGER_DOUBLE_BIT_LENGTH-1

        If aContinousBitOneCount <= UINTEGER_BIT_LENGTH Then
            '下の32ビット限定で作れば良い
            aRefBitMaskLow = MakeBitMaskContinuousBitOne(aContinousBitOneCount)

            Return 'chekced

        End If

        'ここに来る⇔UINTEGER_BIT_LENGTH+1<=aContinousBitOneCount<=UINTEGER_DOUBLE_BIT_LENGTH-1

        '∴下の値は&HFFFFFFFFLと決まっている
        aRefBitMaskLow = &HFFFFFFFFL

        '上については、aContinousBitOneCount-UINTEGER_BIT_LENGTHの値で、上だけで作れば良い
        aRefBitMaskHigh = MakeBitMaskContinuousBitOne(aContinousBitOneCount - UINTEGER_BIT_LENGTH)


        Return
    End Sub

    '連続したビット1のビットマスクに対して、左シフトを行う.checked2024_03_01
    Public Shared Sub SetContinuousBitOneBitMaskUintegerDoubleWithLeftShift(ByVal aContinousBitOneCount As Byte, ByVal aLeftShiftNum As Byte, ByRef aRefBitMaskHigh As UInteger, ByRef aRefBitMaskLow As UInteger)

        aRefBitMaskHigh = 0
        aRefBitMaskLow = 0

        If Not ValueComparater.ExistOneDimSubArea(UINTEGER_DOUBLE_BIT_LENGTH, aLeftShiftNum, aContinousBitOneCount) Then

            Return 'checked

        End If

        'ここに来る⇔ビットマスクに対して、左シフトを実行しても、大丈夫
        '∴ビットマスクを一旦作り、そのビットマスクに対して、左シフトを実行する

        Dim ref_bitmask_high As UInteger = 0
        Dim ref_bitmask_low As UInteger = 0
        SetContinuousBitOneBitMaskUintegerDouble(aContinousBitOneCount, ref_bitmask_high, ref_bitmask_low)

        '左シフトを行う。結果はそのまま参照渡しの変数
        LeftShiftUintegerDouble(ref_bitmask_high, ref_bitmask_low, aLeftShiftNum, aRefBitMaskHigh, aRefBitMaskLow)

        Return

    End Sub


    '64ビット(上下32ビットの連結)の左回転シフト.checked2024_03_01
    Public Shared Sub LeftRotateShiftAsUintegerDouble(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aLeftShiftNum As Byte, ByRef aRefRotateHigh As UInteger, ByRef aRefRotateLow As UInteger)

        '回転シフトなので、mod64を取る
        Dim left_shift_num_mod_sixty_four As Byte = (aLeftShiftNum Mod UINTEGER_DOUBLE_BIT_LENGTH)

        If left_shift_num_mod_sixty_four = 0 Then
            aRefRotateHigh = aUintegerHigh
            aRefRotateLow = aUintegerLow
            Return 'checked
        End If

        'ここにある⇔1<=left_shift_num_mod_sixty_four<=63
        '∴回転左シフトができる

        '元の値の左(left_shift_num_mod_sixty_four)シフト
        Dim ref_left_shift_high As UInteger = 0
        Dim ref_left_shift_low As UInteger = 0
        LeftShiftUintegerDouble(aUintegerHigh, aUintegerLow, left_shift_num_mod_sixty_four, ref_left_shift_high, ref_left_shift_low)

        '元の値の右(64-left_shift_num_mod_sixty_four)シフトを得る
        Dim ref_right_shift_high As UInteger = 0
        Dim ref_right_shift_low As UInteger = 0
        RightShiftUintegerDouble(aUintegerHigh, aUintegerLow, UINTEGER_DOUBLE_BIT_LENGTH - left_shift_num_mod_sixty_four, ref_right_shift_high, ref_right_shift_low)

        '左シフトの結果と、右シフトの結果について、64ビットのORをする
        '64ビットのORは、32ビットづつ、上下別々にOR
        aRefRotateHigh = ORAsUinteger(ref_left_shift_high, ref_right_shift_high) '上32ビット
        aRefRotateLow = ORAsUinteger(ref_left_shift_low, ref_right_shift_low) '下32ビット

        Return

    End Sub

    'uinteger型の連結でビットを得る.checked2023_10_10
    Public Shared Function GetBitUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aUintegerDoubleBitPos As UInteger) As UInteger

        If aUintegerDoubleBitPos >= UINTEGER_DOUBLE_BIT_LENGTH Then

            Return 0 'checked
        End If

        'ここに来る⇔aUintegerDoubleBitPos <= UINTEGER_DOUBLE_BIT_LENGTH-1

        '∴上の値もしくは下の値からビットを取れる

        If aUintegerDoubleBitPos >= UINTEGER_BIT_LENGTH Then 'checked
            'ここに来る⇔UINTEGER_BIT_LENGTH<=aUintegerDoubleBitPos <= UINTEGER_DOUBLE_BIT_LENGTH-1
            '∴上の値からビットを取る

            '上の値に対するビット位置
            Dim high_uinteger_bitpos As UInteger = aUintegerDoubleBitPos - UINTEGER_BIT_LENGTH

            '上の値から取り出したビット
            Dim high_uinteger_bitpos_bit As UInteger = GetBitInUintegerAtBitPos(aHighUinteger, high_uinteger_bitpos)

            Return high_uinteger_bitpos_bit

        End If

        'ここに来る⇔aUintegerDoubleBitPos<=UINTEGER_BIT_LENGTH-1

        '∴下の値からそのまま取り出す

        Dim low_uinteger_bitpos_bit As UInteger = GetBitInUintegerAtBitPos(aLowUinteger, aUintegerDoubleBitPos)
        Return low_uinteger_bitpos_bit



    End Function

    'uinteger型の連結でのビットのセット.checked2023_10_10

    Public Shared Sub SetBitUintegerDouble(ByRef aRefHighUinteger As UInteger, ByRef aRefLowUinteger As UInteger, ByVal aUintegerDoubleBitPos As UInteger, ByVal aBitFlag As Boolean)

        If aUintegerDoubleBitPos >= UINTEGER_DOUBLE_BIT_LENGTH Then

            Return 'checked
        End If

        'ここに来る⇔aUintegerDoubleBitPos<=UINTEGER_DOUBLE_BIT_LENGTH-1


        If aUintegerDoubleBitPos >= UINTEGER_BIT_LENGTH Then
            'ここに来る⇔UINTEGER_BIT_LENGTH<=aUintegerDoubleBitPos<=UINTEGER_DOUBLE_BIT_LENGTH-1
            '∴上の値の aUintegerDoubleBitPos - UINTEGER_BIT_LENGTHビットをセット
            SetBitAsUinteger(aRefHighUinteger, aUintegerDoubleBitPos - UINTEGER_BIT_LENGTH, aBitFlag)

            Return

        End If

        'ここに来る⇔aUintegerDoubleBitPos<=UINTEGER_BIT_LENGTH-1
        '∴下の値のaUintegerDoubleBitPosビットをセット

        SetBitAsUinteger(aRefLowUinteger, aUintegerDoubleBitPos, aBitFlag)


        Return
    End Sub

    '32ビットを上下連結にして、最も高いビット1の位置を得る.checked2023_10_31
    Public Shared Function GetHighestBitOnePosInDoubleUinteger(ByVal aHighUintegerValue As UInteger, ByVal aLowUintegerValue As UInteger) As UInteger

        If (aHighUintegerValue = 0) And (aLowUintegerValue = 0) Then

            Return ERROR_BITPOS 'checked
        End If


        'ここに来る⇔上下のどちらかは非ゼロ
        If aHighUintegerValue <> 0 Then 'checked
            '上が非ゼロの場合は、上の32ビット内で最も高いビット1を探し、そのビット位置にUINTEGER_BIT_LENGTHを足す

            Dim highest_bit_one_pos_high As UInteger = GetHighestBitOnePos(aHighUintegerValue) '上の32ビット内で最も高いビットのビット位置
            Dim highest_bit_one_pos_double As UInteger = highest_bit_one_pos_high + UINTEGER_BIT_LENGTH '上下32ビットでのビット位置
            Return highest_bit_one_pos_double
        End If

        'ここに来る⇔{上下のどちらかは非ゼロ}かつ{上はゼロ}⇔{上はゼロ}かつ{下は非ゼロ}
        '∴下だけ調べて、その値を答えにすれば良い

        Dim low_highest_bitone_pos As UInteger = GetHighestBitOnePos(aLowUintegerValue)

        Return low_highest_bitone_pos

    End Function

    '8バイトをビットの列に変換.checked2023_12_18
    Public Shared Function ConvertEightByteIntoBitByteArray(ByVal aHighUintegerValue As UInteger, ByVal aLowUintegerValue As UInteger) As Byte()

        '上4バイトのビットの配列
        Dim high_uinteger_value_bit_byte_array() As Byte = ConvertUintegerValueIntoBitArray(aHighUintegerValue)


        '下4バイトのビットの配列
        Dim low_uinteger_value_bit_byte_array() As Byte = ConvertUintegerValueIntoBitArray(aLowUintegerValue)

        '上4バイトのビットの配列を右側(インデックスが大きい方)
        '下4バイトのビットの配列を右左側(インデックスが小さい方)
        Dim eight_byte_bit_byte_array() As Byte = ByteArrayOperater.MakeConnectedArray(low_uinteger_value_bit_byte_array, high_uinteger_value_bit_byte_array)

        Return eight_byte_bit_byte_array

    End Function

    '8バイトの値に対して、サーチ対象のビットがヒットしたビット位置の配列を作る.
    Public Shared Function MakeHitBitPosArrayBySearchBitFlagInEightByte(ByVal aHighFourByte As UInteger, ByVal aLowFourByte As UInteger, ByVal aSearchBitFlag As Boolean) As UInteger()

        '上の4バイトについて、サーチ対象のビットがヒットしたビット位置の配列を作る
        Dim hit_bit_pos_array_high() As UInteger = MakeHitBitPosArrayBySearchBitFlagInFourByte(aHighFourByte, aSearchBitFlag)

        '上の4バイトは、本当は下32ビットと連結しているので、ビット位置の値に、一斉に32を足す
        Dim hit_bit_pos_array_high_plus_value() As UInteger = UintegerArrayOperater.MakeCommonValuePlusedArray(hit_bit_pos_array_high, UINTEGER_BIT_LENGTH)

        '下の4バイトについて、サーチ対象のビットがヒットしたビット位置の配列を作る
        Dim hit_bit_pos_array_low() As UInteger = MakeHitBitPosArrayBySearchBitFlagInFourByte(aLowFourByte, aSearchBitFlag)

        '連結した結果が、8バイト(上下4バイトの連結)についての結果
        Dim hit_bit_pos_array_eight_byte() As UInteger = UintegerArrayOperater.MakeConnectedArray(hit_bit_pos_array_low, hit_bit_pos_array_high_plus_value)

        Return hit_bit_pos_array_eight_byte

    End Function



End Class
