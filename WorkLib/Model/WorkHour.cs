//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkLib.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>Work hour object.</summary>
    public partial class WorkHour : Entity<WorkHour>
    {
        public WorkHour(object data) : base(data)
        { }
        /// <summary>Gets or sets the work hour identifier.</summary>
        /// <value>The work hour identifier.</value>
        [Key]
        public int WorkHourID { get; set; }
        /// <summary>Gets or sets the work day date.</summary>
        /// <value>The date.</value>
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
        /// <summary>Gets or sets work begin time.</summary>
        /// <value>Work begin time.</value>
        [Required(ErrorMessage = "Behin time is required.")]
        public DateTime From { get; set; }
        /// <summary>Gets or sets work end time.</summary>
        /// <value>Work end time.</value>
        public DateTime? To { get; set; }
        /// <summary>Gets the work hours.</summary>
        /// <value>The work hours.</value>
        public DateTime? Hours 
        { 
            get
            {
                if (To.HasValue)
                {
                    var ret = new DateTime(Date.Year, Date.Month, Date.Day, To.Value.Hour, To.Value.Minute, 0);
                    ret = ret.Add(new TimeSpan(From.Hour * -1, From.Minute * -1, 0));
                    return ret;
                }
                return null;
            }
        }
        /// <summary>Gets or sets the project identifier.</summary>
        /// <value>The project identifier.</value>
        public int? ProjectId { get; set; }
        /// <summary>Gets or sets the subject.</summary>
        /// <value>The subject.</value>
        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }
        /// <summary>Gets or sets the work description.</summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>Gets or sets the project.</summary>
        /// <value>The project.</value>
        public Project Project { get; set; }
        /// <summary>Gets or sets the user.</summary>
        /// <value>The user.</value>
        public User User { get; set; }
    }
}
