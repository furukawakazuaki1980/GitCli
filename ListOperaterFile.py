import ValueOperaterFile

#リストの長さを得る.checked2024_01_31
def GetListLength(aList):
    type_str = type(aList)

    if type_str != list:
        return 0

    array_length = len(aList)
    return array_length

#一つの値で、自由な長さのリストを作るchecked2024_01_31
def MakeFreeLengthListByCommonValue(aListLength,aCommonValue):
    if (type(aListLength)!=int) or (aListLength<=0) :
        empty_list = []
        return empty_list#checked

    #ここに来る⇔{aListLengthはint型}{aListLength>=1}
    free_length_list_by_common_value=[]

    for i in range(aListLength):
        free_length_list_by_common_value.append(aCommonValue)

    return free_length_list_by_common_value

#部分列を得る。checked2024_01_31
def TakeSubList(aList,aTakeStart,aTakeLength):
    list_length = GetListLength(aList)

    can_take_flag = ValueOperater.ExistOneDimAreaInList(list_length,aTakeStart,aTakeLength)

    if not can_take_flag:
        empty_list=[]
        return empty_list#checked

    #ここに来る⇔aListから抜き出せる

    sub_list = MakeFreeLengthListByCommonValue(aTakeLength,0)

    for i in range(aTakeLength):
        sub_list[i] = aList[aTakeStart+i]

    return sub_list

#リストでサーチを行い、ヒットしたインデックスのリストを返す.checked2024_01_31
def MakeHitIndexListInList(aList,aSearchValue):
    list_length = GetListLength(aList)

    if list_length==0:
        empty_list=[]
        return empty_list#check

    hit_index_list=[]#最初は空列で、追加していく

    for index in range(list_length):
        if aList[index]==aSearchValue:
            hit_index_list.append(index)

    return hit_index_list

#一次元配列同士のコピー.checked2024_01_31
def CopyFromReadListIntoWriteList(aReadList,aReadStart,aWriteList,aWriteStart,aCopyLength):
    #リードの側の判定
    read_list_length = GetListLength(aReadList)
    read_flag = ValueOperater.ExistOneDimAreaInList(read_list_length,aReadStart,aCopyLength)

    #ライトの側の判定
    write_list_length = GetListLength(aWriteList)
    write_flag = ValueOperater.ExistOneDimAreaInList(write_list_length, aWriteStart, aCopyLength)

    #コピーの可否の判定
    can_copy_flag = (read_flag and write_flag)

    if not can_copy_flag:
        return

    #ここに来る⇔コピーが可能
    for index in range(aCopyLength):
        aWriteList[aWriteStart+index]=aReadList[aReadStart+index]

    return


