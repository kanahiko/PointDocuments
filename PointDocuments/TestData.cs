using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointDocuments
{
   /* public class Point
    {
        public int id { get; set; }
        public string name { get; set; }
        public int catId { get; set; }

        public Point(int a,string b, int c)
        {
            id = a;
            name = b;
            catId = c;
        }
    }*/

/*    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public Category(int a, string b)
        {
            id = a;
            name = b;
        }
    }
    public class DocType
    {
        public int id { get; set; }
        public string name { get; set; }
        public DocType(int a, string b)
        {
            id = a;
            name = b;
        }
    }

    public class Doc
    {
        public int id { get; set; }
        public int doctypeid { get; set; }
        public Doc(int a, int b)
        {
            id = a;
            doctypeid = b;
        }
    }*/

/*    public class PointDocConnection
    {
        public int id { get; set; }
        public int docid { get; set; }
        public int pointid { get; set; }
        public PointDocConnection(int a, int b,int c)
        {
            id = a;
            docid = b;
            pointid = c;
        }
    }*/

  /*  public class DocumentHistory
    {
        public int id { get; set; }
        public int docid { get; set; }
        //[DisplayName("Column Name 1")]
        public string name { get; set; }
        public string date { get; set; }
        public string username { get; set; }
        public DocumentHistory(int a, int b, string c,string d, string e)
        {
            id = a;
            docid = b;
            date = c;
            username = d;
            name = e;
        }
    }*/

/*    public static class TestData
    {
        public static Point[] points = new Point[]
        {
            new Point(1,"one",3),new Point(2,"two",4)
        };
        public static Category[] categories = new Category[]
        {
            new Category(3,"one cat"),new Category(4,"two cat")
        };
        public static DocType[] docTypes = new DocType[]
        {
            new DocType(5,"one type"),new DocType(6,"two type")
        };
        public static Doc[] docs = new Doc[]
        {
            new Doc(7,6),new Doc(8,5),new Doc(9,5),new Doc(10,6),new Doc(11,6)
        };
        public static PointDocConnection[] pointDoc = new PointDocConnection[]
        {
            new PointDocConnection(1,7,1),
            new PointDocConnection(2,8,2),
            new PointDocConnection(2,8,1),
            new PointDocConnection(3,9,1),
            new PointDocConnection(4,10,2)
        };
        public static DocumentHistory[] docHistory = new DocumentHistory[]
        {
            new DocumentHistory(1,7,"2021.01.22","A.Moskaleva","Руководство_администратора_ГИС"),
            new DocumentHistory(2,8,"2021.01.21","A.Vasilenko","Таблица с координатами наших камер"),
            new DocumentHistory(2,8,"2021.01.20","A.Vasilenko","Таблица с координатами наших камер"),
            new DocumentHistory(2,8,"2021.01.19","A.Vasilenko","Таблица с координатами наших камер"),
            new DocumentHistory(3,9,"2021.01.22","A.Moskaleva","Проведение приемочных испытаний по сопряжению Системы-112 и МНИС1"),
            new DocumentHistory(3,9,"2021.01.20","A.Moskaleva","Проведение приемочных испытаний по сопряжению Системы-112 и МНИС1"),
            new DocumentHistory(4,10,"2021.01.22","A.Vasilenko","Приложение 1 - форма опросного листа"),
            new DocumentHistory(5,11,"2021.01.22","A.Moskaleva","Перечень станций (отделений, постов)  СМП ноябрь 2019г.")
        };
    }*/
}
