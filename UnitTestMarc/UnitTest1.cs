using common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace UnitTestMarc
{

        [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void TestMethod1()
            {
                string marc = @"01815nam0 2200313   450_
001007295699
00520150713164754.0
010  ǂa978-7-101-10528-5ǂb精装ǂdCNY156.00
049  ǂaA120000TJLǂbUCS01007295699ǂc007295699
100  ǂa20150209d2015    em y0chiy50      ea
1010 ǂachi
102  ǂaCNǂb110000
105  ǂay   z   000gy
106  ar
200  ǂa测试题名1
2001 ǂa杜诗详注ǂ9du shi xiang zhuǂb专著ǂf(唐)杜甫撰ǂg(清)仇兆鳌注
210  ǂa北京ǂc中华书局ǂd2015
215  ǂa3册(50,23,1942页)ǂd22cm
300  ǂa中华国学文库 第四辑
330  ǂa本书精选了杜甫经典诗作，并对每首诗进行了注释。其中包括《游龙门奉先寺》、《房兵曹胡马》、《画鹰》、《杜位宅守岁》、《丽人行》、《九日曲江》、《冬日洛城北谒玄元皇帝庙》等作品。
6060 ǂa杜诗ǂx注释
690  ǂaI222.742ǂv5
701 0ǂc(唐)ǂa杜甫ǂ9du fuǂf(712-770)ǂ4撰
702 0ǂc(清)ǂa仇兆鳌ǂ9qiu zhao aoǂf(1638-1717)ǂ4注
801 0ǂaCNǂbTJLǂc20150415
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML.jpgǂqimage/jpegǂxtype:FrontCover.LargeImage;size:402X500px;source:Amazon:B00U71HW80@webservices.amazon.cn
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML._SL160_.jpgǂqimage/jpegǂxtype:FrontCover.MediumImage;size:129X160px;source:Amazon:B00U71HW80@webservices.amazon.cn
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML._SL75_.jpgǂqimage/jpegǂxtype:FrontCover.SmallImage;size:60X75px;source:Amazon:B00U71HW80@webservices.amazon.cn
997  ǂa9787101105285|杜诗详注|杜甫|中华书局,2015|ǂh1a6dcd99ac886a6fad728978b170de00ǂv0.04
998  ǂu2017-10-28 13:39:23ZǂzW任延华ǂa20171028
***";
                string fieldMap1 = @"ISBN|010$a
题名|200$a
第一作者|200$f
个人主要作者|701$a";

                string fields1 = MarcHelper.GetFields(marc, fieldMap1);

                string result1 = @"ISBN|010$a|978-7-101-10528-5
题名|200$a|测试题名1
第一作者|200$f|(唐)杜甫撰
个人主要作者|701$a|杜甫";

                Assert.AreEqual(result1, fields1);



            }



            [TestMethod]
            public void TestMethod2()
            {
                string marc = @"01815nam0 2200313   450_
001007295699
00520150713164754.0
010  ǂa978-7-101-10528-5ǂb精装ǂdCNY156.00
049  ǂaA120000TJLǂbUCS01007295699ǂc007295699
100  ǂa20150209d2015    em y0chiy50      ea
1010 ǂachi
102  ǂaCNǂb110000
105  ǂay   z   000gy
106  ar
200  ǂa测试题名1
2001 ǂa杜诗详注ǂ9du shi xiang zhuǂb专著ǂf(唐)杜甫撰ǂg(清)仇兆鳌注
210  ǂa北京ǂc中华书局ǂd2015
215  ǂa3册(50,23,1942页)ǂd22cm
300  ǂa中华国学文库 第四辑
330  ǂa本书精选了杜甫经典诗作，并对每首诗进行了注释。其中包括《游龙门奉先寺》、《房兵曹胡马》、《画鹰》、《杜位宅守岁》、《丽人行》、《九日曲江》、《冬日洛城北谒玄元皇帝庙》等作品。
6060 ǂa杜诗ǂx注释
690  ǂaI222.742ǂv5
701 0ǂc(唐)ǂa杜甫ǂ9du fuǂf(712-770)ǂ4撰
702 0ǂc(清)ǂa仇兆鳌ǂ9qiu zhao aoǂf(1638-1717)ǂ4注
801 0ǂaCNǂbTJLǂc20150415
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML.jpgǂqimage/jpegǂxtype:FrontCover.LargeImage;size:402X500px;source:Amazon:B00U71HW80@webservices.amazon.cn
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML._SL160_.jpgǂqimage/jpegǂxtype:FrontCover.MediumImage;size:129X160px;source:Amazon:B00U71HW80@webservices.amazon.cn
8564 ǂ3Cover imageǂuhttps://images-cn-4.ssl-images-amazon.com/images/I/41MpXsU09ML._SL75_.jpgǂqimage/jpegǂxtype:FrontCover.SmallImage;size:60X75px;source:Amazon:B00U71HW80@webservices.amazon.cn
997  ǂa9787101105285|杜诗详注|杜甫|中华书局,2015|ǂh1a6dcd99ac886a6fad728978b170de00ǂv0.04
998  ǂu2017-10-28 13:39:23ZǂzW任延华ǂa20171028
***";

                //
                string fieldMap2 = @"ISBN|010$a|123
题名|200$a|中国人
第一作者|200$f|测试
个人主要作者|701$a|测试2";
                string marc2 = MarcHelper.SetFields(marc, fieldMap2);

                string fieldMap3 = @"ISBN|010$a|123
题名|200$a|中国人
第一作者|200$f|测试
个人主要作者|701$a|测试2";
                string fields3 = MarcHelper.GetFields(marc2, fieldMap3);
                Assert.AreEqual(fieldMap2, fields3);

            }
        }

}
