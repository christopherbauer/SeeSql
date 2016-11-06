using System.Collections.Generic;
using SeeSql.DomainModels;

namespace SeeSql.WebApplication.ViewModels
{
    public class JobListViewModel
    {
        public IList<Job> Jobs { get; set; }
        public string DataTableName { get; set; }
    }
}