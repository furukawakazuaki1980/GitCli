Public Class UintegerSetOperater '配列から重複を排除して集合として扱う
    Inherits UintegerArrayOperater

    'ANDの集合を得る.checked2024_06_12
    Public Shared Function MakeAndSetArray(ByVal aArray1() As UInteger, ByVal aArray2() As UInteger) As UInteger()

        '重複を排除した配列を作る(つまり、実質的に集合にする)
        Dim unique_array1() As UInteger = MakeUniqueArray(aArray1)
        Dim unique_array2() As UInteger = MakeUniqueArray(aArray2)

        '重複を排除した後の長さを得る
        Dim unique_array1_length As UInteger = GenericArrayOperater(Of UInteger).GetArrayLength(unique_array1)
        Dim unique_array2_length As UInteger = GenericArrayOperater(Of UInteger).GetArrayLength(unique_array2)

        'ANDなので、どちらか片方が空なら空
        If (unique_array1_length = 0) Or (unique_array2_length = 0) Then 'checked
            Dim empty_uinteger_array() As UInteger = {}
            Return empty_uinteger_array
        End If

        'ここに来る⇔両方とも非空
        '∴ANDの集合を求められる

        'unique_array1をテスト用の配列で、unique_array2をDB用の配列として、フラグの配列を得る
        Dim exist_flag_array() As Boolean = MakeExistFlagArrayInDataBaseArray(unique_array1, unique_array2)

        'unique_array1から、exist_flag_arrayがTrueになっている場所だけ、取り出す
        'その結果がANDの集合
        Dim and_set_array() As UInteger = GenericArrayOperater(Of UInteger).MakeSelectedValueArrayByFlagArray(unique_array1, exist_flag_array, True)


        Return and_set_array


    End Function

    '集合同士の引き算をする.左の集合-右の集合の結果の集合を求める.checked2024_06_12
    Public Shared Function MakeMinusSetArray(ByVal aLeftArray() As UInteger, ByVal aRightArray() As UInteger) As UInteger()

        '重複を排除して集合にする
        Dim unique_left_array() As UInteger = MakeUniqueArray(aLeftArray)
        Dim unique_right_array() As UInteger = MakeUniqueArray(aRightArray)

        '集合のサイズを得る
        Dim unique_left_array_length As UInteger = GenericArrayOperater(Of UInteger).GetArrayLength(unique_left_array)
        Dim unique_right_array_length As UInteger = GenericArrayOperater(Of UInteger).GetArrayLength(unique_right_array)

        If unique_left_array_length = 0 Then
            Dim empty_uinteger_array() As UInteger = {}
            Return empty_uinteger_array 'checked
        End If

        'ここに来る⇔左の集合は非空

        If unique_right_array_length = 0 Then
            'ここに来る⇔{左の集合は非空}かつ{右の集合は空}
            '∴左の集合をそのままreturnすれば良い
            Return unique_left_array 'checked
        End If

        'ここに来る⇔{左の集合は非空}かつ{右の集合は非空}
        '∴集合同士の引き算を行う

        '左の集合をテスト用の配列、右の集合をデータベース用の配列として、フラグの配列を作る
        Dim exist_flag_array_left() As Boolean = MakeExistFlagArrayInDataBaseArray(unique_left_array, unique_right_array)

        'フラグの配列で、falseになっている箇所(つまり、データベースである右の集合に属していない箇所)の要素を抜き出す
        'その結果が集合のマイナスの結果
        Dim minus_set_array() As UInteger = GenericArrayOperater(Of UInteger).MakeSelectedValueArrayByFlagArray(unique_left_array, exist_flag_array_left, False)

        Return minus_set_array


    End Function

    'ORの集合を求める.checked2024_06_12
    Public Shared Function MakeORSetArray(ByVal aArray1() As UInteger, ByVal aArray2() As UInteger) As UInteger()

        '連結する
        Dim connected_array() As UInteger = GenericArrayOperater(Of UInteger).MakeConnectedArray(aArray1, aArray2)


        '連結した結果から、重複を排除(その結果がORの集合)
        Dim or_set_array() As UInteger = MakeUniqueArray(connected_array)

        Return or_set_array

    End Function

    'EXORの集合を求める。ORの集合と、ANDの集合を求める。EXORの集合=ORの集合-ANDの集合.checked2024_06_12
    'となる。
    Public Shared Function MakeEXORSetArray(ByVal aArray1() As UInteger, ByVal aArray2() As UInteger) As UInteger()

        'ORの集合を作る
        Dim or_set_array() As UInteger = MakeORSetArray(aArray1, aArray2)


        'ANDの集合を作る
        Dim and_set_array() As UInteger = MakeANDSetArray(aArray1, aArray2)

        'ORの集合-ANDの集合の計算をする(その結果がEXORの集合)
        Dim exor_set_array() As UInteger = MakeMinusSetArray(or_set_array, and_set_array)

        Return exor_set_array


    End Function

End Class
