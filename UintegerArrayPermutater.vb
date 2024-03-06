Class UintegerArrayPermutater
    Inherits UintegerArrayOperater

    '�z��̃\�[�g.checked2023_10_14
    Public Shared Function MakeSortedArray(ByVal aArray() As UInteger) As UInteger()

        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Dim empy_uinteger_array() As UInteger = {}
            Return empy_uinteger_array 'checked
        End If

        '�����ɗ����array_length>=1

        If array_length = 1 Then

            Return {aArray(0)} 'checked
        End If

        '�����ɗ����array_length>=2
        '���\�[�g�̏������ł���

        '�ŏ��͈����̔z��̃R�s�[
        Dim sorted_array(array_length - 1) As UInteger
        CopyArray(sorted_array, 0, aArray, 0, array_length)

        '�\�[�g���
        Array.Sort(sorted_array)

        Return sorted_array

    End Function

    '�d��������.checked2023_10_14
    Public Shared Function DeleteDuplicate(ByVal aArray() As UInteger) As UInteger()
        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Dim empy_uinteger_array() As UInteger = {}
            Return empy_uinteger_array 'checked
        End If

        '�����ɗ����array_length>=1

        If array_length = 1 Then

            Return {aArray(0)} 'checked
        End If

        '�����ɗ����array_length>=2
        '���z��̏d���̏����̍�Ƃ��ł���

        Dim delete_duplicate_array() As UInteger = aArray.Distinct().ToArray()

        '�d���������������ʂ�Ԃ�
        Return delete_duplicate_array


    End Function


    '�d�����Ȃ��z�񂩂ǂ����A����.checked2023_10_14
    Public Shared Function IsUniqueAboutArray(ByVal aArray() As UInteger) As Boolean

        '���̔z��̒���
        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length <= 1 Then

            Return True 'checked
        End If

        '�����ɗ���̔z��̒�����2�ȏ�
        '���d�����Ȃ����ǂ����A����ł���

        '�d����r�������z��
        Dim delete_duplicate_array() As UInteger = DeleteDuplicate(aArray)

        '�d����r�������z��̒���
        Dim delete_duplicate_array_length As UInteger = GetArrayLength(delete_duplicate_array)

        '�d����r�����Ă����������̌��X�d�����Ȃ�
        Dim unique_array_flag As Boolean = (delete_duplicate_array_length = array_length)

        Return unique_array_flag

    End Function

    '�u���̔z�񂩂ǂ����A����.checked2023_10_14
    Public Shared Function IsPermutationArray(ByVal aArray() As UInteger) As Boolean


        Dim array_length As UInteger = GetArrayLength(aArray)

        If array_length = 0 Then

            Return False 'checked

        End If

        If array_length = 1 Then
            '�z��̒�����1�Ȃ�A�C���f�b�N�X0�̒l��0���ǂ���
            Return (aArray(0) = 0) 'checked

        End If

        '�����ɗ���̌��̔z��̒�����2�ȏ�

        If Not IsUniqueAboutArray(aArray) Then
            '���̔z��̒�����2�ȏ�ŁA���A�d�����Ȃ��Ȃ�false
            Return False 'checked
        End If

        '�����ɗ����{���̔z��̒�����2�ȏ�}����{�d�����Ȃ�}
        '���\�[�g�����z��̒����͌��̔z��Ɠ����B�\�[�g�����z��́A
        '�擪�Ɩ����̒l���炾���A����ł���
        Dim sorted_array() As UInteger = MakeSortedArray(aArray)

        '{�C���f�b�N�X0�̒l��0}����{�Ō�̒l==����-1}�ɂ��Ĕ���
        Dim permutation_array_flag As Boolean = (sorted_array(0) = 0) And (sorted_array(array_length - 1) = array_length - 1)


        Return permutation_array_flag
    End Function

    '��̒����������u���̔z�񂩂�A�����̒u���𓾂�.checked2023_10_16
    Public Shared Function MakeCompositionPermutation(ByVal aLeftPermutationArray() As UInteger, ByVal aRightPermutationArray() As UInteger) As UInteger()

        '�����̔z�񂪁A�u���̔z�񂩂ǂ����A����
        Dim left_permutation_flag As Boolean = IsPermutationArray(aLeftPermutationArray)

        '�E���̔z�񂪁A�u���̔z�񂩂ǂ����A����
        Dim right_permutation_flag As Boolean = IsPermutationArray(aRightPermutationArray)

        '���E�̔z�񂪁A���ɒu���̔z�񂩂ǂ����A����
        Dim left_right_permutation_flag As Boolean = left_permutation_flag And right_permutation_flag

        '�G���[�`�F�b�N�̂��߂́A����錾
        Dim empty_uinteger_array() As UInteger = {}


        If Not left_right_permutation_flag Then
            Return empty_uinteger_array 'checked
        End If

        '�����ɗ���̓�Ƃ�(����)�u���̔z��

        '���̔z��̒����𓾂�
        Dim left_array_length As UInteger = GetArrayLength(aLeftPermutationArray)

        '�E�̔z��̒����𓾂�
        Dim right_array_length As UInteger = GetArrayLength(aRightPermutationArray)

        If left_array_length <> right_array_length Then

            Return empty_uinteger_array 'checked
        End If

        '�����ɗ����{��Ƃ�(����)�u���̔z��}����{��̔z��̒���������}

        '���u���̍����̏������ł���

        '�u���̍����̌��ʂ�����z����A���[�J���Ő錾(���E�̋��ʂ̒����Ő錾)
        Dim composed_permutation_array(left_array_length - 1) As UInteger

        '�u���̍����̌��ʂ̔z������
        For index As UInteger = 0 To left_array_length - 1
            '�u���̍����̌��ʂ̑�index=index���E�̔z��ňڂ��Ă���A���̔z��ňڂ����l
            composed_permutation_array(index) = aLeftPermutationArray(aRightPermutationArray(index))


        Next

        Return composed_permutation_array


    End Function



    '�t�u���̐����Bchecked2023_10_14
    Public Shared Function MakeInversePermutationArray(ByVal aArray() As UInteger) As UInteger()

        If Not IsPermutationArray(aArray) Then

            Dim empty_array() As UInteger = {}

            Return empty_array 'checked
        End If

        '�����ɗ����aArray�͒u���̔z��
        '���t�u���̔z�������
        Dim array_length As UInteger = GetArrayLength(aArray) '�����ł�1�ȏ�

        '�t�u���̔z���錾(�����́A���̔z��Ɠ���)
        Dim inverse_permutation_array(array_length - 1) As UInteger

        '�t�u���̔z��̒��g�����
        For index As UInteger = 0 To array_length - 1

            '�t�u���̔z��ɂ����āAindex������ꏊ�́AaArray(index)
            inverse_permutation_array(aArray(index)) = index

        Next

        Return inverse_permutation_array

    End Function

    '���̒l(�C���f�b�N�X�����������̒l)���傫���󋵂Ɍ���A�X���b�v���J��Ԃ�.checked2023_10_06
    Public Shared Sub RepeatSwappingWhileLeftValueIsBigger(ByRef aRefArray() As UInteger, ByRef aRefTraceArray() As UInteger, ByVal aStartIndex As UInteger)

        '�X���b�v�Ώۂ̔z��̒����𓾂�
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        '�X�^�[�g�ʒu�ƃX���b�v�Ώۂ̔z��̒����ɂ��āA����
        Dim start_length_flag As Boolean = (array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length)

        If Not start_length_flag Then
            Return 'checked
        End If

        '�����ɗ����(array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length)

        '�g���[�X�̔z��̃`�F�b�N
        Dim trace_array_length As UInteger = GetArrayLength(aRefTraceArray)

        If trace_array_length <> array_length Then

            Return 'checked

        End If

        '�����ɗ����(array_length >= 2) And (aStartIndex >= 1) And (aStartIndex < array_length) And (trace_array_length=array_length)


        '��aStartIndex����A�f�N�������g�̕����ŁA�X���b�v���X�^�[�g�ł���

        Dim index As UInteger = aStartIndex

        'while���[�v�̏����ŁAaRefArray(index - 1)�ɃA�N�Z�X���Ă���̂ŁAindex>=1�����R��������ׂ������ł���
        'index�̏����l��>=1�ŁA�������Awhile���[�v�̒��ŁA�f�N�������g����Aindex��0�Ȃ�
        'while���[�v����break����̂ŁA�K��index>=1�͐������Ă���
        While (aRefArray(index - 1) > aRefArray(index))
            '���̗v�f�̕����傫���̂�swap
            ValueTreater.SwapTwoValues(aRefArray(index - 1), aRefArray(index))

            '�l�̔z��Ɠ����悤�ɁA�g���[�X�̔z���index-1�̒l��index�̒l������
            ValueTreater.SwapTwoValues(aRefTraceArray(index - 1), aRefTraceArray(index))

            index -= 1 'index���f�N�������g

            If index = 0 Then 'index==0�Ŏ��̃��[�v�ɐi�ނ�aRefArray(index - 1)�Ŏ��s���G���[
                Exit While 'checked
            End If


        End While

        Return
    End Sub

    '�z��̃\�[�g�ƁA�u���̃g���[�X�̔z��̃Z�b�g.checked2023_10_16
    Public Shared Sub SortArrayWithSettingTraceArray(ByRef aRefArray() As UInteger, ByRef aRefTraceArray() As UInteger)

        '�z��̒����𓾂�
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        If array_length = 0 Then

            aRefTraceArray = {}
            Return 'checked
        End If


        '�����ɗ����(array_length >= 1) 

        If array_length = 1 Then
            aRefTraceArray = {0}

            Return 'checked

        End If

        '�����ɗ����(array_length >= 2) 

        '���\�[�g�ƃg���[�X�̃Z�b�g�����s�ł���

        'aRefTraceArray���A0�`array_length-1����ׂ��z��ɂ���
        aRefTraceArray = MakeRangeListArray(0, array_length - 1)

        For swap_start As UInteger = 1 To array_length - 1

            '����̃��[�v�J�n���A0�`swap_start-1�܂ł͈̔͂ŁA�����Ƀ\�[�g���Ă���ƌ�����
            'swap_start�ɂ�鐔�w�I�A�[�@�ŏؖ��ł���

            RepeatSwappingWhileLeftValueIsBigger(aRefArray, aRefTraceArray, swap_start)


        Next

        Return

    End Sub

    '�z��̃\�[�g�ƁA���̃\�[�g�̓������̔z��(�g���[�X�̔z��̋t�u��)�̔z��𓾂�.checked2023_10_16
    Public Shared Sub SortArrayWithSettingHowToMoveArray(ByRef aRefArray() As UInteger, ByRef aRefHowToMoveArray() As UInteger)

        '�\�[�g�Ώۂ̔z��̒����𓾂�
        Dim array_length As UInteger = GetArrayLength(aRefArray)

        If array_length = 0 Then
            aRefHowToMoveArray = {}
            Return 'checked
        End If

        '�����ɗ����array_length>=1

        If array_length = 1 Then
            aRefHowToMoveArray = {0}
            Return 'checked
        End If

        '�����ɗ����array_length>=2

        '���\�[�g�Ƃ��̃g���[�X�̔z��̏������ł���

        '�\�[�g�̏��������ăg���[�X�̔z��𓾂�
        Dim ref_trace_array() As UInteger = {}
        SortArrayWithSettingTraceArray(aRefArray, ref_trace_array)

        '�������̔z��́A�g���[�X�̏����̋t�u��
        aRefHowToMoveArray = MakeInversePermutationArray(ref_trace_array)

        Return


    End Sub


End Class