//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PointDocuments
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentHistory
    {
        public int id { get; set; }
        public int DocumentID { get; set; }
        public byte[] DocumentBinary { get; set; }
        public System.DateTime Date { get; set; }
        public string UserName { get; set; }
    
        public virtual Document Document { get; set; }
    }
}
