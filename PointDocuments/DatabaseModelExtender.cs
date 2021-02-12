using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointDocuments
{
    public partial class Document
    {
        public Document(string name, int doctype)
        {
            Name = name;
            DocType = doctype;
        }
    }
    public partial class DocumentHistory
    {
        public DocumentHistory()
        {

        }
        public DocumentHistory(int docID, byte[] file, System.DateTime date, string userName, string fileName)
        {
            DocumentID = docID;
            DocumentBinary = file;
            Date = date;
            UserName = userName;
            DocumentFileName = fileName;
        }
    }
    public partial class Point
    {
        public Point(string name, int type)
        {
            Name = name;
            CategoryID = type;
        }
    }
    public partial class PointType
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _Name
        {
            get { return Name; }
            set
            {
                Name = value;
                this.NotifyPropertyChanged("_Name");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public PointType(string name)
        {
            Name = name;
        }
    }
    public partial class DocumentType
    {
        public DocumentType(string name)
        {
            Name = name;
        }
    }

    public partial class PointDocConnection
    {
        public PointDocConnection()
        {
        }

        public PointDocConnection(int pointID, int docID)
        {
            PointID = pointID;
            DocumentID = docID;
        }
    }

}
