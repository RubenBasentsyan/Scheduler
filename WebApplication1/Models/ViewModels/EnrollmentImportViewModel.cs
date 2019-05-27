using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class EnrollmentImportViewModel
    {
        [Display(Name = "Enrollments File (.csv)"), 
            Required(ErrorMessage = "Please upload the table of student enrollments" ), 
            FileExtensions(Extensions = ".csv", ErrorMessage = "The table of enrollments must be in .csv format")]
        public HttpPostedFile EnrollmentsCSV { get; set;}
    }
}