using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PointDocuments
{
    public static class DatabaseHandler
    {
        private static PointDocumentationEntities databaseContext;

        private static List<PointTable> points;
        private static List<DocumentType> types;
        private static List<PointType> pointTypes;

        public static UserRole userRole;

        static readonly List<string> tables = new List<string>
        {
            "DocumentHistory","Documents","DocumentType","PointDocConnections","Points","PointTypes"
        };

        public static bool canConnect = true;
        public static bool isTested = false;
        public static void Initialize()
        {
            if (isTested && canConnect && databaseContext == null)
            {
                databaseContext = new PointDocumentationEntities();
                userRole = new UserRole();
                string userName = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
                for (int j = 0; j < tables.Count; j++) {
                    var allPermissions = GetPermissions(userName, tables[j]);
                    userRole.AddPermissions(tables[j], allPermissions);
                    //List<string>
                    /*for (int i = 0; i < allPermissions.Count; i++)
                    {
                       var results = GetPermission(userName,tables[j], allPermissions[i]);
                    }*/
                }
            }
        }

        public static bool CheckCanDeleteDocumentType(int docID)
        {
            return !databaseContext.Documents.Where(a => a.DocType == docID).Any();
        }
        public static bool CheckCanDeletePointType(int typeID)
        {
            return !databaseContext.Points.Where(a => a.CategoryID == typeID).Any();
        }


        static List<string> GetPermissions(string userName, string table)
        {
            var sql = $"USE PointDocumentation; SELECT DISTINCT permission_name FROM fn_my_permissions('{table}', 'OBJECT') ; ";
            var param = new System.Data.SqlClient.SqlParameter("objectName", $"[schema].[{table}]");

            return databaseContext.Database.SqlQuery<string>(sql, param).ToList();
        }

        static List<string> GetPermission(string userName, string table, string permission)
        {
            var sql = $" USE PointDocumentation; SELECT subentity_name FROM fn_my_permissions('{table}', 'OBJECT') WHERE permission_name = '{permission}'; ";
            var param = new System.Data.SqlClient.SqlParameter("objectName", $"[schema].[{table}]");
                
            List<string> result;
            try
            {
                result = databaseContext.Database.SqlQuery<string>(sql).ToList();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                result = new List<string>();
            }

            return result;
            
        }

        public static List<PointTable> GetPointsList(bool refresh = false)
        {
            if (refresh || points == null || points.Count == 0)
            {
                //points = databaseContext.Points.ToList();
                var query = from p in databaseContext.Points.OrderBy(pp => pp.Name)
                            let t = databaseContext.PointTypes.Where(pt => pt.id == p.CategoryID).FirstOrDefault()
                            select new PointTable { id = p.id, name = p.Name, type = t.Name , typeID = t.id};
                if (refresh)
                {
                    points.Clear();
                    points.AddRange(query.ToList());
                }
                else
                {
                    points = query.ToList();
                }
                points.AddIndexes();
            }

            return points;
        }


        public static PointTable GetNewPoint()
        {
            GetPointsList(true);
            return points[points.Count - 1];
        }

        public static List<DocumentType> GetDocumentTypes(bool refresh = false)
        {
            if (refresh || types == null || types.Count == 0)
            {
                if (refresh)
                {
                    types.Clear();
                    types.AddRange(databaseContext.DocumentTypes.ToList());
                }
                else
                {
                    types = databaseContext.DocumentTypes.ToList();
                }
            }
            return types;
        }
        public static TypeTable GetLatestDocumentType()
        {
            GetDocumentTypes(true);
            return types.OrderByDescending(b=>b.id).Select(a => new TypeTable { id = a.id, name = a.Name }).FirstOrDefault();

        }
        public static List<TypeTable> GetDocumentTypesTable()
        {
            GetDocumentTypes();
            return types.Select(a => new TypeTable { id = a.id, name = a.Name }).ToList();
        }

        public static string GetPointType(int id)
        {
            return pointTypes.Where(a=>a.id==id).Select(b=>b.Name).FirstOrDefault();
        }

        public static List<PointType> GetPointTypes(bool refresh = false)
        {
            if (refresh || pointTypes == null || pointTypes.Count == 0)
            {
                if (refresh)
                {
                    pointTypes.Clear();
                    pointTypes.AddRange(databaseContext.PointTypes.ToList());
                }
                else
                {
                    pointTypes = databaseContext.PointTypes.ToList();
                }
            }

            return pointTypes;
        }
        public static List<TypeTable> GetPointTypesTable()
        {
            GetPointTypes();
            return pointTypes.Select(a => new TypeTable { id = a.id, name = a.Name }).ToList();
        }
        public static TypeTable GetLatestPointType()
        {
            GetPointTypes(true);
            return pointTypes.OrderByDescending(b => b.id).Select(a => new TypeTable { id = a.id, name = a.Name }).FirstOrDefault();

        }


        internal static string GetDocumentHistoryFileName(int id)
        {
            return databaseContext.DocumentHistories.Where(a => a.id == id).Select(b => b.DocumentFileName).FirstOrDefault();
        }

        public static int GetIdOfDocumentType(int docID)
        {
            return databaseContext.Documents.Where(a => a.id == docID).Select(b => b.DocType).First();
        }

        public static Point GetPoint(int id)
        {
            return databaseContext.Points.Where(a => a.id == id).First();
        }

        internal static void DownloadDocument(int id, string fileName)
        {
            byte[] file = databaseContext.DocumentHistories.Where(a => a.id == id).Select(b => b.DocumentBinary).FirstOrDefault();

            File.WriteAllBytes(fileName, file);
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
                        select new DocTable { id = d.id, name = d.Name, date = p.Date, username = p.UserName, isConnected = pd.Any()};

            return query.ToList();
        }

        public static List<DocTable> GetDocumentHistory(int docID)
        {
            return databaseContext.DocumentHistories
                .Where(a => a.DocumentID == docID)
                .OrderByDescending(b => b.Date)
                .Select(c => new DocTable { id = c.id, date = c.Date, username = c.UserName }).ToList();
        }

        public static DocTable GetLatestHistory(int docID)
        {
            return databaseContext.DocumentHistories
                .Where(a => a.DocumentID == docID)
                .OrderByDescending(b => b.Date)
                .Select(c => new DocTable { id = c.id, date = c.Date, username = c.UserName }).First();

        }

        public static DateTime GetDocumentDate(int docID)
        {
            return databaseContext.DocumentHistories
                .Where(a => a.DocumentID == docID)
                .OrderByDescending(b => b.Date).Select(c => c.Date).FirstOrDefault();
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
            string userName = System.Environment.UserName;
            byte[] file = File.ReadAllBytes(filePath);
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
            DocumentHistory entry;
            entry = new DocumentHistory(id, file, DateTime.Now, userName, fileName);

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
        public static void RestoreDocumentHistory(int id)
        {
            string userName = System.Environment.UserName;
            DocumentHistory history = databaseContext.DocumentHistories.Where(a => a.id == id).FirstOrDefault();

            DocumentHistory restoredHistory = new DocumentHistory(history.DocumentID, history.DocumentBinary, DateTime.Now, userName, history.UserName);
            databaseContext.DocumentHistories.Add(restoredHistory);
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

            GetPointTypes(true);
        }
        public static void ChangePointType(int id,string name)
        {
            PointType type = databaseContext.PointTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Modified;
            type.Name = name;

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
            GetPointTypes(true);
        }

        public static void DeletePointType(int id)
        {
            PointType type = databaseContext.PointTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
            GetPointTypes(true);
        }

        //==============DOC TYPE==============
        public static void CreateNewDocType(string name)
        {
            DocumentType type = new DocumentType(name);

            databaseContext.DocumentTypes.Add(type);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();

            GetDocumentTypes(true);
        }
        public static void ChangeDocType(int id,string name)
        {
            DocumentType type = databaseContext.DocumentTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Modified;
            type.Name = name;

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
            GetDocumentTypes(true);
        }

        public static void DeleteDocType(int id)
        {
            DocumentType type = databaseContext.DocumentTypes.Where(a => a.id == id).First();
            databaseContext.Entry(type).State = System.Data.Entity.EntityState.Deleted;

            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
            GetDocumentTypes(true);
        }

        //==============POINT DOC CONNECTION==============
        public static void CreatePointDocConnection(int pointID, int docID)
        {
            PointDocConnection connection = new PointDocConnection(pointID, docID);
            databaseContext.PointDocConnections.Add(connection);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
        }
        public static void ChangePointDocConnection(int pointID, HashSet<int> docsID)
        {
            foreach(var docID in docsID)
            {
                PointDocConnection connection = databaseContext.PointDocConnections.Where(a => a.DocumentID == docID && a.PointID == pointID).FirstOrDefault();
                if (connection == null)
                {
                    connection = new PointDocConnection(pointID, docID);
                    databaseContext.PointDocConnections.Add(connection);
                }
                else
                {
                    databaseContext.Entry(connection).State = System.Data.Entity.EntityState.Deleted;
                }

            }
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
            //point.PointType = t;

            databaseContext.Points.Add(point);
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();

            GetPointsList(true);
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
            GetPointsList(true);
        }

        public static void DeletePoint(int id)
        {
            Point point = databaseContext.Points.OrderByDescending(a => a.id).First();
            databaseContext.Entry(point).State = System.Data.Entity.EntityState.Deleted;
            databaseContext.SaveChanges();
            databaseContext = new PointDocumentationEntities();
            GetPointsList(true);

        }

    }
}
