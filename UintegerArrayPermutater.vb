Class UintegerArrayPermutater
    Inherits UintegerArrayOperater

    '配列のソート.checked2023_10_14
    Public Shared Function MakeSortedArray(ByVal aArray() As UInteger) As UInteger()

        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Dim empy_uinteger_array() As UInteger = {}
            Return empy_uinteger_array 'checked
        End If

        'ここに来る⇔array_length>=1

        If array_length = 1 Then

            Return {aArray(0)} 'checked
        End If

        'ここに来る⇔array_length>=2
        '∴ソートの処理ができる

        '最初は引数の配列のコピー
        Dim sorted_array(array_length - 1) As UInteger
        CopyArray(sorted_array, 0, aArray, 0, array_length)

        'ソート作業
        Array.Sort(sorted_array)

        Return sorted_array

    End Function

    '重複を除去.checked2023_10_14
    Public Shared Function DeleteDuplicate(ByVal aArray() As UInteger) As UInteger()
        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Dim empy_uinteger_array() As UInteger = {}
            Return empy_uinteger_array 'checked
        End If

        'ここに来る⇔array_length>=1

        If array_length = 1 Then

            Return {aArray(0)} 'checked
        End If

        'ここに来る⇔array_length>=2
        '∴配列の重複の除去の作業ができる

        Dim delete_duplicate_array() As UInteger = aArray.Distinct().ToArray()

        '重複を除去した結果を返す
        Return delete_duplicate_array


    End Function


    '重複がない配列かどうか、判定.checked2023_10_14
    Public Shared Function IsUniqueAboutArray(ByVal aArray() As UInteger) As Boolean

        '元の配列の長さ
        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length <= 1 Then

            Return True 'checked
        End If

        'ここに来る⇔配列の長さは2以上
        '∴重複がないかどうか、判定できる

        '重複を排除した配列
        Dim delete_duplicate_array() As UInteger = DeleteDuplicate(aArray)

        '重複を排除した配列の長さ
        Dim delete_duplicate_array_length As UInteger = GetArrayLength(delete_duplicate_array)

        '重複を排除しても同じ長さ⇔元々重複がない
        Dim unique_array_flag As Boolean = (delete_duplicate_array_length = array_length)

        Return unique_array_flag

    End Function

    '置換の配列かどうか、判定.checked2023_10_14
    Public Shared Function IsPermutationArray(ByVal aArray() As UInteger) As Boolean


        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Return False 'checked

        End If

        If array_length = 1 Then
            '配列の長さが1なら、インデックス0の値が0かどうか
            Return (aArray(0) = 0) 'checked

        End If

        'ここに来る⇔元の配列の長さは2以上

        If Not IsUniqueAboutArray(aArray) Then
            '元の配列の長さが2以上で、かつ、重複がないならfalse
            Return False 'checked
        End If

        'ここに来る⇔{元の配列の長さは2以上}かつ{重複がない}
        '∴ソートした配列の長さは元の配列と同じ。ソートした配列の、
        '先頭と末尾の値からだけ、判定できる
        Dim sorted_array() As UInteger = MakeSortedArray(aArray)

        '{インデックス0の値が0}かつ{最後の値==長さ-1}について判定
        Dim permutation_array_flag As Boolean = (sorted_array(0) = 0) And (sorted_array(array_length - 1) = array_length - 1)


        Return permutation_array_flag
    End Function

    '二つの長さが同じ置換の配列から、合成の置換を得る.checked2023_10_16
    Public Shared Function MakeCompositionPermutation(ByVal aLeftPermutationArray() As UInteger, ByVal aRightPermutationArray() As UInteger) As UInteger()

        '左側の配列が、置換の配列かどうか、判定
        Dim left_permutation_flag As Boolean = IsPermutationArray(aLeftPermutationArray)

        '右側の配列が、置換の配列かどうか、判定
        Dim right_permutation_flag As Boolean = IsPermutationArray(aRightPermutationArray)

        '左右の配列が、共に置換の配列かどうか、判定
        Dim left_right_permutation_flag As Boolean = left_permutation_flag And right_permutation_flag

        'エラーチェックのための、空列を宣言
        Dim empty_uinteger_array() As UInteger = {}


        If Not left_right_permutation_flag Then
            Return empty_uinteger_array 'checked
        End If

        'ここに来る⇔二つとも(非空の)置換の配列

        '左の配列の長さを得る
        Dim left_array_length As UInteger = GetArrayLength(aLeftPermutationArray)

        '右の配列の長さを得る
        Dim right_array_length As UInteger = GetArrayLength(aRightPermutationArray)

        If left_array_length <> right_array_length Then

            Return empty_uinteger_array 'checked
        End If

        'ここに来る⇔{二つとも(非空の)置換の配列}かつ{二つの配列の長さが同じ}

        '∴置換の合成の処理ができる

        '置換の合成の結果を入れる配列を、ローカルで宣言(左右の共通の長さで宣言)
        Dim composed_permutation_array(left_array_length - 1) As UInteger

        '置換の合成の結果の配列を作る
        For index As UInteger = 0 To left_array_length - 1
            '置換の合成の結果の第index=indexを右の配列で移してから、左の配列で移した値
            composed_permutation_array(index) = aLeftPermutationArray(aRightPermutationArray(index))


        Next

        Return composed_permutation_array


    End Function



    '逆置換の生成。checked2023_10_14
    Public Shared Function MakeInversePermutationArray(ByVal aArray() As UInteger) As UInteger()

        If Not IsPermutationArray(aArray) Then

            Dim empty_array() As UInteger = {}

            Return empty_array 'checked
        End If

        'ここに来る⇔aArrayは置換の配列
        '∴逆置換の配列を作れる
        Dim array_length As UInteger = GetArrayLength(aArray) 'ここでは1以上

        '逆置換の配列を宣言(長さは、元の配列と同じ)
        Dim inverse_permutation_array(array_length - 1) As UInteger

        '逆置換の配列の中身を作る
        For index As UInteger = 0 To array_length - 1

            '逆置換の配列において、indexを入れる場所は、aArray(index)
            inverse_permutation_array(aArray(index)) = index

        Next

        Return inverse_permutation_array

    End Function

    '左の値(インデックスが小さい方の値)が大きい状況に限り、スワップを繰り返す.checked2023_10_06
    Public Shared Sub RepeatSwappingWhileLeftValueIsBigger(ByRef aRefArray() As UInteger, ByRef aRefTraceArray() As UInteger, ByVal aStartIndex As UInteger)

        'スワップ対象の配列の長さを得る
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        'スタート位置とスワップ対象の配列の長さについて、判定
        Dim start_length_flag As Boolean = (array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length)

        If Not start_length_flag Then
            Return 'checked
        End If

        'ここに来る⇔(array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length)

        'トレースの配列のチェック
        Dim trace_array_length As UInteger = GetArrayLength(aRefTraceArray)

        If trace_array_length <> array_length Then

            Return 'checked

        End If

        'ここに来る⇔(array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length) And (trace_array_length=array_length)


        '∴aStartIndexから、デクリメントの方向で、スワップをスタートできる

        Dim index As UInteger = aStartIndex

        'whileループの条件で、aRefArray(index - 1)にアクセスしているので、index>=1も当然成立するべき条件である
        'indexの初期値は>=1で、しかも、whileループの中で、デクリメント直後、indexが0なら
        'whileループからbreakするので、必ずindex>=1は成立している
        While (aRefArray(index - 1) > aRefArray(index))
            '左の要素の方が大きいのでswap
            ValueTreater.SwapTwoValues(aRefArray(index - 1), aRefArray(index))

            '値の配列と同じように、トレースの配列のindex-1の値とindexの値を交換
            ValueTreater.SwapTwoValues(aRefTraceArray(index - 1), aRefTraceArray(index))

            index -= 1 'indexをデクリメント

            If index = 0 Then 'index==0で次のループに進むとaRefArray(index - 1)で実行時エラー
                Exit While 'checked
            End If


        End While

        Return
    End Sub

    '配列のソートと、置換のトレースの配列のセット.checked2023_10_16
    Public Shared Sub SortArrayWithSettingTraceArray(ByRef aRefArray() As UInteger, ByRef aRefTraceArray() As UInteger)

        '配列の長さを得る
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        If array_length = 0 Then

            aRefTraceArray = {}
            Return 'checked
        End If


        'ここに来る⇔(array_length >= 1) 

        If array_length = 1 Then
            aRefTraceArray = {0}

            Return 'checked

        End If

        'ここに来る⇔(array_length >= 2) 

        '∴ソートとトレースのセットを実行できる

        'aRefTraceArrayを、0～array_length-1を並べた配列にする
        aRefTraceArray = MakeRangeListArray(0, array_length - 1)

        For swap_start As UInteger = 1 To array_length - 1

            '毎回のループ開始時、0～swap_start-1までの範囲で、昇順にソートしてあると言える
            'swap_startによる数学的帰納法で証明できる

            RepeatSwappingWhileLeftValueIsBigger(aRefArray, aRefTraceArray, swap_start)


        Next

        Return

    End Sub

    '配列のソートと、そのソートの動き方の配列(トレースの配列の逆置換)の配列を得る.checked2023_10_16
    Public Shared Sub SortArrayWithSettingHowToMoveArray(ByRef aRefArray() As UInteger, ByRef aRefHowToMoveArray() As UInteger)

        'ソート対象の配列の長さを得る
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        If array_length = 0 Then
            aRefHowToMoveArray = {}
            Return 'checked
        End If

        'ここに来る⇔array_length>=1

        If array_length = 1 Then
            aRefHowToMoveArray = {0}
            Return 'checked
        End If

        'ここに来る⇔array_length>=2

        '∴ソートとそのトレースの配列の処理ができる

        'ソートの処理をしてトレースの配列を得る
        Dim ref_trace_array() As UInteger = {}
        SortArrayWithSettingTraceArray(aRefArray, ref_trace_array)

        '動き方の配列は、トレースの処理の逆置換
        aRefHowToMoveArray = MakeInversePermutationArray(ref_trace_array)

        Return


    End Sub


End Class