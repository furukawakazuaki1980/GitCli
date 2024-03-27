<?php
    class CountIncrementDecrement
    {
        private  int $mCount;

        public function ResetCount()
        {
            $this->mCount = 0;
        }

        function __construct()
        {
            $this->ResetCount();
        }

        public function IncrementCount()
        {
            $this->mCount++;
        }

        public function DecrementCount():bool
        {
            $decrement_success_flag = false;
            if($this->mCount>=1)//if_checked
            {
                $this->mCount--;
                $decrement_success_flag=true;
            }
            return $decrement_success_flag;
        }

        public function GetCount():int
        {
            return $this->mCount;
        }

        public function ShowCount()
        {
            print "count_is_".(string)$this->mCount."<BR>";

        }
    }

?>

<?php
    $instance =  new CountIncrementDecrement();
    $_POST['hiddenvalue'] =$instance;
?>


<form action="" method="POST">
     <input type="hidden" name='hiddenvalue' value="<?php echo $_POST['hiddenvalue'];?>">
     <input type="submit" name="incrementbutton" value='increment'>
     <input type="submit" name="resetbutton" value = 'reset'>
     <input type="submit" name='decrementbutton' value = 'decrement' >
</form>



