<?php
  require_once "BaseCSVClass.php";
  define("READ_MODE_STR", "r");
  class ReadCSV extends BaseCSV
  {
     private $mFilePointerForRead;  
     function __construct(string $aSetName) 
     {       
        
         parent::__construct($aSetName);  
         print "ReadCSV_constructor_happen"."<BR>";

         //リード用のファイルポインタを決める
         $this->mFilePointerForRead=null;   
         if($this->mFileName!="")//親クラスのファイル名がnullでない⇔ファイルが存在する
         {
            $this->mFilePointerForRead = fopen($this->mFileName,READ_MODE_STR);
         }
     }

     function ReadOneRow()
     {
        if(!$this->mFilePointerForRead)//元々ファイルのオープンに失敗している場合
        {
            return null;
        }
        $read_one_row = fgetcsv($this->mFilePointerForRead);
        if(!$read_one_row)//読み取りに失敗した場合
        {
            return null;
        }
        return $read_one_row;//読み取りに成功した場合は配列
     }

     function CloseForRead()
     {
        if(!$this->mFilePointerForRead)//元々ファイルのオープンに失敗している場合
        {
            return;
        }

        fclose($this->mFilePointerForRead);
        return;
     }
     function __destruct()
     {
        parent::__destruct();
        print "ReadCSV_destructor_happen"."<BR>";
     }
}

//正常にcsvファイルから読み込めた
// $filename = "csvfile2.csv";
// $read_csv_instance = new ReadCSV($filename);
// print $read_csv_instance->GetFileName();
// while(true)
// {
//     $read_one_row = $read_csv_instance->ReadOneRow();
//     if(is_null($read_one_row))
//     {
//         break;
//     }
//     var_dump($read_one_row);

//     $dummy =0;

// }
// $read_csv_instance->CloseForRead();
?>