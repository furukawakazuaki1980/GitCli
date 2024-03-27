<?php

    require_once "BaseCSVClass.php";
   
    require_once "StringArrayOperaterFile.php";
    require_once "OneDimArrayOperaterFile.php";
    require_once "StringTwodimArrayOperaterFile.php";

    define("OVERWRITE_MODE_STR", "w");
    define("APPENDWRITE_MODE_STR", "a");
    define("OVERWRITE_FLAG", false);
    define("APPENDWRITE_FLAG",true);
    define("SEPARATOR_STR", ",");
    define("LINEFEED_STR","\n");
    

    class WriteCSV extends BaseCSV
    {        
        
        private $mFilePointerForWrite;

        //bool値で上書き又は追記の文字列を得る.checked2021_11_07
        function GetWriteModeStr(bool $aWriteModeFlag):string
        {
            $write_mode_str = OVERWRITE_MODE_STR;//引数がOVERWRITE_FLAGだと思っておく
            if($aWriteModeFlag===APPENDWRITE_FLAG)
            {
                $write_mode_str = APPENDWRITE_MODE_STR;//checked
            }
            return $write_mode_str;
        }

        //ファイル名と、上書き又は追加を選ぶ.checked2021_11_07
        function __construct(string $aSetName,bool $aWriteModeFlag) 
        {       
            print "WriteCSV_constructor_happen"."<BR>";
            parent::__construct($aSetName);//親クラスのコンストラクタでprotectedメンバ$this->mFileNameが決まる  
            $this->mFilePointerForWrite=null;//リード用のファイルポインタはnullで初期化


            if($this->mFileName==="")//親クラスのファイル名が空列⇔ファイルが存在しない
            {
                return;//checked
            }

            $write_mode_str = $this->GetWriteModeStr($aWriteModeFlag);
            $this->mFilePointerForWrite = fopen($this->mFileName,$write_mode_str);
        }

        //文字列を書き込む.コンストラクタの引数の設定に応じて上書きと追加を確認checked2021_11_07
        private function WriteStringOneLine(string $aWriteString)//本当はprivate
        {
            //下の繰り返し関数でやるので不要
            //if($this->mFilePointerForWrite===null)
            //{
                //return;//check
            //}

            if(strlen($aWriteString)===0)
            {
                return;//check
            }

            fwrite($this->mFilePointerForWrite, $aWriteString.LINEFEED_STR);//上書きを確認
        }

        //一つの要素が横一行分の配列を、繰り返し書き込んでいく
        public function RepeatWritingLinesByStrArray(&$aLineStrArray)
        {
            if($this->mFilePointerForWrite===null)
            {
                return;//ファイルのopenに失敗したなら何もしないで終了
            }

            $line_str_array_length = GetOnedimArrayCount($aLineStrArray);

            if($line_str_array_length ===0)
            {
                return;//checked
            }

            //ここに来る⇔{ファイルのオープン成功}かつ{繰り返し回数は1以上}
            //書き込みを繰り返す
            for($index=0;$index<= $line_str_array_length-1;$index++)
            {
                $this->WriteStringOneLine($aLineStrArray[$index]);

            }
            return;
        }

        //String型の二次元配列を書き込む。内部で、横一行を連結する処理をする.checked2024_01_17
        public function WriteTwodimStrArray(&$aTwodimStrArray,$aCommonDivideStr)
        {
            //二次元配列の縦と横のサイズを得る
            $ref_row_length=0;
            $ref_col_length=0;
            SetRowColCountAboutTwodimArray($aTwodimStrArray,$ref_row_length,$ref_col_length);

            if($ref_row_length===0 or $ref_col_length===0)
            {
                return;//checked
            }
            //ここに来る⇔二次元配列は非空
            //∴二次元配列に対して、横一行を連結した一次元配列を作れる
            $horizontal_connected_str_array = ConvertTwodimStrArrayIntoHorizontalConnectedStrArray($aTwodimStrArray,$aCommonDivideStr);

            //横一行を連結した配列を書き込む
            $this->RepeatWritingLinesByStrArray($horizontal_connected_str_array);

            return;

        }
       

        //ファイルをクローズ
        public function CloseForWrite()
        {
            if(!$this->mFilePointerForWrite)//ファイルのオープンに失敗している場合
            {
                return;//checked
            }
            fclose($this->mFilePointerForWrite);//checked
            return;

        }


        //メッセージを確認した。checked2021_11_07
        function __destruct()
        {
           parent::__destruct();
           print "WriteCSV_destructor_happen"."<BR>";
        }

    }

    //二次元配列を作成→内部で一次元配列に変換→CSVファイルに書き込む
    //という流れに成功
    // //string型の二次元配列を作る
    // $row_length = 5;
    // $col_length = 6;
    // $twodim_str_array = MakeFreeRowColLengthTwodimArrayByCommonValue($row_length,$col_length,"");
    // for($row=0;$row<=$row_length-1;$row++)
    // {
    //     for($col=0;$col<=$col_length-1;$col++)
    //     {
    //         $twodim_str_array[$row][$col]=$row."_".$col;
    //     }
    // }

  
    // //string型の配列を使った書き込みに成功.
    // $filename = "csvfile.csv";
    // $write_csv_instance = new WriteCSV($filename,"APPENDWRITE_MODE_STR");
    // $test_filename = $write_csv_instance->GetFileName();

    // $write_csv_instance->WriteTwodimStrArray($twodim_str_array,"#");

    // $write_csv_instance->CloseForWrite();

    // return;
    
?>