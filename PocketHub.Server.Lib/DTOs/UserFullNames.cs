using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestExportViews;
using System.Collections.Generic;

namespace PocketHub.Server.Lib.DTOs
{
    public class UserFullNames : IRestExportView
    {
        public string DisplayPath => "user-full-names";

        public int      uid         { get; set; }
        public string   UserName    { get; set; }
        public string   FirstName   { get; set; }
        public string   MiddleName  { get; set; }
        public string   LastName    { get; set; }

        public string FullName => " ".JoinNonBlanks(FirstName, MiddleName, LastName);


        public List<string>  CastArguments (object[] args) => new List<string>();
        public void          PostProcess   () { }
    }
}
