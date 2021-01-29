using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointDocuments
{
    public static class DatabaseHandler
    {
        private static PointDocumentationEntities databaseContext;

        private static List<string> points;
        private static List<DocumentType> types;
        public static void Initialize()
        {
            if (databaseContext == null)
            {
                databaseContext = new PointDocumentationEntities();
            }
        }

        public static List<string> GetPointsList()
        {
            if (points == null || points.Count == 0)
            {
                points = databaseContext.Points.Select(a => a.Name).ToList();
            }

            return points;
        }

        public static List<DocumentType> GetDocumentTypes()
        {
            if (types == null || types.Count == 0)
            {
                types = databaseContext.DocumentTypes.ToList();
            }
            return types;
        }

        public static int GetIdOfDocumentType(int docID)
        {
            return databaseContext.Documents.Where(a => a.id == docID).Select(b => b.DocType).First();
        }

        public static Point GetPoint(int id)
        {
            return databaseContext.Points.Where(a => a.id == id).First();
        }

        public static int GetPointId(int index)
        {
            string pointName = points[index];
            return databaseContext.Points.Where(a => a.Name == pointName).Select(a => a.id).First();
        }

        public static string GetPointCategory(int id)
        {
            return databaseContext.PointTypes.Where(a => a.id == id).Select(a => a.Name).First();
        }

        public static HashSet<int> GetDocumentsID(int id)
        {
            return databaseContext.PointDocConnections.Where(a => a.PointID == id).Select(a => a.DocumentID).ToHashSet();
        }

        public static List<DocumentType> GetSortedCategories(int id, HashSet<int> documentIDs)
        {
            HashSet<int> categories = databaseContext.Documents.Where(a => documentIDs.Contains(a.id)).Select(a => a.DocType).ToHashSet();
            return databaseContext.DocumentTypes.Where(a => categories.Contains(a.id)).OrderBy(a => a.Name).ToList();

        }
        public static List<DocumentType> GetSortedCategories()
        {
            return databaseContext.DocumentTypes.OrderBy(a => a.Name).ToList();
        }

        public static List<DocTable> GetDocTableDocuments(HashSet<int> documentIDs, int documentTypeID)
        {
            var query = from d in databaseContext.Documents.Where(dd => dd.DocType == documentTypeID && documentIDs.Contains(dd.id))
                        let p = databaseContext.DocumentHistories.Where(pp=>pp.DocumentID == d.id).OrderByDescending(a=>a.Date).FirstOrDefault()
                        select new DocTable { id = d.id, name = d.Name, date = p.Date, username = p.UserName};

            return query.ToList();
        }
        public static List<DocTable> GetDocTableDocuments(int pointID, int documentTypeID)
        {
            var query = from d in databaseContext.Documents.Where(dd => dd.DocType == documentTypeID)
                        let p = databaseContext.DocumentHistories.Where(pp => pp.DocumentID == d.id).OrderByDescending(a => a.Date).FirstOrDefault()
                        let pd = databaseContext.PointDocConnections.Where(pdpd => pdpd.PointID == pointID && pdpd.DocumentID == d.id)
                        select new DocTable { id = d.id, name = d.Name, date = p.Date, username = p.UserName, isConnected = pd.Any() };

            return query.ToList();
        }

        public static List<DocTable> GetDocumentHistory(int docID)
        {
            return databaseContext.DocumentHistories
                .Where(a => a.DocumentID == docID)
                .OrderByDescending(b => b.Date)
                .Select(c => new DocTable {id = c.DocumentID, date = c.Date, username = c.UserName }).ToList();
        }

        public static Tuple<int,string> GetDocument(int docID)
        {
            var result = databaseContext.Documents.Where(a => a.id == docID).Select(b=> new { typeID=b.DocType,name = b.Name }).First();
            return Tuple.Create(result.typeID, result.name);
        }

        public static int GetPointDocumentCount(int docID)
        {
            return databaseContext.PointDocConnections.Where(a => a.DocumentID == docID).Count();
        }

        static void ShowErrorMessage()
        {

            System.Windows.MessageBoxResult errorMessage = System.Windows.MessageBox.Show("Ошибка подключения к базе данных.\n" +
                "Обратитесь к администратору.", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
