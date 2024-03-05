Public Class BitOperaterForByte




    Public Const BYTE_BIT_LENGTH As UInteger = 8
    Private Const LOW_FOUR_BIT_BITMASK As Byte = (1 << 4) - 1

    'Byte型に対する左シフト.checked2023_10_08
    Public Shared Function LeftShiftByte(ByVal aByteValue As Byte, ByVal aLeftShiftNum As UInteger) As Byte

        If aLeftShiftNum >= BYTE_BIT_LENGTH Then

            Return 0 'checked

        End If

        'ここに来る⇔aLeftShiftNum<BYTE_BIT_LENGTH

        '左ビットシフトができる

        Dim left_shift_byte_value As Byte = aByteValue << aLeftShiftNum

        Return left_shift_byte_value
    End Function

    'Byte型に対する右シフト.checked2023_10_08

    Public Shared Function RightShiftByte(ByVal aByteValue As Byte, ByVal aRightShiftNum As UInteger) As Byte

        If aRightShiftNum >= BYTE_BIT_LENGTH Then

            Return 0 'checked
        End If

        'ここに来る⇔ aRightShiftNum < BYTE_BIT_LENGTH

        Dim right_shift_byte As Byte = aByteValue >> aRightShiftNum

        Return right_shift_byte

    End Function

    'バイト同士のAND
    Public Shared Function ANDAsByte(ByVal aByte1 As Byte, ByVal aByte2 As Byte) As Byte

        'ANDをした結果
        Dim and_byte As Byte = aByte1 And aByte2

        Return and_byte

    End Function

    'バイト同士のOR
    Public Shared Function ORAsByte(ByVal aByte1 As Byte, ByVal aByte2 As Byte) As Byte

        Dim or_byte As Byte = aByte1 Or aByte2

        Return or_byte

    End Function

    'Byte同士のEXOR
    Public Shared Function EXORAsByte(ByVal aByte1 As Byte, ByVal aByte2 As Byte) As Byte

        Dim exor_byte As Byte = aByte1 Xor aByte2

        Return exor_byte

    End Function

    'バイト型のビット反転
    Public Shared Function NotAsByte(ByVal aByteValue As Byte) As Byte


        Dim not_byte As Byte = Not (aByteValue)

        Return not_byte

    End Function

    '8ビットの符号付き左ビットシフト.checked2024_01_24
    Public Shared Function LeftShiftWithSignBitAsByte(ByVal aByteValue As Byte, ByVal aLeftShiftNum As Byte) As Byte

        If aLeftShiftNum = 0 Then '左シフトしない

            Return aByteValue 'checked

        End If

        'ここに来る⇔aLeftShiftNum>=1

        '最上位ビットの識別.0x00または0x80(checked)
        Dim only_sign_bit_value As Byte = ANDAsByte(aByteValue, &H80L)

        If aLeftShiftNum >= BYTE_BIT_LENGTH - 1 Then

            Return only_sign_bit_value 'checked

        End If

        'ここに来る⇔1<=aLeftShiftNum<=BYTE_BIT_LENGTH - 1=7

        '∴符号つき左シフトの処理ができる

        '通常のaLeftShiftNumの左シフトをする(checked)
        Dim standard_left_shift_value As Byte = LeftShiftByte(aByteValue, aLeftShiftNum)


        '通常のaLeftShiftNumの左シフトの結果に対して、最上位ビットをクリアする(checked)
        Dim standard_left_shift_value_without_sign_bit As Byte = ANDAsByte(standard_left_shift_value, &H7FL)


        If only_sign_bit_value = 0 Then '元々のaByteValueの最上位ビットが0

            Return standard_left_shift_value_without_sign_bit 'checked
        End If

        'ここに来る⇔{1<=aLeftShiftNum<=BYTE_BIT_LENGTH - 1=7}かつ{元々のaByteValueの最上位ビットが1}

        '最上位ビットをクリアした結果に対して、最上位ビットだけ1にする
        'その結果が、符号つき左シフトの結果

        Dim left_shift_with_sign_bit As Byte = ORAsByte(&H80L, standard_left_shift_value_without_sign_bit)

        Return left_shift_with_sign_bit

    End Function
    '8ビットの符号付き右ビットシフト.checked2024_01_24
    Public Shared Function RightShiftWithSignBitAsByte(ByVal aByteValue As Byte, ByVal aRightShiftNum As Byte) As Byte

        If aRightShiftNum = 0 Then

            Return aByteValue 'checked

        End If

        'ここに来る⇔aRightShiftNum>=1

        '元々のaByteValueの最上位ビットだけの値。最上位ビットが1なら0x80で、0なら0x00
        Dim only_sign_bit_value As Byte = ANDAsByte(aByteValue, &H80L)

        If aRightShiftNum >= BYTE_BIT_LENGTH - 1 Then
            '元々の符号ビットだけを8ビット並べた値にする
            Dim all_sign_bit_value As Byte = If(only_sign_bit_value = &H80L, &HFFL, &H0)
            Return all_sign_bit_value 'checked

        End If

        'ここに来る⇔1<=aRightShiftNum<=6
        '∴右シフトの処理ができる

        '通常の方法で右ビットシフト
        Dim standard_right_shift_value As Byte = RightShiftByte(aByteValue, aRightShiftNum)

        If only_sign_bit_value = 0 Then '元々の最上位ビットが0
            Return standard_right_shift_value 'checked
        End If

        'ここに来る⇔{1<=aRightShiftNum<=6}かつ{aByteValueの最上位ビットは1}

        '上位aRightShiftNum個のビットが、全て1のビットパターンを作る
        Dim high_bits_all_one_value As Byte = LeftShiftByte(&HFFL, BYTE_BIT_LENGTH - aRightShiftNum)

        '通常の方法で右ビットシフトした値と、high_bits_all_one_valueをORした結果が、求める値
        Dim right_shift_value_with_sign_bit As Byte = ORAsByte(standard_right_shift_value, high_bits_all_one_value)

        Return right_shift_value_with_sign_bit



    End Function

    '8ビットの左回転シフト(回転シフトは左回転シフトだけで良い。しかも、シフト数はmod8でよい)

    Public Shared Function LeftRotateShiftAsByte(ByVal aByteValue As Byte, ByVal aShiftNum As Byte) As Byte
        Dim shift_num_mod_eight As Byte = (aShiftNum Mod BYTE_BIT_LENGTH)

        If shift_num_mod_eight = 0 Then

            Return aByteValue 'checked
        End If

        'ここに来る⇔1<=shift_num_mod_eight<=7
        '∴通常右シフトと通常左シフトをするシフト数が非ゼロ

        '元の値を、(8-shift_num_mod_eight)だけ、通常右シフト
        Dim right_shift_byte_value As Byte = RightShiftByte(aByteValue, BYTE_BIT_LENGTH - shift_num_mod_eight)

        '元の値を、shift_num_mod_eightだけ、通常左シフト
        Dim left_shift_byte_value As Byte = LeftShiftByte(aByteValue, shift_num_mod_eight)

        '二つの値のORが、回転左シフトの答え
        Dim left_rotate_shift_byte As Byte = ORAsByte(right_shift_byte_value, left_shift_byte_value)

        Return left_rotate_shift_byte

    End Function



    'バイトからブールの変換
    Public Shared Function IsNonZeroByteValue(ByVal aByteValue As Byte) As Boolean

        Return (aByteValue <> 0)
    End Function

    'ブールからバイトへの変換

    Public Shared Function ConvertBooleanIntoByte(ByVal aFlag As Boolean) As Byte

        Return If(aFlag, 1, 0)

    End Function

    '指定したビット位置のビットを調べる.checked2023_10_08
    Public Shared Function GetBitInByteAtBitPos(ByVal aByteValue As Byte, ByVal aByteBitPos As UInteger) As Byte

        If aByteBitPos >= BYTE_BIT_LENGTH Then

            Return 0 'checked
        End If

        'ここに来る⇔aByteBitPos <= BYTE_BIT_LENGTH-1
        '∴aBitPosによりビットの取得ができる

        '第aByteBitPosビットだけ1のビットマスク
        Dim bitmask_bitpos As Byte = LeftShiftByte(1, aByteBitPos)

        '引数の値の、第aByteBitPosBit以外全て0にする
        Dim only_bitpos_bit_byte As Byte = ANDAsByte(aByteValue, bitmask_bitpos)

        '第aByteBitPosのビットを、第0ビットに持ってくる
        Dim bit_in_byte_at_bitpos As Byte = RightShiftByte(only_bitpos_bit_byte, aByteBitPos)

        Return bit_in_byte_at_bitpos

    End Function

    'Byte型の変数に対して、指定したビット位置のビットを1又は0にする.checked2023_10_09
    Public Shared Sub SetBitAsByte(ByRef aRefByte As Byte, ByVal aByteBitPos As Byte, ByVal aBitFlag As Boolean)

        If aByteBitPos >= BYTE_BIT_LENGTH Then
            Return 'checked
        End If

        'ここに来る⇔aByteBitPos<=BYTE_BIT_LENGTH-1

        '∴ビット位置は正常

        '第aByteBitPosだけ1の、ビットマスクを作る

        Dim bitmask_bitpos As Byte = LeftShiftByte(1, aByteBitPos)

        If aBitFlag Then 'checked'ビットを1にする場合

            '右辺の、ORAsByteの実引数のaRefByteは、aRefByteの値を一旦コピーした物
            'aRefByteのコピー値とbitmask_bitposのORの値を、再びaRefByteに入れるという意味
            aRefByte = ORAsByte(aRefByte, bitmask_bitpos)

            Return

        End If

        'ここに来るならaBitFlagはfalseなので、ビットを0にする

        '第aByteBitPosだけ0で、それ以外は1の値
        Dim not_bitmask_bitpos As Byte = NotAsByte(bitmask_bitpos)

        '右辺の、ORAsByteの実引数のaRefByteは、aRefByteの値を一旦コピーした物
        'aRefByteのコピー値とnot_bitmask_bitposのANDの値を、再びaRefByteに入れるという意味
        aRefByte = ANDAsByte(aRefByte, not_bitmask_bitpos)

        Return
    End Sub

    '1バイトの値を、上の4ビットと下の4ビットの値に分割.checked2023_10_13
    Public Shared Sub DivideOneByteIntoHighLowFourBit(ByVal aByte As Byte, ByRef aRefHighFourBit As Byte, ByRef aRefLowFourBit As Byte)

        aRefHighFourBit = RightShiftByte(aByte, 4)

        aRefLowFourBit = ANDAsByte(aByte, LOW_FOUR_BIT_BITMASK)

        Return
    End Sub

    '1バイトの値に対して、マイナスかどうかのフラグと、絶対値を得る.checked2023_12_01
    Public Shared Sub SetMinusFlagAndAbsoluteByteValue(ByVal aByteValue As Byte, ByRef aRefAbsoluteValue As Byte, ByRef aRefMinusFlag As Boolean)

        '引数の最上位ビットを得る
        Dim sign_bit As Byte = GetBitInByteAtBitPos(aByteValue, BYTE_BIT_LENGTH - 1)

        '最上位ビットが1と等しいかどうか、フラグで得る
        aRefMinusFlag = (sign_bit = 1)


        If Not aRefMinusFlag Then '最上位ビットが0

            aRefAbsoluteValue = aByteValue

            Return 'checked

        End If

        'ここに来る⇔最上位ビットが1
        '∴2の補数の処理が必要

        '引数のビットの反転(not_bit_byte_value=0xFF-aByteValue)
        Dim not_bit_byte_value As Byte = NotAsByte(aByteValue)

        '絶対値=0x100-aByteValue=(0xFF-aByteValue)+1=(aByteValueのビット反転)+1
        aRefAbsoluteValue = not_bit_byte_value + 1

        Return

    End Sub

    'ビットをリバースした値を返す.checked2023_11_02
    Public Shared Function MakeReverseBitValue(ByVal aByteValue As Byte) As Byte

        Dim reverse_bit_byte As Byte = 0

        For bitpos As UInteger = 0 To BYTE_BIT_LENGTH - 1

            If GetBitInByteAtBitPos(aByteValue, bitpos) Then

                'これからビット1をセットするビット位置
                Dim reverse_set_bitpos As Byte = BYTE_BIT_LENGTH - 1 - bitpos

                'ビット1をセット
                SetBitAsByte(reverse_bit_byte, reverse_set_bitpos, True)

            End If

        Next

        Return reverse_bit_byte


    End Function


    'Byte型の値の中の、ビット1の個数を返す.
    Public Shared Function GetBitOneCountAsByte(ByVal aByteValue As Byte) As UInteger

        Dim bit_one_count As UInteger = 0
        For bitpos As UInteger = 0 To BYTE_BIT_LENGTH - 1

            '引数のByte型の値の、第bitposビットのビットを得る
            Dim bitpos_bit As Byte = GetBitInByteAtBitPos(aByteValue, bitpos)

            'そのビットを数値として足す。1ならbit_one_countは1増える。
            '0ならそのまま
            bit_one_count += bitpos_bit
        Next

        Return bit_one_count


    End Function

    'Byte型の引数を、長さ8の1又は0の配列に変換.checked2023_12_04
    Public Shared Function ConvertByteIntoBitArray(ByVal aByteValue As Byte) As Byte()

        Dim bit_array(BYTE_BIT_LENGTH - 1) As Byte

        For bitpos As UInteger = 0 To BYTE_BIT_LENGTH - 1
            bit_array(bitpos) = GetBitInByteAtBitPos(aByteValue, bitpos)
        Next

        Return bit_array
    End Function

    'Byte型の引数に対して、ビット1又は0の値の、ビット位置の配列を作る.checked2024_01_05

    Public Shared Function MakeHitBitposArrayBySearchBitFlag(ByVal aByteValue As Byte, ByVal aSearchBitFlag As Boolean) As UInteger()

        'サーチ対象を表すフラグを,1又は0に変換した値
        Dim search_bit_value As Byte = ConvertBooleanIntoByte(aSearchBitFlag)

        Dim hit_bitpos_array_engouh_size(BYTE_BIT_LENGTH - 1) As UInteger '8ビット分のサイズ

        Dim hit_bit_count As UInteger = 0

        For bitpos As UInteger = 0 To BYTE_BIT_LENGTH - 1

            '第bitposのビットが、サーチ対象の値と一致
            If GetBitInByteAtBitPos(aByteValue, bitpos) = search_bit_value Then

                hit_bitpos_array_engouh_size(hit_bit_count) = bitpos

                hit_bit_count += 1



            End If
        Next

        'ここに来た時、hit_bitpos_array_engouh_size(0)～hit_bitpos_array_engouh_size(hit_count-1)
        'が、ヒットしたビット位置

        '∴hit_bitpos_array_engouh_sizeから、先頭のhit_bit_countを抜き出せば良い
        '但し、 hit_bit_count==0なら空列を抜き出すようにする

        Dim hit_bitpos_array() As UInteger = UintegerArrayOperater.TakeSmallArrayFromBigArray(hit_bitpos_array_engouh_size, 0, hit_bit_count)

        Return hit_bitpos_array

    End Function




End Class
