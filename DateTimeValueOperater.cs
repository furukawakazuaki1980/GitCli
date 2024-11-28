using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STATIC_CSHARP_PROJECT
{
    class DateTimeValueOperater
    {
        public const uint ONE_MINUTE_SECOND = 60;
        public const uint ONE_HOUR_MINUTE = 60;
        public const uint ONE_HOUR_SECOND = ONE_HOUR_MINUTE * ONE_MINUTE_SECOND;
        public const uint ONE_DAY_HOUR = 24;
        public const uint ONE_DAY_SECOND = ONE_HOUR_SECOND * ONE_DAY_HOUR;

        public const uint MONTH_MAX = 12;
        public const uint MAX_DAY_COUNT_ONE_MONTH = 31;


        //日と時と分と秒を、トータルの秒に変換
        public static uint GetTotalSecond(uint aDay, uint aHour, uint aMinute, uint aSecond)
        {
            bool is_good_hour_minute_second = (aHour < ONE_DAY_HOUR) && (aMinute < ONE_HOUR_MINUTE) && (aSecond < ONE_MINUTE_SECOND);

            if (!is_good_hour_minute_second)//checked
            {
                return 0;
            }

            uint second_by_day = aDay * ONE_DAY_SECOND;
            uint second_by_hour = aHour * ONE_HOUR_SECOND;
            uint second_by_minute = aMinute * ONE_MINUTE_SECOND;

            uint total_second = second_by_day + second_by_hour + second_by_minute + aSecond;

            return total_second;

        }


        //トータルの秒を、日と時と分と秒に変換
        public static void ConvertTotalSecondIntoDayHourMinuteSecond(uint aTotalSecond, ref uint aRefDay, ref uint aRefHour, ref uint aRefMinute, ref uint aRefSecond)
        {
            //秒にできる部分だけの値
            aRefSecond = (aTotalSecond % ONE_MINUTE_SECOND);

            //分にできる部分だけ、取り出す
            uint total_minute = (uint)(aTotalSecond / ONE_MINUTE_SECOND);
            aRefMinute = total_minute % ONE_HOUR_MINUTE;

            //時にできる部分だけ、取り出す
            uint total_hour = (uint)(aTotalSecond / ONE_HOUR_SECOND);
            aRefHour = total_hour % ONE_DAY_HOUR;

            //日にできる部分だけ、取り出す
            aRefDay = (aTotalSecond / ONE_DAY_SECOND);

            return;

        }


        //うるう年かどうか、判定
        public static bool IsLeapYear(uint aYear)
        {
            if (aYear % 400 == 0)
            {
                return true;
            }

            //ここに来る⇔aYearは400で割り切れない

            if (aYear % 100 == 0)
            {
                //ここに来る⇔{aYearは400で割り切れない}かつ{aYearは100で割り切れる}
                return false;
            }

            //ここに来る⇔{aYearは400で割り切れない}かつ{aYearは100で割り切れない}
            //⇔{aYearは100で割り切れない}

            if (aYear % 4 == 0)
            {   
                //ここに来る⇔{aYearは100で割り切れない}かつ{aYearは4で割り切れない}
                return true;
            }

            //ここに来る⇔{aYearは100で割り切れない}かつ{aYearは4で割り切れない}
            //⇔{aYearは4で割り切れない}

            return false;

        }


        //年と月から、その月の日数を得る
        public static uint GetDayCountByYearAndMonth(uint aYear, uint aMonth)
        {
            bool is_good_about_month = ((1 <= aMonth) && (aMonth <= MONTH_MAX));

            if (!is_good_about_month)
            {
                return 0;
            }

            //ここに来る⇔aMonthの引数は正常
            //∴月毎の日数をreturnできる

            //31日の月かどうか、判定
            bool is_daycount_max = ((aMonth == 1) || (aMonth == 3) || (aMonth == 5) || (aMonth == 7) || (aMonth == 8) || (aMonth == 10) || (aMonth == 12));

            if (is_daycount_max)
            {
                //ここに来る⇔月の日数が31
                return MAX_DAY_COUNT_ONE_MONTH;
            }

            //ここに来る⇔{aMonthの引数は正常}かつ{aMonthの値は、31日に対応する月ではない}

            if (aMonth != 2)
            {
                //ここに来る⇔{aMonthの引数は正常}かつ{aMonthの値は、31日に対応する月ではない}かつ{aMonthは2以外}
                return MAX_DAY_COUNT_ONE_MONTH-1;
            }

            //ここに来る⇔{aMonthの引数は正常}かつ{aMonthの値は、31日に対応する月ではない}かつ{aMonthは2}
            //⇔{aMonthは2}


            //2月なので、一旦28日としておく
            uint day_count_on_2 = 28;

            //うるう年かどうか、調べる
            if (IsLeapYear(aYear))
            {
                //ここに来る⇔{aMonthは2}かつ{aYearはうるう年}
                day_count_on_2 += 1;
            }

            return day_count_on_2;


        }


        //年と月と日が、正しいかどうかを判定
        //年と月から、その月の日数を得て、その日数以下かどうか、判定

        public static bool IsCorrectAboutYearMonthDay(uint aYear, uint aMonth, uint aDay)
        {
            //月の値が正常かどうか、判定
            bool is_correct_about_month = (1 <= aMonth) && (aMonth <= MONTH_MAX);

            //日の値が正常かどうか、判定
            bool is_correct_about_day = (1 <= aDay) && (aDay <= MAX_DAY_COUNT_ONE_MONTH);

            //月と日の値が共に正常かどうか、判定
            bool is_correct_about_month_day = (is_correct_about_month && is_correct_about_day);

            if (!is_correct_about_month_day)
            {
                return false;
            }

            //ここに来る⇔月と日の値が共に正常
            //∴年と月から、その月の日数を計算して、日の値と比較できる

            //年と月から、日数を取得
            uint day_count = GetDayCountByYearAndMonth(aYear, aMonth);

            //引数のaDayが、日数内かどうか、判定
            bool is_correct_about_year_month_day = (aDay <= day_count);

            return is_correct_about_year_month_day;
            
        }

        //年月日時分秒の判定を行う
        public static bool IsCorrectAboutYearMonthDayHourMinuteSecond(uint aYear, uint aMonth, uint aDay, uint aHour, uint aMinute, uint aSecond)
        {
            //引数の時と分と秒についての判定
            bool is_correct_hour_minute_second = ((aHour < ONE_DAY_HOUR) && (aMinute < ONE_HOUR_MINUTE) && (aSecond < ONE_MINUTE_SECOND));


            //年月日の判定
            bool is_correct_year_month_day = IsCorrectAboutYearMonthDay(aYear, aMonth, aDay);


            //年月日時分秒についての判定
            bool is_correct_year_month_day_hour_minute_second = (is_correct_year_month_day && is_correct_hour_minute_second);


            return is_correct_year_month_day_hour_minute_second;
        }

    }
}
