//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Entrollments
    {
        public int Person_Fk { get; set; }
        public int Course_Fk { get; set; }
        public int EnrollmentId { get; set; }
    
        public virtual Courses Courses { get; set; }
        public virtual Persons Persons { get; set; }
    }
}
