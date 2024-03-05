Public Class BitOperaterForUintegerDouble
    Inherits BitOperaterForUinteger

    Public Const UINTEGER_DOUBLE_BIT_LENGTH As UInteger = 2 * UINTEGER_BIT_LENGTH



    '32�r�b�g�㉺�A����OR.checked2023_10_11
    Public Shared Sub ORAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '���uinteger���m��OR
        aRefHighResult = ORAsUinteger(aHighUinteger1, aHighUinteger2)

        '����uinteger���m��OR
        aRefLowResult = ORAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub


    '32�r�b�g�㉺�A����AND.checked2023_10_11
    Public Shared Sub ANDAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '���uinteger���m��AND
        aRefHighResult = ANDAsUinteger(aHighUinteger1, aHighUinteger2)

        '����uinteger���m��AND
        aRefLowResult = ANDAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub


    '32�r�b�g�㉺�A����EXOR.checked2023_10_11
    Public Shared Sub EXORAsUintegerDouble(ByVal aHighUinteger1 As Double, ByVal aLowUinteger1 As Double, ByVal aHighUinteger2 As Double, ByVal aLowUinteger2 As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '���uinteger���m��EXOR
        aRefHighResult = EXORAsUinteger(aHighUinteger1, aHighUinteger2)

        '����uinteger���m��EXOR
        aRefLowResult = EXORAsUinteger(aLowUinteger1, aLowUinteger2)

    End Sub

    '32�r�b�g�㉺�A����NOT.checked2023_10_11
    Public Shared Sub NOTAsUintegerDouble(ByVal aHighUinteger As Double, ByVal aLowUinteger As Double, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '���uinteger��NOT
        aRefHighResult = NOTAsUinteger(aHighUinteger)

        '����uinteger��NOT
        aRefLowResult = NOTAsUinteger(aLowUinteger)

    End Sub


    '32�r�b�g��A������64�r�b�g�ɂ��āA���V�t�g.checked2023_10_10
    Public Shared Sub LeftShiftUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aLeftShiftNum As UInteger, ByRef aRefHighShiftResult As UInteger, ByRef aRefLowShiftResult As UInteger)

        aRefHighShiftResult = 0
        aRefLowShiftResult = 0
        If aLeftShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH Then


            Return 'checked

        End If

        '�����ɗ����aLeftShiftNum<= UINTEGER_DOUBLE_BIT_LENGTH - 1
        '�����V�t�g���ł���
        If aLeftShiftNum >= UINTEGER_BIT_LENGTH Then
            '�����ɗ����UINTEGER_BIT_LENGTH<=aLeftShiftNum<= UINTEGER_DOUBLE_BIT_LENGTH - 1

            '���̃V�t�g���ʂ�0�̂܂�

            '��̃V�t�g���ʂ͉��̈�����<<(aLeftShiftNum - UINTEGER_BIT_LENGTH)�����l(checked)
            aRefHighShiftResult = LeftShiftUinteger(aLowUinteger, aLeftShiftNum - UINTEGER_BIT_LENGTH)

            Return 'checked

        End If

        '�����ɗ����UINTEGER_BIT_LENGTH-1>=aLeftShiftNum >=0

        '���̌��ʂɂ��ẮA���̈����̒l<<aLeftShitNum
        aRefLowShiftResult = LeftShiftUinteger(aLowUinteger, aLeftShiftNum)

        '��̌��ʂɂ��ẮA���̓�̒l��(uinteger���m��)OR
        '�����̏�̒l<<aLeftShiftNum
        Dim high_uinteger_left_shift As UInteger = LeftShiftUinteger(aHighUinteger, aLeftShiftNum)


        '�����̉��̒l>>(UINTEGER_BIT_LENGTH-aLeftShiftNum)
        Dim low_uinteger_right_shift As UInteger = RightShiftUinteger(aLowUinteger, UINTEGER_BIT_LENGTH - aLeftShiftNum)

        '��̌���
        aRefHighShiftResult = ORAsUinteger(high_uinteger_left_shift, low_uinteger_right_shift)

        Return
    End Sub

    '32�r�b�g��A������64�r�b�g�ɂ��āA�E�V�t�g.checked2023_10_10

    Public Shared Sub RightShiftUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aRightShiftNum As UInteger, ByRef aRefHighResult As UInteger, ByRef aRefLowResult As UInteger)

        '�ŏ��A�Q�Ɠn���̕ϐ��́A0�ŏ�����
        aRefHighResult = 0
        aRefLowResult = 0


        If aRightShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH Then
            Return
        End If

        '�����ɗ����aRightShiftNum<=UINTEGER_DOUBLE_BIT_LENGTH-1

        If aRightShiftNum >= UINTEGER_BIT_LENGTH Then 'checked
            '�����ɗ����UINTEGER_BIT_LENGTH<=aRightShiftNum<=UINTEGER_DOUBLE_BIT_LENGTH-1

            '��̎Q�Ɠn���̕ϐ���0�̂܂�

            '���̎Q�Ɠn���̕ϐ��́A��̈�����>>(aRightShiftNum-UINTEGER_BIT_LENGTH)�����l(�l������)
            aRefLowResult = RightShiftUinteger(aHighUinteger, aRightShiftNum - UINTEGER_BIT_LENGTH)

            Return

        End If

        '�����ɗ����aRightShiftNum<=UINTEGER_BIT_LENGTH-1

        '��̌��ʂ́A��̈�����>>aRightShiftNum�����l
        aRefHighResult = RightShiftUinteger(aHighUinteger, aRightShiftNum)

        '���̌��ʂ́A���̓�̒l��OR�ł���

        '��̈�����<<(UINTEGER_BIT_LENGTH-aRightShiftNum)�����l
        Dim high_uinteger_left_shift As UInteger = LeftShiftUinteger(aHighUinteger, UINTEGER_BIT_LENGTH - aRightShiftNum)

        '���̈�����>>aRightShiftNum�����l
        Dim low_uinteger_right_shift As UInteger = RightShiftUinteger(aLowUinteger, aRightShiftNum)

        aRefLowResult = ORAsUinteger(high_uinteger_left_shift, low_uinteger_right_shift)

        Return


    End Sub

    '64�r�b�g(32�r�b�g�㉺�A��)�́A���������V�t�g.checked2024_03_01
    Public Shared Sub LeftShiftUintegerDoubleWithSignBit(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aLeftShiftNum As UInteger, ByRef aRefLeftShiftHigh As UInteger, ByRef aRefLeftShiftLow As UInteger)

        aRefLeftShiftHigh = 0
        aRefLeftShiftLow = 0

        If aLeftShiftNum = 0 Then
            aRefLeftShiftHigh = aUintegerHigh
            aRefLeftShiftLow = aUintegerLow
            Return 'checked
        End If

        '�����ɗ����1<=aLeftShiftNum

        '�ł���̃r�b�g�����L��
        Dim only_sign_bit_memory As UInteger = ANDAsUinteger(aUintegerHigh, &H80000000L)


        If aLeftShiftNum >= UINTEGER_DOUBLE_BIT_LENGTH - 1 Then

            aRefLeftShiftHigh = only_sign_bit_memory
            aRefLeftShiftLow = 0

            Return 'checked

        End If

        '�����ɗ����1<=aLeftShiftNum<=63
        '�����������V�t�g���ł���

        '�ʏ�̍��V�t�g���s��
        Dim ref_left_shift_high As UInteger = 0
        Dim ref_left_shift_low As UInteger = 0
        LeftShiftUintegerDouble(aUintegerHigh, aUintegerLow, aLeftShiftNum, ref_left_shift_high, ref_left_shift_low)

        '�ŏ�ʃr�b�g����U0�ɂ���
        Dim ref_left_shift_high_sign_bit_zero As UInteger = ANDAsUinteger(ref_left_shift_high, &H7FFFFFFF)

        '����32�r�b�g�́A����if���Ƀq�b�g���Ă����Ȃ��Ă��A�ʏ�̍��V�t�g�̌���
        aRefLeftShiftLow = ref_left_shift_low

        If only_sign_bit_memory = 0 Then '���X�́A�L�����Ă������ŏ�ʃr�b�g��0
            aRefLeftShiftHigh = ref_left_shift_high_sign_bit_zero

            Return 'checked
        End If

        '�����ɗ����{1<=aLeftShiftNum<=63}����{���X�́A�L�����Ă������ŏ�ʃr�b�g��1}

        '���32�r�b�g�́A�ŏ�ʃr�b�g����U0�ɂ������ʂƁA0x80000000�Ƃ�OR
        aRefLeftShiftHigh = ORAsUinteger(ref_left_shift_high_sign_bit_zero, &H80000000L)

        '����32�r�b�g�́A�ʏ�̍��V�t�g�̌��ʂȂ̂ŁA���̂܂�

        Return

    End Sub

    '�ŏ������蒼���B32�r�b�g�����́A�r�b�g1�̘A���̃r�b�g�}�X�N(���V�t�g��)������̂ŁA
    '����𗘗p���āA64�r�b�g(32�r�b�g�㉺�A��)�́A�r�b�g1�̘A���̃r�b�g�}�X�N(���V�t�g��)�����Ηǂ�
    ''64�r�b�g(32�r�b�g�㉺�A��)�́A�������E�V�t�g.checked2024_03_01

    Public Shared Sub RightShiftUintegerDoubleWithSignBit(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aRightShift As UInteger, ByRef aRefRightShiftHigh As UInteger, ByRef aRefRightShiftLow As UInteger)

        aRefRightShiftHigh = 0
        aRefRightShiftLow = 0

        '�ŏ�ʃr�b�g���L��
        Dim memory_only_sign_bit As UInteger = ANDAsUinteger(aUintegerHigh, &H80000000L)

        If aRightShift >= UINTEGER_DOUBLE_BIT_LENGTH - 1 Then
            '�ŏ�ʃr�b�g�ɍ��킹�āA�S�ăr�b�g1�܂��͑S�ăr�b�g0
            Dim all_bit_one_or_zero As UInteger = IIf(memory_only_sign_bit = &H80000000L, &HFFFFFFFFL, 0)
            aRefRightShiftHigh = all_bit_one_or_zero
            aRefRightShiftLow = all_bit_one_or_zero
            Return
        End If

        '�����ɗ����aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-2
        If aRightShift = 0 Then
            '�l�n���̈��������̂܂ܓ���ďI��
            aRefRightShiftHigh = aUintegerHigh
            aRefRightShiftLow = aUintegerLow
            Return 'checked
        End If

        '�����ɗ����1<=aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-2

        '�ʏ�̉E�V�t�g������
        Dim standard_right_shift_high As UInteger = 0
        Dim standard_right_shift_low As UInteger = 0
        RightShiftUintegerDouble(aUintegerHigh, aUintegerLow, aRightShift, standard_right_shift_high, standard_right_shift_low)



        If memory_only_sign_bit = 0 Then
            '�ʏ�̉E�V�t�g�̌��ʂ��A�Q�Ɠn���̕ϐ��ɓ����
            aRefRightShiftHigh = standard_right_shift_high
            aRefRightShiftLow = standard_right_shift_low
            Return 'checked
        End If

        '�����ɗ����{1<=aRightShift<= UINTEGER_DOUBLE_BIT_LENGTH-1}
        '���ʏ�̉E�V�t�g�̌��ʂɑ΂��āA���[�ɁAaRightShift�̃r�b�g1��A�����ē����Ηǂ�

        '���V�t�g�����r�b�g�}�X�N�����
        Dim bit_mask_left_shift_high As UInteger = 0
        Dim bit_mask_left_shift_low As UInteger = 0
        SetContinuousBitOneBitMaskUintegerDoubleWithLeftShift(aRightShift, UINTEGER_DOUBLE_BIT_LENGTH - aRightShift, bit_mask_left_shift_high, bit_mask_left_shift_low)

        '�ʏ�̉E�V�t�g�̌��ʂƁA���V�t�g�����r�b�g�}�X�N�Ƃ�OR���A�ŏI�I�ȓ����Ƃ���
        aRefRightShiftHigh = ORAsUinteger(bit_mask_left_shift_high, standard_right_shift_high)
        aRefRightShiftLow = ORAsUinteger(bit_mask_left_shift_low, standard_right_shift_low)

        Return
    End Sub

    '�A�������r�b�g1�̃r�b�g�}�X�N�����Bchecked
    Public Shared Sub SetContinuousBitOneBitMaskUintegerDouble(ByVal aContinousBitOneCount As Byte, ByRef aRefBitMaskHigh As UInteger, ByRef aRefBitMaskLow As UInteger)

        aRefBitMaskHigh = 0
        aRefBitMaskLow = 0
        If aContinousBitOneCount = 0 Then
            Return 'checked
        End If

        '�����ɗ����1<=aContinousBitOneCount

        If aContinousBitOneCount >= UINTEGER_DOUBLE_BIT_LENGTH Then
            aRefBitMaskHigh = &HFFFFFFFFL
            aRefBitMaskLow = &HFFFFFFFFL
            Return 'checked
        End If

        '�����ɗ����1<=aContinousBitOneCount<=UINTEGER_DOUBLE_BIT_LENGTH-1

        If aContinousBitOneCount <= UINTEGER_BIT_LENGTH Then
            '����32�r�b�g����ō��Ηǂ�
            aRefBitMaskLow = MakeBitMaskContinuousBitOne(aContinousBitOneCount)

            Return 'chekced

        End If

        '�����ɗ����UINTEGER_BIT_LENGTH+1<=aContinousBitOneCount<=UINTEGER_DOUBLE_BIT_LENGTH-1

        '�����̒l��&HFFFFFFFFL�ƌ��܂��Ă���
        aRefBitMaskLow = &HFFFFFFFFL

        '��ɂ��ẮAaContinousBitOneCount-UINTEGER_BIT_LENGTH�̒l�ŁA�ゾ���ō��Ηǂ�
        aRefBitMaskHigh = MakeBitMaskContinuousBitOne(aContinousBitOneCount - UINTEGER_BIT_LENGTH)


        Return
    End Sub

    '�A�������r�b�g1�̃r�b�g�}�X�N�ɑ΂��āA���V�t�g���s��.checked2024_03_01
    Public Shared Sub SetContinuousBitOneBitMaskUintegerDoubleWithLeftShift(ByVal aContinousBitOneCount As Byte, ByVal aLeftShiftNum As Byte, ByRef aRefBitMaskHigh As UInteger, ByRef aRefBitMaskLow As UInteger)

        aRefBitMaskHigh = 0
        aRefBitMaskLow = 0

        If Not ValueComparater.ExistOneDimSubArea(UINTEGER_DOUBLE_BIT_LENGTH, aLeftShiftNum, aContinousBitOneCount) Then

            Return 'checked

        End If

        '�����ɗ���̃r�b�g�}�X�N�ɑ΂��āA���V�t�g�����s���Ă��A���v
        '���r�b�g�}�X�N����U���A���̃r�b�g�}�X�N�ɑ΂��āA���V�t�g�����s����

        Dim ref_bitmask_high As UInteger = 0
        Dim ref_bitmask_low As UInteger = 0
        SetContinuousBitOneBitMaskUintegerDouble(aContinousBitOneCount, ref_bitmask_high, ref_bitmask_low)

        '���V�t�g���s���B���ʂ͂��̂܂܎Q�Ɠn���̕ϐ�
        LeftShiftUintegerDouble(ref_bitmask_high, ref_bitmask_low, aLeftShiftNum, aRefBitMaskHigh, aRefBitMaskLow)

        Return

    End Sub


    '64�r�b�g(�㉺32�r�b�g�̘A��)�̍���]�V�t�g.checked2024_03_01
    Public Shared Sub LeftRotateShiftAsUintegerDouble(ByVal aUintegerHigh As UInteger, ByVal aUintegerLow As UInteger, ByVal aLeftShiftNum As Byte, ByRef aRefRotateHigh As UInteger, ByRef aRefRotateLow As UInteger)

        '��]�V�t�g�Ȃ̂ŁAmod64�����
        Dim left_shift_num_mod_sixty_four As Byte = (aLeftShiftNum Mod UINTEGER_DOUBLE_BIT_LENGTH)

        If left_shift_num_mod_sixty_four = 0 Then
            aRefRotateHigh = aUintegerHigh
            aRefRotateLow = aUintegerLow
            Return 'checked
        End If

        '�����ɂ����1<=left_shift_num_mod_sixty_four<=63
        '����]���V�t�g���ł���

        '���̒l�̍�(left_shift_num_mod_sixty_four)�V�t�g
        Dim ref_left_shift_high As UInteger = 0
        Dim ref_left_shift_low As UInteger = 0
        LeftShiftUintegerDouble(aUintegerHigh, aUintegerLow, left_shift_num_mod_sixty_four, ref_left_shift_high, ref_left_shift_low)

        '���̒l�̉E(64-left_shift_num_mod_sixty_four)�V�t�g�𓾂�
        Dim ref_right_shift_high As UInteger = 0
        Dim ref_right_shift_low As UInteger = 0
        RightShiftUintegerDouble(aUintegerHigh, aUintegerLow, UINTEGER_DOUBLE_BIT_LENGTH - left_shift_num_mod_sixty_four, ref_right_shift_high, ref_right_shift_low)

        '���V�t�g�̌��ʂƁA�E�V�t�g�̌��ʂɂ��āA64�r�b�g��OR������
        '64�r�b�g��OR�́A32�r�b�g�ÂA�㉺�ʁX��OR
        aRefRotateHigh = ORAsUinteger(ref_left_shift_high, ref_right_shift_high) '��32�r�b�g
        aRefRotateLow = ORAsUinteger(ref_left_shift_low, ref_right_shift_low) '��32�r�b�g

        Return

    End Sub

    'uinteger�^�̘A���Ńr�b�g�𓾂�.checked2023_10_10
    Public Shared Function GetBitUintegerDouble(ByVal aHighUinteger As UInteger, ByVal aLowUinteger As UInteger, ByVal aUintegerDoubleBitPos As UInteger) As UInteger

        If aUintegerDoubleBitPos >= UINTEGER_DOUBLE_BIT_LENGTH Then

            Return 0 'checked
        End If

        '�����ɗ����aUintegerDoubleBitPos <= UINTEGER_DOUBLE_BIT_LENGTH-1

        '����̒l�������͉��̒l����r�b�g������

        If aUintegerDoubleBitPos >= UINTEGER_BIT_LENGTH Then 'checked
            '�����ɗ����UINTEGER_BIT_LENGTH<=aUintegerDoubleBitPos <= UINTEGER_DOUBLE_BIT_LENGTH-1
            '����̒l����r�b�g�����

            '��̒l�ɑ΂���r�b�g�ʒu
            Dim high_uinteger_bitpos As UInteger = aUintegerDoubleBitPos - UINTEGER_BIT_LENGTH

            '��̒l������o�����r�b�g
            Dim high_uinteger_bitpos_bit As UInteger = GetBitInUintegerAtBitPos(aHighUinteger, high_uinteger_bitpos)

            Return high_uinteger_bitpos_bit

        End If

        '�����ɗ����aUintegerDoubleBitPos<=UINTEGER_BIT_LENGTH-1

        '�����̒l���炻�̂܂܎��o��

        Dim low_uinteger_bitpos_bit As UInteger = GetBitInUintegerAtBitPos(aLowUinteger, aUintegerDoubleBitPos)
        Return low_uinteger_bitpos_bit



    End Function

    'uinteger�^�̘A���ł̃r�b�g�̃Z�b�g.checked2023_10_10

    Public Shared Sub SetBitUintegerDouble(ByRef aRefHighUinteger As UInteger, ByRef aRefLowUinteger As UInteger, ByVal aUintegerDoubleBitPos As UInteger, ByVal aBitFlag As Boolean)

        If aUintegerDoubleBitPos >= UINTEGER_DOUBLE_BIT_LENGTH Then

            Return 'checked
        End If

        '�����ɗ����aUintegerDoubleBitPos<=UINTEGER_DOUBLE_BIT_LENGTH-1


        If aUintegerDoubleBitPos >= UINTEGER_BIT_LENGTH Then
            '�����ɗ����UINTEGER_BIT_LENGTH<=aUintegerDoubleBitPos<=UINTEGER_DOUBLE_BIT_LENGTH-1
            '����̒l�� aUintegerDoubleBitPos - UINTEGER_BIT_LENGTH�r�b�g���Z�b�g
            SetBitAsUinteger(aRefHighUinteger, aUintegerDoubleBitPos - UINTEGER_BIT_LENGTH, aBitFlag)

            Return

        End If

        '�����ɗ����aUintegerDoubleBitPos<=UINTEGER_BIT_LENGTH-1
        '�����̒l��aUintegerDoubleBitPos�r�b�g���Z�b�g

        SetBitAsUinteger(aRefLowUinteger, aUintegerDoubleBitPos, aBitFlag)


        Return
    End Sub

    '32�r�b�g���㉺�A���ɂ��āA�ł������r�b�g1�̈ʒu�𓾂�.checked2023_10_31
    Public Shared Function GetHighestBitOnePosInDoubleUinteger(ByVal aHighUintegerValue As UInteger, ByVal aLowUintegerValue As UInteger) As UInteger

        If (aHighUintegerValue = 0) And (aLowUintegerValue = 0) Then

            Return ERROR_BITPOS 'checked
        End If


        '�����ɗ���̏㉺�̂ǂ��炩�͔�[��
        If aHighUintegerValue <> 0 Then 'checked
            '�オ��[���̏ꍇ�́A���32�r�b�g���ōł������r�b�g1��T���A���̃r�b�g�ʒu��UINTEGER_BIT_LENGTH�𑫂�

            Dim highest_bit_one_pos_high As UInteger = GetHighestBitOnePos(aHighUintegerValue) '���32�r�b�g���ōł������r�b�g�̃r�b�g�ʒu
            Dim highest_bit_one_pos_double As UInteger = highest_bit_one_pos_high + UINTEGER_BIT_LENGTH '�㉺32�r�b�g�ł̃r�b�g�ʒu
            Return highest_bit_one_pos_double
        End If

        '�����ɗ����{�㉺�̂ǂ��炩�͔�[��}����{��̓[��}��{��̓[��}����{���͔�[��}
        '�����������ׂāA���̒l�𓚂��ɂ���Ηǂ�

        Dim low_highest_bitone_pos As UInteger = GetHighestBitOnePos(aLowUintegerValue)

        Return low_highest_bitone_pos

    End Function

    '8�o�C�g���r�b�g�̗�ɕϊ�.checked2023_12_18
    Public Shared Function ConvertEightByteIntoBitByteArray(ByVal aHighUintegerValue As UInteger, ByVal aLowUintegerValue As UInteger) As Byte()

        '��4�o�C�g�̃r�b�g�̔z��
        Dim high_uinteger_value_bit_byte_array() As Byte = ConvertUintegerValueIntoBitArray(aHighUintegerValue)


        '��4�o�C�g�̃r�b�g�̔z��
        Dim low_uinteger_value_bit_byte_array() As Byte = ConvertUintegerValueIntoBitArray(aLowUintegerValue)

        '��4�o�C�g�̃r�b�g�̔z����E��(�C���f�b�N�X���傫����)
        '��4�o�C�g�̃r�b�g�̔z����E����(�C���f�b�N�X����������)
        Dim eight_byte_bit_byte_array() As Byte = ByteArrayOperater.MakeConnectedArray(low_uinteger_value_bit_byte_array, high_uinteger_value_bit_byte_array)

        Return eight_byte_bit_byte_array

    End Function




End Class