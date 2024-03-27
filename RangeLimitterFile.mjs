//別ファイルで使いたいなら必ずexport
export class RangeLimitter
{
    //javascriptでは、メンバ変数は、コンストラクタ内部で、this.をつけて初期化する
    //コンストラクタより上の箇所では、メンバ変数は宣言しない.checked2024_01_19
    constructor(aRangeValue1,aRangeValue2)
    {
        this.mRangeMax = 0
        this.mRangeMin = 0
        this.mInsideValue=0

        if((typeof(aRangeValue1)!='number') || (typeof(aRangeValue2)!='number'))
        {
            return//checked
        }

        this.mRangeMax = (aRangeValue1>=aRangeValue2?aRangeValue1:aRangeValue2);
        this.mRangeMin = (aRangeValue1>=aRangeValue2?aRangeValue2:aRangeValue1);
        this.mInsideValue=this.mRangeMin
        return;

    }

    //メンバ関数の実装の箇所でfunctionは不要
    GetMyRangeMax()
    {
        return this.mRangeMax;
    }

    GetMyRangeMin()
    {
        return this.mRangeMin;
    }

    GetMyInsideValue()
    {
        return this.mInsideValue;
    }

    //レンジの最大より本当に小さい時に限り、増やす.checked
    IncrementMyInsideValueIfCan()
    {
        if( this.mInsideValue < this.mRangeMax) //値を増やせる
        {
            this.mInsideValue += 1 //checked
        }
    }

    //レンジの最小より本当に大きいときに限り、減らす.checked
    DecrementMyInsideValueIfCan()
    {
        if(this.mInsideValue > this.mRangeMin)
        {
            this.mInsideValue -= 1 //checked
        }
    }
}//class

