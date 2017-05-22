using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLookup
{
    class Program
    {
        const string HMDB = "HMDB";
        const string Pubchem = "PUBCHEM CID";
        const string CAS = "CAS";
        const string KEGG = "KEGG";
        const string ChEBI = "CHEBI";
        const string InChI = "INCHI";
        const string InChIKey = "INCHIKEY";
        const string CommonName = "COMMON NAME";
        const string SYNONYMS = "SYNONYMS";
        const string SuperClass = "SUPER CLASS";
        const string Class = "CLASS";
        const string DirectParent = "DIRECT PARENT";
        const string Pathways = "PATHWAYS";


        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments.");
                Console.ReadLine();
            }
            else
            {   
                List<Metabolite> metabolites = new List<Metabolite>();
                string path = Environment.CurrentDirectory;
                //Read in metabolite database to match against
                using (StreamReader reader = new StreamReader(@"MetaboliteOntology.txt"))
                {
                    var map = ColumnMap(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var m = new Metabolite();
                        var split = line.Split('\t', '\n');
                        if (map.ContainsKey(HMDB)) m.HMDB = split[map[HMDB]];
                        if (map.ContainsKey(Pubchem)) m.Pubchem = split[map[Pubchem]];
                        if (map.ContainsKey(CAS)) m.CAS = split[map[CAS]];
                        if (map.ContainsKey(KEGG)) m.KEGG = split[map[KEGG]];
                        if (map.ContainsKey(ChEBI)) m.ChEBI = split[map[ChEBI]];
                        if (map.ContainsKey(InChI)) m.InChI = split[map[InChI]];
                        if (map.ContainsKey(InChIKey)) m.InChIKey = split[map[InChIKey]];
                        if (map.ContainsKey(CommonName)) m.CommonName = split[map[CommonName]];
                        if (map.ContainsKey(SYNONYMS)) m.Synonyms = split[map[SYNONYMS]];
                        if (map.ContainsKey(SuperClass)) m.SuperClass = split[map[SuperClass]];
                        if (map.ContainsKey(Class)) m.Class = split[map[Class]];
                        if (map.ContainsKey(DirectParent)) m.DirectParent = split[map[DirectParent]];
                        if (map.ContainsKey(Pathways)) m.Pathways = split[map[Pathways]];

                        metabolites.Add(m);
                    }
                    reader.Close();
                }

                //Read metabolite names to search for
                string queryPath = args[0];
                using (StreamReader reader = new StreamReader(queryPath))
                {
                    StreamWriter writer = new StreamWriter(Path.GetFullPath(queryPath).Replace(".txt","_MetFound.txt"));
                    writer.WriteLine("Query\tHMDB\tPubChem CID\tCAS\tKEGG\tChEBI\tInChI\tInChIKey\tCommon Name\tSynonyms\tSuper Class\tClass\tDirect Parent\tPathways");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var m = line.Split('\t', '\n')[0];
                        /*
                        var matches = new List<Metabolite>();
                        foreach (var metabolite in metabolites)
                        {
                            foreach (var syn in metabolite.Synonyms)
                            {
                                if (syn.ToUpper().Contains(m.ToUpper()))
                                {
                                    matches.Add(metabolite);
                                }
                            }
                        }*/
                        //var matches = (from metabolite in metabolites from syn in metabolite.Synonyms where syn.ToUpper().Contains(m.ToUpper()) select metabolite).ToList();
                        string mstring = m.ToUpper().Replace(" ", "");
                        var matches = (from metabolite in metabolites from syn in metabolite.AllNames where syn.ToUpper().Replace(" ","").Equals(m.ToUpper().Replace(" ","")) select metabolite).Distinct().ToList();
                        if (matches.Any())
                        {
                            foreach (var match in matches)
                            {
                                string synString = match.Synonyms.Aggregate("", (current, syn) => current + (syn + ", "));
                                writer.WriteLine(
                                    String.Format(
                                        "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}",
                                        m, match.HMDB, match.Pubchem, match.CAS, match.KEGG, match.ChEBI, match.InChI, match.InChIKey, match.CommonName, match.Synonyms, match.SuperClass,
                                        match.Class, match.DirectParent, match.Pathways));
                            }
                        }
                        else
                        {
                            writer.WriteLine(String.Format("{0}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}\t{1}",m,"NA"));
                        }
                    }
                    reader.Close();
                    writer.Close();
                }
            }
        }


        static Dictionary<string, int> ColumnMap(String columnString)
        {

            var columnMap = new Dictionary<string, int>();
			string[] columnTitles = columnString.Split('\t', '\n');

            for (int i = 0; i < columnTitles.Count(); i++)
            {
                var columnTitle = columnTitles[i].ToUpper(); 
                switch (columnTitle)
                {
                    case HMDB:
                        columnMap.Add(HMDB, i);
                        break;
                    case Pubchem:
                        columnMap.Add(Pubchem, i);
                        break;
                    case CAS:
                        columnMap.Add(CAS, i);
                        break;
                    case KEGG:
                        columnMap.Add(KEGG, i);
                        break;
                    case ChEBI:
                        columnMap.Add(ChEBI, i);
                        break;
                    case InChI:
                        columnMap.Add(InChI, i);
                        break;
                    case InChIKey:
                        columnMap.Add(InChIKey, i);
                        break;
                    case CommonName:
                        columnMap.Add(CommonName, i);
                        break;
                    case SYNONYMS:
                        columnMap.Add(SYNONYMS, i);
                        break;
                    case SuperClass:
                        columnMap.Add(SuperClass, i);
                        break;
                    case Class:
                        columnMap.Add(Class, i);
                        break;
                    case DirectParent:
                        columnMap.Add(DirectParent, i);
                        break;
                    case Pathways:
                        columnMap.Add(Pathways, i);
                        break;
                }
            }
            return columnMap;
        } 


    }
}
