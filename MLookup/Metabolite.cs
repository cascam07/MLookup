using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLookup
{
    public class Metabolite
    {
        public string HMDB { get; set; }
        public string Pubchem { get; set; }
        public string CAS { get; set; }
        public string KEGG { get; set; }
        public string ChEBI { get; set; }
        public string InChI { get; set; }
        public string InChIKey { get; set; }
        public string CommonName { get; set; }
        public string Synonyms { get; set; }
        public string SuperClass { get; set; }
        public string Class { get; set; }
        public string DirectParent { get; set; }
        public string Pathways { get; set; }
        
        public List<string> AllNames
        {
            get
            {
                if (_AllNames == null)
                {
                    var allnames = Synonyms.Split(';').ToList();
                    allnames.Add(CommonName);
                    _AllNames = allnames;
                }
                return _AllNames;
            }
        }

        private List<string> _AllNames { get; set; }

        public Metabolite()
        {
            
        }

        public Metabolite(string hmdb, string pubchem, string cas, string kegg, string chebi, string inchi, string inchikey, string name,
            string synonyms, string superclass, string _class, string parent, string pathways)
        {
            HMDB = hmdb;
            Pubchem = pubchem;
            CAS = cas;
            KEGG = kegg;
            ChEBI = chebi;
            InChI = inchi;
            InChIKey = inchikey;
            CommonName = name;
            Synonyms = synonyms;
            SuperClass = superclass;
            Class = _class;
            DirectParent = parent;
            Pathways = pathways;


        }

        public override string ToString()
        {
            return CommonName;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Metabolite) obj);
        }

        protected bool Equals(Metabolite obj)
        {
            if (obj.HMDB != null && this.HMDB != null) return this.HMDB.Equals(obj.HMDB);
            return false;
        }
    }
}
