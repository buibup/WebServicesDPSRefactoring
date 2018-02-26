using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test_WS_MSO
{
    class Program
    {
        static void Main(string[] args)
        {
            WS_MSO.Service1 service1 = new WS_MSO.Service1();
            service1.Url = "http://172.18.41.129/WS_MSO/Service1.asmx";
            
//<<<<<<< .mine
            int i = service1.WS_InsertMOC(5,"01421","51",2,"ทดสอบสร้างcourse",223,"THAILAND"
                            ,"Bangkok Hospital Medical Center","2016-09-22"
                            ,"2016-09-25","","A",4,"http://10.88.17.41:99/Certificate/ทดสอบสร้างcourse/2016/CHADSRI PRACHUABMOH_3616.pdf");
//=======
            int iii = service1.WS_InsertMOC(5,"501418","709",2,"test_MOC",223,"THAILAND"
                            , "Bangkok Hospital Medical Center", "2016-10-06"
                            , "2016-10-08", "", "A", 1, " http://10.88.17.41:99/Certificate/test_MOC/2016/KOCHAKRITWETTAWONG_3621.pdf");
//>>>>>>> .r5307
            int ii = service1.WS_InsertMOC2(5, "01421", "51", 2, "ทดสอบสร้างcourse", 223, "THAILAND"
                            , "Bangkok Hospital Medical Center", "2016-09-22"
                            , "2016-09-25", "", "A", 4, "http://10.88.17.41:99/Certificate/ทดสอบสร้างcourse/2016/CHADSRI PRACHUABMOH_3616.pdf");
            Console.WriteLine(i);
            
       }
    }
}
