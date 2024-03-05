Public Class BitOperaterForUshort
    Inherits BitOperaterForByte

    Public Const USHORT_BIT_LENGTH As UInteger = 2 * BYTE_BIT_LENGTH

    'ushort型の左シフト
    Public Shared Function LeftShiftUshort(ByVal aUshortValue As UShort, ByVal aLeftShiftNum As UInteger) As UShort

        If aLeftShiftNum >= USHORT_BIT_LENGTH Then

            Return 0 'checked

        End If

        'ここに来る⇔aLeftShift<USHORT_BIT_LENGTH

        '∴左ビットシフトができる

        Dim left_shift_value As UShort = aUshortValue << aLeftShiftNum

        Return left_shift_value

    End Function

    'ushort型の左シフト.checke2023_10_08
    Public Shared Function RightShiftUshort(ByVal aUshortValue As UShort, ByVal aRightShiftNum As UInteger) As UShort

        If aRightShiftNum >= USHORT_BIT_LENGTH Then

            Return 0 'checked

        End If

        'ここに来る⇔aRightShift<USHORT_BIT_LENGTH

        '∴左ビットシフトができる

        Dim right_shift_value As UShort = aUshortValue >> aRightShiftNum

        Return right_shift_value

    End Function

    Public Shared Function ANDAsUshort(ByVal aUshortValue1 As UShort, ByVal aUshortValue2 As UShort) As UShort

        Dim and_ushort As UShort = aUshortValue1 And aUshortValue2

        Return and_ushort
    End Function

    Public Shared Function ORAsUshort(ByVal aUshortValue1 As UShort, ByVal aUshortValue2 As UShort) As UShort

        Dim or_ushort As UShort = aUshortValue1 Or aUshortValue2

        Return or_ushort

    End Function

    Public Shared Function EXORAsUshort(ByVal aUshortValue1 As UShort, ByVal aUshortValue2 As UShort) As UShort

        Dim exor_ushort As UShort = aUshortValue1 Xor aUshortValue2

        Return exor_ushort

    End Function

    Public Shared Function NOTAsUshort(ByVal aUshortValue As UShort) As UShort

        Dim not_ushort As UShort = Not (aUshortValue)

        Return not_ushort


    End Function

    '16ビットに対する符号つき左シフト.checked2024_01_24
    Public Shared Function LeftShiftWithSignBitAsUshort(ByVal aUshortValue As UShort, ByVal aLeftShiftNum As Byte) As UShort

        If aLeftShiftNum = 0 Then

            Return aUshortValue 'checked2024_01_24
        End If

        'ここに来る⇔aLeftShiftNum>=1

        '引数aUshortValueの最上位ビットを取得
        Dim only_sign_bit_value As UShort = ANDAsUshort(aUshortValue, &H8000L)


        If aLeftShiftNum >= USHORT_BIT_LENGTH - 1 Then

            Return only_sign_bit_value 'checked

        End If

        'ここに来る⇔1<=aLeftShiftNum<=USHORT_BIT_LENGTH - 1

        '∴左シフトの処理ができる

        '通常の左シフト
        Dim standard_left_shift As UShort = LeftShiftUshort(aUshortValue, aLeftShiftNum)

        '通常の左シフトの値から、最上位ビットをクリア
        Dim standard_left_shift_without_sign_bit As UShort = ANDAsUshort(standard_left_shift, &H7FFFL)

        If only_sign_bit_value = 0 Then '元々のaUshortValueの最上位ビットが0だった

            Return standard_left_shift_without_sign_bit 'checked

        End If

        'ここに来る⇔{1<=aLeftShiftNum<=USHORT_BIT_LENGTH - 1}かつ{元々のaUshortValueの最上位が1}

        'standard_left_shift_without_sign_bitの最上位ビットを1にした値が、符号付き左シフトの結果
        Dim left_shift_with_sign_bit As UShort = ORAsUshort(standard_left_shift_without_sign_bit, &H8000L)

        Return left_shift_with_sign_bit


    End Function

    '16ビットに対する符号つき右シフト.checked2024_01_24
    Public Shared Function RightShiftWithSignBitAsUshort(ByVal aUshortValue As UShort, ByVal aRightShiftNum As Byte) As UShort
        If aRightShiftNum = 0 Then

            Return aUshortValue 'checked
        End If

        'ここに来る⇔aRightShiftNum>=1

        Dim only_sign_bit_value As UShort = ANDAsUshort(aUshortValue, &H8000L)


        If aRightShiftNum >= USHORT_BIT_LENGTH - 1 Then
            '16ビット全てが、最上位ビットに合わせた値
            Dim all_sign_bit_value As UShort = If(only_sign_bit_value = &H8000L, &HFFFFL, &H0)
            Return all_sign_bit_value 'checked
        End If

        'ここに来る⇔1<=aRightShiftNum<=USHORT_BIT_LENGTH

        '∴右シフト処理ができる

        '通常の方法でaRightShiftNumだけ右シフト
        Dim standard_right_shift As UShort = RightShiftUshort(aUshortValue, aRightShiftNum)


        '元々の最上位ビットが0の場合
        If only_sign_bit_value = 0 Then
            '通常の右シフトの値をそのまま返せばよい
            Return standard_right_shift 'checked
        End If

        'ここに来る⇔{1<=aRightShiftNum<=USHORT_BIT_LENGTH}かつ{aUshortValueの最上位ビットは1}


        '左aRightShiftNumビットが、全て1になっている値
        Dim high_bits_all_one As UShort = LeftShiftUshort(&HFFFFL, USHORT_BIT_LENGTH - aRightShiftNum)

        '符号付き右シフトの値は、high_bits_all_oneとstandard_right_shiftのOR
        Dim left_shift_with_sign_bit As UShort = ORAsUshort(high_bits_all_one, standard_right_shift)


        Return left_shift_with_sign_bit


    End Function

    '16ビットの回転左シフト.checked2024_03_01
    Public Shared Function LeftRotateShiftAsUshort(ByVal aUshortValue As UShort, ByVal aLeftShiftNum As Byte) As UShort

        '回転なので、mod16を取る
        Dim left_shift_mod_sixteen As Byte = (aLeftShiftNum Mod USHORT_BIT_LENGTH)

        If left_shift_mod_sixteen = 0 Then

            Return aUshortValue 'checked

        End If

        'ここに来る⇔1<=left_shift_mod_sixteen<=15
        '∴左シフトと右シフトのシフト幅が共に非ゼロ

        '通常の左シフト
        Dim left_shift_ushort As UShort = LeftShiftUshort(aUshortValue, left_shift_mod_sixteen)

        '通常の右シフト
        Dim right_shift_ushort As UShort = RightShiftUshort(aUshortValue, USHORT_BIT_LENGTH - left_shift_mod_sixteen)

        '二つのORが、回転左シフトの結果
        Dim left_rotate_shift_ushort As UShort = ORAsUshort(left_shift_ushort, right_shift_ushort)

        Return left_rotate_shift_ushort


    End Function


    'Ushortからブールの変換
    Public Shared Function IsNonZeroUshortValue(ByVal aUshortValue As UShort) As Boolean

        Return (aUshortValue <> 0)
    End Function

    'ブールからUshortへの変換

    Public Shared Function ConvertBooleanIntoUshort(ByVal aFlag As Boolean) As UShort

        Return If(aFlag, 1, 0)

    End Function

    'Ushort型の中の、ビット位置のビットを取得.checked2023_10_08
    Public Shared Function GetBitInUshortAtBitPos(ByVal aUshortValue As UShort, ByVal aUshortBitPos As UInteger) As UShort

        If aUshortBitPos >= USHORT_BIT_LENGTH Then

            Return 0 'checked
        End If

        'ここに来る⇔aUshortBitPos <= USHORT_BIT_LENGTH-1

        '∴aUshortBitPosによりビットの取得ができる

        '第aUshortBitPosビットだけ1のビットマスク
        Dim bitmask_bitpos As UShort = LeftShiftUshort(1, aUshortBitPos)

        '引数の値の、第aUshortBitPosBit以外全て0にする
        Dim only_bitpos_bit_ushort As UShort = ANDAsUshort(aUshortValue, bitmask_bitpos)

        '第aBitPosのビットを、第0ビットに持ってくる
        Dim bit_in_ushort_at_bitpos As UShort = RightShiftUshort(only_bitpos_bit_ushort, aUshortBitPos)

        Return bit_in_ushort_at_bitpos

    End Function

    'ushort型の値に対して、符号ビットのフラグと、絶対値を得る.checked2023_12_01
    Public Shared Sub SetMinusFlagAndAbsoluteUshortValue(ByVal aUshortValue As UShort, ByRef aRefMinusFlag As Boolean, ByRef aRefAbsoluteValue As UShort)

        'ushort型の最上位ビットを得る
        Dim sign_bit As Byte = GetBitInUshortAtBitPos(aUshortValue, USHORT_BIT_LENGTH - 1)

        'マイナスのフラグを決める
        aRefMinusFlag = (sign_bit = 1)

        If Not aRefMinusFlag Then
            aRefAbsoluteValue = aUshortValue
            Return
        End If

        'ここに来る⇔最上位ビットが1
        '∴負の数として扱う(2の補数の計算が必要)

        '引数の値をビット反転した値(=0xFFFF-引数の値)
        Dim not_ushort_value As UShort = NOTAsUshort(aUshortValue)

        '引数の値をビット反転した値+1=(0xFFFF-引数の値)+1=0x10000-引数の値
        aRefAbsoluteValue = not_ushort_value + 1

        Return

    End Sub

    'Ushort型の変数に対して、指定したビット位置のビットを1又は0にする.checked2023_10_09
    Public Shared Sub SetBitAsUshort(ByRef aRefUshort As UShort, ByVal aUshortBitPos As UShort, ByVal aBitFlag As Boolean)

        If aUshortBitPos >= USHORT_BIT_LENGTH Then
            Return 'checked
        End If

        'ここに来る⇔aUshortBitPos <= USHORT_BIT_LENGTH-1

        '∴ビット位置は正常

        '第aUshortBitPosだけ1の、ビットマスクを作る

        Dim bitmask_bitpos_ushort As UShort = LeftShiftUshort(1, aUshortBitPos)

        If aBitFlag Then 'checked'ビットを1にする場合

            '右辺の、ORAsUshortの実引数のaRefUshortは、aRefUshortの値を一旦コピーした物
            'aRefByteのコピー値とbitmask_bitposのORの値を、再びaRefUshortに入れるという意味
            aRefUshort = ORAsUshort(aRefUshort, bitmask_bitpos_ushort)

            Return

        End If

        'ここに来るならaBitFlagはfalseなので、ビットを0にする

        '第aByteBitPosだけ0で、それ以外は1の値
        Dim not_bitmask_bitpos_ushort As UShort = NOTAsUshort(bitmask_bitpos_ushort)

        '右辺の、ORAsUshortの実引数のaRefUshortは、aRefUhortの値を一旦コピーした物
        'aRefUshortのコピー値とnot_bitmask_bitpos_ushortのANDの値を、
        '再びaRefUshortに入れるという意味
        aRefUshort = ANDAsUshort(aRefUshort, not_bitmask_bitpos_ushort)

        Return
    End Sub

    '2バイトの値のビットリバース.checked2023_11_02
    Public Shared Function MakeReversedUshortValue(ByVal aUshortValue As UShort) As UShort

        '2バイトの値を上下に分割
        Dim ref_high_byte As Byte = 0
        Dim ref_low_byte As Byte = 0
        DivideUshortIntoTwoBytes(aUshortValue, ref_high_byte, ref_low_byte)

        '上のバイトをリバースする
        Dim reversed_high_byte As Byte = MakeReverseBitValue(ref_high_byte)

        '下のバイトをリバース
        Dim reversed_low_byte As Byte = MakeReverseBitValue(ref_low_byte)

        '元の上のバイトをリバースした結果を下のバイトとし、
        '元の下のバイトをリバースした結果を上のバイトとして結合
        Dim reversed_ushort_value As UShort = ConnectTwoBytes(reversed_low_byte, reversed_high_byte)

        Return reversed_ushort_value




    End Function
    'ここからは元々ByteOperaterにあったもの


    '2バイトの値を上下に分離.checked
    Public Shared Sub DivideUshortIntoTwoBytes(ByVal aUshortValue As UShort, ByRef aRefHighByte As Byte, ByRef aRefLowByte As Byte)

        '2バイトの引数を8ビット右にシフトした値が上のバイト
        Dim right_shift_ushort As UShort = RightShiftUshort(aUshortValue, BYTE_BIT_LENGTH)
        aRefHighByte = CType(right_shift_ushort, Byte)

        '2バイトの引数に対して、0x00FFとANDした値が下のバイト
        Dim only_low_byte_ushort As UShort = ANDAsUshort(aUshortValue, &HFF)
        aRefLowByte = CType(only_low_byte_ushort, Byte)


        Return
    End Sub


    'ここからは結合

    '上下のバイトを結合.checked2023_10_09
    Public Shared Function ConnectTwoBytes(ByVal aHighByte As Byte, ByVal aLowByte As Byte) As UShort

        '上のバイトを8ビット左にシフト
        Dim left_shift_high_byte As UShort = LeftShiftUshort(CType(aHighByte, UShort), BYTE_BIT_LENGTH)

        '下のバイトはそのままushort型にキャスト
        Dim low_byte_cast_ushort As UShort = CType(aLowByte, UShort)

        '二つの値のORを得る
        Dim high_low_ushort As UShort = ORAsUshort(left_shift_high_byte, low_byte_cast_ushort)

        Return high_low_ushort


    End Function

    'Ushort型の中の、bit1の個数を返す.checked2023_12_01
    Public Shared Function GetBitOneCountAsUshort(ByVal aUshortValue As UShort) As UInteger

        '引数の2バイトを上下のバイトに分離
        Dim ref_high_byte As Byte = 0
        Dim ref_low_byte As Byte = 0
        DivideUshortIntoTwoBytes(aUshortValue, ref_high_byte, ref_low_byte)

        '上のバイトのbit1の個数
        Dim bit_one_count_high_byte As UInteger = GetBitOneCountAsByte(ref_high_byte)

        '下のバイトのbit1の個数
        Dim bit_one_count_low_byte As UInteger = GetBitOneCountAsByte(ref_low_byte)

        '上下のバイトのbit1の個数
        Dim bit_one_count_as_ushort As UInteger = bit_one_count_high_byte + bit_one_count_low_byte

        Return bit_one_count_as_ushort

    End Function

    '2バイトの引数を、1と0の配列に変換.checked2023_12_04
    Public Shared Function ConvertUshortIntoBitArray(ByVal aUshortValue As UShort) As Byte()

        '引数のushort型の値を上下のバイトに分離
        Dim ref_high_byte As Byte = 0
        Dim ref_low_byte As Byte = 0
        DivideUshortIntoTwoBytes(aUshortValue, ref_high_byte, ref_low_byte)

        '上の1バイトをビットの配列に変換
        Dim high_bit_array() As Byte = ConvertByteIntoBitArray(ref_high_byte)

        '下の1バイトをビットの配列に変換
        Dim low_bit_array() As Byte = ConvertByteIntoBitArray(ref_low_byte)

        'ビットの配列を結合(下のビットの配列の方が、インデックスが小さい方)
        Dim ushort_bit_array() As Byte = ByteArrayOperater.MakeConnectedArray(low_bit_array, high_bit_array)


        Return ushort_bit_array

    End Function

    'サーチのフラグのビットがヒットしたビット位置の配列を返す。checked2024_01_05

    Public Shared Function MakeHitBitposArrayBySearchBitFlagInTwoByte(ByVal aTwoByteValue As UShort, ByVal aSearchBitFlag As Boolean) As UInteger()

        '引数の2バイトを上下に分離
        Dim ref_high_byte As Byte = 0 '上のバイト
        Dim ref_low_byte As Byte = 0 '下のバイト
        DivideUshortIntoTwoBytes(aTwoByteValue, ref_high_byte, ref_low_byte)


        '上のバイトだけの、ヒットしたビット位置の配列を作る
        Dim hit_bitpos_array_high_byte() As UInteger = MakeHitBitposArrayBySearchBitFlag(ref_high_byte, aSearchBitFlag)

        '上のバイトのヒットしたビット位置の配列の全ての要素に、 BYTE_BIT_LENGTHを足す
        Dim hit_bitpos_array_high_byte_plus_byte_bit_length() As UInteger = UintegerArrayOperater.MakeCommonValuePlusedArray(hit_bitpos_array_high_byte, BYTE_BIT_LENGTH)


        '下のバイトだけの、ヒットしたビット位置の配列を作る
        Dim hit_bitpos_array_low_byte() As UInteger = MakeHitBitposArrayBySearchBitFlag(ref_low_byte, aSearchBitFlag)

        '上下のバイトの、ヒットしたビット位置の配列を連結(下のバイトの方が、インデックスが小さい方になる)
        Dim hit_bit_pos_array() As UInteger = UintegerArrayOperater.MakeConnectedArray(hit_bitpos_array_low_byte, hit_bitpos_array_high_byte_plus_byte_bit_length)

        Return hit_bit_pos_array



    End Function

End Class
