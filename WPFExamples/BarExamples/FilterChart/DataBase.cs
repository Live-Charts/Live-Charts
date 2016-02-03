using System;
using System.Linq;

namespace ChartsTest.BarExamples.FilterChart
{
    public static class DataBase
    {
        private static City[] _cities;
        static DataBase()
        {
            var source =
                "Shanghai|24256800|3826| China$Beijing|21516000|1311| China$Lagos|21324000|18206| Nigeria$Delhi|16787941|11320| India$Karachi|15000000|4253| Pakistan$Istanbul|14160467|2593| Turkey$Tokyo|13297629|6075| Japan$Mumbai|12478447|20680| India$Moscow|12197596|4859| Russia$São Paulo|11895893|7821| Brazil$Shenzhen|10467400|5256| China$Jakarta|10075310|15171| Indonesia$Seoul|10048850|17134| South Korea$Guangzhou|12700800|5414| China$Kinshasa|9735000|8710| Democratic Republic of the Congo$Cairo|9278441|3008| Egypt$Lahore|6318745|3566| Pakistan$Mexico City|8874724|5974| Mexico$Lima|8693387|3253| Peru$London|8538689|5431| United Kingdom$New York City|8491079|10833| United States$Bengaluru|8425970|11876| India$Bangkok|8280925|5279| Thailand$Ho Chi Minh City|8224400|3925| Vietnam$Dongguan|8220207|3329| China$Chongqing|8189800|1496| China$Nanjing|8187828|1737| China$Tehran|8154051|11886| Iran$Shenyang|8106171|626| China$Bogotá|7776845|9052| Colombia$Ningbo|7605689|775| China$Hong Kong|7298600|6608| China$Hanoi|7232700|2176| Vietnam$Baghdad|7180889|1576| Iraq$Changsha|7044118|596| China$Dhaka|6970105|45307| Bangladesh$Wuhan|6886253|5187| China$Tianjin|6859779|3360| China$Hyderabad|6809970|10958| India$Rio de Janeiro|6429923|5357| Brazil$Faisalabad|6418745|29907| Pakistan$Foshan|6151622|3023| China$Zunyi|6127009|199| China$Santiago|5743719|4595| Chile$Riyadh|5676621|4600| Saudi Arabia$Ahmedabad|5570585|11728| India$Singapore|5535000|7697| Singapore$Shantou|5391028|2611| China$Yangon|5214000|8708| Myanmar$Saint Petersburg|5191690|3608| Russia$Chennai|4792949|11238| India$Abidjan|4765000|2249| Ivory Coast$Chengdu|4741929|11263| China$Alexandria|4616625|2007| Egypt$Kolkata|4486679|22355| India$Ankara|4470800|2340| Turkey$Xi'an|4467837|5369| China$Surat|4462002|13666| India$Johannesburg|4434827|2696| South Africa$Dar es Salaam|4364541|2676| Tanzania$Suzhou|4327066|2623| China$Harbin|4280701|2491| China$Giza|4239988|14667| Egypt$Zhengzhou|4122087|4059| China$New Taipei City|3954929|1927| Taiwan$Los Angeles|3884307|3200| United States$Cape Town|3740026|1530| South Africa$Yokohama|3680267|8414| Japan$Busan|3590101|4686| South Korea$Hangzhou|3560391|4889| China$Xiamen|3531347|2078| China$Quanzhou|3520846|3315| China$Berlin|3517424|3944| Germany$Rawalpindi|3510000|27638| Pakistan$Jeddah|3456259|1958| Saudi Arabia$Durban|3442361|1502| South Africa$Hyderabad|3429471|30083| Pakistan$Kabul|3414100|12415| Afghanistan$Casablanca|3359818|17168| Morocco$Hefei|3352076|3998| China$Pyongyang|3255388|1541| North Korea$Madrid|3207247|5294| Spain$Peshawar|3201000|25608| Pakistan$Ekurhuleni|3178470|1609| South Africa$Nairobi|3138369|4829| Kenya$Zhongshan|3121275|1750| China$Pune|3115431|6913| India$Addis Ababa|3103673|5889| Ethiopia$Jaipur|3073350|6337| India$Buenos Aires|3054300|15046| Argentina$Wenzhou|3039439|2559| China";

            var cities = source.Split('$');

            _cities = cities.Select(x =>
            {
                var city = x.Split('|');
                return new City
                {
                    Id = Guid.NewGuid(),
                    Name = city[0],
                    Population = double.Parse(city[1]),
                    Density = double.Parse(city[2]),
                    Country = city[3]
                };
            }).ToArray();
        }
        public static City[] Cities
        {
            get
            {
                return _cities;
            }
        }
    }
}