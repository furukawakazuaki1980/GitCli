<?php
require_once("mysql_string.php");
require_once("array_operate.php");
class ManageMySQL
{
    private MySQLi $mMySQLi;
    private string $mTableName;
    private array $mFiledNameArray;

    //$aFiledNameArrayは、先頭が主キーという決まりを守る
    function __construct(string $aTableName, array $aFiledNameArray)//checked.2021_11_08 
    {
        print "constructor_happen"."<BR>";
        //メンバの初期化
        $this->mTableName ="";//空列
        $this->mFiledNameArray = [];//サイズ0の配列
        //テーブル名の引数は非空であることを要求
        if(strlen($aTableName)===0)
        {
            return;//checked
        }
        $this->mTableName = $aTableName;//メンバに入れる

        //フィールド名の配列は、非空で要素が全て文字列型である事を要求
        if(!IsOKDataTypeAllArrayElements($aFiledNameArray, STRING_TYPE))
        {
            return;//checked
        }
        $this->mFiledNameArray =  MakeNotEmptyUnique($aFiledNameArray);//重複を排除してある事を確認

        $this->mMySQLi = new MySQLi(DBSERVER, DBUSER, DBPASSWORD, DBNAME);
        ($this->mMySQLi)->query(MakeSetCharCodeStr(CHARCODE));//文字コードのセット     

     }

     //自分自身のフィールド名の配列を読みだす
     public function GetFieldNameArray():array
     {
         return $this->mFiledNameArray;
     }

     //引数でインデックスを指定して、一個のフィールド名を取得する
     public function GetOneFieldName(int $aIndex):string
     {
         //メンバのフィールド名の配列をチェックする
         $length_this_fieldname_array = count($this->mFiledNameArray);
         if($length_this_fieldname_array===0)
         {
             return "";//checked
         }

         //インデックスのチェック
         if(($aIndex<0) || ($length_this_fieldname_array<=$aIndex))
         {
             return "";
         }

         //ここで$this->mFiledNameArrayは非空(そしてコンストラクタの措置により要素は全て文字列)
         //かつ$aIndexは$this->mFiledNameArrayに対して正しい
         return $this->mFiledNameArray[$aIndex];


     }



     //全てのフィールドから読むためのselect文。checked2021_11_08
     private function MakeSelectStrAllField()
     {
        $select_str_all_field = MakeSelectStr(ALL_FIELD_STR,$this->mTableName);
        return $select_str_all_field;
     }

     //全てのフィールドから読むための条件付きselect文。checked2021_11_08
     private function MakeSelectStrAllFieldWithCondition($aConditionStr)
     {
        $select_str_all_field = MakeSelectStrWithCondition(ALL_FIELD_STR,$this->mTableName,$aConditionStr);
        return $select_str_all_field;
     }

     //これで、条件なしで、テーブルのデータが正しく読みだせていた。
     public function GetResultArrayBySelect()
     {
        $select_str = $this->MakeSelectStrAllField();
        $result_array  = ($this->mMySQLi)->query($select_str);
        return $result_array; 
     }

     //条件付きのselect文でテーブルのデータを読みだす.checked2021_11_08
     public function GetResultArrayBySelectWithCondition($aConditionStr)
     {
        $select_str_with_condition = $this->MakeSelectStrAllFieldWithCondition($aConditionStr);
        if($select_str_with_condition==="")
        {
            return null;//checked
        }

        $result_array_with_condition = ($this->mMySQLi)->query($select_str_with_condition );
        return $result_array_with_condition;
     }

     
     //第一引数の配列から、重複を排除して、並び替えた時、$aLimitの制限に従っているか.
     private static function MakeSortedUniqueArrayLimit(array $aIndexArray, int $aLimit):array//private
     {
         $empty_array = [];
         if($aLimit<=1)
         {
             return $empty_array;//checked
         }
      
         $sorted_unique_array = MakeUniqueSortedArray($aIndexArray);
         $length_sorted_unique = count($sorted_unique_array);

         if($length_sorted_unique===0)
         {
             return $empty_array;//checked
         }

         //$sorted_unique_arrayの最小値(=先頭の値)と最大値(=末尾の値)の判定
         if($sorted_unique_array[$length_sorted_unique-1]>=$aLimit)
         {
             return $empty_array;//checked
         }
        return $sorted_unique_array;
     }
     
     //オリジナルの配列から部分列を取り出す.checked2021_11_08
     private static function GetSubArrayFromOriginal(array $aIndexArray,array $aOriginalArray):array//private
     {
         $length_original = count($aOriginalArray);
        $sorted_unique_array = ManageMySQL::MakeSortedUniqueArrayLimit($aIndexArray, $length_original);
        //一意にしてソートした配列のチェック
        $length_unique = count($sorted_unique_array);
        if($length_unique===0)
        {
            $empty_array = [];
            return $empty_array;//checked
        }

        //1<=$length_uniqueかつ1<=$length_originalかつ$sorted_unique_array[$length_unique-1]<=$length_original-1

        $subarray = array_fill(0,$length_unique,0);
        for($i=0;$i<$length_unique;$i++)
        {
            $subarray[$i]= $aOriginalArray[$sorted_unique_array[$i]];
        }

        return $subarray;
    }

    //メンバのフィールド名の配列から部分列を取り出して、それを文字列で繋げてinsert文を作る.checked2021_11_08
    private function MakeInsertStrByMember(array $aIndexArray, array $aValuesStringArray):string//private
    {
        $sub_fieldname_array = ManageMySQL::GetSubArrayFromOriginal($aIndexArray,$this->mFiledNameArray);
        
        if(count($sub_fieldname_array)===0)
        {
            return "";//checked
        }
        
        $insert_str = MakeInsertStr($this->mTableName,$sub_fieldname_array,$aValuesStringArray);
        return $insert_str;
    }

    //データベースへの挿入を実行.checked2021_11_08
    public function ExecuteInsert(array $aIndexArray, array $aValuesStringArray):bool
    {
        $insert_str = $this->MakeInsertStrByMember($aIndexArray, $aValuesStringArray);
        if(strlen($insert_str)===0)
        {
           return false;
        }
       
        $insert_success_flag= ($this->mMySQLi)->query($insert_str);
        return $insert_success_flag;
    }

    //Delete実行.checked2021_11_08
    public function ExecuteDelete($aConditionStr):bool
    {
        $delete_str_by_member = MakeDeleteStr($this->mTableName,$aConditionStr);
        if(strlen($delete_str_by_member)===0)
        {
            return false;//checked
        }
        $delete_success_flag = ($this->mMySQLi)->query($delete_str_by_member);

        return $delete_success_flag;
    }



     function __destruct()
    {
        print "destructor_happen"."<BR>";
    }
}
/*
$str1 = "abcdf";

$str2 = "bcdefg";

$compare_value = strcmp("","a");
*/


/*
$fieldname_array = array('bango','groupname','personalname','shincho', 'taiju');
$tablename = "furukawatable";
$mysql_instance = new ManageMySQL($tablename, $fieldname_array);
$one_fieldname = $mysql_instance->GetOneFieldName(0);
*/
/*
$index_array = array(1,4,1,4,0,0,2,3,1,2);
$values_array = array((string)25,"'C'", "'C8'", (string)179, (string)66);
//$sub_values_str_array = array("'D'","'D1'",(string)59);//この作り方で正常な挿入に成功

$delete_str = $mysql_instance->ExecuteDelete("bango = 99");
*/


//$k=0;








  
?>