using System;
using System.Collections.Generic;
using System.IO;
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

        public static DocTable GetLatestDocument(int docType, int id)
        {
            int latestDocId = databaseContext.DocumentHistories.OrderByDescending(a => a.Date).Select(a => a.DocumentID).First();

            var query = from d in databaseContext.Documents.Where(dd => dd.id == latestDocId)
                        let p = databaseContext.DocumentHistories.Where(pp => pp.DocumentID == d.id).OrderBy(a => a.Date).FirstOrDefault()
                        select new DocTable { id = latestDocId, name = d.Name, date = p.Date, username = p.UserName };
            DocTable doc = query.First();

            if (id != -1)
            {
                doc.isConnected = databaseContext.PointDocConnections.Where(a => a.DocumentID == latestDocId && a.PointID == id).Any();
            }
            return doc;
        }


        static int GetLatestDocument()
        {            
            return databaseContext.Documents.OrderByDescending(a=>a.id).Select(b=>b.id).First();
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

        public static DocTable GetLatestHistory(int docID)
        {
            return databaseContext.DocumentHistories
                .Where(a => a.DocumentID == docID)
                .OrderByDescending(b => b.Date)
                .Select(c => new DocTable { id = c.DocumentID, date = c.Date, username = c.UserName }).First();

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
        //------------------------------------CHANGING ENTRIES-------------------------------------------------
        //==============DOCUMENT HISTORY==============
        public static void CreateDocumentHistrory(int id, string filePath)
        {
            string urName = System.Environment.UserName;
            byte[] file = File.ReadAllBytes(filePath);
            DocumentHistory entry;
            entry = new DocumentHistory(id, file, DateTime.Now, urName);

            databaseContext.DocumentHistories.Add(entry);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        public static void DeleteDocumentHistory(HashSet<int> ids)
        {
            List<DocumentHistory> documentHistories = databaseContext.DocumentHistories.Where(a => ids.Contains(a.id)).ToList();
            for(int i = 0; i < documentHistories.Count; i++)
            {
                databaseContext.Entry(documentHistories[i]).State = System.Data.Entity.EntityState.Deleted;
            }

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        //==============DOCUMENT==============
        public static void CreateDocument(string filePath, int type, string name)
        {
            Document doc = new Document(name, type);

            databaseContext.Documents.Add(doc);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();

            int latstDocID = GetLatestDocument();

            CreateDocumentHistrory(latstDocID, filePath);
        }

        public static void ChangeDocument(int id,string name, int docType)
        {
            Document doc = databaseContext.Documents.Where(a => a.id == id).First();
            databaseContext.Entry(doc).State = System.Data.Entity.EntityState.Modified;
            if (name != doc.Name)
            {
                doc.Name = name;
            }
            else
            {
                databaseContext.Entry(doc).Property(x => x.Name).IsModified = false;
            }

            if (doc.DocType != docType)
            {
                doc.DocType = docType;
            }
            else
            {
                databaseContext.Entry(doc).Property(x => x.DocType).IsModified = false;
            }
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        public static void DeleteDocument(int id)
        {
            List<PointDocConnection> connections = databaseContext.PointDocConnections.Where(a => a.DocumentID == id).ToList();

            for (int i = 0; i < connections.Count; i++)
            {
                databaseContext.Entry(connections[i]).State = System.Data.Entity.EntityState.Deleted;
            }

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        //==============POINT TYPE==============
        public static void CreateNewPointType(string name)
        {
            PointType type = new PointType(name);

            databaseContext.PointTypes.Add(type);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }
        public static void ChangePointType(int id,string name)
        {
            PointType type = databaseContext.PointTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Modified;
            type.Name = name;

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        public static void DeletePointType(int id)
        {
            PointType type = databaseContext.PointTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        //==============DOC TYPE==============
        public static void CreateNewDocType(string name)
        {
            DocumentType type = new DocumentType(name);

            databaseContext.DocumentTypes.Add(type);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }
        public static void ChangeDocType(int id,string name)
        {
            DocumentType type = databaseContext.DocumentTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Modified;
            type.Name = name;

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        public static void DeleteDocType(int id)
        {
            DocumentType type = databaseContext.DocumentTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        //==============POINT DOC CONNECTION==============
        public static void CreatePointDocConnection(int pointID, int docID)
        {
            PointDocConnection connection = new PointDocConnection(pointID, docID);
            databaseContext.PointDocConnections.Add(connection);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }
        public static void DeletePointDocConnection(int id)
        {
            PointDocConnection connection = databaseContext.PointDocConnections.Where(a=>a.id == id).First();
            databaseContext.Entry(connection).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        //==============POINT==============
        public static void CreatePoint(string name, int category)
        {
            Point point = new Point(name, category);
            PointType t = databaseContext.PointTypes.Where(a => a.id == category).First();
            point.PointType = t;

            databaseContext.Points.Add(point);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }
        public static void ChangePoint(int id, string name, int category)
        {
            Point point = databaseContext.Points.Where(a => a.id == id).First();
            databaseContext.Entry(point).State = System.Data.Entity.EntityState.Modified;
            if (point.Name != name)
            {
                point.Name = name;
            }
            else
            {
                databaseContext.Entry(point).Property(x => x.Name).IsModified = false;
            }
            if (point.CategoryID != category)
            {
                point.CategoryID = category;
            }
            else
            {
                databaseContext.Entry(point).Property(x => x.CategoryID).IsModified = false;
            }

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }

        public static void DeletePoint(int id)
        {
            Point point = databaseContext.Points.OrderByDescending(a => a.id).First();
            databaseContext.Entry(point).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();

        }

        static void ShowErrorMessage()
        {

            System.Windows.MessageBoxResult errorMessage = System.Windows.MessageBox.Show("Ошибка подключения к базе данных.\n" +
                "Обратитесь к администратору.", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
