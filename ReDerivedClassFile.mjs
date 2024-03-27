import{DerivedClass} from './DerivedClassFile.mjs';

export class ReDerivedClass extends DerivedClass
{
    constructor(aInitialBaseStr,aInitialDerivedValue,aInitialReDerivedFlag)
    {
        //最左と中央の引数で、親クラスのコンストラクタ
        super(aInitialBaseStr,aInitialDerivedValue)

        this.mDerivedFlag = false
        if(typeof(aInitialReDerivedFlag)!='boolean')
        {
            return//checked
        }

        this.mDerivedFlag=aInitialReDerivedFlag
    }

    GetMyreDerivedFlag()
    {
        return this.mDerivedFlag
    }

}

