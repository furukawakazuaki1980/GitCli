Public Class BitOperaterForUinteger
    Inherits BitOperaterForUshort

    Public Const UINTEGER_BIT_LENGTH As UInteger = 2 * USHORT_BIT_LENGTH

    Public Const ERROR_BITPOS As UInteger = &HFFFFFFFFL





    'uinteger型の左シフト.checked2023_10_08
    Public Shared Function LeftShiftUinteger(ByVal aUintegerValue As UInteger, ByVal aLeftShiftNum As UInteger) As UInteger

        If aLeftShiftNum >= UINTEGER_BIT_LENGTH Then

            Return 0 'checked

        End If

        'ここに来る⇔aLeftShiftNum < UINTEGER_BIT_LENGTH 

        '∴左シフトができる

        Dim left_shift_uinteger_value As UInteger = aUintegerValue << aLeftShiftNum

        Return left_shift_uinteger_value


    End Function

    'uinteger型の左シフト.checked2023_10_08
    Public Shared Function RightShiftUinteger(ByVal aUintegerValue As UInteger, ByVal aRightShiftNum As UInteger) As UInteger

        If aRightShiftNum >= UINTEGER_BIT_LENGTH Then

            Return 0 'checked

        End If

        'ここに来る⇔aRightShift<UINTEGER_BIT_LENGTH

        '∴左ビットシフトができる

        Dim right_shift_value As UInteger = aUintegerValue >> aRightShiftNum

        Return right_shift_value

    End Function

    '32ビットのAND
    Public Shared Function ANDAsUinteger(ByVal aUintegerValue1 As UInteger, ByVal aUintegerValue2 As UInteger) As UInteger

        Dim and_uinteger As UInteger = aUintegerValue1 And aUintegerValue2

        Return and_uinteger

    End Function

    '32ビットOR
    Public Shared Function ORAsUinteger(ByVal aUintegerValue1 As UInteger, ByVal aUintegerValue2 As UInteger) As UInteger

        Dim or_uinteger As UInteger = aUintegerValue1 Or aUintegerValue2

        Return or_uinteger
    End Function

    '32ビットEXOR

    Public Shared Function EXORAsUinteger(ByVal aUintegerValue1 As UInteger, ByVal aUintegerValue2 As UInteger) As UInteger

        Dim exor_uinteger As UInteger = aUintegerValue1 Xor aUintegerValue2

        Return exor_uinteger

    End Function

    '32ビットNOT

    Public Shared Function NOTAsUinteger(ByVal aUintegerValue1 As UInteger) As UInteger

        Dim not_uinteger As UInteger = Not (aUintegerValue1)
        Return not_uinteger



    End Function

    '符号ありの32ビット左シフト.checked2024_01_26
    Public Shared Function LeftShiftWithSignBitAsUinteger(ByVal aUintegerValue As UInteger, ByVal aLeftShiftNum As Byte) As UInteger

        If aLeftShiftNum = 0 Then
            Return aUintegerValue 'checked
        End If

        'ここに来る⇔aLeftShiftNum>=1

        '最上位ビットだけの値を得る

        Dim only_sign_bit_value As UInteger = ANDAsUinteger(aUintegerValue, &H80000000L)

        If aLeftShiftNum >= UINTEGER_BIT_LENGTH - 1 Then
            '最上位ビットだけの値を返す
            Return only_sign_bit_value 'checked
        End If

        'ここに来る⇔1<=aLeftShiftNum<=UINTEGER_BIT_LENGTH - 2
        '∴符号つきの左シフトの処理ができる

        '通常の左シフトを行う
        Dim standard_left_shift As UInteger = LeftShiftUinteger(aUintegerValue, aLeftShiftNum)

        '通常の左シフトの結果の、最上位ビットを0にする
        Dim standard_left_shift_without_sign_bit As UInteger = ANDAsUinteger(standard_left_shift, &H7FFFFFFFL)

        If only_sign_bit_value = 0 Then '元々のaUintegerValueの最上位ビットが0なら
            '通常の左シフトの結果から、最上位ビットを0にした結果をそのまま返す
            Return standard_left_shift_without_sign_bit 'checked
        End If

        'ここに来る⇔{1<=aLeftShiftNum<=UINTEGER_BIT_LENGTH - 2}かつ{aUintegerValueの最上位ビットは1}

        Dim left_shift_with_sign_bit As UInteger = ORAsUinteger(standard_left_shift_without_sign_bit, &H80000000L)

        Return left_shift_with_sign_bit

    End Function


    '符号付きの右シフト.checked2024_01_26
    Public Shared Function RightShiftWithSignBitAsUinteger(ByVal aUintegerValue As UInteger, ByVal aRightShiftNum As Byte) As UInteger
        If aRightShiftNum = 0 Then

            Return aUintegerValue 'checked
        End If

        'ここに来る⇔aRightShiftNum>=1

        Dim only_sign_bit As UInteger = ANDAsUinteger(aUintegerValue, &H80000000L)



        If aRightShiftNum >= UINTEGER_BIT_LENGTH - 1 Then

            '最上位ビットに合わせた値にする
            Dim all_bits_one_or_zero As UInteger = If(only_sign_bit = &H80000000L, &HFFFFFFFFL, &H0)

            Return all_bits_one_or_zero 'checked

        End If

        'ここに来る⇔UINTEGER_BIT_LENGTH - 1>=aRightShiftNum>=1

        '∴符号つき右シフトの処理ができる

        '通常の右シフト
        Dim standard_right_shift_value As UInteger = RightShiftUinteger(aUintegerValue, aRightShiftNum)

        If only_sign_bit = 0 Then '最上位ビットが0

            '通常の右シフトの値をそのままreturn
            Return standard_right_shift_value 'checked

        End If

        'ここに来る⇔{UINTEGER_BIT_LENGTH - 1>=aRightShiftNum>=1}かつ{引数の最上位ビットは1}

        '通常の右シフトの値とORするための値を作る。高いビット位置のビットが全て1
        Dim high_bits_all_one As UInteger = LeftShiftUinteger(&HFFFFFFFFL, UINTEGER_BIT_LENGTH - aRightShiftNum)

        Dim right_shift_with_sign_bit As UInteger = ORAsUinteger(high_bits_all_one, standard_right_shift_value)

        Return right_shift_with_sign_bit



    End Function

    '回転左シフト.checked2024_03_01
    Public Shared Function LeftRotateShiftAsUinteger(ByVal aUintegerValue As UInteger, ByVal aLeftShiftNum As Byte) As UInteger

        Dim left_shift_num_mod_thirty_two As Byte = (aLeftShiftNum Mod UINTEGER_BIT_LENGTH)

        If left_shift_num_mod_thirty_two = 0 Then
            Return aUintegerValue 'checked
        End If

        'ここに来る⇔1<=left_shift_num_mod_thirty_two<=31
        '∴回転シフトの処理をする意味がある

        '元の値の、<<left_shift_num_mod_thirty_twoを得る
        Dim left_shift_value As UInteger = LeftShiftUinteger(aUintegerValue, left_shift_num_mod_thirty_two)


        '元の値の、>>(32-left_shift_num_mod_thirty_two)を得る
        Dim right_shift_value As UInteger = RightShiftUinteger(aUintegerValue, UINTEGER_BIT_LENGTH - left_shift_num_mod_thirty_two)

        '左シフトと右シフトの結果をORして、左回転シフトを得る
        Dim left_rotate_value As UInteger = ORAsUinteger(left_shift_value, right_shift_value)

        Return left_rotate_value

    End Function

    'Uintegerからブールの変換
    Public Shared Function IsNonZeroUintegerValue(ByVal aUintegerValue As UInteger) As Boolean

        Return (aUintegerValue <> 0)
    End Function

    'ブールからUintegerへの変換

    Public Shared Function ConvertBooleanIntoUinteger(ByVal aFlag As Boolean) As UInteger

        Return If(aFlag, 1, 0)

    End Function

    'Uinteger型の中の、指定したビット位置の、ビットを得る.checked2023_10_08
    Public Shared Function GetBitInUintegerAtBitPos(ByVal aUintegerValue As UInteger, ByVal aUintegerBitPos As UInteger) As UInteger

        If aUintegerBitPos >= UINTEGER_BIT_LENGTH Then

            Return 0 'checked
        End If

        'ここに来る⇔aUintegerBitPos <= UINTEGER_BIT_LENGTH-1

        '∴aUintegerBitPosによりビットの取得ができる

        '第aUintegerBitPosビットだけ1のビットマスク
        Dim bitmask_bitpos As UInteger = LeftShiftUinteger(1, aUintegerBitPos)

        '引数の値の、第aUintegerBitPosBit以外全て0にする
        Dim only_bitpos_bit_uinteger As UInteger = ANDAsUinteger(aUintegerValue, bitmask_bitpos)

        '第aUintegerBitPosのビットを、第0ビットに持ってくる
        Dim bit_in_uinteger_at_bitpos As UInteger = RightShiftUinteger(only_bitpos_bit_uinteger, aUintegerBitPos)

        Return bit_in_uinteger_at_bitpos

    End Function

    'uinteger型の引数に対して、符号ビットのフラグと絶対値を得る.checked2023_12_01
    Public Shared Sub SetMinusFlagAndAbsoluteUintegerValue(ByVal aUintegerValue As UInteger, ByRef aRefMinusFlag As Boolean, ByRef aRefAbsoluteValue As UInteger)

        '引数の最上位ビットを得る
        Dim sign_bit As UInteger = GetBitInUintegerAtBitPos(aUintegerValue, UINTEGER_BIT_LENGTH - 1)

        '最上位ビットが1かどうか、参照渡しのフラグに記憶
        aRefMinusFlag = (sign_bit = 1)

        If Not aRefMinusFlag Then '最上位ビットが0の時
            '絶対値は、引数の値自身
            aRefAbsoluteValue = aUintegerValue
            Return 'checked
        End If

        'ここに来る⇔aRefMinusFlagがtrue⇔最上位ビットが1
        '∴絶対値を得るための処理が必要


        '引数の値の反転(=0xFFFFFFFF-引数の値)
        Dim not_uinteger_value As UInteger = NOTAsUinteger(aUintegerValue)

        '絶対値=引数の値の反転+1=(0xFFFFFFFF-引数の値)+1=0x100000000-引数の値
        aRefAbsoluteValue = not_uinteger_value + 1

        Return

    End Sub

    'Ushort型の変数に対して、指定したビット位置のビットを1又は0にする.checked2023_10_09
    Public Shared Sub SetBitAsUinteger(ByRef aRefUinteger As UInteger, ByVal aUintegerBitPos As UInteger, ByVal aBitFlag As Boolean)

        If aUintegerBitPos >= UINTEGER_BIT_LENGTH Then
            Return 'checked
        End If

        'ここに来る⇔aUintegerBitPos <= UINTEGER_BIT_LENGTH-1

        '∴ビット位置は正常

        '第aUintegerBitPosだけ1の、ビットマスクを作る

        Dim bitmask_bitpos_uinteger As UInteger = LeftShiftUinteger(1, aUintegerBitPos)

        If aBitFlag Then 'checked'ビットを1にする場合


            aRefUinteger = ORAsUinteger(aRefUinteger, bitmask_bitpos_uinteger)

            Return

        End If

        'ここに来るならaBitFlagはfalseなので、ビットを0にする

        '第aByteBitPosだけ0で、それ以外は1の値
        Dim not_bitmask_bitpos_uinteger As UInteger = NOTAsUinteger(bitmask_bitpos_uinteger)


        aRefUinteger = ANDAsUinteger(aRefUinteger, not_bitmask_bitpos_uinteger)

        Return
    End Sub

    '最も高いビット1の位置を得る関数.checked2023_10_31
    Public Shared Function GetHighestBitOnePos(ByVal aUintegerValue As UInteger) As UInteger

        If aUintegerValue = 0 Then

            Return ERROR_BITPOS 'checked

        End If

        'ここに来る⇔引数は非ゼロ⇔どこかにビット1がある
        '∴最も高いビット1の場所をサーチできる

        'bitposをデクリメントさせてforループ
        Dim highest_bitpos As UInteger = 0 'returnするのでここで宣言
        For bitpos As Integer = UINTEGER_BIT_LENGTH - 1 To 0 Step -1

            '引数のaUintegerValueは非ゼロなので、必ずif文でヒットする
            If GetBitInUintegerAtBitPos(aUintegerValue, bitpos) = 1 Then
                highest_bitpos = bitpos
                Exit For 'checked
            End If

        Next


        'ここに来る時、bitposは、if文にヒットしてforループから抜け出した時の値なので、
        '最も高いビット1のビット位置になっている

        Return highest_bitpos

    End Function

    'uinteger型に対するビットリバース
    Public Shared Function MakeReversedUintegerValue(ByVal aUintegerValue As UInteger) As UInteger

        '引数の4バイトを、上下2バイトに分割
        Dim ref_high_ushort As UShort = 0
        Dim ref_low_ushort As UShort = 0
        DivideUintegerIntoTwoUshorts(aUintegerValue, ref_high_ushort, ref_low_ushort)

        '上2バイトを、リバース
        Dim reversed_high_ushort As UShort = MakeReversedUshortValue(ref_high_ushort)

        '下2バイトをリバース
        Dim reversed_low_ushort As UShort = MakeReversedUshortValue(ref_low_ushort)

        '元の上2バイトをリバースした結果を下2バイトにする
        '元の下2バイトをリバースした結果を上2バイトにする
        Dim reversed_uinteger_value As UInteger = ConnectTwoUshorts(reversed_low_ushort, reversed_high_ushort)


        Return reversed_uinteger_value

    End Function
    '4バイトの値を上下2バイトに分離.checked2023_10_09

    Public Shared Sub DivideUintegerIntoTwoUshorts(ByVal aUintegerValue As UInteger, ByRef aRefHighUshort As UShort, ByRef aRefLowUshort As UShort)

        '4バイトの値を16ビット右にシフトした値が上2バイト
        Dim right_shift_uinteger As UInteger = RightShiftUinteger(aUintegerValue, USHORT_BIT_LENGTH)
        aRefHighUshort = CType(right_shift_uinteger, UShort)

        '4バイトの値と0x0000FFFFとのANDを取った値が下2バイト
        Dim uinteger_low_twobyte As UInteger = ANDAsUinteger(aUintegerValue, &HFFFF)
        aRefLowUshort = CType(uinteger_low_twobyte, UShort)


        Return
    End Sub

    '4バイトの値を1バイト四個に分離.checked2023_10_09
    Public Shared Sub DivideUintegerIntoFourBytes(ByVal aUintegerValue As UInteger, ByRef aRefByte3 As Byte, ByRef aRefByte2 As Byte, ByRef aRefByte1 As Byte, ByRef aRefByte0 As Byte)

        '4バイトの値をまず上下2バイトに分離
        Dim ref_high_ushort As UShort = 0
        Dim ref_low_ushort As UShort = 0
        DivideUintegerIntoTwoUshorts(aUintegerValue, ref_high_ushort, ref_low_ushort)

        '上2バイトからaRefByte3とaRefByte2を得る
        DivideUshortIntoTwoBytes(ref_high_ushort, aRefByte3, aRefByte2)

        '下2バイトからaRefByte1とaRefByte0を得る
        DivideUshortIntoTwoBytes(ref_low_ushort, aRefByte1, aRefByte0)

        Return

    End Sub

    '上下2バイトを結合.checked2023_10_09
    Public Shared Function ConnectTwoUshorts(ByVal aHighUshort As UShort, ByVal aLowUshort As UShort) As UInteger

        '上2バイトを、16ビット左にシフト
        Dim right_shift_high As UInteger = LeftShiftUinteger(CType(aHighUshort, UInteger), USHORT_BIT_LENGTH)

        '下2バイトを、そのままuinteger型にキャスト
        Dim low_ushort_cast_uinteger As UInteger = CType(aLowUshort, UInteger)

        '上下2バイトを結合
        Dim connected_uinteger As UInteger = ORAsUinteger(right_shift_high, low_ushort_cast_uinteger)

        Return connected_uinteger

    End Function

    '四個のバイトを結合.checked2023_10_09
    Public Shared Function ConnectFourBytes(ByVal aByte3 As Byte, ByVal aByte2 As Byte, ByVal aByte1 As Byte, ByVal aByte0 As Byte) As UInteger

        'aByte1を上、aByte0を下にして結合
        Dim ushort_1_0 As UShort = ConnectTwoBytes(aByte1, aByte0)

        'aByte3を上、aByte2を下にして結合
        Dim ushort_3_2 As UShort = ConnectTwoBytes(aByte3, aByte2)

        'ushort_3_2を上2バイト、ushort_1_0を下2バイトにして結合
        Dim connected_four_bytes As UInteger = ConnectTwoUshorts(ushort_3_2, ushort_1_0)

        Return connected_four_bytes

    End Function

    'uinteger型の値の中の、bit1の個数を得る.checked2023_12_01
    Public Shared Function GetBitOneCountAsUinteger(ByVal aUintegerValue As UInteger) As UInteger

        '上下2バイトの値に分離
        Dim ref_high_ushort As UShort = 0
        Dim ref_low_ushort As UShort = 0
        DivideUintegerIntoTwoUshorts(aUintegerValue, ref_high_ushort, ref_low_ushort)

        '上2バイトのbit1の個数を得る
        Dim bit_one_count_high_ushort As UInteger = GetBitOneCountAsUshort(ref_high_ushort)

        '下2バイトのbit1の個数を得る
        Dim bit_one_count_low_ushort As UInteger = GetBitOneCountAsUshort(ref_low_ushort)

        '上下2バイトのbit1個数を足す
        Dim bit_one_count_as_uinteger As UInteger = bit_one_count_high_ushort + bit_one_count_low_ushort

        Return bit_one_count_as_uinteger

    End Function

    'uinteger型(32ビット)の値を、ビットの配列に変換.checked2023_12_04
    Public Shared Function ConvertUintegerValueIntoBitArray(ByVal aUintegerValue As UInteger) As Byte()

        '4バイトの引数を、上下2バイトに分離
        Dim ref_high_ushort As UShort = 0
        Dim ref_low_ushort As UShort = 0
        DivideUintegerIntoTwoUshorts(aUintegerValue, ref_high_ushort, ref_low_ushort)

        '下2バイトの値を、ビットの配列に変換
        Dim low_ushort_bit_array() As Byte = ConvertUshortIntoBitArray(ref_low_ushort)

        '上2バイトの値を、ビットの配列に変換
        Dim high_ushort_bit_array() As Byte = ConvertUshortIntoBitArray(ref_high_ushort)

        '上下のビットの配列を結合(下のビットの配列の方がインデックスが小さい)
        Dim uinteger_bit_array() As Byte = ByteArrayOperater.MakeConnectedArray(low_ushort_bit_array, high_ushort_bit_array)

        Return uinteger_bit_array

    End Function

    '4バイトの値に対して、サーチ対象のビットがヒットしたビット位置の配列を作る.checked2024_01_05
    Public Shared Function MakeHitBitPosArrayBySearchBitFlagInFourByte(ByVal aFourByteValue As UInteger, ByVal aSearchBitFlag As Boolean) As UInteger()
        '4バイトの値を、上下2バイトに分割
        Dim ref_high_twobyte As UShort = 0
        Dim ref_low_twobyte As UShort = 0
        DivideUintegerIntoTwoUshorts(aFourByteValue, ref_high_twobyte, ref_low_twobyte)

        '上2バイトについて、サーチのビットがヒットしたビット位置の配列を得る
        Dim high_twobyte_hit_bitpos_array() As UInteger = MakeHitBitposArrayBySearchBitFlagInTwoByte(ref_high_twobyte, aSearchBitFlag)


        '上2バイトについて、サーチのビットがヒットしたビット位置の配列の、全ての要素に対して、USHORT_BIT_LENGTHを足す
        Dim high_twobyte_hit_bitpos_array_plus_two_byte_bit_length() As UInteger = UintegerArrayOperater.MakeCommonValuePlusedArray(high_twobyte_hit_bitpos_array, USHORT_BIT_LENGTH)

        '下2バイトについて、サーチのビットがヒットしたビット位置の配列を得る
        Dim low_twobyte_hit_bitpos_array() As UInteger = MakeHitBitposArrayBySearchBitFlagInTwoByte(ref_low_twobyte, aSearchBitFlag)

        'ビット位置の配列を連結
        Dim hit_bitpos_array_four_byte() As UInteger = UintegerArrayOperater.MakeConnectedArray(low_twobyte_hit_bitpos_array, high_twobyte_hit_bitpos_array_plus_two_byte_bit_length)

        Return hit_bitpos_array_four_byte
    End Function


    'Uinteger型の引数に対して、最小限の長さの、ビットの値の列を作る。やがて文字列にする都合から、ushort型の配列にする
    'Private.checked2024_02_16
    Public Shared Function MakeSmallestLengthBitUshortArrayFromUintegerValue(ByVal aUintegerValue As UInteger) As UShort()

        If aUintegerValue = 0 Then
            Dim only_zero_ushort_array() As UShort = {0}
            Return only_zero_ushort_array 'checked
        End If

        'ここに来る⇔aUintegerValue>=1⇔aUintegerValueの、最も高い位置のビット1が存在
        '⇔GetHighestBitOnePos関数を正常に実行できる

        '∴GetHighestBitOnePos関数により、最も高いビット1の場所を求める
        Dim highest_bitone_pos As Byte = GetHighestBitOnePos(aUintegerValue)


        'サイズhighest_bitone_pos+1の、配列をローカルで宣言(やがてreturnする)
        Dim bit_ushort_array_smallest_length(highest_bitone_pos) As UShort

        'bit_ushort_array_smallest_length(highest_bitone_pos)は必ず1
        bit_ushort_array_smallest_length(highest_bitone_pos) = 1


        If highest_bitone_pos = 0 Then 'checked '⇔元々のaUintegerValue==1

            'このまま bit_ushort_array_smallest_lengthをreturnして終われば良い
            Return bit_ushort_array_smallest_length

        End If


        'bit_ushort_arrayの、中身を作る
        'すでにbit_ushort_array_smallest_length(highest_bitone_pos)は1にしているので、
        'bit_ushort_array_smallest_length(0)～bit_ushort_array_smallest_length(highest_bitone_pos - 1)
        'をこれから作る
        For bitpos As UInteger = 0 To highest_bitone_pos - 1

            If GetBitInUintegerAtBitPos(aUintegerValue, bitpos) = 1 Then 'aUintegerの、第bitposビットが1

                bit_ushort_array_smallest_length(bitpos) = 1

            End If
        Next

        Return bit_ushort_array_smallest_length

    End Function

    'Uinteger型の値から、必要最小限の長さの、ビットの文字列を作る.checked2024_02_16
    Public Shared Function MakeBitStringBySmallestLengthFromUintegerValue(ByVal aUintegerValue As UInteger) As String

        '必要最小限の長さの、ビットの値の列を得る
        Dim bit_ushort_array_smallest_length() As UShort = MakeSmallestLengthBitUshortArrayFromUintegerValue(aUintegerValue)

        'ビットの値の列をリバースする(やがて文字列にする事に備える。文字列は、インデクス0が左に来るから)
        'リバースにより、高いビットの値が、インデックスが少ない方になる
        Dim reversed_bit_ushort_array_smallest_length() As UShort = GenericArrayOperater(Of UShort).MakeRversedArray(bit_ushort_array_smallest_length)

        'リバースした結果に、一斉に、0x0030(つまりZERO_ASCII)を足す
        '足した結果は、やがて文字列にする配列
        Dim ushort_array_string_change() As UShort = UshortArrayOperater.MakeCommonValuePlusArray(reversed_bit_ushort_array_smallest_length, ASCIIConst.ZERO_ASCII_USHORT)

        'ushort型の配列を文字列にする
        Dim bit_string_smallest_length As String = UshortArrayOperater.ConvertUshortArrayIntoOneString(ushort_array_string_change)

        Return bit_string_smallest_length

    End Function

    '32ビットのビットマスクを作る.ビット1が下の方に連続している.checked2024_03_01
    Public Shared Function MakeBitMaskContinuousBitOne(ByVal aCountinuousBitOneCount As Byte) As UInteger

        If aCountinuousBitOneCount = 0 Then

            Return 0 'checked
        End If

        If aCountinuousBitOneCount >= UINTEGER_BIT_LENGTH Then

            Return &HFFFFFFFFL 'checked

        End If

        'ここに来る⇔1<=aCountinuousBitOneCount<=UINTEGER_BIT_LENGTH-1=31
        '∴1をaCountinuousBitOneCountだけ左シフトして、1を引けば良い。(オーバーフローは起きない)

        Dim bitmask As UInteger = LeftRotateShiftAsUinteger(1, aCountinuousBitOneCount) - 1

        Return bitmask

    End Function

    'ビット1が連続しているビットマスクで、下の方のビットゼロのカウントも指定する.checked2024_03_01
    Public Shared Function MakeContinuousBitOneWithLeftShift(ByVal aContinousBitOneCount As UInteger, ByVal aLeftShiftNum As UInteger) As UInteger

        Dim count_shift_num_flag As Boolean = ValueComparater.ExistOneDimSubArea(UINTEGER_BIT_LENGTH, aLeftShiftNum, aContinousBitOneCount)

        If Not count_shift_num_flag Then

            Return 0 'checked
        End If

        'ここに来る⇔{aContinousBitOneCount + aLeftShiftNum <= UINTEGER_BIT_LENGTH}かつ{1<=aContinousBitOneCount}

        '∴aContinousBitOneCount個のビット1が続いたビットマスクを作り、<<aLeftShiftNumしてよい

        'ビットマスク作成
        Dim bitmask As UInteger = MakeBitMaskContinuousBitOne(aContinousBitOneCount)

        'ビットマスクを左シフト
        Dim bitmask_left_shift As UInteger = LeftShiftUinteger(bitmask, aLeftShiftNum)

        Return bitmask_left_shift

    End Function

End Class
