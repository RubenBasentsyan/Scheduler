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
    
    public partial class Colors
    {
        public int Course_Fk { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> TimeSlot { get; set; }
    
        public virtual Courses Courses { get; set; }
    }
}