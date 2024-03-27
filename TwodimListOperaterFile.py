import ListOperater
import ValueOperater


#二次元配列の縦の長さを得る.checked2024_01_31
def GetRowLengthOfTwodimList(aTwodimList):
    row_length = len(aTwodimList)
    return row_length

#二次元配列の横の長さを得るchecked2024_01_31
def GetColLengthOfTwodimList(aTwodimList):
    row_length = GetRowLengthOfTwodimList(aTwodimList)
    if row_length==0:
        return 0#checked

    #ここに来る⇔縦の長さ>=1
    #∴横一行を取得できる

    #横一行を持ってきて、その長さを得る
    col_length = ListOperater.GetListLength(aTwodimList[0])

    return col_length

#共通の値の、指定した縦横の長さの二次元配列を得る.
def MakeFreeLengthTwodimListByCommonValue(aRowLength,aColLength,aCommonValue):
    row_flag = ((type(aRowLength)==int) and (aRowLength>=1))
    col_flag = ((type(aColLength)==int) and (aColLength>=1))

    can_make_twodim_list_flag = (row_flag and col_flag)

    if not can_make_twodim_list_flag:
        empty_twodim_list = [[]]
        return empty_twodim_list#checked

    #ここに来る⇔{縦と横の長さは共に1以上}
    #∴縦と横の長さで二次元配列を作れる

    #この書き方はやってはいけない。aRowLength個のインスタンスが、同一のリストになり、どこかの行の要素に書いた値が、
    #他の行にも反映されるから。
    #twodim_list = [[aCommonValue]*aColLength]*aRowLength


    twodim_list = [[0] * aColLength for i in range(aRowLength)]
    return twodim_list

#二次元のリストのコピー.checked2024_01_31
def CopyTwodimList(aReadTwodimList,aReadStartRow,aReadStartCol,aWriteTwodimList,aWriteStartRow,aWriteStartCol,aCopyRowLength,aCopyColLength):

    #リード側の二次元配列の縦と横のサイズを得る
    read_row_length = GetRowLengthOfTwodimList(aReadTwodimList)
    read_col_length = GetColLengthOfTwodimList(aReadTwodimList)

    #リード側について、リードできるかどうか、判定
    can_read_flag = ValueOperater.ExistSubTwodimAreaInTwodimList(read_row_length,aReadStartRow,aCopyRowLength,read_col_length,aReadStartCol,aCopyColLength)

    #ライト側の二次元配列の縦と横のサイズを得る
    write_row_length = GetRowLengthOfTwodimList(aWriteTwodimList)
    write_col_length = GetColLengthOfTwodimList(aWriteTwodimList)

    #ライト側について、ライトできるかどうか、判定
    can_write_flag = ValueOperater.ExistSubTwodimAreaInTwodimList(write_row_length, aWriteStartRow, aCopyRowLength,write_col_length, aWriteStartCol, aCopyColLength)

    #コピーできるかどうか、判定
    can_copy_flag = (can_read_flag and can_write_flag)

    if not can_copy_flag:
        return#checked

    #ここに来る⇔コピーが可能
    for row_index in range(aCopyRowLength):
        for col_index in range(aCopyColLength):
            aWriteTwodimList[aWriteStartRow+row_index][aWriteStartCol+col_index]=aReadTwodimList[aReadStartRow+row_index][aReadStartCol+col_index]

    return


